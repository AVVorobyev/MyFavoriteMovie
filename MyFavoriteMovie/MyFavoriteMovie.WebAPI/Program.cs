using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MyFavoriteMovie.Core.Contexts;
using MyFavoriteMovie.Core.Repositories;
using MyFavoriteMovie.Core.Repositories.Interfaces;
using MyFavoriteMovie.Core.Services;
using MyFavoriteMovie.Core.Services.Interfaces;
using MyFavoriteMovie.WebAPI.Middlewares;
using Newtonsoft.Json.Serialization;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRouting();
builder.Services.AddControllers();

builder.Services.AddCors(cors =>
    cors.AddDefaultPolicy(options =>
        options
        .WithOrigins(
            builder.Configuration.GetValue<string>(
                "frontend_url"))
        .AllowAnyMethod()
        .AllowAnyHeader()
        .AllowCredentials()));

// JSON Serializer
builder.Services.AddControllersWithViews()
    .AddNewtonsoftJson(options =>
    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft
    .Json.ReferenceLoopHandling.Ignore)
    .AddNewtonsoftJson(options => options.SerializerSettings.ContractResolver
    = new DefaultContractResolver());

builder.Services.AddDbContext<MSSQLDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("test")));

// Auth
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(
    options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters()
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration.GetValue<string>("JWTSecretKey")))
        };
    });

builder.Services.AddAntiforgery(
    options =>
    {
        options.Cookie = new CookieBuilder()
        {
            Name = "x-xsrf-token",
            HttpOnly = true,
            SameSite = SameSiteMode.None
        };
    }
);

builder.Services.AddAuthorization();
builder.Services.AddScoped<IMovieRepository, MovieRepository>();
builder.Services.AddScoped<IActorRepository, ActorRepository>();
builder.Services.AddScoped<IEpisodeRepository, EpisodeRepository>();
builder.Services.AddScoped<IAuthRepository, AuthRepository>();
builder.Services.AddSingleton<IAuthService>(
    new AuthService(
        builder.Configuration.GetValue<string>("JWTSecretKey"),
        builder.Configuration.GetValue<int>("JWTLifespan")));

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseCors();

app.UseMiddleware<TokenMiddleware>();

app.UseMiddleware<XsrfProtectionMiddleware>();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.UseEndpoints(endpoints =>
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}"));

app.Run();
