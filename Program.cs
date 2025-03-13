var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi


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

app.MapGet("/", () =>
{
    return "Job Portal";
})
.WithName("JobpPortal");

app.Run();

