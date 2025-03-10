using Microsoft.EntityFrameworkCore;
using MVCRestaurante.Repositories;

var builder = WebApplication.CreateBuilder(args);

// CONFIGURAR BASE DE DATOS CON ENTITY FRAMEWORK
builder.Services.AddDbContext<RestauranteContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("SqlRestaurante")
    )
);

// CONFIGURAR CACH� EN MEMORIA
builder.Services.AddMemoryCache();

// INYECTAR REPOSITORIOS
builder.Services.AddScoped<IRepositoryRestaurante, RepositoryRestaurante>();
builder.Services.AddScoped<RepositoryMenu>();

// AGREGAR CONTROLADORES CON VISTAS
builder.Services.AddControllersWithViews();

// CONSTRUIR LA APLICACI�N
var app = builder.Build();

// CONFIGURAR EL PIPELINE DE MIDDLEWARE
if (!app.Environment.IsDevelopment())
{
    // MANEJO DE ERRORES EN PRODUCCI�N
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

// REDIRECCIONAR A HTTPS
app.UseHttpsRedirection();

// SERVIR ARCHIVOS EST�TICOS (CSS, JS, ETC.)
app.UseStaticFiles();

// HABILITAR RUTEO
app.UseRouting();

// HABILITAR AUTORIZACI�N
app.UseAuthorization();

// CONFIGURAR RUTA POR DEFECTO
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}"
);

// EJECUTAR LA APLICACI�N
app.Run();
