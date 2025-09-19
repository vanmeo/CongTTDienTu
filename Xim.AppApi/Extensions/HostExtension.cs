namespace Xim.AppApi.Extensions
{
    public static class HostExtension
    {
        public static void InitNewtonjson(this IMvcBuilder builder, IConfiguration configuration)
        {
            builder.AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.DateFormatHandling = Newtonsoft.Json.DateFormatHandling.IsoDateFormat;
                options.SerializerSettings.DateTimeZoneHandling = Newtonsoft.Json.DateTimeZoneHandling.Local;
                //options.SerializerSettings.DateFormatString = "yyyy'-'MM'-'ddT'HH':'mm':'ss.ffff";
                options.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
            });
        }
    }
}
