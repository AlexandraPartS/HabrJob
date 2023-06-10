namespace habrJob
{
    public delegate void InteractionDeviceHandler(string message);
    class Program
    {
        const string templateUri = "https://career.habr.com/vacancies?q=c%23&qid=4&type=all";
        static List<HabrJob>? jobs;

        static async Task Main(string[] args)
        {
            DataReceiver.RegisterHandler(UseInterfaceConsole);
            DataReceiver dataReceiver = new DataReceiver();
            PageManager dataPage = new PageManager(dataReceiver.Data);
            SearchResult searchResultData = new(await PageManager.CheckAvailabilityDataFromHtml());

            if (SearchResult.AmtResult)
            {
                jobs = new List<HabrJob>();
                for (int i = 1; i <= SearchResult.TotalPages; i++)
                {
                    PageManager.CreateCurrentPageUri(i);
                    if (i != 1)
                    {
                        await PageManager.LoadCurrentPageHtml();
                    }
                    Parser.GetAllUnitDataFromCurrentPageAndPutInList(PageManager.html, jobs);
                }
                DataExport.Export(DataExport.MetaType.XML, jobs);
            }

            DataReceiver.SearchResultReport();

            void UseInterfaceConsole(string message) => Console.WriteLine(message);
        }

    }

}
