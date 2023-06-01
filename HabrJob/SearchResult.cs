using Newtonsoft.Json.Linq;

namespace habrJob
{
    internal struct SearchResult
    {
        public static int TotalResults { get; set; }
        public static int TotalPages { get; set; }
        public static bool HasResult { get; set; }

        static JobSearchResultReportHandler? report;
        public SearchResult(JObject obj)
        {
            TotalResults = Int32.Parse(obj.GetValue("totalResults").ToString());
            TotalPages = Int32.Parse(obj.GetValue("totalPages").ToString());
            if (TotalResults == 0) HasResult = false;
            else HasResult = true;
        }
        static public void RegisterHandler(JobSearchResultReportHandler del)
        {
            report = del;
        }
        public static void SearchResultReport()
        {
            string basetxt = $"Searching results:\n" +
                             $"TotalResults: {TotalResults}\n";
            if (HasResult)
            {
                report?.Invoke($"{basetxt}" +
                               $"List of vacancies in XML file\n" +
                               $"[File directory: {Directory.GetCurrentDirectory()}] ");
            }
            else
            {
                report?.Invoke($"{basetxt}");
            }
        }
    }
}
