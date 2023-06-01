using HtmlAgilityPack;
using System.Xml.Serialization;
using Newtonsoft.Json.Linq;
using System.Text.RegularExpressions;

namespace habrJob
{
    public delegate void JobSearchResultReportHandler(string message);

    class Program
    {
        static string startUri = "https://career.habr.com/vacancies?q=c%23&qid=4&type=all";
        static string currentPageUri = "";
        static List<HabrJob> jobs;
        static HttpClient client;
        static HttpResponseMessage result;
        static HtmlDocument html;
        public static Uri uri;
        static string XMLpath = "HabrJobs.xml";
        static XmlSerializer xmlSerializer;

        static async Task Main(string[] args)
        {
            jobs = new List<HabrJob>();
            client = new HttpClient();
            html = new HtmlDocument();

            GetSrartLink();
            html.LoadHtml(await GetHtmlContent(startUri));
            ParseAndSetSearchResultsData(SubtractDataStringFromHtml(html));

            if (SearchResult.HasResult)
            {
                xmlSerializer = new XmlSerializer(typeof(List<HabrJob>));
                for (int i = 1; i <= SearchResult.TotalPages; i++)
                {
                    currentPageUri = GetCurrentPageUri(i);
                    if (i != 1)
                    {
                        html.LoadHtml(await GetHtmlContent(currentPageUri));
                    }
                    GetVacancyDataFromCurrentPage();
                    PutReceivedVacanciesToXMLFile();
                }
            }

            SearchResult.RegisterHandler(PrintOnConsoleReport);
            void PrintOnConsoleReport(string message) => Console.WriteLine(message);
            SearchResult.SearchResultReport();

        }


        static void GetSrartLink()
        {
            while (true)
            {
                Console.WriteLine($"Insert a link from career.habr.com with configured search parameters:");
                string txt = Console.ReadLine();
                if (LinkValidation(txt))
                {
                    startUri = txt; 
                    //set default link
                    //Console.WriteLine($"We set default link: {startUri}");
                    Console.WriteLine($"Wait for the results...\n");
                    break;
                }
                else Console.WriteLine($"Invalid input.");
            }
        }
        static bool LinkValidation(string txt)
        {
            string pattern = @"^https://career.habr.com/vacancies\?((\S+=\S+)&?)+";
            //the pattern should be more complicated => uri Habr analytics required
            if (Regex.IsMatch(txt, pattern, RegexOptions.IgnoreCase))
            {
                return true;
            }
            else return false;
        }

        static string GetCurrentPageUri(int currentPage)
        {
            uri = new Uri(startUri);
            return $"https://{uri.Host}{uri.AbsolutePath}?page={currentPage}&{uri.Query.TrimStart('?')}";
        }

        static async Task<string> GetHtmlContent(string uri)
        {
            result = await client.GetAsync(uri);
            return await result.Content.ReadAsStringAsync();
        }


        static string SubtractDataStringFromHtml(HtmlDocument html)
        {
            var htmlText = html.DocumentNode.Descendants()
            .Where(e => e.Name == "script" && e.Attributes.Contains("type"))
            .First()
            .InnerHtml;

            string data1 = htmlText.Substring(htmlText.IndexOf(@"{""totalResults"));
            string data = data1.Substring(0, data1.IndexOf(@"},"));

            return data;
        }

        static void ParseAndSetSearchResultsData(string txt)
        {
            JObject jsonObj = JObject.Parse(txt);//or do it by Regex
            SearchResult srData = new(jsonObj);
        }

        static void GetVacancyDataFromCurrentPage()
        {
            var divNodes = html.DocumentNode.SelectNodes(HabrJob.htmlXPath[0]);
            if (divNodes.Count > 0)
            {
                foreach (var i in divNodes)
                {
                    HabrJob job = new HabrJob()
                    {
                        Title = i.Descendants().FirstOrDefault(node => node.HasClass(HabrJob.htmlXPath[1])).InnerText,
                        Url = i.Descendants().FirstOrDefault(node => node.HasClass(HabrJob.htmlXPath[2])).Attributes["href"].Value,
                        Company = i.Descendants().FirstOrDefault(node => node.HasClass(HabrJob.htmlXPath[3])).InnerText,
                        Price = i.Descendants().FirstOrDefault(node => node.HasClass(HabrJob.htmlXPath[4])).InnerText
                    };
                    jobs.Add(job);
                }
            }
        }

        static void PutReceivedVacanciesToXMLFile()
        {
            using (FileStream fs = new FileStream(XMLpath, FileMode.OpenOrCreate))
            {
                xmlSerializer.Serialize(fs, jobs);
            }
        }
    }

}
