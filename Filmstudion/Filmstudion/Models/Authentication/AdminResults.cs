using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Filmstudion.Models.Authentication
{
    public class AdminResults
    {
        public int UserId { get; set; }
        public string Role { get; set; }
        public string UserName { get; set; }
    }
}
