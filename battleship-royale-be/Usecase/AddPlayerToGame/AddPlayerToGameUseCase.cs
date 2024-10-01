using battleship_royale_be.Data;
using battleship_royale_be.Models;
using battleship_royale_be.Models.Builders;
using battleship_royale_be.Usecase.StartNewGame;
using Microsoft.EntityFrameworkCore;

namespace battleship_royale_be.Usecase.CreateNewGame
{
    public class AddPlayerToGameUseCase : IAddPlayerToGameUseCase
    {
        private readonly BattleshipAPIContext _context;
        private readonly ICreateNewPlayerUseCase _createNewPlayerUseCase;


        public AddPlayerToGameUseCase(BattleshipAPIContext context, ICreateNewPlayerUseCase createNewPlayerUseCase)
        {
            _context = context;
            _createNewPlayerUseCase = createNewPlayerUseCase;
        }

        public async void AddPlayer(Guid gameId, Player player)
        {
            var game = await _context.Games.Where(g => g.Id == gameId).FirstOrDefaultAsync();
            if (game == null) 
                return;


            if (game.Players.Count >= 2)
                return;

            if (game.Players.Where(p => p.Id == player.Id).FirstOrDefault() != null)
                return;

            game.Players.Add(player);
            await _context.SaveChangesAsync();
        }
    }
}
