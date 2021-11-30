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
    public class UniversityRepository : GeneralRepository<MyContext, University, int>
    {
        private readonly MyContext context;
        public UniversityRepository(MyContext myContext) : base(myContext)
        {
            this.context = myContext;
        }
        /*public IEnumerable<ProfileVM> GetCountUniv(string UnivName)
        {
            var univ = from e in context.Employees
                       join a in context.Accounts on e.NIK equals a.NIK
                       join p in context.Profilings on a.NIK equals p.NIK
                       join ed in context.Educations on p.EducationId equals ed.EducationId
                       join u in context.Universities on ed.UniversityId equals u.UniversityId
                       where u.Name == UnivName
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
            return univ.ToList();
        }*/
        /*public IEnumerable<ProfileVM> GetCountUniv(string UnivName)
        {
            var univ = from p in context.Profilings
                       join ed in context.Educations on p.EducationId equals ed.EducationId
                       join u in context.Universities on ed.UniversityId equals u.UniversityId
                       where u.Name == UnivName
                       select new ProfileVM()
                       {
                           UniversityName = u.Name
                       };
            return univ.ToList();
        }*/
        /*public IEnumerable GetCountUniv()
        {
            var univ = (from e in context.Employees
                       join a in context.Accounts on e.NIK equals a.NIK
                       join p in context.Profilings on a.NIK equals p.NIK
                       join ed in context.Educations on p.EducationId equals ed.EducationId
                       join u in context.Universities on ed.UniversityId equals u.UniversityId
                       group u by u.Name into x
                       select new
                       {
                           UniversityName = x.Key,
                           CountStudent = x.Count()
                       }).AsEnumerable();
            return univ.ToList();
        }*/
        public IEnumerable GetCountUniv()
        {
            var univ = (from p in context.Profilings
                        join ed in context.Educations on p.EducationId equals ed.EducationId
                        join u in context.Universities on ed.UniversityId equals u.UniversityId
                        group u by u.Name into cnt
                        select new
                        {
                            UniversityName = cnt.Key,
                            CountStudent = cnt.Count()
                        }).AsEnumerable();
            return univ.ToList();
        }
        public KeyValuePair<List<string>, List<int>> GetCountEmployees()
        {
            var data = (from p in context.Profilings
                        join ed in context.Educations on p.EducationId equals ed.EducationId
                        join u in context.Universities on ed.UniversityId equals u.UniversityId
                        group u by u.Name into cnt
                        select new
                        {
                            UnivName = cnt.Key,
                            StudentsCount = cnt.Count()
                        }).ToList();

            List<string> univName = new List<string>();
            List<int> univStudent = new List<int>();

            foreach (var item in data)
            {
                univName.Add(item.UnivName);
                univStudent.Add(item.StudentsCount);
            }

            return new KeyValuePair<List<string>, List<int>>(univName, univStudent);
        }

        /*public IEnumerable<ProfileVM> GetCountUniv(string UnivName)
        {
            var univ = from p in context.Profilings
                       join ed in context.Educations on p.EducationId equals ed.EducationId
                       join u in context.Universities on ed.UniversityId equals u.UniversityId
                       group u by u.Name into x
                       select new ProfileVM()
                       {
                           //UniversityName = u.Name
                       };
            return univ.ToList();
        }*/
    }
}
