namespace Tiles
{
    public class TileRegen : Tile
    {
        public override void ApplyPowerUP()
        {
            Player.healAmount += 5;
        }
    }
}