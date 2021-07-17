using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShopBridgeApi.Config
{
    public class LazyInitializer<T>:Lazy<T> where T:class
    {
        public LazyInitializer(IServiceProvider serviceProvider)
            : base(() => serviceProvider.GetRequiredService<T>())
        {
            
        }
    }
}
