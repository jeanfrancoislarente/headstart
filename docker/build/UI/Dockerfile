# escape=`

ARG BASE_IMAGE

FROM ${BASE_IMAGE} as base

ARG NODE_VERSION
ENV GIT_VERSION=2.30.2
ENV GIT_PATCH_VERSION=1

SHELL ["pwsh", "-Command", "$ErrorActionPreference = 'Stop';$ProgressPreference='silentlyContinue';"]

RUN Invoke-WebRequest -OutFile nodejs.zip -UseBasicParsing $('https://nodejs.org/dist/v{0}/node-v{0}-win-x64.zip' -f $env:NODE_VERSION); `
    Expand-Archive nodejs.zip -DestinationPath C:\; `
    Rename-Item $('C:\\node-v{0}-win-x64' -f $env:NODE_VERSION) c:\nodejs; `
    Remove-Item nodejs.zip -Force

RUN Invoke-WebRequest -OutFile 'mingit.zip' -UseBasicParsing $('https://github.com/git-for-windows/git/releases/download/v{0}.windows.{1}/MinGit-{0}-busybox-64-bit.zip' -f $env:GIT_VERSION, $env:GIT_PATCH_VERSION) -Verbose; `
    Expand-Archive mingit.zip -DestinationPath c:\mingit ; `
    Remove-Item mingit.zip -Force ;

SHELL ["cmd.exe", "/s" , "/c"]
USER ContainerAdministrator
RUN SETX /M PATH "%PATH%;C:\nodejs;c:\mingit\cmd"
USER ContainerUser

RUN npm config set registry http://registry.npmjs.org/ --global
RUN npm install -g @angular/cli

FROM base as build

ARG ROLE

WORKDIR /build

COPY src/UI/${ROLE}/package*.json ./

RUN npm install

COPY src/UI/${ROLE}/ .
EXPOSE 80
CMD "npm run container"
