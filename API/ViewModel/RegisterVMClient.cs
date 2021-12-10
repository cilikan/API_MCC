using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.ViewModel
{
    public class RegisterVMClient
    {

        //isi field yang diperlukan untuk register
        //field dr model employee
        public string NIK { get; set; }
        public string FullName { get; set; }
        public string Phone { get; set; }
        /*public DateTime BirthDate { get; set; }*/
        public int Salary { get; set; }
        public string Email { get; set; }
        public string Gender { get; set; }

        //field dr model account
        /*public string Password { get; set; }*/
        //field dr model education
        public string Degree { get; set; }
        public string GPA { get; set; }
        //field dr model university
        public string UniversityName { get; set; }
    }
}
