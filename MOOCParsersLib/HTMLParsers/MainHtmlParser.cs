using AngleSharp.Html.Parser;
using System.Threading.Tasks;
using System;

namespace MOOCParsersLib.HTMLParsers
{
    public class MainHtmlParser<T> where T : class
    {
        DataLoader loader;
        IHtmlParserSettings parserSettings;

        public IHtmlParser<T> Parser { get; set; }

        public IHtmlParserSettings ParserSettings
        {
            get
            {
                return parserSettings;
            }
            set
            {
                parserSettings = value;
                loader = new DataLoader(value);
            }
        }

        public MainHtmlParser(IHtmlParser<T> parser)
        {
            Parser = parser;
        }

        public MainHtmlParser(IHtmlParser<T> parser, IHtmlParserSettings parserSettings) : this(parser)
        {
            ParserSettings = parserSettings;
        }

        public async Task<T> StartParsing()
        {
            string source = await loader.GetSiteHtml();

            HtmlParser domParser = new HtmlParser();
            var document = domParser.ParseDocumentAsync(source).Result;

            T result = await Parser.Parse(document);

            return result;
        }
    }
}
