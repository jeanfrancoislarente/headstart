function WriteLines {
    Param (
        [Parameter(Mandatory = $true)]
        [ValidateScript( { Test-Path $_ -IsValid })]
        [ValidateScript( { [System.IO.Path]::IsPathRooted($_) })]
        [string]
        $File,

        [string[]]
        $Content,

        [System.Text.Encoding]
        $Encoding = [System.Text.Encoding]::UTF8,

        [int]
        $Retries = 10
    )

    $enc = $Encoding
    $crlf = $enc.GetBytes([Environment]::NewLine)
    $tries = 0
    $fileLock = $false

    if (!(Test-Path -Path $File)) {
        New-Item -Path $File
    }

    do {
        try {
            $fileLock = [System.IO.File]::Open($File, 'Open', 'ReadWrite', 'None')
        }
        catch {
            Write-Warning -Message "Failed to get lock on file. $File"
            $tries++
            Start-Sleep -Milliseconds 100
        }
    } until ($fileLock -or ($tries -eq $Retries))

    if ($tries -eq $Retries) {
        throw "Unable to get lock on file $File after $Retries attempt(s)."
    }

    $fileLock.SetLength(0)

    foreach ($line in $Content) {
        $newLine = $enc.GetBytes($line)
        $fileLock.Write($newLine, 0, $newLine.Length)
        $fileLock.Write($crlf, 0, $crlf.Length)
    }

    $fileLock.Close()
}

function Add-HostsEntry {
    Param (
        [Parameter(Mandatory = $true, Position = 0)]
        [ValidateNotNullOrEmpty()]
        [string]
        $Hostname,

        [string]
        [ValidateNotNullOrEmpty()]
        $IPAddress = "127.0.0.1",

        [string]
        $Path = (Join-Path -Path $env:windir -ChildPath "system32\drivers\etc\hosts")
    )

    if (-not (Test-Path $Path)) {
        Write-Warning "No hosts file found, hosts have not been updated"
        return
    }

    # Create backup
    Copy-Item $Path "$Path.backup"
    Write-Verbose "Created backup of hosts file to $Path.backup"

    # Build regex match pattern
    $pattern = '^' + [Regex]::Escape($IPAddress) + '\s+' + [Regex]::Escape($HostName) + '\s*$'

    $hostsContent = @(Get-Content -Path $Path -Encoding UTF8)

    # Check if exists
    $existingEntries = $hostsContent -match $pattern
    if ($existingEntries.Count -gt 0) {
        Write-Verbose "Existing host entry found for $IPAddress with hostname '$HostName'"
        return
    }

    # Add it
    $hostsContent += "$IPAddress`t$HostName"
    WriteLines -File $Path -Content $hostsContent -Encoding ([System.Text.Encoding]::UTF8)
    Write-Verbose "Host entry for $IPAddress with hostname '$HostName' has been added"
}

Add-HostsEntry "buyer.headstart.localhost"
Add-HostsEntry "seller.headstart.localhost"
Add-HostsEntry "api.headstart.localhost"

New-Item -Path data\traefik\certs -ItemType Directory -Force
Push-Location data\traefik\certs
try {
    $mkcert = ".\mkcert.exe"
    if ($null -ne (Get-Command mkcert.exe -ErrorAction SilentlyContinue)) {
        # mkcert installed in PATH
        $mkcert = "mkcert"
    }
    elseif (-not (Test-Path $mkcert)) {
        Write-Host "Downloading and installing mkcert certificate tool..." -ForegroundColor Green
        $preference = $ProgressPreference
        $ProgressPreference = "SilentlyContinue" # Makes the download exponentially faster
        Invoke-WebRequest "https://github.com/FiloSottile/mkcert/releases/download/v1.4.1/mkcert-v1.4.1-windows-amd64.exe" -UseBasicParsing -OutFile mkcert.exe
        $ProgressPreference = $preference
        if ((Get-FileHash mkcert.exe).Hash -ne "1BE92F598145F61CA67DD9F5C687DFEC17953548D013715FF54067B34D7C3246") {
            Remove-Item mkcert.exe -Force
            throw "Invalid mkcert.exe file"
        }
    }
    Write-Host "Generating Traefik TLS certificate..." -ForegroundColor Green
    & $mkcert -install
    & $mkcert "*.headstart.localhost"
}
catch {
    Write-Error "An error occurred while attempting to generate TLS certificate: $_"
}
finally {
    Pop-Location
}