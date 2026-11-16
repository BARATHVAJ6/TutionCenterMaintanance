using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;
using TutionCenterMaintanance.Client.Auth;
using TutionCenterMaintanance.Client.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

// Register HttpClient
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

// Register MudBlazor
builder.Services.AddMudServices();

// Authorization & LocalStorage
builder.Services.AddAuthorizationCore();
builder.Services.AddScoped<ILocalStorageService, LocalStorageService>();
builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthStateProvider>();
builder.Services.AddScoped<IApiService, ApiService>();

await builder.Build().RunAsync();
