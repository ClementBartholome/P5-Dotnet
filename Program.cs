using System.Security.Claims;
using System.Text.Json.Serialization;
using Express_Voitures.Data;
using Express_Voitures.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args); 
var configuration = builder.Configuration;

if (builder.Environment.IsDevelopment())
{
    builder.Services.AddDbContext<AppDbContext>(options =>
        options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
}
else
{
    builder.Services.AddDbContext<AppDbContext>(options =>
        options.UseSqlServer(Environment.GetEnvironmentVariable("SQLAZURECONNSTR_prod")));
}

builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<AppDbContext>();
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Admin";
});

builder.Services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();

builder.Services.AddControllersWithViews();

builder.Services.AddTransient<IEmailSender, EmailSender>();
builder.Services.Configure<AuthMessageSenderOptions>(builder.Configuration);

builder.Services.AddAuthorizationBuilder()
    .AddPolicy("Admin", policyBuilder =>
        policyBuilder.RequireClaim("Admin"));

builder.Services.AddScoped<VoitureService>();
builder.Services.AddScoped<MarqueService>();
builder.Services.AddScoped<ModeleService>();
builder.Services.AddScoped<FinitionService>();
builder.Services.AddScoped<AnnonceService>();

builder.Services.AddControllersWithViews().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    using var scope = app.Services.CreateScope();
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    dbContext.Database.Migrate();
    
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
    var rootUser = new ApplicationUser { UserName = "admin@expressvoitures.com", Email = "admin@expressvoitures.com", EmailConfirmed = true };

    if (await userManager.FindByNameAsync(rootUser.UserName) == null)
    {
        await userManager.CreateAsync(rootUser, "Password123$");
        await userManager.AddClaimAsync(rootUser, new Claim("Admin", "true"));
    }
}

if (app.Environment.IsProduction())
{
    using var scope = app.Services.CreateScope();
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    dbContext.Database.Migrate();
    
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
    var rootUser = new ApplicationUser { UserName = Environment.GetEnvironmentVariable("ROOT_USERNAME"), Email = Environment.GetEnvironmentVariable("ROOT_EMAIL"), EmailConfirmed = true };

    if (await userManager.FindByNameAsync(rootUser.UserName) == null)
    {
        await userManager.CreateAsync(rootUser, Environment.GetEnvironmentVariable("ROOT_PASSWORD"));
        await userManager.AddClaimAsync(rootUser, new Claim("Admin", "true"));
    }
}

var logger = app.Services.GetRequiredService<ILogger<Program>>();

logger.LogInformation("Connection string: {ConnectionString}", configuration.GetConnectionString("DefaultConnection"));

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
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

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");
    endpoints.MapRazorPages(); 
});

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();