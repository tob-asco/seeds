# seeds - A Database of Creativity
## Topics Page Preview


https://github.com/tob-asco/seeds/assets/113723271/f899e2fe-660b-48e3-af80-88651b9301d3


## TODOs
- [ ] DTO for CUP (bc. we have a GUID PK now)
- [ ] if it's your idea in the feed, make your feed entry's name flashy
- [ ] figure out which special characters can be `UrlEncode`'d and forbid the others in the View
- [ ] add platform specific styles by using the code `<Setter Property="x" Value="{OnPlatform Android=x, WinUI=x, iOS=x, MacCatalyst=x, Tizen=x}" />` from e.g. `CommunityToolkit.Maui.Sample\Views\Popups\ToggleSizePopup.xaml`
## How To Get It To Work
(In case of troubles, maybe you find a fix in the file `error-resolution.md`)
1. I use `Visual Studio 22 (17.7) Community` on Windows 11.
2. Clone this repository into VS22 and load all (currently 6) projects of the solution. Of course it will also build if you don't load the (currently 3) `xUnit` test projects.
3. Make sure you download all the `NuGet packages` (e.g. by right-clicking the solution -> `Restore NuGet Packages`).
4. 2 of the 3 non-test projects need to be run independently:
   1. `seeds.Api` (the ASP.NET Web API project - i.e. the server)
   2. `seeds1` (the .NET MAUI project - i.e. the mobile app)
