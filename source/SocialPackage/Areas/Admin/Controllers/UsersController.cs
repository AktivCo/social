using SocialPackage.Areas.Admin.Models;
using SocialPackage.Infrastructure;
using SocialPackage.Infrastructure.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SocialPackage.Areas.Admin.Controllers
{
    public class UsersController : BaseAdminController
    {
        //
        // GET: /Admin/Users/
        EfDbContext db = new EfDbContext();

        public ActionResult Index()
        {
            var allUsrs = db.Users.ToList();
            return View(allUsrs);
        }

        public ActionResult AddUser()
        {
            var User = new UserViewModel()
            {
                AvailLimits = db.Limit.ToList(),
                allRoles = db.UserRole.ToList(),
                currRoles = new List<UserRole>(),
                SelectedRoles = new int[0]
            };

            // ViewBag.limits = db.Limit.Select(el => new { id = el.Id, values = string.Format("{0}/{1}/{2}", el.Fitnes, el.Proezd, el.CultureEvents) });
            return View(User);
        }

        public ActionResult Edit(int id)
        {
            var usr = db.Users.Find(id);
            if (usr == null)
                return HttpNotFound();
            var allLimits = db.Limit.ToList();
            var Roles = db.UserRole.ToList();

            var usrVm = new UserViewModel()
            {
                FullName = usr.FullName,
                Email = usr.Email,
                Limit = usr.Limit,
                Login = usr.Login,
                SelectedLimitId = usr.Limit != null ? usr.Limit.Id : allLimits.First().Id,
                AvailLimits = allLimits,

                allRoles = Roles,
                currRoles = usr.Roles,
                SelectedRoles = usr.Roles.Select(el => el.RoleId).ToArray()

            };
            return View(usrVm);

        }

        [HttpPost]
        public ActionResult Edit(UserViewModel user)
        {
            var usr = db.Users.Find(user.id);
            usr.Roles.Clear();
            usr.Roles = user.SelectedRoles.Select(el => db.UserRole.Find(el)).ToList();
            usr.Limit = db.Limit.Find(user.SelectedLimitId);
            usr.FullName = user.FullName;
            usr.Email = user.Email;


            db.Entry(usr).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult AddUser(UserViewModel user)
        {

            if (ModelState.IsValid)
            {
                var usr = new User()
                {
                    FullName = user.FullName,
                    Email = user.Email,
                    Login = user.Login,
                    Limit = db.Limit.Find(user.SelectedLimitId),
                    Roles = user.SelectedRoles.Select(el => db.UserRole.Find(el)).ToList()
                };

                db.Users.Add(usr);
                db.SaveChanges();
                FixBid(user.realProezdLimit, user.realFitnesLimit, user.realCultureEvent, usr);
                return RedirectToAction("Index");
            }
            return View(new UserViewModel()
            {
                AvailLimits = db.Limit.ToList()
            });
        }

        private void FixBid(decimal proezd, decimal fitnes, decimal soc, User user)
        {
            var now = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            if (user.Limit.Proezd > proezd)
                db.Bids.Add(new Bid() { BidUser = user, BidSender = user, category = db.Categories.Find(1), FixBid = true, Date = now, Status = BidStatus.Paid, Summ = user.Limit.Proezd - proezd, ImageUrl = "fix" });
            if (user.Limit.Fitnes > fitnes)
                db.Bids.Add(new Bid() { BidUser = user, BidSender = user, category = db.Categories.Find(2), FixBid = true, Date = now, Status = BidStatus.Paid, Summ = user.Limit.Fitnes - fitnes, ImageUrl = "fix" });
            if (user.Limit.CultureEvents > soc)
                db.Bids.Add(new Bid() { BidUser = user, BidSender = user, category = db.Categories.Find(3), FixBid = true, Date = now, Status = BidStatus.Paid, Summ = user.Limit.CultureEvents - soc, ImageUrl = "fix" });
            db.SaveChanges();
        }

    }
}