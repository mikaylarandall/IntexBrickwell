using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using IntexBrickwell.Data;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;

var builder = WebApplication.CreateBuilder(args);

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
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();
app.Run();