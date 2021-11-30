using API.Context;
using API.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Repository
{
    public class OldEmployeeRepository : Interface.OldIEmployeeRepository
    {
        private readonly MyContext context;

        public OldEmployeeRepository(MyContext context)
        {
            this.context = context;
        }

        public int Delete(string NIK)
        {
            var entity = context.Employees.Find(NIK);
            if (entity != null)
            {
                context.Remove(entity);
            }
            var result = context.SaveChanges();
            return result;
        }
        public IEnumerable<Employee> Get()
        {
            return context.Employees.ToList();
        }
        public Employee Get(string NIK)
        {
            return context.Employees.Find(NIK);
        }
        public int Insert(Employee employee)
        {
            var checknik = context.Employees.Find(employee.NIK);
            if (checknik == null)
            {
               context.Employees.Add(employee);
            }
            var result = context.SaveChanges();
            return result;
        }

        public int Update(Employee employee)
        {
            context.Entry(employee).State = EntityState.Modified;
            var result = context.SaveChanges();
            return result;
        }
    }
}
