﻿namespace OrderCloud.Integrations.Library.Models
{
    public class InvalidHeaderError
    {
        public InvalidHeaderError(string name)
        {
            Header = name;
        }

        public string Header { get; set; }
    }
}
