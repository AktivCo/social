using SocialPackage.Infrastructure;
using SocialPackage.Infrastructure.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SocialPackage.Areas.Admin.Controllers
{
    public class LimitsController : BaseAdminController
    {
        EfDbContext db = new EfDbContext();
        //
        // GET: /Admin/Limits/

        public ActionResult Index()
        {
            return View(db.Limit.ToList());
        }

        public ActionResult AddLimit()
        {
            return View(new Limit());
        }


        public ActionResult Edit(int id)
        {
            var limit = db.Limit.Find(id);
            if (limit == null)
                return HttpNotFound();
            return View(limit);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Limit limit)
        {
            if (ModelState.IsValid)
            {
                db.Entry(limit).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(limit);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddLimit(Limit limit)
        {
            if (ModelState.IsValid)
            {
                db.Limit.Add(limit);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(limit);
        }


    }
}