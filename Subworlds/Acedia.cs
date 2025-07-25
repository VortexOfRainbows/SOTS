using SubworldLibrary;
using Terraria.WorldBuilding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;

namespace SOTS.Subworlds
{
    public class Acedia : Subworld
    {
        public override int Width => 6000;
        public override int Height => 2400;
        public override List<GenPass> Tasks => new() 
        {
            new ExampleGenPass()
        };
        public override void OnLoad()
        {
            Main.dayTime = true;
            Main.time = 27000;
            Main.spawnTileX = Main.maxTilesX / 2;
            Main.spawnTileY = 600;
        }
    }
}
