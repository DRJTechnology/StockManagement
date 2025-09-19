using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.JSInterop;
using StockManagement.Client.Interfaces;
using StockManagement.Client.Interfaces.Finance;
using StockManagement.Client.Services;
using StockManagement.Client.Services.Finance;
using System.Globalization;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.Services.AddAuthorizationCore();
builder.Services.AddCascadingAuthenticationState();
builder.Services.AddAuthenticationStateDeserialization();

builder.Services.AddHttpClient("WebAPI", client => client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress));

// Supply HttpClient instances that include access tokens when making requests to the server project
builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient("WebAPI"));

builder.Services.AddScoped<IAccountDataService, AccountDataService>();
builder.Services.AddScoped<IAccountTypeDataService, AccountTypeDataService>();
builder.Services.AddScoped<IActivityDataService, ActivityDataService>();
builder.Services.AddScoped<IContactDataService, ContactDataService>();
builder.Services.AddScoped<IDeliveryNoteDataService, DeliveryNoteDataService>();
builder.Services.AddScoped<IDeliveryNoteDetailDataService, DeliveryNoteDetailDataService>();
builder.Services.AddScoped<IJavascriptMethodsService, JavascriptMethodsService>();
builder.Services.AddScoped<ILookupsDataService, LookupsDataService>();
builder.Services.AddScoped<IProductDataService, ProductDataService>();
builder.Services.AddScoped<IProductTypeDataService, ProductTypeDataService>();
builder.Services.AddScoped<IReportDataService, ReportDataService>();
builder.Services.AddScoped<ISettingDataService, SettingDataService>();
builder.Services.AddScoped<IStockOrderDetailDataService, StockOrderDetailDataService>();
builder.Services.AddScoped<IStockOrderDataService, StockOrderDataService>();
builder.Services.AddScoped<IStockSaleDataService, StockSaleDataService>();
builder.Services.AddScoped<IStockSaleDetailDataService, StockSaleDetailDataService>();
builder.Services.AddScoped<ILocationDataService, LocationDataService>();
builder.Services.AddScoped<ITransactionDataService, TransactionDataService>();
builder.Services.AddScoped<IInventoryBatchDataService, InventoryBatchDataService>();

var host = builder.Build();

// Set the culture globally to en-GB (UK)
var js = host.Services.GetRequiredService<IJSRuntime>();
var culture = new CultureInfo("en-GB");
CultureInfo.DefaultThreadCurrentCulture = culture;
CultureInfo.DefaultThreadCurrentUICulture = culture;

await host.RunAsync();