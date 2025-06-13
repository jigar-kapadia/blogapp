using BloggingApp.Persistence;
using BloggingApp.Persistence.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

//SQLite
builder.Services.AddDbContext<ApplicationDbContext>((options) =>
{
    options.UseSqlite("Data Source=bloggingapp.db");
    //builder.Configuration.GetConnectionString("DefaultConnection"));

    // Register OpenIddict services
    options.UseOpenIddict();
});

// Add ASP.NET Identity with custom user
builder.Services.AddIdentity<AppUser, IdentityRole>(options =>
{
    options.Password.RequireDigit = false;
    options.Password.RequiredLength = 6;
    options.Password.RequireLowercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
}).AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();

// Add OpenIddict services
builder.Services.AddOpenIddict()
    .AddCore((options) =>
    {
        options.UseEntityFrameworkCore()
            .UseDbContext<ApplicationDbContext>();
    })
    .AddServer((options) =>
    {
        options.AllowPasswordFlow();
        options.AllowRefreshTokenFlow();
        options.SetTokenEndpointUris("/connect/token");
        options.AcceptAnonymousClients();
        options.UseAspNetCore()
               .EnableTokenEndpointPassthrough();

        //     .AllowClientCredentialsFlow()
        //     .SetTokenEndpointUris("/connect/token")
        //     .SetAuthorizationEndpointUris("/connect/authorize")
        //     .SetLogoutEndpointUris("/connect/logout")
        //     .SetUserinfoEndpointUris("/connect/userinfo");
        // // options.AllowAuthorizationCodeFlow()
        //     .AllowRefreshTokenFlow()
        //     .SetAccessTokenLifetime(TimeSpan.FromHours(1))
        //     .SetAuthorizationCodeLifetime(TimeSpan.FromMinutes(5))
        //     .SetRefreshTokenLifetime(TimeSpan.FromDays(14));

        // options.SetIssuer(new Uri("https://localhost:5001/"));
        // options.SetSigningKey(new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes("your-very-secure-signing-key")));


        // Only for development
        options.AddDevelopmentEncryptionCertificate()
               .AddDevelopmentSigningCertificate();

        // for production, you would typically use a real certificate
        // .AddEncryptionCertificate("path-to-cert.pfx", "password")
        //.AddSigningCertificate("path-to-cert.pfx", "password")
    })
    .AddValidation((options) =>
    {
        options.UseLocalServer();
        options.UseAspNetCore();

        // options.UseReferenceAccessTokens();
        // options.UseReferenceRefreshTokens();
    });

// builder.Services.AddAuthentication()
//     .AddIdentityCookies();

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddRazorPages();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();
app.MapRazorPages();
app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

 
app.Run();
