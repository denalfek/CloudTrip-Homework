using CloudTrip.Homework.BL.Infrastructure;
using CloudTrip.Homework.Dal.Mongo.Infrastructure;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;

// Add services to the container.
services.RegisterRepositories(builder.Configuration);
services.RegisterServices();

services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
services.AddEndpointsApiExplorer();
services.AddSwaggerGen();
services.AddCors(opts =>
{
    opts.AddPolicy("CorsPolicy",
        policy => policy
            .WithOrigins("http://localhost:53072", "http://127.0.0.1:53072", "http://localhost:9090/")
            .AllowAnyMethod()
            .AllowAnyHeader());
});
var app = builder.Build();

app.UseDefaultFiles();
app.UseStaticFiles();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();
app.UseCors("CorsPolicy");
app.MapControllers();

app.MapFallbackToFile("/index.html");

app.Run();
