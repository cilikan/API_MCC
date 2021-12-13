using API.Models;
using API.ViewModel;
using Client.Base.Controllers;
using Client.Repositories.Data;
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
            return View();
        }
        [HttpPost]
        public JsonResult PostLogin(LoginVM loginVM)
        {
            var result = accountRepository.PostLogin(loginVM);
            return Json(result);
        }
    }
}
