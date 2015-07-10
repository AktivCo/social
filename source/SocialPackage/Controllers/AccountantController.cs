using SocialPackage.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SocialPackage.Controllers
{
    [Authorize(Roles = "Бухгалтер,Администратор")]
    public class AccountantController : BaseController
    {
        EfDbContext db = new EfDbContext();
        //
        // GET: /Accountant/


        public ActionResult Index()
        {
            //var Bids = db.Bids.Where(el => el.Status == Models.BidStatus.Uploaded || el.Status == Models.BidStatus.Approved).ToList();
            return View();
        }

        public ActionResult Pay()
        {
            return View();
        }
        public FileResult Download()
        {
            var fullPath = System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath + @"\bid_images\epf\Social.epf";
            byte[] fileBytes = System.IO.File.ReadAllBytes(fullPath);
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, "Обработка1с.epf");
        }

    }
}