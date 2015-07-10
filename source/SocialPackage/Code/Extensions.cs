using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SocialPackage.Code
{
    public static class Extensions
    {
        public static string ToViewString(this DateTime month)
        {
            var monthNames = new string[] {"Январь", "Февраль", "Март", "Апрель", "Май", "Июнь",
        "Июль", "Август", "Сентябрь", "Октябрь", "Ноябрь", "Декабрь"};
            return string.Format("{0} {1}", monthNames[month.Month-1], month.Year);
        }
    }
}