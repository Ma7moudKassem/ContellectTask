var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

builder.Services.AddScoped<Toaster>();
builder.Services.AddSingleton<InteropSettings>();

builder.Services.AddScoped<IContactsHttpInterceptor, ContactsHttpInterceptor>();
builder.Services.AddScoped<AuthenticationStateProvider, AuthStateProvider>();

builder.Services.AddBlazorBootstrap();
builder.Services.AddLocalization();
builder.Services.AddAuthorizationCore();
builder.Services.AddBlazoredLocalStorage();

WebAssemblyHost host = builder.Build();

await host.SetDefaultCulture();

await host.RunAsync();