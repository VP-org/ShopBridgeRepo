using Logic.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShopBridgeApi.Controllers
{
    public abstract class BaseController : Controller
    {
        protected IProductProvider _productProvider
        {
            get
            {
                return _lazyProductProvider.Value;
            }
        }

        protected Lazy<IProductProvider> _lazyProductProvider;
    }
}
