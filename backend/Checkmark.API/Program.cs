using Checkmark.API.Data;
using Checkmark.API.Interfaces;
using Checkmark.API.Repositories;
using Microsoft.EntityFrameworkCore;
using Checkmark.API.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<DataDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<ICheckmarkRepository, CheckmarkRepository>();
builder.Services.AddScoped<ICheckmarkService, CheckmarkService>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp",
        policy =>
        {
            policy.WithOrigins("http://localhost:3000")
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<DataDbContext>();
    
    Console.WriteLine("Creating database...");
    context.Database.EnsureCreated();
    
    try
    {
        var canConnect = context.Database.CanConnect();
        Console.WriteLine($"Database can connect: {canConnect}");
        
        if (canConnect)
        {
            var tableExists = context.CheckmarkItems.Any();
            Console.WriteLine($"Table exists: {tableExists}");
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Database error: {ex.Message}");
    }
}

app.UseCors("AllowReactApp");
// app.UseHttpsRedirection(); // 
app.UseAuthorization();
app.MapControllers();

app.MapGet("/test-db", async (DataDbContext context) =>
{
    try
    {
        var count = await context.CheckmarkItems.CountAsync();
        return Results.Ok(new { message = "Database OK!", count });
    }
    catch (Exception ex)
    {
        return Results.Problem($"Database error: {ex.Message}");
    }
});

app.Run();