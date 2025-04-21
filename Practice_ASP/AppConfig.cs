namespace Practice_ASP
{
    public class AppConfig
    {
        public string RandomApi { get; set; }
        public AppSettings Settings { get; set; }
    }

    public class AppSettings
    {
        public List<string> BlackList { get; set; } = new();
    }

}
