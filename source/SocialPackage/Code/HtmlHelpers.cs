using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace SocialPackage.Code
{
    public static class HtmlHelpers
    {
          public static MvcHtmlString CheckBoxList(this HtmlHelper helper, string name, IEnumerable<SelectListItem> items, string ngmodel)
        {
            var output = new StringBuilder();
            output.Append(@"<div cb-list-checker ng-model=""").Append(ngmodel).Append(@""" class=""checkboxList"">");

            foreach (var item in items)
            {
                output.Append(@"<input type=""checkbox"" name=""");
                output.Append(name);
                output.Append("\" value=\"");
                output.Append(item.Value);
                output.Append("\"");

                if (item.Selected)
                    output.Append(@" checked=""checked""");

                output.Append(" />");
                output.Append(item.Text);
                output.Append("<br />");
            }

            output.Append("</div>");

            return new MvcHtmlString(output.ToString());
        }
    
    }
}