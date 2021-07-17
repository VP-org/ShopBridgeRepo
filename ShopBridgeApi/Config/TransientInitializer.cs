using Logic.Interfaces;
using Logic.Providers;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace ShopBridgeApi.Config
{
    public static class TransientInitializer
    {
        public static void AddTrasients(this IServiceCollection services)
        {
            services.AddTransient<IProductProvider, ProductProvider>();
            services.AddTransient(typeof(Lazy<>),typeof(LazyInitializer<>));
        }
    }
}