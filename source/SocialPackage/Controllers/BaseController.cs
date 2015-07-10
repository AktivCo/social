using SocialPackage.Filters;
using SocialPackage.Infrastructure.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SocialPackage.Controllers
{
    [StartupActionFilter]
    public class BaseController : Controller
    {
        // GET: Base
        protected virtual new User User
        {
            get { return HttpContext.User as User; }
        }
        public RedirectToRouteResult RedirectToAction()
        {
            return base.RedirectToAction("Index", "StartSettings", new { area = "Admin" });
        }
    }
}