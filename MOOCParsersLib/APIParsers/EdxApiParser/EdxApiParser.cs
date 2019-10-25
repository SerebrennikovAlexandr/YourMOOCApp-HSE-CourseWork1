using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MOOCParsersLib.APIParsers.EdxApiParser
{
    public static class EdxApiParser
    {
        public async static Task<List<Course>> Parse(DataLoader loader, string request)
        {
            List<Course> result = new List<Course>();
            JToken jsonCourses = null;

            if (request == "")
            {
                int courseAmount = 6;
                double score = 4.8;

                string url = "featured_course_ids=course-v1:UQx+LGDM2x+1T2019," +
                    "course-v1:MichiganX+py4e101x+1T2019," +
                    "course-v1:AdelaideX+Project101x+1T2017," +
                    "course-v1:RITx+SKILLS103x+3T2019," +
                    "course-v1:Wharton+StrategyX+2T2019," +
                    "course-v1:UCSanDiegoX+DSE200x+3T2019," +
                    "course-v1:ImperialBusinessX+ICBS005+3T2018," +
                    "course-v1:GTx+CS1301xI+1T2019," +
                    "course-v1:AdelaideX+BigDataX+3T2018" +

                    "&featured_programs_uuids=3c32e3e0-b6fe-4ee4-bd4f-210c6339e074," +
                    "8a17f173-4e52-40aa-bd1b-f4e9b7646872," +
                    "7149dfe5-f964-464e-8d6c-ec9a22b4887e," +
                    "317e9c1e-4b7c-472c-8aa0-1716625ad28c," +
                    "0de98c3a-0a5a-4cf2-b0a0-634862d47e11," +
                    "a171a08f-f558-4990-8614-fe1adaf85c7e," +
                    "9b729425-b524-4344-baaa-107abdee62c6," +
                    "d8b4bdef-d2ee-4232-8504-6a0342283cb5," +
                    "a11c408f-0986-4393-8268-8bc16500cdf3";

                string text = await loader.GetJsonData(url);

                if (text == "")
                {
                    return result;
                }

                JObject json = JObject.Parse(text);
                try
                {
                    jsonCourses = json["featured_course_runs"]["objects"]["results"];
                }
                catch (Exception)
                {
                    return result;
                }

                foreach (JToken courseInfo in jsonCourses)
                {
                    Course course = new Course("", "EdX Platform.", "", Color.LightSteelBlue);

                    try
                    {
                        course.Name = courseInfo["title"].ToString();
                        course.SourсeRef = courseInfo["marketing_url"].ToString();
                        course.Source += "Course by " + courseInfo["org"].ToString();
                        course.Rating = score.ToString();
                        string pictureRef = courseInfo["image_url"].ToString();
                        if (course.Name == null || course.Name == "" ||
                            pictureRef == null || pictureRef == "" ||
                            course.SourсeRef == null || course.SourсeRef == "") continue;
                        else
                        {
                            course.Picture = new UriImageSource
                            {
                                CachingEnabled = true,
                                Uri = new Uri(pictureRef)
                            };
                        }
                        score -= 0.1;
                    }
                    catch (Exception)
                    {
                        continue;
                    }

                    try
                    {
                        string people = courseInfo["enrollment_count"].ToString();
                        if (people != null && people != "")
                            course.EnrolledPeopleAmount = people;
                        else
                            course.EnrolledPeopleAmount = "473";
                    }
                    catch (Exception)
                    {
                        course.EnrolledPeopleAmount = "473";
                    }

                    result.Add(course);

                    if (result.Count >= courseAmount) break;
                }
            }
            else
            {
                int courseAmount = 5;
                double score = 4.8;

                string url = $"query={request}";

                string text = await loader.GetJsonData(url);

                if (text == "")
                {
                    return result;
                }

                JObject json = JObject.Parse(text);
                try
                {
                    jsonCourses = json["objects"]["results"];
                }
                catch (Exception)
                {
                    return result;
                }

                foreach (JToken courseInfo in jsonCourses)
                {
                    Course course = new Course("", "EdX Platform.", "", Color.LightSteelBlue);

                    try
                    {
                        course.Name = courseInfo["title"].ToString();
                        course.SourсeRef = courseInfo["marketing_url"].ToString();
                        course.Source += "Course by " + courseInfo["org"].ToString();
                        course.Rating = score.ToString();
                        string pictureRef = courseInfo["image_url"].ToString();
                        if (course.Name == null || course.Name == "" ||
                            pictureRef == null || pictureRef == "" ||
                            course.SourсeRef == null || course.SourсeRef == "") continue;
                        else
                        {
                            course.Picture = new UriImageSource
                            {
                                CachingEnabled = true,
                                Uri = new Uri(pictureRef)
                            };
                        }
                        score -= 0.1;
                    }
                    catch (Exception)
                    {
                        continue;
                    }

                    try
                    {
                        text = await loader.GetJsonData("https://www.edx.org/api/v1/catalog/search?" +
                            "featured_course_ids=" + courseInfo["key"].ToString());
                        JToken jsonfull = JObject.Parse(text)["featured_course_runs"]["objects"]["results"];

                        string people = jsonfull["enrollment_count"].ToString();
                        if (people != null && people != "")
                            course.EnrolledPeopleAmount = people;
                        else
                            course.EnrolledPeopleAmount = "473";
                    }
                    catch (Exception)
                    {
                        course.EnrolledPeopleAmount = "473";
                    }

                    result.Add(course);

                    if (result.Count >= courseAmount) break;
                }
            }

            return result;
        }
    }
}
