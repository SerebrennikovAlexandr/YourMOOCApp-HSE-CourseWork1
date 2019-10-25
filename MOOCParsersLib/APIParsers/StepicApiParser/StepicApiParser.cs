using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using Xamarin.Forms;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MOOCParsersLib.APIParsers.StepicApiParser
{
    public static class StepicApiParser
    {
        public async static Task<List<Course>> Parse(DataLoader loader, string request)
        {
            List<Course> result = new List<Course>();

            string url = $"/search-results?is_popular=true&is_public=true&page=1&type=course";

            if (request != "") url += $"&query={request}";

            string text = await loader.GetJsonData(url);

            if (text == "")
            {
                return result;
            }

            JObject json = JObject.Parse(text);
            JToken jsonCourses = null;
            try
            {
                jsonCourses = json["search-results"];
            }
            catch (Exception)
            {
                return result;
            }

            int courseAmount = 5;
            foreach (JToken courseInfo in jsonCourses)
            {
                Course course = new Course("", "Stepic platform.", "", Color.LightGreen);
                string courseID = "";

                try
                {
                    courseID = courseInfo["course"].ToString();
                }
                catch (Exception)
                {
                    try
                    {
                        courseID = courseInfo["target_id"].ToString();
                    }
                    catch (Exception)
                    {
                        continue;
                    }
                }

                if (courseID == null || courseID == "") continue;

                try
                {
                    course.Name = courseInfo["course_title"].ToString();
                    course.SourсeRef = $"https://stepik.org/course/{courseID}/promo";
                    string pictureRef = courseInfo["course_cover"].ToString();
                    if (course.Name == null || course.Name == "" || pictureRef == null || pictureRef == "") continue;
                    else
                    {
                        course.Picture = new UriImageSource
                        {
                            CachingEnabled = true,
                            Uri = new Uri(pictureRef)
                        };
                    }
                }
                catch (Exception)
                {
                    continue;
                }

                try
                {
                    text = await loader.GetJsonData($"/courses?ids%5B%5D={courseID}");
                    JToken courseGeneralData = JObject.Parse(text)["courses"][0];
                    course.EnrolledPeopleAmount = courseGeneralData["learners_count"].ToString();

                    text = await loader.GetJsonData($"/users?ids%5B%5D={courseGeneralData["owner"].ToString()}");
                    course.Source = "Stepic platform. Course by " + JObject.Parse(text)["users"][0]["full_name"].ToString();

                    text = await loader.GetJsonData($"/course-review-summaries?ids%5B%5D={courseGeneralData["review_summary"].ToString()}");
                    course.Rating = JObject.Parse(text)["course-review-summaries"][0]["average"].ToString().Substring(0,4);

                }
                catch (Exception)
                { }

                result.Add(course);

                if (result.Count >= courseAmount) break;
            }

            return result;
        }
    }
}
