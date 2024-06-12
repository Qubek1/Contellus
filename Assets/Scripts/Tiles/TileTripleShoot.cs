using System.Collections;
using System.Collections.Generic;
using Tiles;
using UnityEngine;
namespace Tiles
{
    public class TileTripleShoot : Tile
    {
        public override void ApplyPowerUP()
        {
            Player.tripleShot = true;
        }
    }
}
