using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.Hosting;
using PortfolioMaster.Models;
using PortfolioMaster.Data;
using Microsoft.AspNetCore.Identity.UI.Services;
using PortfolioMaster.Services;
using Hangfire;
using PortfolioMaster.Workers;
using Hangfire.SqlServer;
using PortfolioMaster.Contexts;

var builder = WebApplication.CreateBuilder(args);

// Update the builder to include UserSecrets for the development environment
builder.Configuration.AddUserSecrets<Program>(optional: true);

builder.Services.AddIdentity<User, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

// Add HttpClient in ConfigureServices
builder.Services.AddHttpClient();

builder.Services.AddAuthentication()
    .AddGoogle(options =>
    {
        options.ClientId = builder.Configuration["Authentication:Google:ClientId"];
        options.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"];
    })
    .AddMicrosoftAccount(options =>
    {
        options.ClientId = builder.Configuration["Authentication:Microsoft:ClientId"];
        options.ClientSecret = builder.Configuration["Authentication:Microsoft:ClientSecret"];
    });

// Add default DB
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// register email handler
builder.Services.AddSingleton<IEmailSender, EmailSender>();

builder.Services.AddScoped<PreciousMetalsService>();
builder.Services.AddScoped<AssetHoldingService>();
builder.Services.AddScoped<StockService>();
builder.Services.AddScoped<AssetService>();
builder.Services.AddScoped<IPortfolioService, PortfolioService>();


// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

if (builder.Environment.IsDevelopment())
{
    // Allow anonymous access in the development environment
    builder.Services.AddMvc(options =>
    {
        options.Filters.Add(new AllowAnonymousFilter());
    });
}
else
{
    // Require authentication in the production environment
    builder.Services.AddMvc(options =>
    {
        var policy = new AuthorizationPolicyBuilder()
            .RequireAuthenticatedUser()
            .Build();
        options.Filters.Add(new AuthorizeFilter(policy));
    });
}

builder.Services.AddHangfire(configuration => configuration
    .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
    .UseSimpleAssemblyNameTypeSerializer()
    .UseRecommendedSerializerSettings()
    .UseSqlServerStorage(builder.Configuration.GetConnectionString("DefaultConnection"), new SqlServerStorageOptions
    {
        CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
        SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
        QueuePollInterval = TimeSpan.Zero,
        UseRecommendedIsolationLevel = true,
        DisableGlobalLocks = true
    }));

builder.Services.AddHangfireServer();

var app = builder.Build();

app.UseHangfireDashboard();
RecurringJob.AddOrUpdate<PreciousMetalsPriceUpdater>(updater => updater.UpdatePrices(), Cron.Daily);


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    // Seed the database with mock data in Development mode
    using (var scope = app.Services.CreateScope())
    {
        var services = scope.ServiceProvider;
        try
        {
            var dbContext = services.GetRequiredService<ApplicationDbContext>();
            var userManager = services.GetRequiredService<UserManager<User>>();
            await DbInitializer.Initialize(services);
        }
        catch (Exception ex)
        {
            var logger = services.GetRequiredService<ILogger<Program>>();
            logger.LogError(ex, "An error occurred while seeding the database.");
        }
    }
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();

}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseAuthentication();
app.UseRouting();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");
    endpoints.MapRazorPages();
});


app.Run();

