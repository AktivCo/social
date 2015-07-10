using SocialPackage.Infrastructure;
using SocialPackage.Infrastructure.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SocialPackage.Areas.Admin.Controllers
{
    public class CategoriesController : BaseAdminController
    {
        EfDbContext db = new EfDbContext();
        //
        // GET: /Admin/Categories/

        public ActionResult Index()
        {
            var allCategories = db.Categories.ToList();
            return View(allCategories);
        }

        public ActionResult AddCategory()
        {
            return View(new Category());
        }

        public ActionResult Edit(int id)
        {
            var cat = db.Categories.Find(id);
            if (cat == null)
                return HttpNotFound();
            return View(cat);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Category cat)
        {
            if (ModelState.IsValid)
            {
                db.Entry(cat).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(cat);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddCategory(Category cat)
        {
            if (ModelState.IsValid)
            {
                db.Categories.Add(cat);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(cat);

        }

    }
}