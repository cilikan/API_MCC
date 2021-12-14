using API.Models;
using API.ViewModel;
using Client.Base.Controllers;
using Client.Repositories.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Client.Controllers
{
    public class AccountsController : BaseController<Account, AccountRepository, string>
    {
        private AccountRepository accountRepository;
        public AccountsController(AccountRepository repository) : base(repository)
        {
            this.accountRepository = repository;
        }

        public IActionResult Index()
        {
            return View("Login");
        }
        /*[HttpPost]
        public JsonResult PostLogin(LoginVM loginVM)
        {
            var result = accountRepository.PostLogin(loginVM);
            return Json(result);
        }*/
        [HttpPost]
        public async Task<IActionResult> Auth(LoginVM login)
        {
            var jwtToken = await accountRepository.Auth(login);
            var token = jwtToken.Token;

            if (token == null)
            {
                //return RedirectToAction("Dashboard", "Employees");
                return Json(Url.Action("Index", "Accounts"));
            }

            HttpContext.Session.SetString("JWToken", token);
            //HttpContext.Session.SetString("Name", jwtHandler.GetName(token));
            //HttpContext.Session.SetString("ProfilePicture", "assets/img/theme/user.png");   
            return Json(Url.Action("Index", "Employees"));
            //return RedirectToAction("Dashboard", "Employees");
        }
        
        public IActionResult Logout()
        {
            var sessionData = HttpContext.Session.GetString("JWToken");

            if (sessionData != null)
            {
                HttpContext.Session.Clear();
                return RedirectToAction("Index", "Accounts");
            }

            return RedirectToAction("Index", "Employees");
        }
    }
}
