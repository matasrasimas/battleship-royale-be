using battleship_royale_be.Data;
using Microsoft.EntityFrameworkCore;
using battleship_royale_be.Usecase.StartNewGame;
using battleship_royale_be.Usecase.GetGameById;
using battleship_royale_be.Usecase.Shoot;
using battleship_royale_be.Hubs;
using battleship_royale_be.Usecase.CreateNewGame;
using battleship_royale_be.Usecase.FindGameUseCase;
using battleship_royale_be.Usecase.Surrender;
using battleship_royale_be.Models.Command;
using battleship_royale_be.Usecase.Pause;
using battleship_royale_be.Models.Observer;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<BattleshipAPIContext>(options => 
   options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnectionString")));

builder.Services.AddScoped<ICreateNewPlayerUseCase, CreateNewPlayerUseCase>();
builder.Services.AddScoped<IGetGameByIdUseCase, GetGameByIdUseCase>();
builder.Services.AddScoped<IShootUseCase, ShootUseCase>();
builder.Services.AddScoped<IAddPlayerToGameUseCase, AddPlayerToGameUseCase>();
builder.Services.AddScoped<IFindGameUseCase, FindGameUseCase>();
builder.Services.AddScoped<ISurrenderUseCase, SurrenderUseCase>();
builder.Services.AddScoped<IPauseUseCase, PauseUseCase>();
builder.Services.AddSingleton<Subject, Server>();
builder.Services.AddSingleton<CommandController>();
builder.Services.AddSignalR(o => {
    o.EnableDetailedErrors = true;
});

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


//app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

//app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapHub<GameHub>("/Game");

app.Run();
