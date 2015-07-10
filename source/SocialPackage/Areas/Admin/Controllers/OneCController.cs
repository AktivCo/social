using AdUsers;
using SocialPackage.Areas.Admin.Models;
using SocialPackage.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SocialPackage.Areas.Admin.Controllers
{
    [AllowAnonymous]
    public class OneCController : BaseApiController
    {
        private EfDbContext db;
        private AdUserProvider provider;
        public OneCController()
        {
            db = new EfDbContext();
            provider = new AdUserProvider(db.Settings.First().Domain);
        }
        [HttpPost]
        [Route("/api/onec/forpay/")]
        public List<BidForPay> ForPay(string login, string password, DateTime Date)
        {

            var user = provider.GetUserByLogin(login);
            //var date = DateTime.ParseExact(Date, "MM-yyyy", CultureInfo.InvariantCulture);
            if (provider.Validate(login, password) && db.Users.Any(el => el.Login == user.Sid && el.Roles.Any(r => r.RoleName == "Бухгалтер" || r.RoleName == "Администратор")))
            {
                Configuration.Formatters.XmlFormatter.UseXmlSerializer = true;
                var bids = db.Bids.Where(el => el.Date <= Date && el.Status == SocialPackage.Infrastructure.Entities.BidStatus.Approved).GroupBy(el => el.BidUser.FullName).ToArray()
                    .Select(el => new BidForPay(el.Key, string.Join(",", el.Select(b => b.id)), el.Sum(s => s.Summ))).ToList();



                return bids;
            }
            throw new HttpResponseException(System.Net.HttpStatusCode.Unauthorized);
        }

        [HttpPost]
        public bool Pay(string login, string password, string bids)
        {
            var user = provider.GetUserByLogin(login);
            if (provider.Validate(login, password) && db.Users.Any(el => el.Login == user.Sid && el.Roles.Any(r => r.RoleName == "Бухгалтер" || r.RoleName == "Администратор")))
            {
                var bidsForPay = bids.Split(',');
                var dbBids = db.Bids.Where(el => bidsForPay.Contains(el.id.ToString()) && el.Status == SocialPackage.Infrastructure.Entities.BidStatus.Approved).ToArray();
                foreach (var bid in dbBids)
                    bid.Status = SocialPackage.Infrastructure.Entities.BidStatus.Paid;
                db.SaveChanges();
                return dbBids.Count() == bidsForPay.Count();
            }
            throw new HttpResponseException(System.Net.HttpStatusCode.Unauthorized);
        }
        

    }
}
