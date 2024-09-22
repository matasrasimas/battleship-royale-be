using Microsoft.EntityFrameworkCore;

namespace battleship_royale_be.Models
{
    public class UserConnection
    {
        public string Id { get; set; }
        public string GameId { get; set; }

        public UserConnection(string Id, string GameId)
        {
            this.Id = Id;
            this.GameId = GameId;
        }
    }
}
