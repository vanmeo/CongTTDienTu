namespace API.Constants
{
    public class FTPSettings
    {
        public string Host { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public int Port { get; set; }
        public string RemoteDirectory { get; set; }
    }
    public static class GlobalConfig
    {
        public static FTPSettings Ftp { get; set; }
    }
}
