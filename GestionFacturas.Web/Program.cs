using GestionFacturas.AccesoDatosSql;
using GestionFacturas.Aplicacion;
using GestionFacturas.Dominio;
using GestionFacturas.Web.Pages.Facturas;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme) // Sets the default scheme to cookies
    .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
    {
        options.AccessDeniedPath = "/seguridad/acceso/accesodenegado";
        options.LoginPath = "/seguridad/acceso/entrar";
    });

builder.Services.AddAuthorization(options =>
{
    options.FallbackPolicy = new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .Build();

    // Crear otras policy
});
var connectionString = builder.Configuration.GetConnectionString("ConnectionString");
builder.Services.AddScoped(m=> 
    new SqlDb(connectionString));


// Add services to the container.
builder.Services.AddRazorPages(options =>
{
    options.Conventions.AddPageRoute(ListaGestionFacturasModel.NombrePagina, "");

    options.Conventions.AuthorizeFolder("/");
    options.Conventions.AllowAnonymousToFolder("/seguridad/acceso");
});

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.PropertyNameCaseInsensitive = false;
    options.JsonSerializerOptions.PropertyNamingPolicy = null;
    options.JsonSerializerOptions.WriteIndented = true;
});


builder.Services.AddScoped<ServicioEmail>();
builder.Services.AddScoped<ServicioFactura>();
builder.Services.AddScoped<ServicioCliente>();

var app = builder.Build();

// Configure the HTTP request pipeline.
//if (!app.Environment.IsDevelopment())
//{
//    app.UseExceptionHandler("/Error");
//    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
//    app.UseHsts();
//}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();
app.MapDefaultControllerRoute();

app.Run();
