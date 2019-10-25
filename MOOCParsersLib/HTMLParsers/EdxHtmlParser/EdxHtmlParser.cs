using AngleSharp;
using AngleSharp.Html.Dom;
using AngleSharp.Html.Parser;
using System;
using System.Collections.Generic;

using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MOOCParsersLib.HTMLParsers.EdxHtmlParser
{
    class EdxHtmlParser : IHtmlParser<List<Course>>
    {
        public async Task<List<Course>> Parse(IHtmlDocument site)
        {
            List<Course> list = new List<Course>();
            HtmlParser domParser = new HtmlParser();
            
            var courseNodes = site.QuerySelectorAll("div.discovery-card").
                Where(item => item.ClassName == "discovery-card course-card shadow verified");

            if (courseNodes != null)
            {
                foreach (var node in courseNodes)
                {
                    string nodeHtml = node.ToHtml();
                    var document = domParser.ParseDocument(nodeHtml);

                    var item = document.QuerySelector("a.course-link");
                    if (item != null && item.Attributes["href"].Value.Trim() != null)
                    {
                        Course course = new Course("Edx Course",
                                            "Edx Platform",
                                            item.Attributes["href"].Value.Trim(),
                                            Color.LightYellow);

                        var item1 = document.QuerySelector("div.label");
                        var item2 = document.QuerySelector("div.img-wrapper img");
                        var item3 = document.QuerySelector("h3.title-heading");

                        if (item1 != null && item1.TextContent.Trim() != null && item1.TextContent.Trim() != "")
                            course.Source = "Edx Platform. Course by " + item1.TextContent.Trim();
                        if (item2 != null && item2.Attributes["src"].Value.Trim() != null)
                            course.Picture = new UriImageSource
                            {
                                CachingEnabled = true,
                                Uri = new Uri(item2.Attributes["src"].Value.Trim())
                            };
                        if (item3 != null && item3.TextContent.Trim() != null && item3.TextContent.Trim() != "")
                            course.Name = item.TextContent.Trim();

                        list.Add(course);
                    }
                }
            }
            return list;
        }
    }
}
