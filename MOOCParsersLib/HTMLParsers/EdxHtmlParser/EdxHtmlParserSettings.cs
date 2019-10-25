using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MOOCParsersLib.HTMLParsers.EdxHtmlParser
{
    class EdxHtmlParserSettings : IHtmlParserSettings
    {
        public string BaseUrl { get; set; } = "https://www.edx.org/course";
        public string Prefix { get; set; } = "";

        public EdxHtmlParserSettings(string prefix)
        {
            Prefix = prefix;
        }
    }
}
