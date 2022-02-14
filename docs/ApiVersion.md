# api version

為何需要version，我們經常需要為app新增新功能或是進行維護。version允許我們可以安全的進行變更而不會破壞現有功能。
並非所有針對api的更改都是重大變更。

- additive changes are not breaking changes
    1.新增new endpoint
    2.新增optional query parameter
    3.Dto增加新的屬性

- 針對api進行替換或刪除會造成breaking changes
    - 變更Dto屬性型別
    - 移除Dto屬性或是endpoint
    - 更名Dto屬性或是endpoint
    - 對request新增必要欄位

## installation

- net6

````
dotnet add package Microsoft.AspNetCore.Mvc.Versioning
dotnet add package Microsoft.AspNetCore.Mvc.Versioning.ApiExplorer
````

## how to use

````
builder.Services.AddApiVersioning(o =>
{
    //未指定時使用預設值
    o.AssumeDefaultVersionWhenUnspecified = true;
    //預設格式為1.0
    o.DefaultApiVersion = new Microsoft.AspNetCore.Mvc.ApiVersion(1, 0);
    //在header中提供 api-deprecated-versions呈現api version
    o.ReportApiVersions = true;
    //設定從request中的那個位置讀取api version
    //from a query string, request header, and media type

    o.ApiVersionReader = ApiVersionReader.Combine(
        new QueryStringApiVersionReader("api-version"),
        new HeaderApiVersionReader("X-Version"),
        new MediaTypeApiVersionReader("ver"));
});
````

````
    builder.Services.AddVersionedApiExplorer(
    options =>
    {
        //設定version as “‘v’major[.minor][-status]”.
        options.GroupNameFormat = "'v'VVV";
        //僅透過URI segment 取得versioning
        options.SubstituteApiVersionInUrl = true;
    });
````

````
    [Route("api/[controller]")]
    [ApiController]
    [ApiVersion("1.0")]
    public class UserController : ControllerBase
````

- Query String Parameter Versioning
    > 新增query string property api-version並提供版本
    > https://localhost:7088/api/User/users?api-version=1.0

- Header API Versioning
    > 在header中設定configuration中的X-Version屬性，並提供版本
    > X-Version:1.0

- Media API Versioning
    > 在header中的accept屬性中，加入ver=1.0

- URI Versioning
    > 最常見的version control solution,好處是可以直接在uri得知版本訊息
    > 以範例為例 https://localhost:7088/api/v1/User

````
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [ApiVersion("1.0")]
    public class UserController : ControllerBase
````

- Deprecating Previous Versions
    > 當新增功能後，舊版本不在維護時，可以透過Deprecated屬性標示目前那個api version是不支援的
    > header中會出現api-deprecated-version提示client

````
    [Route("api/v{version:apiVersion}")]
    [ApiController]
    [ApiVersion("2.0",Deprecated =true)]
    public class UserController : ControllerBase
````

## 參考資料

[api-versioning](https://code-maze.com/aspnetcore-api-versioning/)