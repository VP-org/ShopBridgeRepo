# ShopBridgeRepo

**How to use:**

   Visual Studio 2019 and the .NET Core SDK.

**Technologies:**
  ASP.NET WebApi Core
  Entity Framework Core
  Swagger (via Swashbuckle)
  NUnit
  
  
**Setup and Run application**

1. Retore Database backup file 'Shop_Bridge_db.bak' which is placed in repo root path

2. Set Database Name As : Shop_Bridge_db

3. Modify the connection string in appsettings.json to reflect your database environment
 
    e.g.  "ShopBridgeDb": "Server=DBServerName;Database=Shop_Bridge_db;Trusted_Connection=True;"
    
4. Build and run the ShopBridgeApi project

5. Open /swagger url



