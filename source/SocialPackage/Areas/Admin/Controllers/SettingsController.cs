using SocialPackage.Areas.Admin.Controllers;
using SocialPackage.Controllers;
using SocialPackage.Infrastructure;
using SocialPackage.Infrastructure.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;

namespace SocialPackage.Areas.Admin
{

    public class SettingsController : BaseAdminController
    {
        EfDbContext db = new EfDbContext();
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public dynamic Save(Settings settings)
        {
            if (!ModelState.IsValid)
            {
                List<string> errors = new List<string>();
                foreach (ModelState modelState in ViewData.ModelState.Values)
                    errors.AddRange(modelState.Errors.Select(er => er.ErrorMessage));
                Response.StatusCode = 400;
                return Json(errors);
            }

            if (db.Settings.Count() == 0)
            {
                db.Settings.Add(settings);
            }
            else
            {
                db.Entry(settings).State = System.Data.Entity.EntityState.Modified;
            }

            db.SaveChanges();
            return Json(settings);

        }

    }
}