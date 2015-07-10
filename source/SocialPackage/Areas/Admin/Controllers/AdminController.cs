using AdUsers;
using SocialPackage.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SocialPackage.Areas.Admin.Controllers
{
    public class AdminController : BaseApiController
    {
        EfDbContext db = new EfDbContext();
        public List<AdUser> GetDomainUsers()
        {
            var activeDirUsers = new AdUserProvider(db.Settings.First().Domain).GetAllUsers(); ;

            // activeDirUsers.RemoveAll(u => u.sAMAccountType != 805306368 || currentUsers.Any(usr => usr.Login.Remove(0, DOMAIN_PREFIX.Length) == u.Login));
            activeDirUsers.RemoveAll(u => u.sAMAccountType != 805306368);
            //!Regex.IsMatch(u.CN.Substring(0, 1), "[А-Яа-я]")

            return activeDirUsers.OrderBy(u => u.CN).ToList();

        }
    }
}
