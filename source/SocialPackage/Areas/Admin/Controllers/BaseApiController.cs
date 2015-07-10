using SocialPackage.Infrastructure.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace SocialPackage.Areas.Admin.Controllers
{
    public class BaseApiController : ApiController
    {
        protected virtual new User User
        {
            get { return HttpContext.Current.User as User; }
        }
    }
}
