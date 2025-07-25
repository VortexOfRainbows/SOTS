using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.WorldBuilding;
using SubworldLibrary;
using Terraria.ID;
using Terraria.IO;
using Terraria;
using Terraria.Map;

namespace SOTS.Subworlds
{
    public class ExampleGenPass : GenPass
    {
        //TODO: remove this once tML changes generation passes
        public ExampleGenPass() : base("Terrain", 1) { }

        protected override void ApplyPass(GenerationProgress progress, GameConfiguration configuration)
        {
            progress.Message = "Generating terrain"; // Sets the text displayed for this pass
            for (int i = 0; i < Main.maxTilesX; i++)
            {
                for (int j = 600; j < Main.maxTilesY; j++)
                {
                    progress.Set((j + i * Main.maxTilesY) / (float)(Main.maxTilesX * Main.maxTilesY)); // Controls the progress bar, should only be set between 0f and 1f
                    Tile tile = Main.tile[i, j];
                    tile.HasTile = true;
                    tile.TileType = TileID.Dirt;
                }
            }
        }
    }
}