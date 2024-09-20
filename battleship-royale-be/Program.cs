using battleship_royale_be.Data;
using Microsoft.EntityFrameworkCore;
using battleship_royale_be.Usecase.StartNewGame;
using battleship_royale_be.Usecase.GetGameById;
using battleship_royale_be.Usecase.Shoot;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<BattleshipAPIContext>(options => 
   options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnectionString")));

builder.Services.AddScoped<IStartNewGameUseCase, StartNewGameUseCase>();
builder.Services.AddScoped<IGetGameByIdUseCase, GetGameByIdUseCase>();
builder.Services.AddScoped<IShootUseCase, ShootUseCase>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

using (var scope = app.Services.CreateScope()) {
    var context = scope.ServiceProvider.GetRequiredService<BattleshipAPIContext>();
    context.Database.EnsureDeleted();
    context.Database.EnsureCreated();
}

app.UseCors(options => options
    .WithOrigins("http://localhost:5173")
    .AllowAnyHeader()
    .AllowAnyMethod()
    .AllowCredentials());


app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
