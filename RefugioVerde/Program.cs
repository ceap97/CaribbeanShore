using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.EntityFrameworkCore;
using RefugioVerde.Models;
using RefugioVerde.Servicios.Contrato;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<RefugioVerdeContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("conexion")));
builder.Services.AddScoped<IUsuarioServices, UsuariosSevices>();
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;
})
.AddCookie(options =>
{
    options.LoginPath = "/Inicio/IniciarSesion";
    options.ExpireTimeSpan = TimeSpan.FromMinutes(20);
})
.AddGoogle(options =>
{
    options.ClientId = "881115237966-9r2rcn5il1p1cckmsksmd6td28jabqrn.apps.googleusercontent.com";
    options.ClientSecret = "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";
    options.CallbackPath = "/Inicio/GoogleResponse";
});
builder.Services.AddControllersWithViews(options =>
{
    options.Filters.Add(
    new ResponseCacheAttribute
    {
        NoStore = true,
        Location = ResponseCacheLocation.None,
    }
    );
});
builder.WebHost.ConfigureKestrel(serverOptions =>
{
    serverOptions.ConfigureEndpointDefaults(listenOptions =>
    {
        listenOptions.Protocols = HttpProtocols.Http1;
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
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

app.MapControllerRoute(
    name: "dashboard",
    pattern: "Dashboard",
    defaults: new { controller = "Usuarios", action = "Dashboard" });
app.MapControllerRoute(
    name: "Cliente",
    pattern: "Cliente",
    defaults: new { controller = "Home", action = "Cliente" });
app.Run();
