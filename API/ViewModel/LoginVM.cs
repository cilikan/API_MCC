using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.ViewModel
{
    public class LoginVM
    {
        //isi field yang diperlukan untuk login
        //field dr model employee
        public string Email { get; set; }
        public string Phone { get; set; }
        //field dr model account
        public string Password { get; set; }
    }
}
