using AngleSharp;
using AngleSharp.Html.Dom;
using AngleSharp.Html.Parser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MOOCParsersLib.HTMLParsers.CourseraHtmlParser
{
    public class CourseraHtmlParser : IHtmlParser<List<Course>>
    {
        public async Task<List<Course>> Parse(IHtmlDocument site)
        {
            List<Course> list = new List<Course>();

            HtmlParser domParser = new HtmlParser();

            int courseAmount = 7;
    
            var courseNodes = site.QuerySelectorAll("a.rc-DesktopSearchCard").Where(item => item.ClassName == "rc-DesktopSearchCard anchor-wrapper");
            if (courseNodes != null)
            {
                foreach (var node in courseNodes)
                {

                    if (node != null && node.Attributes["href"].Value != null && node.Attributes["href"].Value != "")
                    {
                        var href = node.Attributes["href"].Value.Trim();
                        Course course = new Course("Курс на Coursera",
                                            "Coursera",
                                            "https://www.coursera.org" + href,
                                            Color.LightSkyBlue);

                        string nodeHtml = node.ToHtml();
                        var document = domParser.ParseDocument(nodeHtml);

                        var name = document.QuerySelector("h2.color-primary-text.card-title.headline-1-text");
                        var partner = document.QuerySelector("span.partner-name");
                        var enrollment = document.QuerySelector("span.enrollment-number");
                        var rating = document.QuerySelector("span.ratings-text");
                        var pict = document.QuerySelector("img.product-photo");

                        if (name != null && name.TextContent.Trim() != null && name.TextContent.Trim() != "")
                            course.Name = name.TextContent.Trim();
                        if (partner != null && partner.TextContent.Trim() != null && partner.TextContent.Trim() != "")
                            course.Source = "Coursera Platform. Course by " + partner.TextContent.Trim();
                        if (pict != null && pict.Attributes["src"].Value.Trim() != null)
                            course.Picture = new UriImageSource
                            {
                                CachingEnabled = true,
                                Uri = new Uri(pict.Attributes["src"].Value.Trim())
                            };
                        if (rating != null && rating.TextContent.Trim() != null && rating.TextContent.Trim() != "")
                            course.Rating = rating.TextContent.Trim();
                        if (enrollment != null && enrollment.TextContent.Trim() != null && enrollment.TextContent.Trim() != "")
                            course.EnrolledPeopleAmount = enrollment.TextContent.Trim();

                        list.Add(course);

                        if (list.Count >= courseAmount) break;
                    }
                }
            }

            return list;
        }
    }
}
