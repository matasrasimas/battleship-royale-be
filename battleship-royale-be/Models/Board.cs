namespace battleship_royale_be.Models
{
    public record Board(
        Cell[,] Grid,
        List<Ship> Ships
    )
    {
        public bool CanShoot(Coordinates coords)
        {
            bool canShoot = ShotIsWithinGridBounds(coords) && !CellHasAlreadyBeenHit(coords);
            return ShotIsWithinGridBounds(coords) && !CellHasAlreadyBeenHit(coords) && !CellContainsIsland(coords);
        }

        public Ship? FindShipByCoordinates(ShotCoordinates targetCoords)
        {
            return Ships.FirstOrDefault(ship => ship.Coordinates.Any(coord => 
                coord.Row.Equals(targetCoords.Row) && coord.Col.Equals(targetCoords.Col)
            ));
        }

        public Cell[,] CloneGrid()
        {
            var clonedGrid = (Cell[,])Grid.Clone();
            return clonedGrid;
        }

        private bool CellHasAlreadyBeenHit(Coordinates coords)
        {
            return Grid[coords.Row, coords.Col].IsHit;
        }

        private bool CellContainsIsland(Coordinates coords) {
            return Grid[coords.Row, coords.Col].IsIsland;
        }

        private bool ShotIsWithinGridBounds(Coordinates coords)
        {
            int row = coords.Row;
            int col = coords.Col;
            return row >= 0 && row < Grid.GetLength(0) && col >= 0 && col < Grid.GetLength(1);
        }
    }
}
