using Microsoft.EntityFrameworkCore;
using PruebaProgra2.Models;
using PruebaProgra2.Workers;

var builder = WebApplication.CreateBuilder(args);

// Configurar la base de datos
builder.Services.AddDbContext<ProyectoPrograDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("ProyectoConnection")));

// Registrar el TaskWorkerService como un servicio en segundo plano
builder.Services.AddSingleton<TaskWorkerService>();

// Agregar los controladores con vistas
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
