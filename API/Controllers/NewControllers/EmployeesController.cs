using API.Base;
using API.Models;
using API.Repository.Data;
using API.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace API.Controllers.NewControllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("AllowOrigin")]
    public class EmployeesController : BaseController<Employee, EmployeeRepository, string>
    {
        private EmployeeRepository employeeRepository;
        public IConfiguration _configuration;
        public EmployeesController(EmployeeRepository repository, IConfiguration configuration) : base(repository)
        {
            this.employeeRepository = repository;
            this._configuration = configuration;
        }
        [HttpPost("Register")]
        public ActionResult Post(RegisterVM registerVM)
        {
            var masuk = employeeRepository.Register(registerVM);
            if (masuk == 1)
            {
                /*return Ok(new { status = HttpStatusCode.OK, result = masuk, message = "Pendaftaran Berhasil" });*/
                return Ok(masuk);
            }
            else if (masuk == 2)
            {
                return NotFound(new { status = HttpStatusCode.NotFound, result = masuk, message = "NIK sudah terdaftar, Register Gagal" });
            }
            else if (masuk == 3)
            {
                return NotFound(new { status = HttpStatusCode.NotFound, result = masuk, message = "Email Sudah terdaftar, Register Gagal" });
            }
            else if (masuk == 4)
            {
                return NotFound(new { status = HttpStatusCode.NotFound, result = masuk, message = "Nomor Telepon sudah terdaftar, Register Gagal" });
            }
            return NotFound(new { status = HttpStatusCode.NotFound, result = masuk, message = "Register Gagal" });
        }
        /*[HttpPost("Login")]
        public ActionResult Post(LoginVM loginVM)
        {
            employeeRepository.Login(loginVM);
            return Ok();
        }*/
        /*[Authorize(Roles = "Director, Manager")]*/
        [HttpGet("AllRegisteredData")]
        public ActionResult<Employee> GetAll()
        {
            var result = employeeRepository.GetAll();
            if (result == null)
            {
                /*return NotFound(new { status = HttpStatusCode.NotFound, 
                    result = result, 
                    message = "Data Masih Kosong" });*/
                return NotFound(result);
            }
            /*return Ok(new { status = HttpStatusCode.OK, 
                result = result, 
                message = "Semua data berhasil ditampilkan" });*/
            return Ok(result);
        }
        [HttpGet("TestCORS")]
        public ActionResult TestCORS()
        {
            return Ok("Test CORS Berhasil");
        }
    }
}
