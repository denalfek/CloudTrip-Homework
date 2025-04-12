using CloudTrip.Homework.Adapters;
using CloudTrip.Homework.BL.Infrastructure;
using CloudTrip.Homework.Caching.Redis;
using CloudTrip.Homework.Dal.Mongo.Infrastructure;
using CloudTrip.Homework.Mock.DataProviders;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;

// Add services to the container.

services.RegisterProviders();
services.RegisterAdapters();
services.RegisterCache();
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

LoggingConfigurator.BuildLogger(builder);
LoggingConfigurator.EnsureTtlIndex();

string _secret = "Cloud-trip-client-awessome-secret-key";
string _tokenIssuer = "CloudTrip";
string _audience = "CloudTripClient";

services.AddAuthentication(opts =>
{
    opts.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    opts.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(opts =>
{
    opts.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidIssuer = _tokenIssuer,
        ValidAudience = _audience,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secret)),
        ClockSkew = TimeSpan.Zero,
    };
});
var app = builder.Build();
app.UseMiddleware<RequestLoggingMiddleware>();

app.UseDefaultFiles();
app.UseStaticFiles();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.UseCors("CorsPolicy");
app.MapControllers();

app.MapFallbackToFile("/index.html");

app.Run();
