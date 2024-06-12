using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Tiles
{
    public class TileAttackSpeed : Tile
    {
        public override void ApplyPowerUP()
        {
            Player.timeBetweenShots = 1/(1/Player.timeBetweenShots + 1.5f);
        }
    }
}
