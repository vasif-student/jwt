using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainModels.Models
{
    public class User : IdentityUser
    {
        public string FullName { get; set; }
        public bool IsBlocked { get; set; }
    }
}
