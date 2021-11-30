using API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.ViewModel
{
    //class ViewModel untuk Register
    public class RegisterVM
    {
        //isi field yang diperlukan untuk register
        //field dr model employee
        public string NIK { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }
        public DateTime BirthDate { get; set; }
        public int Salary { get; set; }
        public string Email { get; set; }
        public Gender Gender { get; set; }

        //field dr model account
        public string Password { get; set; }
        //field dr model education
        public string Degree { get; set; }
        public string GPA { get; set; }
        //field dr model university
        public int UniversityId { get; set; }
    }
    public enum Gender
    {
        Male,
        Female
    }
}
