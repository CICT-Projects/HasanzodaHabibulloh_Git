using backend.Data;
using backend.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// --- 1. ДОБАВЛЕНИЕ ВСЕХ СЕРВИСОВ (ДО builder.Build()) ---

// Add DbContext
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite("Data Source=warehouse.db"));

// Add MVC support
builder.Services.AddControllersWithViews();

// Add CORS (ВАЖНО: должно быть ДО builder.Build())
builder.Services.AddCors(options => {
    options.AddDefaultPolicy(policy => {
        policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
    });
});

// --- 2. СБОРКА ПРИЛОЖЕНИЯ ---
var app = builder.Build();

// --- 3. СОЗДАНИЕ БД ---
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.EnsureCreated();
}

// --- 4. НАСТРОЙКА PIPELINE (middleware) ---

// Middleware для обработки исключений
app.Use(async (context, next) =>
{
    try
    {
        await next.Invoke();
    }
    catch (Exception ex)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = 500;
        await context.Response.WriteAsJsonAsync(new { error = ex.Message });
    }
});

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();

// Установить корневую папку для статических файлов
app.UseStaticFiles();

app.UseRouting();

// Включить CORS перед маршрутизацией
app.UseCors();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// Маршрут для CRUD интерфейса
app.MapGet("/", () => Results.Redirect("/crud.html"));

// Запустить приложение
try
{
    app.Urls.Add("http://localhost:5016");
    app.Run();
}
catch (Exception ex)
{
    Console.WriteLine($"Критическая ошибка при запуске: {ex.Message}");
    throw;
}
