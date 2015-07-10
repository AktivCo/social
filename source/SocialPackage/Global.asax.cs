using SocialPackage.Code;
using SocialPackage.Infrastructure;
using SocialPackage.Infrastructure.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;


namespace SocialPackage
{
    // Примечание: Инструкции по включению классического режима IIS6 или IIS7 
    // см. по ссылке http://go.microsoft.com/?LinkId=9394801

    public class WebApiApplication : System.Web.HttpApplication
    {
        
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            ModelBinders.Binders.Add(typeof(decimal), new DecimalModelBinder());
            
        }






        protected void Application_PostAuthenticateRequest(Object sender, EventArgs e)
        {
            var cu = Context.User;
            EfDbContext db = new EfDbContext();
            if (db.Users.Count() != 0 && db.Users.Any(c => c.Roles.Any(r => r.RoleName == "Администратор")))
            {

                string currentSid = null;
                var identity = Context.User.Identity as WindowsIdentity;
                if (identity.User != null) currentSid = identity.User.Value;

                
                SocialPackage.Infrastructure.Entities.User usr = db.Users.Include("Limit").FirstOrDefault(el => el.Login == currentSid);
                if (usr != null)
                {
                    usr.Identity = identity;
                    Context.User = usr;
                    //throw new MembershipCreateUserException("заведите пользователя в базе данных");
                }
                

            }
            else
            {
                var user = new User
                {
                    Roles = new List<UserRole>
                    {
                        new UserRole
                        { RoleName = "Администратор" }
                    }
                };

                user.Identity = Context.User.Identity as WindowsIdentity;
                Context.User = user;
            }

        }


        //protected void Application_PostAuthenticateRequest(Object sender, EventArgs e)
        //{
        //    string currentSid = null;
        //    var identity = Context.User.Identity as WindowsIdentity;
        //    if (identity.User != null) currentSid = identity.User.Value;

        //    EfDbContext db = new EfDbContext();
        //    SocialPackage.Infrastructure.Entities.User usr = db.Users.Include("Limit").FirstOrDefault(el => el.Login == currentSid);
        //    if (usr != null)
        //    {
        //        usr.Identity = identity;
        //        Context.User = usr;
        //        //throw new MembershipCreateUserException("заведите пользователя в базе данных");
        //    }

        //}

        protected void Application_Error(object sendrer, EventArgs e)
        {
            var ex = Server.GetLastError();

        }
    }
}