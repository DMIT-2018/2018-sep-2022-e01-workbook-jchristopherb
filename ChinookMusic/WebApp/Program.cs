using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WebApp.Data;

#region Additional Namespaces
using ChinookSystem;
#endregion

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Supplied database connection due to the fact that we created this web app to use Individual Accounts
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// Add another GetConnectionString to reference our DB connectionString

var connectionStringChinook = builder.Configuration.GetConnectionString("ChinookDB");

// Given for the DB connection to DefaultConnection string
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

// Code the DBConnection to the application DB context for ChinookDB
// The implementation of the connect and registration of theChinookSystem services will be done
//      in the ChinookSystem class library
// So to accomplish this task, we will be using an "extension method"
// The extension method will extend the IServiceCollection class
// The extension method requires a parameter options.UseSqlServer(xxxx)
//      where the xxxx is the conenction string variable

builder.Services.ChinookSystemBackendDependencies(options =>
    options.UseSqlServer(connectionStringChinook));


builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddRazorPages();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();

app.Run();
