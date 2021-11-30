using API.Context;
using API.Models;
using API.ViewModel;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace API.Repository.Data
{
    public class AccountRepository : GeneralRepository<MyContext, Account, string>
    {
        private readonly MyContext context;
        public AccountRepository(MyContext myContext) : base(myContext)
        {
            this.context = myContext;
        }
        /*public int Login(LoginVM loginVM)
        {

            var checkEmail = context.Employees.Where(x =>
            x.Email == loginVM.Email).FirstOrDefault();
            var checkPhone = context.Employees.Where(x =>
            x.Phone == loginVM.Phone).FirstOrDefault();
            if (checkEmail != null || checkPhone != null)
            {
                var data = (from e in context.Employees
                            where
e.Email == loginVM.Email || e.Phone == loginVM.Phone
                            join a in context.Accounts on e.NIK equals a.NIK
                            select a.Password).Single();

                var checkPassword = Hashing.Hashing.ValidatePassword(loginVM.Password, data);
                if (checkPassword == true)
                {
                    return 2;
                }
                else
                {
                    return 3;
                }
            }
            return 1;
        }*/
        public int Login(LoginVM loginVM)
        {
            var getlogin = context.Employees.Include("Account").Where(e => e.Email == loginVM.Email || e.Phone == loginVM.Phone).FirstOrDefault();
            if(getlogin != null)
            {
                var checkPassword = Hashing.Hashing.ValidatePassword(loginVM.Password, getlogin.Account.Password);
                if (checkPassword == true)
                {
                    return 2;
                }
                return 3;
            }
            return 1;
        }
        /*public Tuple<int, string> Login(LoginVM loginVM)
        {
            var checkEmail = context.Employees.Where(x => x.Email == loginVM.Email).FirstOrDefault();
            var checkPhone = context.Employees.Where(x => x.Phone == loginVM.Phone).FirstOrDefault();
            if (checkEmail != null || checkPhone != null)
            {
                var NIK = (from e in context.Employees
                           where e.Email == loginVM.Email || e.Phone == loginVM.Phone
                           join a in context.Accounts on e.NIK equals a.NIK
                           select a.NIK).Single();
                var Pass = (from e in context.Employees
                            where e.Email == loginVM.Email || e.Phone == loginVM.Phone
                            join a in context.Accounts on e.NIK equals a.NIK
                            select a.Password).Single();
                var checkPassword = Hashing.Hashing.ValidatePassword(loginVM.Password, Pass);
                if (checkPassword == true)
                {
                    return Tuple.Create(2, NIK);
                }
                return Tuple.Create(3, "");
            }
            return Tuple.Create(1, "");
        }*/
        public IEnumerable<ProfileVM> GetProfile(LoginVM loginVM)
        {
            var profile = from e in context.Employees
                          where
e.Email == loginVM.Email || e.Phone == loginVM.Phone
                          join a in context.Accounts on e.NIK equals a.NIK
                          join p in context.Profilings on a.NIK equals p.NIK
                          join ed in context.Educations on p.EducationId equals ed.EducationId
                          join u in context.Universities on ed.UniversityId equals u.UniversityId
                          select new ProfileVM()
                          {
                              NIK = e.NIK,
                              FullName = e.FirstName + " " + e.LastName,
                              Phone = e.Phone,
                              Salary = e.Salary,
                              Email = e.Email,
                              GPA = ed.GPA,
                              Degree = ed.Degree,
                              UniversityName = u.Name
                          };
            return profile.ToList();
        }
        /*public IEnumerable<ProfileVM> Profile(string key)
        {
            var prof = from e in context.Employees
                       where e.NIK == key
                       join a in context.Accounts on e.NIK equals a.NIK
                       join p in context.Profilings on a.NIK equals p.NIK
                       join ed in context.Educations on p.EducationId equals ed.EducationId
                       join u in context.Universities on ed.UniversityId equals u.UniversityId
                       select new ProfileVM()
                       {
                           NIK = e.NIK,
                           FullName = e.FirstName + " " + e.LastName,
                           Phone = e.Phone,
                           Salary = e.Salary,
                           Email = e.Email,
                           GPA = ed.GPA,
                           Degree = ed.Degree,
                           UniversityName = u.Name
                       };
            return prof.ToList();
        }*/
        /*public int ForgetPassword(ForgetPasswordVM forgetPasswordVM)
        {
            var PassOld = (from e in context.Employees
                           where e.Email == forgetPasswordVM.Email
                           join a in context.Accounts on e.NIK equals a.NIK
                           select a.NIK).Single();
            var Finding = context.Accounts.Find(PassOld);
            var name = (from e in context.Employees
                        where e.Email == forgetPasswordVM.Email
                        select e.FirstName).Single();
            string UniqueString = Guid.NewGuid().ToString();
            Finding.Password = Hashing.Hashing.HashPassword(UniqueString);
            context.SaveChanges();

            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("<h1>Use this backup password to create the new Password</h1>");
            stringBuilder.Append($"<h2>Hi {name}!<h2>");
            stringBuilder.Append($"<h3>Your New Password : {UniqueString} </h3>");

            if (Finding != null)
            {
                try
                {
                    MailMessage mailMessage = new MailMessage();
                    mailMessage.IsBodyHtml = true;
                    mailMessage.From = new MailAddress("faiz9fadh@gmail.com");
                    mailMessage.Subject = $"Recovery your Password {DateTime.Now}";
                    mailMessage.Body = stringBuilder.ToString();
                    mailMessage.To.Add(forgetPasswordVM.Email);

                    using (SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587))
                    {
                        smtpClient.Credentials = new System.Net.NetworkCredential("faiz9fadh@gmail.com", "fnfaiz2728");
                        smtpClient.EnableSsl = true;
                        smtpClient.Send(mailMessage);
                    }
                }
                catch (Exception)
                {
                    return 2;
                }
                return 1;
            }
            else
            {
                return 3;
            }
        }*/
        public int ForgetPassword(ForgetPasswordVM forgetPasswordVM)
        {
            var getData = context.Employees.Include("Account").Where(x => x.Email == forgetPasswordVM.Email).FirstOrDefault();
            var Finding = context.Accounts.Find(getData.Account.NIK);
            var name = getData.FirstName;
            string UniqueString = Guid.NewGuid().ToString();
            Finding.Password = Hashing.Hashing.HashPassword(UniqueString);
            context.SaveChanges();

            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("<h1>Use this backup password to create the new Password</h1>");
            stringBuilder.Append($"<h2>Hi {name}!<h2>");
            stringBuilder.Append($"<h3>Your New Password : {UniqueString} </h3>");

            if (Finding != null)
            {
                try
                {
                    MailMessage mailMessage = new MailMessage();
                    mailMessage.IsBodyHtml = true;
                    mailMessage.From = new MailAddress("faiz9fadh@gmail.com");
                    mailMessage.Subject = $"Recovery your Password {DateTime.Now}";
                    mailMessage.Body = stringBuilder.ToString();
                    mailMessage.To.Add(forgetPasswordVM.Email);

                    using (SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587))
                    {
                        smtpClient.Credentials = new System.Net.NetworkCredential("faiz9fadh@gmail.com", "fnfaiz2728");
                        smtpClient.EnableSsl = true;
                        smtpClient.Send(mailMessage);
                    }
                }
                catch (Exception)
                {
                    return 2;
                }
                return 1;
            }
            else
            {
                return 3;
            }
        }
        /*public int ChangePassword(ChangePasswordVM changePasswordVM)
        {
            var PassOld = (from e in context.Employees
                           where e.Email == changePasswordVM.Email
                           join a in context.Accounts on e.NIK equals a.NIK
                           select a.NIK).Single();
            var Finding = context.Accounts.Find(PassOld);
            if (Finding != null)
            {
                if (Hashing.Hashing.ValidatePassword(changePasswordVM.CurrentPassword, Finding.Password))
                {
                    if (changePasswordVM.NewPassword == changePasswordVM.ConfirmNewPassword)
                    {
                        Finding.Password = Hashing.Hashing.HashPassword(changePasswordVM.NewPassword);
                        context.SaveChanges();
                        return 1;
                    }
                    else
                    {
                        return 2;
                    }
                }
                else
                {
                    return 3;
                }
            }
            else
            {
                return 4;
            }
        }*/
        public int ChangePassword(ChangePasswordVM changePasswordVM)
        {
            var getData = context.Employees.Include("Account").Where(x => x.Email == changePasswordVM.Email).FirstOrDefault();
            var Finding = context.Accounts.Find(getData.Account.NIK);
            if (Finding != null)
            {
                if (Hashing.Hashing.ValidatePassword(changePasswordVM.CurrentPassword, Finding.Password))
                {
                    if (changePasswordVM.NewPassword == changePasswordVM.ConfirmNewPassword)
                    {
                        Finding.Password = Hashing.Hashing.HashPassword(changePasswordVM.NewPassword);
                        context.SaveChanges();
                        return 1;
                    }
                    else
                    {
                        return 2;
                    }
                }
                else
                {
                    return 3;
                }
            }
            else
            {
                return 4;
            }
        }

    }
}
