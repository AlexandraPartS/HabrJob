using HtmlAgilityPack;
using Newtonsoft.Json.Linq;

namespace habrJob
{
    internal class Parser
    {
        static public Dictionary<string, int> GetValueFromJsonString(string substring)
        {
            JObject obj = JObject.Parse(substring);
            Dictionary<string, int> jsonconv = new Dictionary<string, int>() {
                {"TotalResults", Int32.Parse(obj.GetValue("totalResults").ToString()) },
                {"TotalPages", Int32.Parse(obj.GetValue("totalPages").ToString())}
            };
            return jsonconv;
        }

        static public string ParseHtmlToJsonStringData(HtmlDocument html)
        {
            var htmlText = html.DocumentNode.Descendants()
                            .Where(e => e.Name == "script" && e.Attributes.Contains("type"))
                            .First()
                            .InnerHtml;
            string data1 = htmlText.Substring(htmlText.IndexOf(@"{""totalResults"));
            string data = data1.Substring(0, data1.IndexOf(@"},"));
            return data;
        }

        static public void GetAllUnitDataFromCurrentPageAndPutInList(HtmlDocument html,  List<HabrJob> jobs)
        {
            var divNodes = html.DocumentNode.SelectNodes(HabrJob.htmlXPath[0]);
            if (divNodes.Count > 0)
            {
                foreach (var node in divNodes)
                {
                    HabrJob job = new HabrJob()
                    {
                        Title = node.Descendants().FirstOrDefault(node => node.HasClass(HabrJob.htmlXPath[1])).InnerText,
                        Url = node.Descendants().FirstOrDefault(node => node.HasClass(HabrJob.htmlXPath[2])).Attributes["href"].Value,
                        Company = node.Descendants().FirstOrDefault(node => node.HasClass(HabrJob.htmlXPath[3])).InnerText,
                        Price = node.Descendants().FirstOrDefault(node => node.HasClass(HabrJob.htmlXPath[4])).InnerText
                    };
                    jobs.Add(job);
                }
            }
        }
    }
}
