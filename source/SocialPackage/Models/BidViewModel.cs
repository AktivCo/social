using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SocialPackage.Models
{
    [NotMapped]
    public class BidViewModel
    {
        [Range(0.0, Double.MaxValue)]
        public decimal Sum { get; set; }
        public string Date { get; set; }
        public int CategoryId { get; set; }
        public HttpPostedFileBase file { get; set; }

        public int ForUserId { get; set; }
    }
}