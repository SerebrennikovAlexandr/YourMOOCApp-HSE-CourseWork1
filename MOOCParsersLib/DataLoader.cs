using System;
using System.Net;
using System.Net.Http;
using System.IO;
using System.Text;
using Xamarin.Forms;
using AngleSharp;
using AngleSharp.Dom;
using MOOCParsersLib.HTMLParsers;
using MOOCParsersLib.APIParsers;
using System.Threading.Tasks;

namespace MOOCParsersLib
{
    public class DataLoader
    {
        string url;

        public DataLoader(IHtmlParserSettings settings)
        {
            url = $"{settings.BaseUrl}" + $"{settings.Prefix}";
        }

        public DataLoader(string baseUrl)
        {
            url = baseUrl;
        }

        public async Task<string> GetSiteHtml()
        {
            /*
            HttpClient client = new HttpClient();
            HttpResponseMessage response = client.GetAsync(url).Result;
            HttpContent content = response.Content;
            return content.ReadAsStringAsync().Result;
            */

            string data = "";

            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);

                //no use
                System.Net.Cookie cookie = new System.Net.Cookie
                {
                    Name = "beget",
                    Value = "begetok"
                };

                request.CookieContainer = new CookieContainer();
                request.CookieContainer.Add(new Uri(url), cookie);
                //

                WebResponse response = await request.GetResponseAsync();
                HttpWebResponse httpResponse = (HttpWebResponse)response;
                if (httpResponse.StatusCode == HttpStatusCode.OK)
                {
                    Stream receiveStream = httpResponse.GetResponseStream();
                    StreamReader readStream = null;
                    readStream = new StreamReader(receiveStream);
                    data = readStream.ReadToEnd();
                    httpResponse.Close();
                    response.Close();
                    readStream.Close();
                }
            }
            catch (Exception e)
            { }

            return data;
        }

        public async Task<IDocument> GetSiteIDocument()
        {
            IDocument document = null;
            var config = Configuration.Default.WithDefaultLoader();

            try
            {
                document = await BrowsingContext.New(config).OpenAsync(url);
            }
            catch
            {}

            return document;
        }

        public async Task<string> GetJsonData(string prefix)
        {
            string data = "";

            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url + prefix);

                //no use
                System.Net.Cookie cookie = new System.Net.Cookie
                {
                    Name = "beget",
                    Value = "begetok"
                };

                request.CookieContainer = new CookieContainer();
                request.CookieContainer.Add(new Uri(url + prefix), cookie);
                //

                WebResponse response = await request.GetResponseAsync();
                HttpWebResponse httpResponse = (HttpWebResponse)response;
                if (httpResponse.StatusCode == HttpStatusCode.OK)
                {
                    Stream receiveStream = httpResponse.GetResponseStream();
                    StreamReader readStream = null;
                    readStream = new StreamReader(receiveStream);
                    data = readStream.ReadToEnd();
                    httpResponse.Close();
                    response.Close();
                    readStream.Close();
                }
            }
            catch (Exception e)
            { }

            return data;
        }
    }
}