5. In order for `seeds1` to talk to the server endpoints of `seeds.Api.Controllers`, `seeds1` & `seeds.Api` need to agree on `seeds.Api`'s server URL. There are two options:
   1. Use `localhost`, which only works if you launch `seeds1` on the platform *Windows* (i.e. not on an Android emulator, Android local device, iOS device, ...):
      1. To figure out your `BaseAddress` for that, launch `seeds.Api` and copy the URL you see in your web browser into the correct place in the constructor of `seeds.Dal.Wrapper.HttpClientWrapper.cs` (if you don't know what part exactly to copy, have a look at the current constructor to see what I used for `BaseAddress`).
      2. Make sure no `DevTunnel` is active (if you don't know what this is, probably non is active).
   2. Use a `DevTunnel`, which works for *any platform*, but is slightly more work:
      1. Create a `DevTunnel`, e.g. by following https://learn.microsoft.com/en-us/aspnet/core/test/dev-tunnels?view=aspnetcore-7.0#create-a-tunnel
         1. Go to `View` -> `Other Windows` -> `Dev Tunnels`,
         2. in the added window click :heavy_plus_sign:,
         3. add a new public one,
         4. make it the active tunnel in the VS22 instance that has `seeds.Api` open
      2. Proceed by doing the first step of the `localhost` guide above.
6. I know of two ways to run 2 projects independently with VS22 (non of which are too convenient):
   1. Simply open VS22 *twice* and then proceed as you would expect, or
   2. launch both within one VS22 instance, e.g. by following https://learn.microsoft.com/en-us/visualstudio/ide/how-to-set-multiple-startup-projects?view=vs-2022#to-set-multiple-startup-projects
      1. right-click the solution -> `Configure Startup Projects...`,
      2. choose `Multiple startup projects` and start both `seeds.Api` and `seeds1` (order is not very relevant).
7. Of course you need a database (DB), for which I use `postgreSQL`:
   1. On Windows this is pretty straightforward, just download the installer, e.g. from https://www.postgresql.org/download/windows/
   2. On other OSs this is probably trickier, please refer to documentation in this case.
8. You should now have created and remembered a postgres user. Enter all the necessary details like `server`, `port` (probably 5432), `username` (maybe postgres), `database` (seedsdb) and your `password`, into the corresponding line of `seeds.Api.appsettings.json`.
9. The DB's structure is encoded in `C#` (using `EF Core`), so you only need to tell VS22 to create it:
   1. In VS22, open a `Terminal` (i.e. `Developer PowerShell`) by going to `View` -> `Terminal`; or hitting `Ctrl + '`,
   2. in this terminal navigate to the server project, i.e. type `cd seeds.Api` (depending on where your current path is, but probably this works),
   3. now make the DB by typing `dotnet-ef database update` or `ef database update` or `dotnet ef database update` or similar. This should run a lot of lines where you should mainly see some good-looking :green_circle: output, no bad :red_circle: output.
      1. This step might cause some issues, most likely due to missing `NuGet packages`.
      2. If you find some error of the kind *This is not a command :skull_and_crossbones:*, then the command might look slightly different.
10. The DB will be filled upon the first launch of `seeds.Api`, dictated by `seeds.Api.Data.DataSeeder.cs`, where you can also find the usernames that you'll need for Login (try username `tobi` and empty password).
11. If you manage to login and see some ideas, you have been successful, and I am really proud of you! :1st_place_medal:
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
    - *GET* endpoint test recommendations
      1. `ControllerName_GetEndpoint_ReturnsItself()`
      2. `ControllerName_GetEndpoint_IfColumn1NotExistReturnsNotFound()`, ... for other Column2,3,...
    - *PUT* endpoint test recommendations
      1. `ControllerName_PutEndpoint_ReturnsSuccessAndUpdatedDb()` (i.e. update the context)
      2. `ControllerName_PutEndpoint_IfColumns1NotExistReturnsNotFound()`, ... for other Column2,3,...
      3. if `MyModelFromDb` and hence an AutoMapper map exists: `ControllerName_PutEndpoint_LeavesSomeProperty()` to ensure that AutoMapper doesn't update provided values
    - *POST* endpoint test recommendations
      1. `ControllerName_PostEndpoint_ReturnsSuccessAndUpdatedDb()` (i.e. update the context)
      2. `ControllerName_PostEndpoint_IfExistReturnsConflict()`
      3. if `MyModelToDb` exists: `ControllerName_PostEndpoint_ReturnsUpdatedPk()` to ensure that we recieve the model as is in the DB from POST
    - e.g. `UserControllerTests.cs`
  - `ProgramTest.cs` (the test server startup class)
- **seeds.Dal.Tests** (xUnit test project of the DAL units)
  - **Services**: Unit tests
    - *GET* service test recommendations
      1. `ServiceName_GetModelAsync_ReturnsItself()`
      2. `ServiceName_GetModelAsync_IfNotExistReturnsNull()`
    - *PUT* (depending on whether a PUT must succeed or can fail, e.g. if the PK need not exist)
      1. `ServiceName_PutModelAsync_ReturnsTrue()` or `ServiceName_PutModelAsync_Returns()`
      2. `ServiceName_PutModelAsync_IfNotSuccessReturnsFalse()` or `ServiceName_PutModelAsync_IfNotSuccessThrows()`
    - *POST* service test recommendations
      1. `ServiceName_PostModelAsync_ReturnsTrue()` or ... (cf. PUT)
      2. `ServiceName_PostModelAsync_IfNotSuccessReturnsFalse()` or ... (cf. PUT)
    - e.g. `UsersServiceTests.cs`
    - `DalBaseServiceTests.cs` (generic unit tests)
      - tests that `default(T)` w/ `T` a Model gives `null`
      - distinguishes `HttpStatusCode.NotFound` and other bad responses in *GET* (for the cases where `default(T)` is not a clear sign of error, e.g. `T = typeof(int)`)
- **seeds1.Tests** (xUnit test project of the MAUI App)
  - **ViewModel**: Unit tests. Tests include
    - *Raise Property Changed Event* (PCE) - tests
    - *Navigates to* - tests (using mocked navigation services)

## What You Should Do When...
### **You want to add a new EF Core model class that defines a DB entity** (join entity similar)
  1. :heavy_plus_sign: EF Core model class `seeds.Dal.Model.MyModel.cs`
     - the class's name should be singular and CamelCase, the table should be plural and sql_case
     - the columns should be singular and sql_case
  3. :heavy_plus_sign: DTO Model `MyModelDto.cs` somewhere appropriate in `seeds.Dal.Dto`
  4. :heavy_plus_sign: AutoMapper mappings in `seeds.Api.Helpers.AutoMapperProfiles.cs`
  5. :heavy_plus_sign: configuration class `seeds.Api.Data.MyModelConfiguration.cs` w/ relations and call it in `seedsApiContext.cs`
  6. Migrate to the DB using `dotnet-ef migrations add new_entity_mymodel` in the API's console
  7. Scaffold out a controller by right-clicking `seeds.Api.Controllers` :arrow_right: Add API Controller with actions, using EF :arrow_right: choose `MyModel` as model and the existing context class and hence create `seeds.Api.Controllers.MyModelsController.cs`
     1. delete useless endpoints
     2. adapt the endpoints to take ToDb DTOs and return FromDb DTOs
     3. `HttpUtility.UrlDecode` the parameters (if you'll likely `UrlEncode` them while calling the endpoint)
  9. :heavy_plus_sign: interface `seeds.Dal.Interfaces.IMyModelService.cs`
  10. Implement it in a service class `seeds.Dal.Services.MyModelService.cs` that accesses the endpoints
  11. Register the last two points to the DI container
  12. Use the model in the VMs *a little bit* (to see whether it actually suits your needs) and then write tests `seeds.Api.Tests.Controllers.MyModelsControllerTests.cs` and `seeds.Dal.Tests.Services.MyModelServiceTests.cs`

## Philosophies / Streamlining
### Data Retrieval from DB to MAUI
PerfTip showed that endpoint calls have a naked computation time of `>50ms`, so loops over endpoints are :skull_and_crossbones:.
:bulb::
1. Data that is independent of the `CurrentUser` should be retrieved in rather big :package: upon startup.
   1. this includes static data like `Tag`s or `Category`s
   2. but also `Idea`s (by both `CurrentUser` and any other user)
2. User data like `UserPreference`s or `UserIdeaInteraction`s should also be retrieved in :package:.
   They should be indexed by Guid PKs, so C# can build `Dictionary<Guid, MyUserDataModel>`s that can quickly be accessed. 

### Exception Handling
Throwing exceptions & try-catching them throughout the solution:
1. The first method that is sure that a certain response indicates an error, is the one that needs to `throw new Exception("A message providing all the info");`.
2. The method closest to the view (e.g. directly called by a command in a VM) needs to `try`-`catch (Exception ex)` and `await Shell.Current.DisplayAlert(...);`

### URL En-/Decoding
`HttpUtility.UrlEncode` and `UrlDecode` throughout the solution:
1. The first method that builds the URL should encode
  - ➕ we don't have to pass the URL fragments through the layers (service ➡️ DalBaseService ➡️ HttpWrapper)
  - ➕ we can decide for each concrete paramater separately
  - ➖ in later layers we cannot control the URL anymore
  - ➖ we pass the weird, encoded URL through the layers
2. The endpoint should always decode parameters, just to make sure
  - although C# already decodes a little for you, e.g. spaces are not decoded without `UrlDecode`
