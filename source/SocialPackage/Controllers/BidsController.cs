using SocialPackage.Areas.Admin.Controllers;
using SocialPackage.Infrastructure;
using SocialPackage.Infrastructure.Entities;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace SocialPackage.Controllers
{
    public class BidsController : BaseApiController
    {
        EfDbContext db = new EfDbContext();

        [Authorize(Roles = "Администратор,Бухгалтер")]
        [HttpGet]
        [Route("api/bids/getbidsforapprove")]
        public IHttpActionResult GetBidsForApprove()
        {
            var model = db.Bids.Include("category").Where(el => el.Status == BidStatus.Uploaded && !el.FixBid).ToList();
            return Ok(model);
        }


        [Authorize(Roles = "Администратор,Бухгалтер")]
        [HttpPost]
        [Route("api/bids/getlastbids")]
        public IHttpActionResult GetLastBids(dynamic data)
        {
            var UserId = (int)data.UserId;
            var catId = (int)data.CategoryId;
            var model = db.Bids.Where(el => el.BidUser.id == UserId && el.category.id == catId && !el.FixBid).OrderByDescending(el => el.id).Take(5).ToList();
            return Ok(model);
        }


        [Authorize(Roles = "Администратор,Бухгалтер")]
        [HttpGet]
        [Route("api/bids/getbidsforpay/")]
        public List<Bid> GetBidsForPay(string data)
        {
            var date = DateTime.Parse(data);
            return db.Bids.Where(el => (el.Status == BidStatus.Approved || el.Status == BidStatus.Paid) && el.Date.Month == date.Month && el.Date.Year == date.Year && !el.FixBid).ToList();
        }


        [Authorize(Roles = "Администратор,Бухгалтер")]
        [HttpPost]
        [Route("api/bids/approve/")]
        public IHttpActionResult Approve(dynamic data)
        {

            var bid = db.Bids.Find((int)data.bid);
            bid.Status = BidStatus.Approved;
            bid.Comment = (string)data.comment;
            db.SaveChanges();
            Task.Run(() => new MailController(db).SendUpload(bid).Deliver());
            return Ok();
        }

        [Authorize(Roles = "Администратор,Бухгалтер")]
        [HttpPost]
        [Route("api/bids/pay/")]
        public IHttpActionResult Pay(int id)
        {
            var bid = db.Bids.Find(id);
            bid.Status = BidStatus.Paid;
            db.SaveChanges();
            return Ok();
        }

        [Authorize(Roles = "Администратор,Бухгалтер")]
        [HttpPost]
        [Route("api/bids/withdraw/")]
        public IHttpActionResult Withdraw(dynamic data)
        {
            var bid = db.Bids.Find((int)data.bid.id);
            bid.Status = BidStatus.Declined;
            bid.Comment = (string)data.bid.Comment;
            db.SaveChanges();
            Task.Run(() => new MailController(db).SendUpload(bid).Deliver());
            return Ok();
        }


        [HttpGet]
        [Route("api/bids/delete/{id}")]
        public HttpResponseMessage Delete(int id)
        {
            var bid = db.Bids.Find(id);
            if (bid.Status == BidStatus.Uploaded && (bid.BidUser.id == User.id || bid.BidSender.id == User.id))
            {
                db.Bids.Remove(bid);
                db.SaveChanges();
            }
            return new HttpResponseMessage(System.Net.HttpStatusCode.OK);
        }


        [HttpGet]
        [Route("api/bids/startmonth")]
        public DateTime StartMonth()
        {
            var now = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            var last = db.Bids.Where(b => b.Status == BidStatus.Paid && !b.FixBid).OrderByDescending(el => el.Date).FirstOrDefault();
            return last != null ? last.Date.AddMonths(1) : now; ;

        }

        [HttpGet]
        [Route("api/bids/getsettings")]
        public Settings GetSettings()
        {
            try
            {
                return db.Settings.First();
            }
            catch
            {
                return null;
            }

        }
    }
}
