﻿using System.Collections.Generic;
using OrderCloud.SDK;

namespace Headstart.Models
{
    public class HSUser : User<UserXp>, IHSObject
    {
    }

    public class UserXp
    {
    public string Country { get; set; }

    public string OrderEmails { get; set; }

    public string RequestInfoEmails { get; set; }

    public List<string> AddtlRcpts { get; set; }
    }
}
