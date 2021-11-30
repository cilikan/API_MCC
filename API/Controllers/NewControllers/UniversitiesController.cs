using API.Base;
using API.Models;
using API.Repository.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace API.Controllers.NewControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UniversitiesController : BaseController<University, UniversityRepository, int>
    {
        private UniversityRepository universityRepository;
        public UniversitiesController(UniversityRepository repository) : base(repository)
        {
            this.universityRepository = repository;
        }
        /*[HttpGet("CountUniv/{UnivName}")]
        public ActionResult Count(string UnivName)
        {
            var hitung = universityRepository.GetCountUniv(UnivName);
            if (hitung.Count() != 0)
            {
                return Ok(new { status = HttpStatusCode.OK, result = $"Terdapat {hitung.Count()} orang lulusan Universitas {UnivName}", message = "Data berhasil ditampilkan" });
            }
            else
            {
                return NotFound(new { status = HttpStatusCode.NotFound, message = "Data gagal dihitung" });
            }
        }*/
        [HttpGet("CountUniv")]
        public ActionResult Counting()
        {
            var hitung = universityRepository.GetCountUniv();
            if(hitung != null)
            {
                return Ok(new { status = HttpStatusCode.OK, result = hitung, message = "Data Ditemukan" });
            }
            return NotFound(new { status = HttpStatusCode.NotFound, result = hitung, message = "Data tak ditemukan" });
        }
        [HttpGet("CountEmp")]
        public ActionResult Counts()
        {
            var hitung = universityRepository.GetCountEmployees();
            if (hitung.Key != null)
            {
                return Ok(new { status = HttpStatusCode.OK, result = hitung, message = "Data Ditemukan" });
            }
            return NotFound(new { status = HttpStatusCode.NotFound, result = hitung, message = "Data tak ditemukan" });
        }
    }
}
