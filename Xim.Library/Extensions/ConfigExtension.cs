using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Xim.Library.Extensions
{
    public static class ConfigExtension
    {
        public static TConfig InjectConfig<TConfig>(this IConfiguration configuration, IServiceCollection services = null)
        {
            var config = configuration.Get<TConfig>();
            Console.WriteLine($"-----: {JsonConvert.SerializeObject(config)}");

            if (services != null)
            {
                services.AddSingleton(typeof(TConfig), config);
            }

            return config;
        }

        public static TConfig InjectConfig<TConfig>(this IConfiguration configuration, string configSection, IServiceCollection services = null)
        {
            var config = configuration.GetSection(configSection).Get<TConfig>();
            Console.WriteLine($"----- {configSection}: {JsonConvert.SerializeObject(config)}");

            if (config == null)
            {
                throw new Exception($"Missing config {configSection}");
            }

            if (services != null)
            {
                services.AddSingleton(typeof(TConfig), config);
            }

            return config;
        }
    }
}
