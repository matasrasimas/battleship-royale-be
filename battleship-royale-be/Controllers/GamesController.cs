using Microsoft.AspNetCore.Mvc;
using battleship_royale_be.Data;
using battleship_royale_be.Usecase.StartNewGame;
using battleship_royale_be.Usecase.GetGameById;
using battleship_royale_be.Models;
using battleship_royale_be.Usecase.Shoot;
using battleship_royale_be.Usecase.Move;
using battleship_royale_be.Usecase.CreateNewGame;
using battleship_royale_be.Usecase.FindGameUseCase;
using battleship_royale_be.DesignPatterns.Facade;
using battleship_royale_be.Models.Command;
using battleship_royale_be.Models.Observer;
using battleship_royale_be.Usecase.Pause;
using battleship_royale_be.Usecase.Surrender;

namespace battleship_royale_be.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GamesController : Controller
    {
        private GameFacade _gameFacade;

        public GamesController(BattleshipAPIContext context,
            ICreateNewPlayerUseCase createNewPlayerUseCase,
            IGetGameByIdUseCase getGameByIdUseCase,
            IShootUseCase shootUseCase,
            IMoveUseCase moveUseCase,
            IAddPlayerToGameUseCase addPlayerToGameUseCase,
            ISurrenderUseCase surrenderUseCase,
            IPauseUseCase pauseUseCase,
            IFindGameUseCase findGameUseCase,
            CommandController commandController,
            Subject server)
        {
            _gameFacade = new GameFacade(context,
                            createNewPlayerUseCase,
                            getGameByIdUseCase,
                            shootUseCase,
                            moveUseCase,
                            addPlayerToGameUseCase,
                            surrenderUseCase,
                            pauseUseCase,
                            findGameUseCase,
                            commandController,
                            server);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Game>> GetGameById(Guid id)
        {
            var game = await _gameFacade.FindGameById(id);
            if (game == null)
                return BadRequest(new { message = "Game by given id not found" });
            return Ok(game);
        }

        [HttpPost]
        public async Task<ActionResult<Guid>> FindGame()
        {
            Guid id = await _gameFacade.FindAvailableGameId();
            return Ok(id);
        }
    }
}
