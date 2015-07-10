using SocialPackage.Infrastructure.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SocialPackage.Areas.Admin.Models
{
    [NotMapped]
    public class UserViewModel : User
    {
        [Display(Name="Выберите лимит")]
        public List<Limit> AvailLimits { get; set; }

        public int SelectedLimitId { get; set; }


        public List<UserRole> allRoles { get; set; }
        public List<UserRole> currRoles { get; set; }

        [Display(Name = "Начальный лимит для Фитнеса")]
        public decimal realFitnesLimit { get; set; }
        [Display(Name = "Начальный лимит для Проезда")]
        public decimal realProezdLimit { get; set; }
        [Display(Name = "Начальный лимит для Соц.Мероприятий")]
        public decimal realCultureEvent { get; set; }

        public int[] SelectedRoles { get; set; }
       
    }
}