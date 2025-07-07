using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using StockManagement.Client.Interfaces;
using StockManagement.Client.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.Services.AddAuthorizationCore();
builder.Services.AddCascadingAuthenticationState();
builder.Services.AddAuthenticationStateDeserialization();

builder.Services.AddHttpClient("WebAPI", client => client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress));

// Supply HttpClient instances that include access tokens when making requests to the server project
builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient("WebAPI"));

builder.Services.AddScoped<IActivityDataService, ActivityDataService>();
builder.Services.AddScoped<IDeliveryNoteDetailDataService, DeliveryNoteDetailDataService>();
builder.Services.AddScoped<IDeliveryNoteDataService, DeliveryNoteDataService>();
builder.Services.AddScoped<IJavascriptMethodsService, JavascriptMethodsService>();
builder.Services.AddScoped<ILookupsDataService, LookupsDataService>();
builder.Services.AddScoped<IProductDataService, ProductDataService>();
builder.Services.AddScoped<IProductTypeDataService, ProductTypeDataService>();
builder.Services.AddScoped<IReportDataService, ReportDataService>();
builder.Services.AddScoped<ISupplierDataService, SupplierDataService>();
builder.Services.AddScoped<IVenueDataService, VenueDataService>();

await builder.Build().RunAsync();
