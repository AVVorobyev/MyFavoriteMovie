using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MyFavoriteMovie.Core.Contexts;
using MyFavoriteMovie.Core.Repositories;
using MyFavoriteMovie.Core.Repositories.Interfaces;
using MyFavoriteMovie.Core.Services;
using MyFavoriteMovie.Core.Services.Interfaces;
using Newtonsoft.Json.Serialization;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRouting();
builder.Services.AddControllers();

builder.Services.AddCors(cors =>
    cors.AddDefaultPolicy(options =>
        options.WithOrigins(
            builder.Configuration.GetSection(
                "frontend_url").ToString()!).AllowAnyMethod().AllowAnyHeader()));

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

builder.Services.AddScoped<IMovieRepository, MovieRepository>();
builder.Services.AddScoped<IActorRepository, ActorRepository>();
builder.Services.AddScoped<IEpisodeRepository, EpisodeRepository>();
builder.Services.AddScoped<IAuthRepository, AuthRepository>();
builder.Services.AddSingleton<IAuthService>(new AuthService(
    builder.Configuration.GetValue<string>("JWTSecretKey"),
    builder.Configuration.GetValue<int>("JWTLifespan")));

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseCors(options => options.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

app.UseHttpsRedirection();

app.MapControllers();

app.UseRouting();

app.UseStaticFiles();

app.UseAuthentication();

app.UseEndpoints(endpoints =>
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}"));

app.Run();
