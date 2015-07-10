using SocialPackage.Controllers;
using SocialPackage.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SocialPackage.Areas.Admin.Controllers
{
    [Authorize(Roles = "Администратор")]
    public class BaseAdminController : BaseController { }
}