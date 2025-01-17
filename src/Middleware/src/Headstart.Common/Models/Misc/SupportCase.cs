﻿using Microsoft.AspNetCore.Http;

namespace Headstart.Common.Models.Misc
{
    public class SupportCase
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string Vendor { get; set; }

        public string Subject { get; set; }

        public string Message { get; set; }

        public IFormFile File { get; set; }
    }
}
