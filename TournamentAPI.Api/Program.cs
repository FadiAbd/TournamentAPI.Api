using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using TournamentAPI.Api.Extentions;
using TournamentAPI.Core.Repositories;
using TournamentAPI.Data;
using TournamentAPI.Data.Data;
using TournamentAPI.Data.TournamentAPI;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers(opt => opt.ReturnHttpNotAcceptable = true)
    .AddNewtonsoftJson()
    .AddXmlDataContractSerializerFormatters();

builder.Services.AddDbContext<TournamentApiContext>(options =>
               options.UseSqlServer(builder.Configuration.GetConnectionString("TournamentApiContext")));

builder.Services.AddScoped<IGameRepository, GameRepository>();
builder.Services.AddScoped<ITournamentRepository, TournamentRepository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Tournament API v1");
    });
}



app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<TournamentApiContext>();
    await SeedData.InitAsync(context);
}

app.Run();
