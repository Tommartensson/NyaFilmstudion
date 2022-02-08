using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Filmstudion.Interfaces
{
    interface IUserAuthenticate
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
