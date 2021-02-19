using System;
using System.Collections.Generic;

#nullable disable

namespace AlfalahApp.Server.Models
{
    public partial class User
    {
        public Guid Id { get; set; }
        public string Username { get; set; }
        public string Passwordhash { get; set; }
    }
}
