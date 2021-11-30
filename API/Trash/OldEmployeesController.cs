using API.Context;
using API.Models;
using API.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OldEmployeesController : ControllerBase
    {
        private OldEmployeeRepository employeeRepository;
        public OldEmployeesController(OldEmployeeRepository employeeRepository)
        {
            this.employeeRepository = employeeRepository;
        }
        //melakukan post
        [HttpPost]
        public ActionResult Post (Employee employee)
        {
            var masuk = employeeRepository.Insert(employee);
            if (masuk != 0)
            {
                return Ok(new { 
                    status = HttpStatusCode.OK, 
                    result = masuk, 
                    message = "Data Telah Masuk" });
            }
            return BadRequest(new { 
                status = HttpStatusCode.BadRequest, 
                result = masuk, 
                message = "NIK Sama, Gagal Menambahkan Data" });
            /*employeeRepository.Insert(employee);
            return Ok();*/
        }
        //melakukan get semua data
        [HttpGet]
        public IEnumerable<Employee> GetAllEmployees()
        {
            var check = employeeRepository.Get();
            return check;
            /*if (check.Count != 0)
            {
                return (IEnumerable<Employee>)Ok(new { status = HttpStatusCode.OK, result = check, message = "Data Berhasil ditampilkan" });
            }
            return (IEnumerable<Employee>)NotFound(new { status = HttpStatusCode.NotFound, result = check, message = "Database Kosong" });*/
        }
        //melakukan get berdasarkan nomor NIK
        [HttpGet("{NIK}")]
        public ActionResult GetByNIK(string NIK)
        {
            var Found = employeeRepository.Get(NIK);
            if (Found != null)
            {
                return Ok(new { 
                    status = HttpStatusCode.OK, 
                    result = Found, 
                    message = $"Data dengan NIK : {NIK} Berhasil Ditampilkan!" });
            }
            return NotFound(new { 
                status = HttpStatusCode.NotFound, 
                result = Found, 
                message = $"Data dengan NIK : {NIK} Tidak Ditemukan" });
        }
        //melakukan delete berdasarkan nomor NIK
        [HttpDelete("{NIK}")]
        public ActionResult Delete(string NIK)
        {
            var Found = employeeRepository.Delete(NIK);
            if (Found != 0)
            {
                return Ok(new { 
                    status = HttpStatusCode.OK, 
                    result = Found, 
                    message = $"Data dengan NIK : {NIK} Berhasil Dihapus!" });
            }
            return NotFound(new { 
                status = HttpStatusCode.NotFound, 
                result = Found, 
                message = $"Data dengan NIK : {NIK} Tidak Ditemukan" });

            //return Ok();
        }
        //melakukan update
        [HttpPut]
        public ActionResult Update(Employee employee)
        {
            employeeRepository.Update(employee);
            return Ok();
        }
    }
}
