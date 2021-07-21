using DataService;
using System;
using System.Collections.Generic;
using System.Text;

namespace Logic.Providers
{
    public abstract class BaseProvider
    {
        protected Shop_Bridge_dbContext _db
        {
            get
            {
                return _lazyDb.Value;
            }
        }
        protected BaseProvider(Lazy<Shop_Bridge_dbContext> db)
        {
            _lazyDb = db;
        }

        protected Lazy<Shop_Bridge_dbContext> _lazyDb { get; }
    }
}
