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
    [Authorize(Roles = "Администратор")]
    public class StartSettingsController : Controller
    {
        private EfDbContext dbContext = new EfDbContext();

        // GET: Admin/StartSettings
        public ActionResult Index()
        {
            if (dbContext.Settings.Count() != 0 && dbContext.Users.Count() != 0 &&
                dbContext.Users.Any(c => c.Roles.Any(r => r.RoleName == "Администратор")))
                return RedirectToAction("Index", "Settings");
            return View();
        }

        [HttpPost]
        public ActionResult Index(SettingsViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            //context.Categories.RemoveRange(context.Categories);
            //context.Limit.RemoveRange(context.Limit);
            //var categories = new List<Category> {
            //    new Category { Color = "#755bee", Name="Проездные"},
            //    new Category { Color = "#3ec172", Name="Фитнес"},
            //    new Category { Color = "#e73f72", Name="Соц. Мероприятия"}
            //};
            Limit firstUserLimit;
            if (dbContext.Limit.Count() == 0)
            {
                firstUserLimit = new Limit
                {
                    CultureEvents = -1,
                    Fitnes = -1,
                    Proezd = -1,
                    Name = "Пустой лимит"
                };
                dbContext.Limit.Add(firstUserLimit);
            }
            else
                firstUserLimit = dbContext.Limit.First();

            if (dbContext.Categories.Count() == 0)
            {
                var categories = new List<Category> {
                    new Category { Color = "#755bee", Name="Проездные"},
                    new Category { Color = "#3ec172", Name="Фитнес"},
                    new Category { Color = "#e73f72", Name="Соц. Мероприятия"}
                };
                dbContext.Categories.AddRange(categories);
            }

            List<UserRole> roles;
            if (dbContext.UserRole.Count() == 0)
            {
                roles = new List<UserRole> {
                    new UserRole { RoleName = "Пользователь"},
                    new UserRole { RoleName = "Администратор"},
                    new UserRole { RoleName = "Бухгалтер"},
                    new UserRole { RoleName = "Секретарь"}
                };
                dbContext.UserRole.AddRange(roles);
            }
            else
                roles = dbContext.UserRole.ToList();

            

            var user = new User
            {
                FullName = model.User.FullName,
                Email = model.User.Email,
                Login = model.User.Login,
                Roles = roles.Where(c => c.RoleName == "Администратор").ToList(),
                Limit = firstUserLimit
            };

            dbContext.Users.Add(user);
            dbContext.SaveChanges();
            Session.Abandon();
            return RedirectToAction("Index", "Users");
        }
    }
}