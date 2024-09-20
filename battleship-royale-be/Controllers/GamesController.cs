using Microsoft.AspNetCore.Mvc;
using battleship_royale_be.Data;
using battleship_royale_be.Usecase.StartNewGame;
using battleship_royale_be.Usecase.GetGameById;
using battleship_royale_be.Models;
using battleship_royale_be.Usecase.Shoot;

namespace battleship_royale_be.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GamesController : Controller
    {
        private readonly BattleshipAPIContext _context;
        private readonly IStartNewGameUseCase _startNewGameUseCase;
        private readonly IGetGameByIdUseCase _getGameByIdUseCase;
        private readonly IShootUseCase _shootUseCase;

        public GamesController(BattleshipAPIContext context,
            IStartNewGameUseCase startNewGameUseCase,
            IGetGameByIdUseCase getGameByIdUseCase,
            IShootUseCase shootUseCase)
        {
            _context = context;
            _startNewGameUseCase = startNewGameUseCase;
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
        public async Task<ActionResult<Guid>> StartNewGame()
        {
            Guid id = await _startNewGameUseCase.Start();

            return Ok(id);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Game>> MakeShot(Guid id, [FromBody] ShotCoordinates shotcoords) {
            var gameAfterShot = await _shootUseCase.Shoot(id, shotcoords);

            if (gameAfterShot == null)
                return BadRequest(new { message = "Game by given id not found" });

            var ggg = await _getGameByIdUseCase.Get(id);

            return Ok(ggg);
        }
    }
}
