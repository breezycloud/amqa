﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlfalahApp.Shared
{
    public class User
    {
        public Guid Id { get; set; }
        public string Username { get; set; }
        public string Passwordhash { get; set; }
    }
}
