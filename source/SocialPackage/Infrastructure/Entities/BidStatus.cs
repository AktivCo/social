using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SocialPackage.Infrastructure.Entities
{
    public enum BidStatus
    {
        [Display(Name = "Загружен")]
        Uploaded = 0,
        [Display(Name = "Утвержден")]
        Approved = 1,
        [Display(Name = "Оплачен")]
        Paid = 2,
        [Display(Name = "Отклонен")]
        Declined = 3
    }
}