using battleship_royale_be.DesignPatterns.FactoryMethod.Cells;

namespace battleship_royale_be.DesignPatterns.FactoryMethod
{
    public class WaterCellCreator : CellCreator
    {
        public override Cell CreateCell()
        {
            var random = new Random();
            int randomRow = random.Next(0, 10);
            int randomCol = random.Next(0, 10);
            return new WaterCell(Guid.NewGuid(), randomRow, randomCol, false, false);
        }
    }
}
