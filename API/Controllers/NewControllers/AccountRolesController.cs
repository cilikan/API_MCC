using API.Base;
using API.Models;
using API.Repository.Data;
using API.ViewModel;
using Microsoft.AspNetCore.Authorization;
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
    public class AccountRolesController : BaseController<AccountRole, AccountRoleRepository, int>
    {
        private AccountRoleRepository AccountRoleRepository;
        public AccountRolesController(AccountRoleRepository repository) : base(repository)
        {
            this.AccountRoleRepository = repository;
        }
        /*[HttpPost("AddSign/{key}/{key2}")]
        public ActionResult Post(string key, int key2)
        {
            var post = AccountRoleRepository.Sign(key, key2);
            if (post == 1)
            {
                return Ok(new { status = HttpStatusCode.OK, post, message = "Berhasil" });
            }
            return BadRequest(new { status = HttpStatusCode.BadRequest, message = "Gagal" });
        }*/
        [Authorize(Roles = "Director")]
        [HttpPost("AddSignManager")]
        public ActionResult SignManager(SignManagerVM signManagerVM)
        {
            var post = AccountRoleRepository.SignManager(signManagerVM);
            if (post == 1)
            {
                return Ok(new { status = HttpStatusCode.OK, result = post, message = "Berhasil" });
            }
            return NotFound(new { status = HttpStatusCode.NotFound, result = post, message = "Gagal" });
        }
        [Authorize]
        [HttpGet("TestJWT")]
        public ActionResult TestJWT()
        {
            return Ok("Test JWT Berhasil");
        }
    }
}
