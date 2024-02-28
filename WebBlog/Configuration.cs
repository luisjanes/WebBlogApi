namespace WebBlog
{
    public static class Configuration
    {
        public static string JwtKey = "MzE2MmQwY2ItYjZlOS00ZDQyLWEzNjMtYWM4YjA0MDc3YzJk";
        public static string ApiKeyName = "api_key";
        public static string ApiKey = "curso_api_lSFalQlkUa2mOBw1s4JxA==";
        public static SmtpConfiguration Smtp = new();
        public class SmtpConfiguration
        {
            public string Host { get; set; }
            public int Port { get; set; } = 25;
            public string UserName { get; set; }
            public string Password { get; set; }
        }
    }
}
