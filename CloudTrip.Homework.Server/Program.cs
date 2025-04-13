using CloudTrip.Homework.Adapters;
using CloudTrip.Homework.BL.Infrastructure;
using CloudTrip.Homework.BL.Jwt;
using CloudTrip.Homework.Caching.Redis;
using CloudTrip.Homework.Dal.Mongo.Infrastructure;
using CloudTrip.Homework.Mock.DataProviders;
using CloudTrip.Homework.Server.Orchestration;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;

// Add services to the container.

services.RegisterProviders();
services.RegisterAdapters();
services.RegisterCache(builder.Configuration);
services.RegisterRepositories(builder.Configuration);
services.RegisterServices();

services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
services.AddEndpointsApiExplorer();
services.AddSwaggerGen();

var policyName = builder.Configuration.GetValue<string>("CORS:Name")!;
services.AddCors(opts =>
{
    var origins = builder.Configuration.GetValue<string>("CORS:AllowedOrigins")!;
    opts.AddPolicy(
        policyName,
        policy => policy
            .WithOrigins(origins.Split(';'))
            .AllowAnyMethod()
            .AllowAnyHeader());
});

LoggingConfigurator.BuildLogger(builder);

services.AddHostedService<CacheWarmupService>();

services.Configure<AuthSettings>(builder.Configuration.GetSection(nameof(AuthSettings)));
var authSettings = builder.Configuration.GetValue<AuthSettings>(nameof(AuthSettings))!;
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
        ValidIssuer = authSettings.TokenIssuer,
        ValidAudience = authSettings.Audience,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authSettings.Secret)),
        ClockSkew = TimeSpan.Zero,
    };
});
var port = Environment.GetEnvironmentVariable("PORT") ?? "8080";
builder.WebHost.UseUrls($"http://0.0.0.0:{port}");

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

app.UseAuthentication();
app.UseAuthorization();
app.UseCors(policyName);
app.MapControllers();

app.MapFallbackToFile("/index.html");

app.Run();
