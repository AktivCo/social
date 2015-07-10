using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SocialPackage.Infrastructure.Entities
{
    public class Category
    {
        [Key]
        public int id { get; set; }

        [Display(Name = "Название")]
        [Required(ErrorMessage = "Поле не может быть пустым")]
        public string Name { get; set; }

        [Display(Name = "Цвет")]
        [Required(ErrorMessage = "Поле не может быть пустым")]
        public string Color { get; set; }
    }
}