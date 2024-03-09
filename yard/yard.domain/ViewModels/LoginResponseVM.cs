using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using yard.domain.Models;

namespace yard.domain.ViewModels
{
    public class LoginResponseVM
    {
        public string Token { get; set; }
        public AppUser User { get; set; }
    }
}
