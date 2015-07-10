using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace SocialPackage.Infrastructure.Entities
{
    [DataContract]
    public class UserRole
    {
        [DataMember]
        [Display(Name = "Id")]
        [Key]
        public int RoleId { get; set; }

        [DataMember]
        [Display(Name = "Наименование")]
        [Required]
        public string RoleName { get; set; }

        [IgnoreDataMember]
        public virtual List<User> Users { get; set; }
    }
}