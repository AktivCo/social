using SocialPackage.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http.Filters;
using System.Web;
using System.Net.Http;
using System.Web.Mvc;
using System.Web.Routing;
using SocialPackage.Controllers;
using SocialPackage.Areas.Admin;
using SocialPackage.Areas.Admin.Controllers;

namespace SocialPackage.Filters
{
    public class StartupActionFilter : System.Web.Mvc.ActionFilterAttribute
    {
        EfDbContext dbContext = new EfDbContext();
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (dbContext.Users.Count() == 0 || dbContext.Settings.Count() == 0 || !(dbContext.Users.Where(c => c.Roles.Exists(r => r.RoleName == "Администратор")) != null))
            {
                if (filterContext.Controller.GetType() != typeof(SettingsController))
                {
                    var controller = (BaseController)filterContext.Controller;
                    filterContext.Result = controller.RedirectToAction();
                }

            }

            base.OnActionExecuting(filterContext);
        }


    }

}