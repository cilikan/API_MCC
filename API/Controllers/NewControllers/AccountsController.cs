using API.Base;
using API.Context;
using API.Models;
using API.Repository.Data;
using API.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace API.Controllers.NewControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : BaseController<Account, AccountRepository, string>
    {
        private AccountRepository accountRepository;
        public IConfiguration _configuration;
        private readonly MyContext context;
        public AccountsController(AccountRepository repository, IConfiguration configuration, MyContext myContext) : base(repository)
        {
            this.accountRepository = repository;
            this._configuration = configuration;
            this.context = myContext;
        }
        [HttpPost("Login")]
        public ActionResult Post(LoginVM loginVM)
        {
            var log = accountRepository.Login(loginVM);
            var profile = accountRepository.GetProfile(loginVM);
            if (log == 2)
            {
                var getUserData = (from e in context.Employees
                                   where e.Email == loginVM.Email
                                   join a in context.Accounts on e.NIK equals a.NIK
                                   join ar in context.AccountRoles on a.NIK equals ar.AccountNIK
                                   join r in context.Roles on ar.RoleId equals r.RoleId
                                   select new
                                   {
                                       Email = e.Email,
                                       Name = r.Name
                                   }).ToList();

                var claims = new List<Claim>
                        {
                            new Claim(JwtRegisteredClaimNames.Email, getUserData[0].Email)
                        };

                foreach (var userRole in getUserData)
                {
                    claims.Add(new Claim(ClaimTypes.Role, userRole.Name));
                }

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:key"]));
                var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                var token = new JwtSecurityToken(
                    _configuration["Jwt:Issuer"],
                    _configuration["Jwt:Audience"],
                    claims,
                    expires: DateTime.UtcNow.AddMinutes(10),
                    signingCredentials: signIn
                    );

                var idtoken = new JwtSecurityTokenHandler().WriteToken(token);
                claims.Add(new Claim("TokenSecurity", idtoken.ToString()));
                return Ok(new { status = HttpStatusCode.OK, idtoken, profile, Message = $"Login Berhasil" });
                //return Ok(new { status = HttpStatusCode.OK, result = profile, message = "Login Berhasil" });
                //return RedirectToAction("GetProfile", "Accounts", loginVM);
            }
            else if (log == 3)
            {
                return BadRequest(new { status = HttpStatusCode.BadRequest, result = log, message = "Password Salah, Login Gagal" });
            }
            return NotFound(new { status = HttpStatusCode.NotFound, result = log, message = "Email/Phone Number Tidak Ditemukan, Login Gagal" });
        }
        //jwt
        /*[HttpPost("Login")]
        public ActionResult Post(LoginVM loginVM)
        {
            var log = accountRepository.Login(loginVM);
            if (log.Item1 == 2)
            {
                var getUserData = (from e in context.Employees where e.Email == loginVM.Email
                                   join a in context.Accounts on e.NIK equals a.NIK
                                   join ar in context.AccountRoles on a.NIK equals ar.AccountNIK
                                   join r in context.Roles on ar.RoleId equals r.RoleId
                                   select new
                                   {
                                       Email = e.Email,
                                       Name = r.Name
                                   }).ToList();

                var claims = new List<Claim>
                        {
                            new Claim(JwtRegisteredClaimNames.Email, getUserData[0].Email)
                        };

                foreach (var userRole in getUserData)
                {
                    claims.Add(new Claim(ClaimTypes.Role, userRole.Name));
                }

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:key"]));
                var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                var token = new JwtSecurityToken(
                    _configuration["Jwt:Issuer"],
                    _configuration["Jwt:Audience"],
                    claims,
                    expires: DateTime.UtcNow.AddMinutes(10),
                    signingCredentials: signIn
                    );

                var idtoken = new JwtSecurityTokenHandler().WriteToken(token);
                claims.Add(new Claim("TokenSecurity", idtoken.ToString()));
                return Ok(new { status = HttpStatusCode.OK, result = idtoken, Message = $"Login Berhasil" });
                //return RedirectToAction("GetProfile", "Accounts", new { Key = log.Item2 });
            }
            else if (log.Item1 == 3)
            {
                return BadRequest(new { status = HttpStatusCode.BadRequest, message = "Password Salah" });
            }
            return BadRequest(new { status = HttpStatusCode.BadRequest, message = "Akun tidak ditemukan" });
        }*/
        /*[HttpGet("Profile/{Key}")]
        public ActionResult<LoginVM> GetProfile(string key)
        {
            var get = accountRepository.Profile(key);
            if (get == null)
            {
                return NotFound(new { status = HttpStatusCode.NotFound, message = "Data Tidak Ada" });
            }
            return Ok(new { status = HttpStatusCode.OK, result = get, message = "Data Berhasil ditampilkan" });
        }*/
        [HttpGet("Profile/{Key}")]
        public ActionResult<LoginVM> GetProfile(LoginVM loginVM)
        {
            var get = accountRepository.GetProfile(loginVM);
            if (get == null)
            {
                return NotFound(new { status = HttpStatusCode.NotFound, message = "Data Tidak Ada" });
            }
            return Ok(new { status = HttpStatusCode.OK, result = get, message = "Data Berhasil ditampilkan" });
        }
        [HttpPost("ForgetPassword")]
        public ActionResult ForgetCurrentPassword(ForgetPasswordVM forgetPasswordVM)
        {
            var pass = accountRepository.ForgetPassword(forgetPasswordVM);

            if (pass == 1)
            {
                return Ok(new { status = HttpStatusCode.OK, result = pass, message = "Email Recovery Password Berhasil dikirim!" });
            }
            else if (pass == 2)
            {
                return BadRequest(new { status = HttpStatusCode.BadRequest, message = "Email Recovery Gagal Dikirim, Password gagal diganti" });
            }
            else if (pass == 3)
            {
                return BadRequest(new { status = HttpStatusCode.BadRequest, message = "Akun tidak ditemukan, Password gagal diganti" });
            }
            return BadRequest(new { status = HttpStatusCode.BadRequest, message = "Gagal Forget Password" });
        }
        [HttpPut("ChangePassword")]
        public ActionResult ChangeCurrentPassword(ChangePasswordVM changePasswordVM)
        {
            var pass = accountRepository.ChangePassword(changePasswordVM);
            if (pass == 1)
            {
                return Ok(new { status = HttpStatusCode.OK, result = pass, message = "Password Berhasil Diganti" });
            }
            else if (pass == 2)
            {
                return BadRequest(new { status = HttpStatusCode.BadRequest, message = "Konfirmasi password salah, Password gagal diganti" });
            }
            else if (pass == 3)
            {
                return BadRequest(new { status = HttpStatusCode.BadRequest, message = "Akun gagal ditemukan, Password gagal diganti" });
            }
            else
            {
                return BadRequest(new { status = HttpStatusCode.BadRequest, message = "Password Gagal diganti" });
            }
        }
    }
}
