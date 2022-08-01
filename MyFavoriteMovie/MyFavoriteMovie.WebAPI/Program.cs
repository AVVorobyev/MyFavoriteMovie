using Microsoft.AspNetCore.Builder;
using MyFavoriteMovie.Core.Repositories;
using MyFavoriteMovie.Core.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using MyFavoriteMovie.Core.Contexts;
using Newtonsoft.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRouting();
builder.Services.AddControllers();

builder.Services.AddCors(cors =>
    cors.AddPolicy("AllowOrigin", options =>
        options.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader()));

//JSON Serializer
builder.Services.AddControllersWithViews()
    .AddNewtonsoftJson(options =>
    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft
    .Json.ReferenceLoopHandling.Ignore)
    .AddNewtonsoftJson(options => options.SerializerSettings.ContractResolver
    = new DefaultContractResolver());

builder.Services.AddDbContext<MSSQLDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("test")));

builder.Services.AddScoped<IMovieRepository, MovieRepository>();
builder.Services.AddScoped<IActorRepository, ActorRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseCors(options => options.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

app.UseHttpsRedirection();

app.MapControllers();

app.UseRouting();


app.UseAuthorization();

app.UseEndpoints(endpoints =>
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}"));

app.Run();
