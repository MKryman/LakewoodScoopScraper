using AngleSharp.Dom;
using AngleSharp.Html.Parser;
using static System.Net.Mime.MediaTypeNames;

namespace Homework_06_07.Data
{
    public static class TLScoopScraper
    {
        public static List<TLSItem> Scrape()
        {
            var html = GetTLSHtml();
            var items =  ParseHtml(html);
            return items;
        }

        private static string GetTLSHtml()
        {
            var handler = new HttpClientHandler
            {
                AutomaticDecompression = System.Net.DecompressionMethods.GZip | System.Net.DecompressionMethods.Deflate,
                UseCookies = true
            };
            using var client = new HttpClient(handler);
            return client.GetStringAsync("https://thelakewoodscoop.com/").Result;
        }

        private static List<TLSItem> ParseHtml(string html)
        {
            var parser = new HtmlParser();
            var document = parser.ParseDocument(html);

            var divs = document.QuerySelectorAll(".td_module_flex");
            var items = new List<TLSItem>();

            foreach (var div in divs)
            {
                TLSItem item = new();
                items.Add(item);

                IElement titleElement = div.QuerySelector(".td-module-title");
                if (titleElement != null)
                {
                    var aTag = titleElement.QuerySelector("a");
                                        
                    if (aTag != null)
                    {
                        item.Title = aTag.TextContent;
                        item.Url = aTag.Attributes["href"].Value;
                    }
                }

                var textBlurb = div.QuerySelector(".td-excerpt");
                if (textBlurb != null)
                {
                    item.TextBlurb = textBlurb.TextContent;
                }

                var commentsAmount = div.QuerySelector(".td-module-comments");
                if (commentsAmount != null)
                {
                    item.Comments = int.Parse(commentsAmount.TextContent);
                    var comLink = commentsAmount.QuerySelector("a");
                    item.CommentsLink = comLink.Attributes["href"].Value;
                }

                var date = div.QuerySelector(".td-post-date");
                if (date != null)
                {
                    item.DatePosted = DateTime.Parse(date.TextContent).ToShortDateString();
                }

                var imageWrapper = div.QuerySelector(".td-module-thumb");
                if (imageWrapper != null)
                {
                    var image = imageWrapper.QuerySelector("span");
                    if (image != null)
                    {
                        item.Image = image.Attributes["data-img-url"].Value;
                    }

                   
                }

            }
            return items;
        }

    }
}