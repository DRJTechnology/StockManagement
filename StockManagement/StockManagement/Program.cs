using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;
using QuestPDF.Infrastructure;
using StockManagement.Client.Interfaces;
using StockManagement.Client.Services;
using StockManagement.ClientDataServices;
using StockManagement.Components;
using StockManagement.Components.Account;
using StockManagement.Models.Automapper;
using StockManagement.Models.Dto.Profile;
using StockManagement.Repositories;
using StockManagement.Repositories.Interfaces;
using StockManagement.Services;
using StockManagement.Services.Interfaces;
using StockManagement.UserManagement;
using System.Data;

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

builder.Services.AddScoped<IActionService, ActionService>();
builder.Services.AddScoped<IActivityService, ActivityService>();
builder.Services.AddScoped<IDeliveryNoteDetailService, DeliveryNoteDetailService>();
builder.Services.AddScoped<IDeliveryNoteService, DeliveryNoteService>();
builder.Services.AddScoped<ILookupsService, LookupsService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IProductTypeService, ProductTypeService>();
builder.Services.AddScoped<IReportService, ReportService>();
builder.Services.AddScoped<IRoleService, RoleService>();
builder.Services.AddScoped<ISettingService, SettingService>();
builder.Services.AddScoped<ISupplierService, SupplierService>();
builder.Services.AddScoped<IStockReceiptDetailService, StockReceiptDetailService>();
builder.Services.AddScoped<IStockReceiptService, StockReceiptService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IVenueService, VenueService>();
builder.Services.AddScoped<IActionRepository, ActionRepository>();
builder.Services.AddScoped<IActivityRepository, ActivityRepository>();
builder.Services.AddScoped<IDeliveryNoteDetailRepository, DeliveryNoteDetailRepository>();
builder.Services.AddScoped<IDeliveryNoteRepository, DeliveryNoteRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IProductTypeRepository, ProductTypeRepository>();
builder.Services.AddScoped<IReportRepository, ReportRepository>();
builder.Services.AddScoped<IRoleRepository, RoleRepository>();
builder.Services.AddScoped<ISettingRepository, SettingRepository>();
builder.Services.AddScoped<ISupplierRepository, SupplierRepository>();
builder.Services.AddScoped<IStockReceiptDetailRepository, StockReceiptDetailRepository>();
builder.Services.AddScoped<IStockReceiptRepository, StockReceiptRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IVenueRepository, VenueRepository>();

// Client data services
builder.Services.AddScoped<IActivityDataService, ClientActivityDataService>();
builder.Services.AddScoped<IDeliveryNoteDetailDataService, ClientDeliveryNoteDetailDataService>();
builder.Services.AddScoped<IDeliveryNoteDataService, ClientDeliveryNoteDataService>();
builder.Services.AddScoped<IJavascriptMethodsService, JavascriptMethodsService>();
builder.Services.AddScoped<ILookupsDataService, ClientLookupsDataService>();
builder.Services.AddScoped<IProductDataService, ClientProductDataService>();
builder.Services.AddScoped<IProductTypeDataService, ClientProductTypeDataService>();
builder.Services.AddScoped<ISupplierDataService, ClientSupplierDataService>();
builder.Services.AddScoped<ISettingDataService, ClientSettingDataService>();
builder.Services.AddScoped<IStockReceiptDetailDataService, ClientStockReceiptDetailDataService>();
builder.Services.AddScoped<IStockReceiptDataService, ClientStockReceiptDataService>();
builder.Services.AddScoped<IReportDataService, ClientReportDataService>();
builder.Services.AddScoped<IVenueDataService, ClientVenueDataService>();

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
