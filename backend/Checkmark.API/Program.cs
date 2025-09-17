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

app.UseCors("AllowReactApp"); 
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers(); 


using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<DataDbContext>();
    context.Database.EnsureCreated(); 
}

app.Run();
