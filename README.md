# net6.api

- 確認dotnet sdk version

````bash
dotnet --version
````

- infrastructure安裝 ef package 

## 專案結構

Project.API
  - Applications
    > 包含Controller與Domain Entity關聯的物件
    
    - Commands
    > 與CommandHandler配合，等價AppService中的方法參數物件，通常負責該業務基本的Insert、Update、Delete事務
    
    - Contracts
    > 使用於Controller Action Method，為對外公開的API合約協定
    > 與宇傑討論Controller是直接對外揭露Command，還是統一使用Contract對外公開，Controller內部在自行將參數處理/不處理建置Command實例並傳入事件匯流排發送

    - Mapping
    > 使用automapper，放置Profile

    - DomainEventHandler
    > 為該業務核心處理業務，通常會改變物件狀態或是該業務必要處理動作
    
    - Queries
    > 為該業務Read Model，處理no side effect事務，使用Dapper直接對資料庫進行查詢
    
    - Results
    > 為Controller對外公開的訊息結構
    
    - Services
    > 為API所使用的應用服務，非本業務核心處理服務
    
    - Validators
        > 驗證Contracts參數合法性
    
    - ViewModels
        > Queries中各no side effect事務的ViewModel，用於回應結果

  - Configuration
    > 放置有關組態設定與Program.cs相關
    - AppSetting
      - 放置Option pattern物件
    - Registry
      > ServiceCollection與MiddleWare 
    - Swagger
      > 處理有關Swagger Open API設定

Project.Domain
    - AggregateModel
    > 聚合根，通常由Entity與ValueObject組成
    - Events
    > 核心業務事件
    - Kernel
    > 底層物件，日後可能移至基礎組件專案，統一提供所有專案參考
    - Repositories
    > 定義repo 存取資料庫的方法

Project.Infrastructure
    - Data
    > 有關資料庫初始化設定
    - Repositories
    > 實作repo存取
    - Contexts
    > 宣告本業務相關資料表與設定transaction、Rollback、SaveChange