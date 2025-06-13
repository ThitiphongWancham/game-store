using GameStore.API.Data;
using GameStore.API.Features.Games;
using GameStore.API.Features.Genres;
using GameStore.API.Shared.ErrorHandling;
using GameStore.API.Shared.FileUpload;
using GameStore.API.Shared.Timing;
using Microsoft.AspNetCore.HttpLogging;

// Builder will be used to create our application
var builder = WebApplication.CreateBuilder(args);

// For global exception handling with problem details in response
builder.Services.AddProblemDetails().AddExceptionHandler<GlobalExceptionHandler>();

// Register the db-context as a scoped service
var connString = builder.Configuration.GetConnectionString("GameStore");
builder.Services.AddSqlite<GameStoreContext>(connString);
// Another way to register the db context
// builder.Services.AddDbContext<GameStoreContext>(options => options.UseSqlite(connString));

// For HttpLogging middleware
// In order to use HttpLogging middleware, we need to inject a service and enable it in appsettings.json
builder.Services.AddHttpLogging(options =>
{
    options.LoggingFields = HttpLoggingFields.RequestMethod | HttpLoggingFields.RequestPath |
                            HttpLoggingFields.ResponseStatusCode | HttpLoggingFields.Duration;
    options.CombineLogs = true;
});

// For Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// For File-Upload service
builder.Services.AddHttpContextAccessor().AddSingleton<FileUploader>();

// Build an application
var app = builder.Build();

// Map endpoints with our services
app.MapGames();
app.MapGenres();

// For HttpLogging middleware
app.UseHttpLogging();

if (app.Environment.IsDevelopment())
{
    // For Swagger
    app.UseSwagger();
}
else
{
    // For global exception handling
    app.UseExceptionHandler();
}

// For global exception handling
app.UseStatusCodePages();

// Custom middleware (Experiment)
app.UseMiddleware<RequestTimingMiddleware>();

await app.InitializeDb();

app.Run();