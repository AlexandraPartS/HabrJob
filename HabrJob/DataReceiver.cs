namespace habrJob
{
    /// <summary>
    /// Class that initializes a dialog-interface with the user and validates the entered data
    /// Current input implementation: validation of the entered link.
    /// (Alternative Solution: entering search parameters by selecting from a list)
    /// </summary>

    internal class DataReceiver
    {
        string? data;
        static public InteractionDeviceHandler ondevice;
        static public void RegisterHandler(InteractionDeviceHandler del) => ondevice = del;

        public string Data { 
            get => data; 
            set 
            {
                data = null;
                bool result = Uri.TryCreate(value, UriKind.Absolute, out Uri uriResult)
                                && (uriResult.Scheme == Uri.UriSchemeHttps);
                if (result)
                {
                    string pattern = @"https://career.habr.com/vacancies";
                    if (value.StartsWith(pattern))
                    {
                        data = value;
                    }
                }
            }
        }

        public DataReceiver()
        {
            while (true)
            {
                ondevice?.Invoke($"Insert a link from career.habr.com with configured search parameters:");//Add an exit button
                Data = Console.ReadLine();
                if (Data is not null)
                {
                    ondevice?.Invoke($"Wait for the results...\n");
                    break;
                }
                else ondevice?.Invoke($"Invalid input.");
            }
        }

        public static void SearchResultReport()
        {
            string basetxt = $"Searching results:\n" +
                             $"TotalResults: {SearchResult.TotalResults}\n";
            if (SearchResult.AmtResult)
            {
                ondevice?.Invoke($"{basetxt}" +
                               $"List of vacancies in XML file\n" +
                               $"[File directory: {Directory.GetCurrentDirectory()}] ");
            }
            else
            {
                ondevice?.Invoke($"{basetxt}");
            }
        }
    }
}
