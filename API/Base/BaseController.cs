using API.Repository.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace API.Base
{
    [Route("api/[controller]")]
    [ApiController]
    public class BaseController<Entity, Repository, Key> : ControllerBase
        where Entity : class
        where Repository : IRepository<Entity, Key>
    {
        private readonly Repository repository;

        public BaseController(Repository repository)
        {
            this.repository = repository;
        }
        [HttpPost]
        public ActionResult<Entity> Post(Entity entity)
        {
            var Masuk = repository.Insert(entity);
            if (Masuk != 0)
            {
                /*return Ok(new { status = HttpStatusCode.OK, result = Masuk, message = "Data Masuk" });*/
                return Ok(Masuk);
            }
            return BadRequest(new { status = HttpStatusCode.BadRequest, result = Masuk, message = "Data sama, gagal menambahkan data" });
        }
        [HttpGet("{key}")]
        public ActionResult GetByNIK(Key key)
        {
            var result = repository.Get(key);
            return Ok(result);
        }
        [HttpGet]
        public ActionResult Get()
        {
            var result = repository.Get();
            if (result.Count() != 0)
            {
                //return Ok(new { status = HttpStatusCode.OK, result = result, Message = "Data ditampilkan" });
                return Ok(result);

            }
            return NotFound(new { status = HttpStatusCode.NotFound, Message = $"Data belum tersedia" });
        }

        [HttpDelete("{key}")]
        public ActionResult<Entity> Delete(Key key)
        {
            var Found = repository.Delete(key);
            if (Found != 0)
            {
                /*return Ok(new { status = HttpStatusCode.OK, result = Found, message = "Data berhasil dihapus"});*/
                return Ok(Found);
            }
            return NotFound(new{ status = HttpStatusCode.NotFound, result = Found, message = "Data tidak ditemukan"});
        }
        [HttpPut]
        public ActionResult Update(Entity entity, Key key)
        {
            var result = repository.Update(entity, key);
            try
            {
                /*return Ok(new { status = HttpStatusCode.OK, result = result, Message = "Data terupdate" });*/
                return Ok(result);
            }
            catch (Exception)
            {
                return BadRequest(new { status = HttpStatusCode.BadRequest, Message = "Gagal update" });
            }
        }
    }
}
