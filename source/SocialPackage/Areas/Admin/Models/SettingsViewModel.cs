using SocialPackage.Infrastructure.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SocialPackage.Areas.Admin.Models
{
    public class SettingsViewModel
    {
        public bool IsStart { get; set; }
        public User User { get; set; }
    }
}