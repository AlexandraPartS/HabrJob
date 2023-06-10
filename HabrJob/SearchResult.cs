namespace habrJob
{
    
    internal struct SearchResult
    {
        public static int TotalResults { get; set; }
        public static int TotalPages { get; set; }
        public static bool AmtResult { get; set; }

        public SearchResult(string substring)
        {
            Dictionary<string, int> valuePairs = Parser.GetValueFromJsonString(substring);

            TotalResults = valuePairs["TotalResults"];
            TotalPages = valuePairs["TotalPages"];
            AmtResult = (TotalResults == 0) ? false : true;
        }
    }
}
