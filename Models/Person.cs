﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVCDashboardAuth.Models {
    public class Person {
        public string Login { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
    }
}