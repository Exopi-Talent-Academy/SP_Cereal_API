using Cereal_API.API.Database;
using Cereal_API.API.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// SQLite database service
builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlite(AppDbContext.ConnectionPath));

var imageFolder = Path.Combine(builder.Environment.ContentRootPath, "Database", builder.Configuration["ImageSettings:Folder"]!);
builder.Services.AddTransient<ICerealService>(provider =>
{
    return new CerealService(provider.GetRequiredService<AppDbContext>(), imageFolder);
});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Initialize the database using the cereal.csv file
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    await db.Database.EnsureCreatedAsync();
    await SeedData.InitializeAsync(db);
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
