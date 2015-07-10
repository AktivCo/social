using SocialPackage.Filters;
using SocialPackage.Infrastructure;
using SocialPackage.Infrastructure.Entities;
using SocialPackage.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace SocialPackage.Controllers
{

    public class HomeController : BaseController
    {
        EfDbContext db = new EfDbContext();
        [Authorize]
        public ActionResult Index()
        {
            return View(db.Bids.Where(el => el.BidUser.id == User.id).ToList());
        }


        public ActionResult Login()
        {
            return View();
        }

        

        [System.Web.Mvc.Authorize(Roles = "Администратор,Секретарь")]
        public ActionResult SendedBids()
        {
            return View(db.Bids.Where(el => el.BidSender.id == User.id && el.BidUser.id != User.id).ToList());
        }

        [HttpPost]
        public dynamic AddNewBid(BidViewModel Bid)
        {

            if (ModelState.IsValid)
            {

                if (Bid.ForUserId != User.id && !(User.IsInRole("Секретарь") || User.IsInRole("Администратор")))
                {
                    var error = new { message = "No Rights" };
                    Response.StatusCode = 401;
                    return Json(error, JsonRequestBehavior.AllowGet);
                }
                var date = DateTime.Parse(Bid.Date);
                if (date < new BidsController().StartMonth() || date.Year > (DateTime.Now.Year + 1))
                {
                    var error = new { message = "BadDate" };
                    //Response.StatusCode = 500;
                    return Json(error, JsonRequestBehavior.AllowGet);
                }
                //int sum;
                //if (!int.TryParse(Bid.Sum, out sum))
                //{ 
                //    var error = new {message = "BadString"};
                //    Response.StatusCode = 500;
                //    return Json(error, JsonRequestBehavior.AllowGet);
                //}
                var forUser = db.Users.Find(Bid.ForUserId);

                var availLimit = getAvaliableLimits(forUser, Bid.CategoryId, date.Year);
                if (availLimit == 0 || (Bid.Sum > availLimit && availLimit != -1))
                {
                    var error = new { message = "No Limit", availLimit = availLimit };
                    // Response.StatusCode = 500;
                    return Json(error, JsonRequestBehavior.AllowGet);
                }

                if (Bid.file != null)
                {

                    string fileName = Guid.NewGuid() + System.IO.Path.GetExtension(Bid.file.FileName);

                    string path = System.IO.Path.Combine(Server.MapPath("~/bid_images"), fileName);
                    //string path = System.IO.Path.Combine(@"D:\projects\SocialPackageGit\SocialPackage\SocialPackage\bid_images", fileName);
                    while (System.IO.File.Exists(path))
                        // path = System.IO.Path.Combine(@"D:\projects\SocialPackageGit\SocialPackage\SocialPackage\bid_images", Guid.NewGuid() + System.IO.Path.GetExtension(Bid.file.FileName));
                        path = System.IO.Path.Combine(Server.MapPath("~/bid_images"), Guid.NewGuid() + System.IO.Path.GetExtension(Bid.file.FileName));

                    Bid.file.SaveAs(path);


                    var bid = new Bid()
                    {
                        BidUser = forUser,
                        category = db.Categories.Find(Bid.CategoryId),
                        Date = DateTime.Parse(Bid.Date),
                        Status = BidStatus.Uploaded,
                        ImageUrl = System.IO.Path.GetFileName(path),
                        Summ = Bid.Sum,
                        BidSender = db.Users.Find(User.id)
                    };
                    db.Bids.Add(bid);
                    db.SaveChanges();
                    Task.Run(() => new MailController(db).SendUpload(bid).Deliver());
                }

                return Json(new { message = "OK" }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var error = new { message = "BadString" };
                //Response.StatusCode = 500;
                return Json(error, JsonRequestBehavior.AllowGet);

            }
        }


        private decimal getAvaliableLimits(User user, int categoryId, int year)
        {
            var userLimitsInCurrYear = db.Bids.Where(el => el.BidUser.id == user.id && el.Date.Year == year);

            decimal limit = 0;
            switch (categoryId)
            {
                case 1:
                    limit = user.Limit.Proezd > 0 ? user.Limit.Proezd - userLimitsInCurrYear.Where(el => el.category.id == 1 && el.Status != BidStatus.Declined).ToArray().Sum(el => el.Summ) : user.Limit.Proezd;
                    break;
                case 3:
                    limit = user.Limit.CultureEvents > 0 ? user.Limit.CultureEvents - userLimitsInCurrYear.Where(el => el.category.id == 3 && el.Status != BidStatus.Declined).ToArray().Sum(el => el.Summ) : user.Limit.CultureEvents;
                    break;
                case 2:
                    limit = user.Limit.Fitnes > 0 ? user.Limit.Fitnes - userLimitsInCurrYear.Where(el => el.category.id == 2 && el.Status != BidStatus.Declined).ToArray().Sum(el => el.Summ) : user.Limit.Fitnes;
                    break;
            }
            return limit;

        }


        [HttpGet]
        public ActionResult SettingsInit()
        {

            return View();
        }
    }

}