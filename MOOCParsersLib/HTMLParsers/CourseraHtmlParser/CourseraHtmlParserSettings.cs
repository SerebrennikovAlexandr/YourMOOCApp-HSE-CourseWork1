using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MOOCParsersLib.HTMLParsers.CourseraHtmlParser
{
    public class CourseraHtmlParserSettings : IHtmlParserSettings
    {
        public string BaseUrl { get; set; } = "https://www.coursera.org/";
        public string Prefix { get; set; } = "";

        public CourseraHtmlParserSettings(string prefix)
        {
            Prefix = prefix;
        }
    }
}
