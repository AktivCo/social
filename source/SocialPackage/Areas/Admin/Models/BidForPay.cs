using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace SocialPackage.Areas.Admin.Models
{
    public class BidForPay
    {
        [XmlAttribute]
        public string UserName { get; set; }

        [XmlAttribute]
        public string Bids { get; set; }

        [XmlAttribute]
        public decimal Sum { get; set; }


        public BidForPay() { }
        public BidForPay(string userName, string bids, decimal sum)
        {
            UserName = userName;
            Bids = bids;
            Sum = sum;
        }
    }
}