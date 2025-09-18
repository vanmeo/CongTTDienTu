namespace Xim.Application.Services
{

    public class FtpSettings
    {

        public string Host { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
    public static class GlobalConfig
    {
        public static FtpSettings Ftp { get; set; }
    }
}
