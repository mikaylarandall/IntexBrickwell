using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using IntexBrickwell.Data;
using Azure.Identity;
using IntexBrickwell.Middleware;
using Azure.Security.KeyVault.Secrets;
using IntexBrickwell.Models;
using ApplicationDbContext = IntexBrickwell.Data.ApplicationDbContext;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthentication().AddGoogle(googleOptions =>
{
    googleOptions.ClientId = builder.Configuration["Authentication:Google:ClientId"];
    googleOptions.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"];
});

var keyVaultUri = "https://intexkv.vault.azure.net/";
builder.Configuration.AddAzureKeyVault(new Uri(keyVaultUri), new DefaultAzureCredential());

// Add services to the container.
// Fetch the database password secret from the configuration
var dbPassword = builder.Configuration["DatabasePassword"]; // Ensure this matches the name of your secret in Azure Key Vault

// Retrieve the connection string without the password
var baseConnectionString = builder.Configuration.GetConnectionString("DefaultConnection") ??
                           throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

// Replace the placeholder in the connection string with the actual password
var connectionString = baseConnectionString.Replace("{PasswordPlaceholder}", dbPassword);

// Configure your DbContext to use SQL Server with the updated connection string and enable transient error retry logic
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString, sqlServerOptions => sqlServerOptions.EnableRetryOnFailure(
        maxRetryCount: 5, // Maximum number of retry attempts
        maxRetryDelay: TimeSpan.FromSeconds(30), // Maximum delay between retries
        errorNumbersToAdd: null))); // SQL error numbers to consider for retries, null for defaults

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<IntexBrickwell.Data.ApplicationDbContext>();
builder.Services.AddControllersWithViews();

builder.Services.AddHsts(options =>
{
    options.Preload = true;
    options.IncludeSubDomains = true;
    options.MaxAge = TimeSpan.FromDays(60);
    // options.ExcludedHosts.Add("example.com");
    // options.ExcludedHosts.Add("www.example.com");
});

builder.Services.Configure<IdentityOptions>(options =>
{
    // Default Password settings.
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequireUppercase = true;
    options.Password.RequiredLength = 10;
    options.Password.RequiredUniqueChars = 1;
});

builder.Services.AddScoped<IOrderRepository, EFOrderRepository>();
builder.Services.AddScoped<IUserRepository, EFUserRepository>();
builder.Services.AddScoped<IProductRepository, EFProductRepository>();
builder.Services.AddScoped<ICustomerRepository, EFCustomerRepository>();
builder.Services.AddScoped<ILineItemRepository, EFLineItemRepository>();

// Register session services
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(20); // Set session timeout
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true; // Make session cookie essential
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

// Use session middleware
app.UseSession(); // This line must be added before UseRouting()

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.UseMiddleware<CookieConsent>();

app.Run();
