using Microsoft.EntityFrameworkCore;
using CruiseCompany.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<CruiseDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddCors(o => o.AddDefaultPolicy(p =>
    p.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader()));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// ¬ызов сидера при старте
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<CruiseDbContext>();
    db.Database.Migrate(); // примен€ем миграции
    DbSeeder.Seed(db);     // заполн€ем тестовыми данными
}

app.UseAuthorization();
app.MapControllers();
app.Run();
app.UseCors();
