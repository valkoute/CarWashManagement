using CarWashManagement.Api.Services;
using CarWashManagement.Api.Data;
using Microsoft.EntityFrameworkCore;
var builder = WebApplication.CreateBuilder(args);

// Add services to the dependency injection container.
builder.Services.AddControllers();
builder.Services.AddDbContext<CarWashDbContext>(options =>
    options.UseSqlite("Data Source=carwashmanagement.db"));
builder.Services.AddScoped<CustomerService>();
builder.Services.AddScoped<VehicleService>();
builder.Services.AddScoped<WashProgramService>();

// Add OpenAPI documentation.
builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();