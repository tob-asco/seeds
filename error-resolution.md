# How I Have Resolved the Tough Nuts of Errors
- (01.08.23) `System.IO.FileNotFoundException` thrown on `InitializeComponents()` in `App.xaml.cs`
  - Problem: I have turned on to break on all "Common Language Runtime Exceptions" in the Exception settings
  - Solution: Open the exception settings in VS22 and uncheck to break on all of the above mentioned exceptions.
    Either break on non of them, or on the standard ones (find them by googling, or opening a vanilla intallation or so)
- (30.07.23) Non-deterministic behaviour of the API endpoints when using a DevTunnel
  - Problem: The concrete DevTunnel in use
  - Solution: New DevTunnel (public - private showed some other errors), or use localhost
- (08.07.23) User-Unhandled Exception thrown in `JNINativeWrapper.g.cs` (Android) or `App.g.i.cs` (Windows)
  - Problem: Non-registered dependencies
  - Solution: Register all interfaces of each project that are DI'ed into some constructor.
    Also register all VMs and the respective pages/views.
