# seeds - Humanity's Database of Creativity
## TODOs
- [ ] test MAUI Services
- [ ] test `CategoriesController.GetCategoriesAsync`
- [ ] create CurrentUser singleton
## Solution Structure
**seeds** (solution)
- **seeds.Api** (the web API project)
  - **Controllers**
    (almost no changes made w.r.t. the scaffolded controllers)
  - **Data** (DB access stuff)
  - **Helpers**
  - **Migrations** (auto-generated by *$ dotnet ef migrations add my_migration*)
  - `Program.cs` (the server startup class)
- **seeds.Dal** (the Data Access Layer project)
  - **Dto** (Data Transfer Objects - slightly different to the Model classes)
    - **ToApi** (DTOs that can be passed **to** the API)
  - **Interfaces** (of the services)
  - **Model** (the EF Core models)
  - **Services**
    - `DalBaseService.cs` (injected into each DAL Service class)
      - provides a *public* HttpClientWrapper* property (bc. it is injected, not inherited from)
      - provides generic methods for *GET*,*PUT*,*POST* methods of DAL Service classes (:exclamation: *GET* returns default(T) on NotFound HttpResponse, otherwise throws)
    - e.g. `UsersService.cs`
  - **Wrappers** (wrappers of static methods to enable testing)
- **seeds1** (the .NET MAUI frontend project)
  - **Converter** (classes used by the views)
  - **MauiModels** (models only used by the frontend project)
  - **Services**
  - **View** (the .xaml files plus their .xaml.cs "*code-behind*"s)
  - **ViewModel**
  - `App.xaml` (global view resources, e.g. styles)
  - `AppShell.xaml` (definition of the navigating Shell)
  - `MauiProgram.cs` (the MAUI startup class)
 
## Tests Structure
**seeds** (solution)
- **seeds.Api.Tests** (xUnit test project of the Web API)
  - **Controllers**: No Unit tests, only endpoint tests
    - `ApiBaseControllerTests.cs` (provides unpopulated _context and an _httpClient accessing it, through the `ProgramTest.cs` in-memory server)
    - *GET*
      1. `ControllerName_GetEndpoint_ReturnsItself()`
      2. `ControllerName_GetEndpoint_IfNotExistReturnsNotFound()`
    - *PUT*
      1. `ControllerName_PutEndpoint_ReturnsSuccessAndUpdatedDb()` (i.e. update the context)
      2. `ControllerName_PutEndpoint_IfNotExistReturnsNotFound()`
    - *POST*
      1. `ControllerName_PostEndpoint_ReturnsSuccessAndUpdatedDb()` (i.e. update the context)
      2. `ControllerName_PostEndpoint_IfExistReturnsConflict()`
    - e.g. `UserControllerTests.cs`
  - `ProgramTest.cs` (the test server startup class)
- **seeds.Dal.Tests** (xUnit test project of the DAL units)
  - **Services**: Unit tests
    - *GET*
      1. `ServiceName_GetModelAsync_ReturnsItself()`
      2. `ServiceName_GetModelAsync_IfNotExistReturnsNull()`
    - *PUT*
      1. `ServiceName_PutModelAsync_ReturnsTrue()`
      2. `ServiceName_PutModelAsync_IfNotSuccessReturnsFalse()`
    - *POST*
      1. `ServiceName_PostModelAsync_ReturnsTrue()`
      2. `ServiceName_PostModelAsync_IfNotSuccessReturnsFalse()`
    - e.g. `UsersServiceTests.cs`
    - `DalBaseServiceTests.cs` (generic unit tests)
      - tests that `default(T)` w/ `T` a Model gives `null`
      - distinguishes `HttpStatusCode.NotFound` and other bad responses in *GET* (for the cases where `default(T)` is not a clear sign of error, e.g. `T = typeof(int)`)
- **seeds1.Tests** (xUnit test project of the MAUI App)
  - **ViewModel**: Unit tests. Tests include
    - *Raise Property Changed Event* (PCE) - tests
    - *Navigates to* - tests (using mocked navigation services)

## What You Should Do When...
- **You want to add a new EF Core model class that defines a seperate entity** (not a join entity)
  1. Add an EF Core model class `seeds.Dal.Model.MyModel.cs`
  2. Add a DTO Model `MyModelDto.cs` somewhere appropriate in `seeds.Dal.Dto`
  3. Create the corresponding AutoMapper mappings in `seeds.Api.Helpers.AutoMapperProfiles.cs`
  4. Add a configuration class `seeds.Api.Data.MyModelConfiguration.cs`
  5. Scaffold out a controller by right-clicking `seeds.Api.Controllers` :arrow_right: Add API Controller with actions, using EF :arrow_right: choose `MyModel` as model and the existing context class and hence create `seeds.Api.Controllers.MyModelsController.cs`
  6. Adapt the `MyModelsController` class to return not the EF model, but the DTO model; delete useless endpoints
  7. Create a service class `seeds.Dal.Services.MyModelService.cs` that accesses the endpoints
  8. Create its interface `seeds.Dal.Interfaces.IMyModelService.cs`
  9. Use the model in the VMs *a little bit* (to see whether it actually suits your needs) and then write tests `seeds.Api.Tests.Controllers.MyModelsControllerTests.cs` and `seeds.Dal.Tests.Services.MyModelServiceTests.cs`
- **You add a new join entity EF Core model class**
  1. Add an EF Core model class `seeds.Dal.Model.MyJoinEntity.cs`
     - contains 2 foreign keys
     - probably contains payload
  2. Add a configuration class `seeds.Api.Data.MyJoinEntityConfiguration.cs`
     - probably needs to define its primary key as a combination of the two FKs like `builder.HasKey(je => new {je.FK1, je.FK2});`
  3. Scaffold out a controller by right-clicking `seeds.Api.Controllers` :arrow_right: Add API Controller with actions, using EF :arrow_right: choose `MyJoinEntity` as model and the existing context class and hence create `seeds.Api.Controllers.MyJoinEntitiesController.cs`
  4. Minimally adapt the `MyJoinEntitiesController` class, e.g. delete useless endpoints
  5. Create a service class `seeds.Dal.Services.MyJoinEntityService.cs` that accesses the endpoints
  6. Create its interface `seeds.Dal.Interfaces.IMyJoinEntityService.cs`
  7. Use the model in the VMs *a little bit* (to see whether it actually suits your needs) and then write tests `seeds.Api.Tests.Controllers.MyJoinEntitiesControllerTests.cs` and `seeds.Dal.Tests.Services.MyJoinEntityServiceTests.cs`
