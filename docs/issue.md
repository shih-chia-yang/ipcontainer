# issue

- [x] create sln
  -  建立方案與專案
    ````bash
    dotnet new sln -o api_practice 
    cd api_practice 
    
    ````

- [x] create webapi project

    ````
    dotnet new webapi -o practice.api
    dotnet sln add ./practice.api
    ````

- [x] create classlib project

    ````
    dotnet new classlib -o practice.infrastructure
    dotnet sln add ./practice.infrastructure
    ````

- [x] add package

    ````
    dotnet add ./practice.infrastructure package Microsoft.EntityFrameworkCore
    dotnet add ./practice.infrastructure package Microsoft.EntityFrameworkCore.SqlServer
    dotnet add ./practice.infrastructure package Microsoft.EntityFrameworkCore.Tools
    dotnet add ./practice.infrastructure package Microsoft.EntityFrameworkCore.Design
    dotnet add ./practice.api package Microsoft.EntityFrameworkCore.InMemory
    dotnet add ./practice.api package Microsoft.VisualStudio.Web.CodeGeneration.Design
    ````

- [] create user endpoint
  - pull user information
  - store user information
  - create user table
  - update user information

- [] database management
  - Context
  - UOW for the Context
  - Repository
  - add dbcontext in api program