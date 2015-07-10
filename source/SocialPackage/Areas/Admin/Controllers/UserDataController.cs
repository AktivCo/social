using SocialPackage.Infrastructure;
using SocialPackage.Infrastructure.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace SocialPackage.Areas.Admin.Controllers
{

    public class UserDataController : ApiController
    {
        EfDbContext db = new EfDbContext();
        //
        // GET: /Admin/UserData/

        [HttpGet]
        [Route("api/userdata/getcategories")]
        public IHttpActionResult GetCategories()
        {
            return Ok(db.Categories.ToList());
        }

        [HttpGet]
        [Route("api/userdata/getallusers")]
        [System.Web.Mvc.Authorize(Roles = "Администратор,Секретарь")]
        public List<User> GetAllUsers()
        {
            return db.Users.ToList();
        }

        [HttpGet]
        [System.Web.Mvc.Authorize(Roles = "Администратор,Секретарь")]
        [Route("api/userdata/getuserdata")]
        public dynamic GetUserData()
        {
            var b = HttpContext.Current.User as SocialPackage.Infrastructure.Entities.User;
            var user = db.Users.Find(b.id);

            var userLimitsInCurrYear = db.Bids.Where(el => el.BidUser.id == user.id && el.Date.Year == DateTime.Now.Year);

            var CurrentLimits = new Limit()
            {
                Proezd = user.Limit.Proezd > 0 ? user.Limit.Proezd - userLimitsInCurrYear.Where(el => el.category.id == 1 && el.Status != BidStatus.Declined).ToArray().Sum(el => el.Summ) : user.Limit.Proezd,
                CultureEvents = user.Limit.CultureEvents > 0 ? user.Limit.CultureEvents - userLimitsInCurrYear.Where(el => el.category.id == 3 && el.Status != BidStatus.Declined).ToArray().Sum(el => el.Summ) : user.Limit.CultureEvents,
                Fitnes = user.Limit.Fitnes > 0 ? user.Limit.Fitnes - userLimitsInCurrYear.Where(el => el.category.id == 2 && el.Status != BidStatus.Declined).ToArray().Sum(el => el.Summ) : user.Limit.Fitnes
            };

            return new { id = user.id, FullName = user.FullName, GeneralLimit = user.Limit, Limit = CurrentLimits, Roles = user.Roles.Select(el => el.RoleName).ToArray() };
        }


    }
}
