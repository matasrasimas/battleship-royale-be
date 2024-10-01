using Microsoft.AspNetCore.Mvc;
using battleship_royale_be.Data;
using battleship_royale_be.Usecase.StartNewGame;
using battleship_royale_be.Usecase.GetGameById;
using battleship_royale_be.Models;
using battleship_royale_be.Usecase.Shoot;
using battleship_royale_be.Usecase.CreateNewGame;
using battleship_royale_be.Usecase.FindGameUseCase;

namespace battleship_royale_be.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GamesController : Controller
    {
        private readonly BattleshipAPIContext _context;
        private readonly IFindGameUseCase _findGameUseCase;
        private readonly IAddPlayerToGameUseCase _addPlayerToGameUseCase;
        private readonly IGetGameByIdUseCase _getGameByIdUseCase;
        private readonly IShootUseCase _shootUseCase;

        public GamesController(BattleshipAPIContext context,
            IFindGameUseCase findGameUseCase,
            IAddPlayerToGameUseCase addPlayerToGameUseCase,
            IGetGameByIdUseCase getGameByIdUseCase,
            IShootUseCase shootUseCase)
        {
            _context = context;
            _findGameUseCase = findGameUseCase;
            _addPlayerToGameUseCase = addPlayerToGameUseCase;
            _getGameByIdUseCase = getGameByIdUseCase;
            _shootUseCase = shootUseCase;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Game>> GetGameById(Guid id)
        {
            var game = await _getGameByIdUseCase.Get(id);

            if (game == null)
                return BadRequest(new { message = "Game by given id not found" });

            return Ok(game);
        }

        [HttpPost]
        public async Task<ActionResult<Guid>> FindGame()
        {
            Guid id = await _findGameUseCase.FindGame();

            return Ok(id);
        }
    }
}
