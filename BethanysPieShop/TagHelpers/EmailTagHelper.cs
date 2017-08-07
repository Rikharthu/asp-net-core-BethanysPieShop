using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BethanysPieShop.TagHelpers
{
    // Will be a tag helper as such: <email>
    public class EmailTagHelper : TagHelper
    {
        // <email address="vasja@pupkin.com" content="contact us"></email>
        public string Address { get; set; }
        public string Content { get; set; }

        // TagHelper needs to override a Process() method
        // This method gets invoked when TagHelper is displayed
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            // configure the output
            output.TagName = "a";
            output.Attributes.SetAttribute("href", "mailto:" + Address);
            output.Content.SetContent(Content); // content goes between opening and closing tag, thus <email/> wont work
            // <email address="info@@bethanypieshop.com" content="Contact us"></email>
            // Output: <a href="mailto:info@bethanypieshop.com">Contact us</a>
        }
    }
}
