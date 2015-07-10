using ActionMailer.Net.Mvc;
using SocialPackage.Code;
using SocialPackage.Infrastructure;
using SocialPackage.Infrastructure.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SocialPackage.Controllers
{
    public class MailController : MailerBase
    {
        private EfDbContext dbContext;
        public MailController(EfDbContext dbContext)
            : base(new SocialPackageMailSender(new SocialPackageSmtpClient(dbContext)))
        {
            this.dbContext = dbContext;
        }


        public EmailResult SendUpload(Bid bid)
        {
            var states = new Dictionary<int, string>()
            {
                {0, "загружена"},
                {1, "утверждена"},
                {3, "отклонена"}
            };
            From = dbContext.Settings.First().ServiceEmailUser;
            var mailTo = dbContext.Settings.First().Email;
            if (!string.IsNullOrEmpty(mailTo))
                To.Add(mailTo);
            if (bid.Status != BidStatus.Uploaded)
                To.Add(bid.BidUser.Email);

            Subject = string.Format("{0} - Заявка {1}", bid.BidUser.FullName, states[(int)bid.Status]);
            var fullPath = System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath + @"\bid_images\" + bid.ImageUrl;
            byte[] img = System.IO.File.ReadAllBytes(fullPath);
            Attachments.Add("ticket" + Path.GetExtension(fullPath), img);
            return Email("SendBid", bid);
        }
    }
}
