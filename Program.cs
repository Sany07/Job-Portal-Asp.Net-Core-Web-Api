using Microsoft.EntityFrameworkCore;
using JobPortal.Data;
using JobPortal.Services;
using System.Reflection;
using JobPortal.CQRS.Mapping;
using FluentValidation;
using FluentValidation.AspNetCore;
using JobPortal.Infrastructure.Behaviors;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// Add DbContext with SQLite
builder.Services.AddDbContext<JobPortal.Data.JobPortalDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// Register validation service
builder.Services.AddScoped<IAccountValidationService, AccountValidationService>();

// Register AutoMapper
builder.Services.AddAutoMapper(typeof(MappingProfile));

// Register FluentValidation
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

// Register MediatR with pipeline behaviors
builder.Services.AddMediatR(cfg => {
    cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
    cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
    cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
});

// Add controllers
builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    // Map OpenAPI endpoint
    app.MapOpenApi();
    
    // Don't require HTTPS in development
    app.UseHttpsRedirection();
}
else
{
    // In production, enforce HTTPS
    app.UseHttpsRedirection();
}

// Create database if it doesn't exist
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var dbContext = services.GetRequiredService<JobPortalDbContext>();
        dbContext.Database.EnsureCreated();
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while creating the database.");
    }
}

app.MapControllers();

app.MapGet("/", () =>
{
    return "Job Portal";
})
.WithName("JobpPortal");

app.Run();

