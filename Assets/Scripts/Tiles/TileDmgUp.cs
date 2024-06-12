namespace Tiles
{
    public class TileDmgUp : Tile
    {
        public override void ApplyPowerUP()
        {
            Player.baseDamage += 5;
        }
    }
}