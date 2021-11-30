 using API.Base;
using API.Context;
using API.Models;
using API.ViewModel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Repository.Data
{
    public class EmployeeRepository : GeneralRepository<MyContext, Employee, string>
    {
        private readonly MyContext context;
        public EmployeeRepository(MyContext myContext) : base(myContext)
        {
            this.context = myContext;
        }
        public int Register(RegisterVM registerVM)
        {
            var checkNIK = context.Employees.Find(registerVM.NIK);
            var checkEmail = context.Employees.Where(x => x.Email == registerVM.Email).FirstOrDefault();
            var checkPhone = context.Employees.Where(x => x.Phone == registerVM.Phone).FirstOrDefault();
            if (checkNIK != null)
            {
                return 2;
            }
            else if (checkEmail != null)
            {
                return 3;
            }
            else if (checkPhone != null)
            {
                return 4;
            }

            Employee employee = new Employee()
            {
                NIK = registerVM.NIK,
                FirstName = registerVM.FirstName,
                LastName = registerVM.LastName,
                Phone = registerVM.Phone,
                BirthDate = registerVM.BirthDate,
                Salary = registerVM.Salary,
                Email = registerVM.Email,
                Gender = (Models.Gender)registerVM.Gender
            };
            Account account = new Account()
            {
                NIK = employee.NIK,
                Password = Hashing.Hashing.HashPassword(registerVM.Password)
            };
            Education education = new Education()
            {
                Degree = registerVM.Degree,
                GPA = registerVM.GPA,
                UniversityId = registerVM.UniversityId
            };

            context.Employees.Add(employee);
            context.Accounts.Add(account);
            context.Educations.Add(education);
            context.SaveChanges();
            Profiling profiling = new Profiling()
            {
                EducationId = education.EducationId,
                NIK = registerVM.NIK
            };
            context.Profilings.Add(profiling);
            context.SaveChanges();
            AccountRole accountRole = new AccountRole()
            {
                AccountNIK = employee.NIK,
                RoleId = 2
            };
            context.AccountRoles.Add(accountRole);
            context.SaveChanges();
            return 1;
        }
        /*public int Register(RegisterVM registerVM)
        {
            var getData = context.Employees.Where(x => x.Email == registerVM.Email || x.Phone == registerVM.Phone || x.NIK == registerVM.NIK).FirstOrDefault();
            *//*var checkNIK = context.Employees.Find(registerVM.NIK);
            var checkEmail = context.Employees.Where(x => x.Email == registerVM.Email).FirstOrDefault();
            var checkPhone = context.Employees.Where(x => x.Phone == registerVM.Phone).FirstOrDefault();*//*
            if (getData.NIK != null)
            {
                return 2;
            }
            else if (getData.Email != null)
            {
                return 3;
            }
            else if (getData.Phone != null)
            {
                return 4;
            }

            Employee employee = new Employee()
            {
                NIK = registerVM.NIK,
                FirstName = registerVM.FirstName,
                LastName = registerVM.LastName,
                Phone = registerVM.Phone,
                BirthDate = registerVM.BirthDate,
                Salary = registerVM.Salary,
                Email = registerVM.Email,
                Gender = (Models.Gender)registerVM.Gender
            };
            Account account = new Account()
            {
                NIK = employee.NIK,
                Password = Hashing.Hashing.HashPassword(registerVM.Password)
            };
            Education education = new Education()
            {
                Degree = registerVM.Degree,
                GPA = registerVM.GPA,
                UniversityId = registerVM.UniversityId
            };

            context.Employees.Add(employee);
            context.Accounts.Add(account);
            context.Educations.Add(education);
            context.SaveChanges();
            Profiling profiling = new Profiling()
            {
                EducationId = education.EducationId,
                NIK = registerVM.NIK
            };
            context.Profilings.Add(profiling);
            context.SaveChanges();
            AccountRole accountRole = new AccountRole()
            {
                AccountNIK = employee.NIK,
                RoleId = 2
            };
            context.AccountRoles.Add(accountRole);
            context.SaveChanges();
            return 1;
        }*/
        /*public int Login(LoginVM loginVM)
        {
            Employee employee = new Employee()
            {
                Email = loginVM.Email
            };
            Account account = new Account()
            {
                Password = loginVM.Password
            };
            return 0;
        }*/
        public IEnumerable GetAll()
        {
            var register = from e in context.Set<Employee>()
                           join a in context.Set<Account>() on e.NIK equals a.NIK
                           join p in context.Set<Profiling>() on a.NIK equals p.NIK
                           join ed in context.Set<Education>() on p.EducationId equals ed.EducationId
                           join u in context.Set<University>() on ed.UniversityId equals u.UniversityId
                           select new
                           {
                               NIK = e.NIK,
                               Fullname = e.FirstName + " " + e.LastName,
                               Gender = e.Gender == 0 ? "Male" : "Female",
                               Phone = e.Phone,
                               Salary = e.Salary,
                               Email = e.Email,
                               GPA = ed.GPA,
                               Degree = ed.Degree,
                               UniversityName = u.Name
                           };
            return register.ToList();
        }
    }
}
