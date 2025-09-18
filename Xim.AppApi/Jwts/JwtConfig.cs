namespace Xim.AppApi.Jwts
{
    public class JwtConfig
    {
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public string Secret { get; set; }
        public int TokenExpiredSecond { get; set; }
        public int RefreshExpiredSecond { get; set; }
    }
}
