using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MOOCParsersLib.APIParsers.StepicApiParser;

namespace MOOCParsersLib.APIParsers
{
    public static class MainApiParser
    {
        public async static Task<List<Course>> StartParsing(string request)
        {
            List<Course> result = new List<Course>();

            List<Course> resultTemp = await
                StepicApiParser.StepicApiParser.Parse(new DataLoader("https://stepik.org/api"), request);

            foreach(Course course in resultTemp)
            {
                result.Add(course);
            }

            resultTemp = await 
                EdxApiParser.EdxApiParser.Parse(new DataLoader("https://www.edx.org/api/v1/catalog/search?"), request);

            foreach (Course course in resultTemp)
            {
                result.Add(course);
            }

            return result;
        }
    }
}
