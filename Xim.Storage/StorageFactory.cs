using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using Xim.Storage.FileStorages;

namespace Xim.Storage
{
    public static class StorageFactory
    {
        public static IStorageService UseFileStorage(this IConfiguration configuration, IServiceCollection services)
        {
            var storageConfig = configuration.GetSection("Storage").Get<StorageConfig>();
            services.AddSingleton(storageConfig);

            var fileConfig = configuration.GetSection("Storage:File").Get<FileStorageConfig>();
            var service = new FileStorageService(fileConfig);
            services.AddSingleton<IStorageService>(service);
            return service;
        }
    }
}
