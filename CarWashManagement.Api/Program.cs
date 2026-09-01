using CarWashManagement.Api.Services;
using CarWashManagement.Api.Data;
using Microsoft.EntityFrameworkCore;
var builder = WebApplication.CreateBuilder(args);

// Add services to the dependency injection container.
builder.Services.AddControllers();
builder.Services.AddDbContext<CarWashDbContext>(options =>
    options.UseSqlite(
        builder.Configuration.GetConnectionString("DefaultConnection")
    ));
builder.Services.AddScoped<CustomerService>();
builder.Services.AddScoped<VehicleService>();
builder.Services.AddScoped<WashStationService>();
builder.Services.AddScoped<DbInitializer>();
builder.Services.AddScoped<WashTransactionService>();
builder.Services.AddHostedService<WashCompletionBackgroundService>();

// Add OpenAPI documentation.
builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen();

var app = builder.Build();
using (var scope = app.Services.CreateScope())
{
    var initializer = scope.ServiceProvider.GetRequiredService<DbInitializer>();
    await initializer.InitializeAsync();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers();
app.UseDefaultFiles();
app.UseStaticFiles();

app.Run();