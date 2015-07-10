using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SocialPackage.Infrastructure.Entities
{
    public class Bid
    {
        [Key]
        public int id { get; set; }

        public virtual Category category { get; set; }

        public DateTime Date { get; set; }

        public BidStatus Status { get; set; }

        public string Comment { get; set; }

        [Required]
        public string ImageUrl { get; set; }
        [Required]
        [Range(0.0, Double.MaxValue)]
        public decimal Summ { get; set; }

        public virtual User BidUser { get; set; }

        public virtual User BidSender { get; set; }


        public bool FixBid { get; set; }

        //отправитель
    }
}