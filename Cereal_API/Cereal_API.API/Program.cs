using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add authentication
builder.Services.AddAuthentication("Basic").AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>("Basic", null);
builder.Services.AddAuthorization();
builder.Services.AddHttpContextAccessor();

// Add SQLite database service
builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlite(AppDbContext.ConnectionPath));

var imageFolder = Path.Combine(builder.Environment.ContentRootPath, "Database", builder.Configuration["ImageSettings:Folder"]!);
builder.Services.AddTransient<ICerealService>(provider =>
{
    return new CerealService(provider.GetRequiredService<AppDbContext>(), provider.GetRequiredService<IHttpContextAccessor>(), imageFolder);
});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Basic", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "basic",
        In = ParameterLocation.Header,
        Description = "Basic Authentication"
    });

    options.OperationFilter<AuthorizeCheckOperationFilter>();
});

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

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
