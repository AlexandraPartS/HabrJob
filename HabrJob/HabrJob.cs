namespace habrJob
{
    public class HabrJob
    {
        string? uriId;
        public string? Title { get; set; }
        public string Url
        {
            get => uriId; 
            set => uriId = $"https://{PageManager.uriHost}{value}";
        }
        public string? Company { get; set; }
        public string? Price { get; set; }


        public static string[] htmlXPath = {
            "//div[@class='vacancy-card']/div",
            "vacancy-card__title",
            "vacancy-card__title-link",
            "vacancy-card__company-title",
            "vacancy-card__salary"
        };
    }
}
