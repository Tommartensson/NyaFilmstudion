using Filmstudion.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Filmstudion.Models.Authentication
{
    public class UserAuthenticate : IUserAuthenticate
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
