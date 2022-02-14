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

- [x] create user endpoint
  - [x]create base entity model
    - id,status,trxdate
  - [x]create user model 
    - first name
    - last name
    - email
    - phone
    - organization
    - unit
  - [x] pull user information
  - [x] store user information
  - [x] create user table
  - [x] update user information

- [x] database management
  - Context
  - UnitOfWork for the Context
  - Repository
  - add dbcontext in api program