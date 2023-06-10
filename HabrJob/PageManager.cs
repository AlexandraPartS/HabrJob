using HtmlAgilityPack;

namespace habrJob
{
    /// <summary>
    /// Struct that works with the data of current page: uri and html content
    /// 1. accepts/creates and stores the uri and uriComponents of the current page
    /// 2. sends a request and stores html content
    /// </summary>
    
    internal struct PageManager
    {
        static string? UriPage { get; set; }
        static public string? uriHost;
        static string? uriAbsolutePath, uriQuery;

        static HttpClient? client;
        static public HtmlDocument? html;
        static HttpResponseMessage? response;

        public PageManager(string data)
        {
            UriPage = data;
            UriParse(UriPage);
            html = new HtmlDocument();
            client = new HttpClient();
        }

        void UriParse(string uritxt)
        {
            Uri uri = new Uri(uritxt);
            uriHost = uri.Host;
            uriAbsolutePath = uri.AbsolutePath;
            uriQuery = uri.Query.TrimStart('?');
        }
        static public void CreateCurrentPageUri(int i) => UriPage = $"https://{uriHost}{uriAbsolutePath}?page={i}&{uriQuery}";

        public static async Task<string> GetPageHtml()
        {
            response = await client.GetAsync(UriPage);
            return await response.Content.ReadAsStringAsync();
        }

        static public async Task<string> CheckAvailabilityDataFromHtml()
        {
            await LoadCurrentPageHtml();
            return Parser.ParseHtmlToJsonStringData(html);
        }

        static public async Task LoadCurrentPageHtml() => html.LoadHtml(await GetPageHtml());
    }
}
