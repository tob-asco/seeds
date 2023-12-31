# How I Have Resolved the Tough Nuts of Errors
- (02.09.2023) `StatusCode=500(Internal Server Error)` returned by the GET endpoint for paginated feed entries
  - :tornado: I eagerly loaded a many-to-many navigation property `Idea.Tags` via `context.Idea.Include(i => i.Tags)`. But then this also includes the nav. prop. `Tag.Ideas`, which again includes `Idea.Tags`, ... ad infinitum.
  - :sun_with_face: Either project the query to the first layer of nav. props., like `.Include(i => i.Tags).Select(i => new Idea(){ Id = i.Id, ..., Tags = i.Tags})`. Annoying part is that you have to manually list all columns of the table `Idea.cs`
  - :sun_with_face: Or delete one of the navigation properties. E.g., do you need to have a navigation `Tag.Ideas`? Probably yes, so this is also annoying.
  - :sun_with_face: Or maybe this SE answer https://stackoverflow.com/a/53780466 claiming that `options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;` works well (haven't tried).
- (03.08.2023) `System.InvalidOperationException` thrown in a PUT endpoint on `_context.Entry(myEntity).State = EntityState.Modified;`
  - :tornado: Within the PUT endpoint, `myEntity` has already been added to the `_context`'s *Change Tracker* (because I retrieved it through a `FirstOrDefault()`) and it is forbidden to modify an entity's state explicitly if it is already part of the *Change Tracker*.
    The reason is that the changes made before (of which EF Core assumes some exist because `myEntity` is listed in the *Change Tracker*) would be lost.
  - :sun_with_face: Either clear the *Change Tracker* before PUTting (:thumbsdown:) or simply change `myEntity` directly (using its setter methods or so) and it will be tracked *because it has already been added to the `_context`'s Change Tracker* beforehand (:thumbsup:)
- (01.08.2023) `System.IO.FileNotFoundException` thrown on `InitializeComponents()` in `App.xaml.cs`
  - :tornado: I enabled the option to break on all "Common Language Runtime Exceptions" in the Exception settings
  - :sun_with_face: Open the exception settings in VS22 and uncheck to break on all of the above mentioned exceptions.
    Either break on non of them, or on the standard ones (find them by googling, or opening a vanilla intallation or so)
- (30.07.2023) Non-deterministic behaviour of the API endpoints when using a DevTunnel
  - :tornado: The concrete DevTunnel in use
  - :sun_with_face: New DevTunnel (public - private showed some other errors), or use localhost
- (08.07.2023) User-Unhandled Exception thrown in `JNINativeWrapper.g.cs` (Android) or `App.g.i.cs` (Windows)
  - :tornado: Non-registered dependencies
  - :sun_with_face: Register all interfaces to the app builder of each project that are DI'ed into some constructor.
    Also register all VMs and the respective pages/views.
    This is done in `MauiProgram.cs`, the entry point of the app.
