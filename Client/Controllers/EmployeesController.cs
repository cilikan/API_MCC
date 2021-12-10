using API.Models;
using Client.Base.Controllers;
using Client.Repositories.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Client.Controllers
{
    /*[Authorize]*/
    public class EmployeesController : BaseController<Employee, EmployeeRepository, string>
    {
        private readonly EmployeeRepository employeeRepository;
        
        public EmployeesController(EmployeeRepository repository) : base(repository)
        {
            this.employeeRepository = repository;
        }

        public IActionResult Index()
        {
            /*return View("Employees");*/
            return View();
        }
        [HttpGet]
        public async Task<JsonResult> GetRegister()
        {
            var result = await employeeRepository.GetRegister();
            return Json(result);
        }
    }
}
