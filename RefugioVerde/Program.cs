using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.EntityFrameworkCore;
using RefugioVerde.Models;
using RefugioVerde.Servicios.Contrato;
using RefugioVerde.Servicios.Implementacion;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<RefugioVerdeContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("conexion")));
builder.Services.AddScoped<IUsuarioServices, UsuariosSevices>();
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
.AddCookie(options =>
{
    options.LoginPath = "/Inicio/IniciarSesion";
    options.ExpireTimeSpan = TimeSpan.FromMinutes(20);
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
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.Use(async (context, next) =>
{
    var user = context.User;
    if (user.Identity.IsAuthenticated)
    {
        var usuarioService = context.RequestServices.GetRequiredService<IUsuarioServices>();
        var usuario = await usuarioService.GetUsuario(user.Identity.Name, null); // Assuming null for the second parameter
        if (usuario != null)
        {
            if (context.Request.Path.StartsWithSegments("/Dashboard") && usuario.EmpleadoId == null)
            {
                context.Response.Redirect("/Home/Index");
                return;
            }
            if (context.Request.Path.StartsWithSegments("/Cliente") && usuario.EmpleadoId != null)
            {
                context.Response.Redirect("/Home/Index");
                return;
            }
        }
    }
    await next();
});

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
