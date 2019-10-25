using AngleSharp.Html.Dom;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MOOCParsersLib.HTMLParsers
{
    public interface IHtmlParser<T> where T : class
    {
        Task<T> Parse(IHtmlDocument site);
    }
}