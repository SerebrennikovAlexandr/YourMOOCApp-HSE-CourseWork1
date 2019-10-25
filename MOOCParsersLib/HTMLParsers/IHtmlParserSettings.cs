using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MOOCParsersLib.HTMLParsers
{
    public interface IHtmlParserSettings
    {
        string BaseUrl { get; set; }
        string Prefix { get; set; }
    }
}
