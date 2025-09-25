using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;
using QuestPDF.Infrastructure;
using StockManagement.Client.Interfaces;
using StockManagement.Client.Interfaces.Finance;
using StockManagement.Client.Services;
using StockManagement.ClientDataServices;
using StockManagement.ClientDataServices.Finance;
using StockManagement.Components;
using StockManagement.Components.Account;
using StockManagement.Models.Automapper;
using StockManagement.Models.Dto.Profile;
using StockManagement.Repositories;
using StockManagement.Repositories.Finance;
using StockManagement.Repositories.Interfaces;
using StockManagement.Repositories.Interfaces.Finanace;
using StockManagement.Services;
using StockManagement.Services.Finanace;
using StockManagement.Services.Interfaces;
using StockManagement.Services.Interfaces.Finance;
using StockManagement.UserManagement;
using System.Data;
using System.Globalization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveWebAssemblyComponents()
    .AddAuthenticationStateSerialization();

builder.Services.AddCascadingAuthenticationState();
builder.Services.AddScoped<IdentityUserAccessor>();
builder.Services.AddScoped<IdentityRedirectManager>();

builder.Services.AddAuthentication(options =>
    {
        options.DefaultScheme = IdentityConstants.ApplicationScheme;
        options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
    })
    .AddIdentityCookies();
builder.Services.AddAuthorization();

builder.Services.AddTransient<IDbConnection>(db => new SqlConnection(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddIdentityCore<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddUserStore<UserStore>()
    .AddRoles<ApplicationRole>()
    .AddRoleStore<RoleStore>()
    .AddClaimsPrincipalFactory<CustomUserClaimsPrincipalFactory>()
    .AddSignInManager()
    .AddDefaultTokenProviders();

builder.Services.AddSingleton<IEmailSender<ApplicationUser>, IdentityNoOpEmailSender>();

builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<IAccountTypeService, AccountTypeService>();
builder.Services.AddScoped<IActionService, ActionService>();
builder.Services.AddScoped<IActivityService, ActivityService>();
builder.Services.AddScoped<IContactService, ContactService>();
builder.Services.AddScoped<IDeliveryNoteDetailService, DeliveryNoteDetailService>();
builder.Services.AddScoped<IDeliveryNoteService, DeliveryNoteService>();
builder.Services.AddScoped<ILookupsService, LookupsService>();
builder.Services.AddScoped<IInventoryBatchService, InventoryBatchService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IProductTypeService, ProductTypeService>();
builder.Services.AddScoped<IReportService, ReportService>();
builder.Services.AddScoped<IRoleService, RoleService>();
builder.Services.AddScoped<ISettingService, SettingService>();
builder.Services.AddScoped<IStockOrderDetailService, StockOrderDetailService>();
builder.Services.AddScoped<IStockOrderService, StockOrderService>();
builder.Services.AddScoped<IStockSaleDetailService, StockSaleDetailService>();
builder.Services.AddScoped<IStockSaleService, StockSaleService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ILocationService, LocationService>();
builder.Services.AddScoped<ITransactionService, TransactionService>();
builder.Services.AddScoped<IAccountRepository, AccountRepository>();
builder.Services.AddScoped<IAccountTypeRepository, AccountTypeRepository>();
builder.Services.AddScoped<IActionRepository, ActionRepository>();
builder.Services.AddScoped<IActivityRepository, ActivityRepository>();
builder.Services.AddScoped<IContactRepository, ContactRepository>();
builder.Services.AddScoped<IDeliveryNoteDetailRepository, DeliveryNoteDetailRepository>();
builder.Services.AddScoped<IDeliveryNoteRepository, DeliveryNoteRepository>();
builder.Services.AddScoped<IInventoryBatchRepository, InventoryBatchRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IProductTypeRepository, ProductTypeRepository>();
builder.Services.AddScoped<IReportRepository, ReportRepository>();
builder.Services.AddScoped<IRoleRepository, RoleRepository>();
builder.Services.AddScoped<ISettingRepository, SettingRepository>();
builder.Services.AddScoped<IStockOrderDetailRepository, StockOrderDetailRepository>();
builder.Services.AddScoped<IStockOrderRepository, StockOrderRepository>();
builder.Services.AddScoped<IStockSaleDetailRepository, StockSaleDetailRepository>();
builder.Services.AddScoped<IStockSaleRepository, StockSaleRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<ILocationRepository, LocationRepository>();
builder.Services.AddScoped<ITransactionRepository, TransactionRepository>();
builder.Services.AddScoped<IErrorLogRepository, ErrorLogRepository>();

// Client data services
builder.Services.AddScoped<IAccountDataService, ClientAccountDataService>();
builder.Services.AddScoped<IAccountTypeDataService, ClientAccountTypeDataService>();
builder.Services.AddScoped<IActivityDataService, ClientActivityDataService>();
builder.Services.AddScoped<IContactDataService, ClientContactDataService>();
builder.Services.AddScoped<IDeliveryNoteDetailDataService, ClientDeliveryNoteDetailDataService>();
builder.Services.AddScoped<IDeliveryNoteDataService, ClientDeliveryNoteDataService>();
builder.Services.AddScoped<IInventoryBatchDataService, ClientInventoryBatchDataService>();
builder.Services.AddScoped<IJavascriptMethodsService, JavascriptMethodsService>();
builder.Services.AddScoped<ILookupsDataService, ClientLookupsDataService>();
builder.Services.AddScoped<IProductDataService, ClientProductDataService>();
builder.Services.AddScoped<IProductTypeDataService, ClientProductTypeDataService>();
builder.Services.AddScoped<IReportDataService, ClientReportDataService>();
builder.Services.AddScoped<ISettingDataService, ClientSettingDataService>();
builder.Services.AddScoped<IStockOrderDetailDataService, ClientStockOrderDetailDataService>();
builder.Services.AddScoped<IStockOrderDataService, ClientStockOrderDataService>();
builder.Services.AddScoped<IStockSaleDataService, ClientStockSaleDataService>();
builder.Services.AddScoped<IStockSaleDetailDataService, ClientStockSaleDetailDataService>();
builder.Services.AddScoped<ILocationDataService, ClientLocationDataService>();
builder.Services.AddScoped<ITransactionDataService, ClientTransactionDataService>();

// Auto Mapper Configurations
var mappingConfig = new MapperConfiguration(mc =>
{
    mc.AddProfile(new MappingProfile());
});
IMapper mapper = mappingConfig.CreateMapper();
builder.Services.AddSingleton(mapper);

// Add API Endpoints
builder.Services.AddControllers();

var app = builder.Build();

// Set the culture globally to en-GB (UK)
var culture = new CultureInfo("en-GB");
CultureInfo.DefaultThreadCurrentCulture = culture;
CultureInfo.DefaultThreadCurrentUICulture = culture;

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

//app.UseHttpsRedirection();

app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(StockManagement.Client._Imports).Assembly);

// Add additional endpoints required by the Identity /Account Razor components.
app.MapAdditionalIdentityEndpoints();

app.MapControllers();

QuestPDF.Settings.License = LicenseType.Community;

app.Run();
