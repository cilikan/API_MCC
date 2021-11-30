using API.Context;
using API.Models;
using API.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Repository.Data
{
    public class AccountRoleRepository : GeneralRepository<MyContext, AccountRole, int>
    {
        private readonly MyContext context;
        public AccountRoleRepository(MyContext myContext) : base(myContext)
        {
            this.context = myContext;
        }
        /*public int Sign(string key)
        {
            var search = from e in context.Accounts
                         where e.NIK == key
                         join a in context.AccountRoles on e.NIK equals a.AccountNIK
                         join b in context.Roles on a.AccountRoleId equals b.RoleId
                         select new
                         {
                             NIK = e.NIK,
                             RoleId = b.RoleId
                         };
            return ;
        }*/
        /*public int SignManager(string key, string key2)
        {
            var checkNIK = context.Accounts.Where(x => x.NIK == key).FirstOrDefault();
            if (checkNIK != null)
            {
                AccountRole accountRole = new AccountRole()
                {
                    AccountNIK = key,
                    RoleId = key2
                };
                context.AccountRoles.Add(accountRole);
                context.SaveChanges();
                return 1;
            }
            return 0;
        }*/
        /*public int Sign(string key, int key2)
        {
            var checkNIK = context.Accounts.Where(x => x.NIK == key).FirstOrDefault();
            if (checkNIK == null)
            {
                return 0;
            }
            AccountRole accountRole = new AccountRole()
            {
                AccountNIK = key,
                RoleId = key2
            };
            context.AccountRoles.Add(accountRole);
            context.SaveChanges();
            return 1;
        }*/
        public int SignManager(SignManagerVM signManagerVM)
        {
            var checkNIK = context.Accounts.Find(signManagerVM.NIK);
            if (checkNIK == null)
            {
                return 0;
            }
            AccountRole accountRole = new AccountRole()
            {
                AccountNIK = signManagerVM.NIK,
                RoleId = signManagerVM.RoleId
            };
            context.AccountRoles.Add(accountRole);
            context.SaveChanges();
            return 1;
        }
    }
}
