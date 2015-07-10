using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Runtime.Serialization;

namespace SocialPackage.Infrastructure.Entities
{
    public class User : IPrincipal
    {
        [Key]
        public int id { get; set; }

        [Required]
        [Display(Name = "ФИО")]
        public string FullName { get; set; }

        [Display(Name = "E-mail")]
        [EmailAddress(ErrorMessage = "Введите корректный email адрес")]
        public string Email { get; set; }

        public string Login { get; set; }

        [Display(Name = "Роли")]
        public virtual List<UserRole> Roles { get; set; }

        [Display(Name = "Лимит")]
        public virtual Limit Limit { get; set; }


        public User()
        { }

        public User(IPrincipal user)
        {
            this.Identity = user.Identity;
        }


        public bool IsInRole(string role)
        {
            return this.Roles != null && Roles.Count > 0 && Roles.Any(r => r.RoleName.Equals(role));
        }

        [IgnoreDataMember]
        public IIdentity Identity { get; set; }
    }
}