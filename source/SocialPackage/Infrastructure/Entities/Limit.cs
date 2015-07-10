using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SocialPackage.Infrastructure.Entities
{
    public class Limit
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Поле не может быть пустым")]
        [Display(Name = "Название")]
        public string Name { get; set; }

        [Display(Name = "Проездные")]
        [Required(ErrorMessage = "Поле не может быть пустым")]
        [Range(-1, 9999999999999999.00)]
        public decimal Proezd { get; set; }


        [Display(Name = "Фитнес")]
        [Required(ErrorMessage = "Поле не может быть пустым")]
        [Range(-1, 9999999999999999.00)]
        public decimal Fitnes { get; set; }

        [Display(Name = "Соц. Мероприятия")]
        [Required(ErrorMessage = "Поле не может быть пустым")]
        [Range(-1, 9999999999999999.00)]
        public decimal CultureEvents { get; set; }
    }
}