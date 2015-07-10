using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SocialPackage.Infrastructure.Entities
{
    public class Settings
    {
        [Key]
        public int id { get; set; }
        public string Rules { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Контроллер домена")]
        public string Domain { get; set; }

        [Required]
        [Display(Name = "Адрес SMTP сервера")]
        public string Host { get; set; }

        [Required]
        [Display(Name = "Порт SMTP сервера")]
        public int Port { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Учетная запись почты")]
        public string ServiceEmailUser { get; set; }

        [Required]
        [Display(Name = "Пароль учетной записи почты")]
        public string ServiceEmailPassword { get; set; }

    }
}