using SOTS.Items.Chaos;
using Microsoft.Xna.Framework;
using System;
using System.Threading;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;
using SOTS.Void;
using Terraria.ID;

namespace SOTS
{
    internal sealed class PhaseWorldgenHelper
    {
        public static bool Generating { get; private set; }
        public static bool ClearPrevious = false;
        public static string GetStatus()
        {
            return "Generating starlight...";
        }
        private static void ClearPreviousGen()
        {
            int amtDestroyed = 0;
            for(int i = 10; i < Main.maxTilesX - 10; i++)
            {
                for(int j = 10; j < Main.worldSurface * 0.35f + 40; j++)
                {
                    Tile tile = Main.tile[i, j];
                    if(tile.active() && tile.type == ModContent.TileType<PhaseOreTile>())
                    {
                        tile.active(false);
                        amtDestroyed++;
                    }
                }
            }
            if (Main.netMode == NetmodeID.Server)
                NetMessage.BroadcastChatMessage(NetworkText.FromLiteral(amtDestroyed.ToString()), VoidPlayer.ChaosPink);
            else
                Main.NewText(amtDestroyed, VoidPlayer.ChaosPink);
        }
        private static void DoGen(object state)
        {
            Generating = true;
            if(ClearPrevious)
                ClearPreviousGen();
            ClearPrevious = false;
            float worldPercent;
            int total = 6; //4 clumps
            int scattered = 18;
            if (Main.maxTilesX > 6000) //medium
            {
                total = 8; //6 clumps
                scattered = 27;
            }
            if (Main.maxTilesX > 8000) //large
            {
                total = 10; //8 clumps
                scattered = 36;
            }
            int clearRadius = 15;
            float spread = 1f / total;
            float randomMult = 0;
            for(int i = 1; i < total; i++)
            {
                if(i != total / 2)
                {
                    worldPercent = spread * i;
                    float randomMinMax = 20 * randomMult;
                    int xPos = (int)(MathHelper.Lerp(40 + randomMinMax, Main.maxTilesX - 40 - randomMinMax, worldPercent) + WorldGen.genRand.NextFloat(-randomMinMax, randomMinMax));
                    int yPos = WorldGen.genRand.Next(80, (int)(Main.worldSurface * 0.25f));
                    if (SOTSWorldgenHelper.Empty(xPos - clearRadius * 2, yPos - clearRadius * 2, clearRadius * 4, clearRadius * 4, 1))
                    {
                        SOTSWorldgenHelper.GeneratePhaseOre(xPos, yPos, 20, 2); //generate primary branches
                        randomMult = 0;
                    }
                    else
                    {
                        i--;
                        randomMult += 1;
                    }
                }
            }
            for (int j = 1; j < scattered; j++)
            {
                float randomMinMax = 20 * (randomMult + 1);
                int newX = (int)(MathHelper.Lerp(50, Main.maxTilesX - 50, j / (float)scattered) + WorldGen.genRand.NextFloat(-randomMinMax, randomMinMax));
                int yPos = WorldGen.genRand.Next(80, (int)(Main.worldSurface * 0.3f));
                if (SOTSWorldgenHelper.Empty(newX - clearRadius, yPos - clearRadius, clearRadius * 2, clearRadius * 2, 1))
                {
                    int size = 3 + WorldGen.genRand.Next(2);
                    SOTSWorldgenHelper.GeneratePhaseOre(newX, yPos, size * 4, 2);
                    randomMult = 0;
                }
                else
                {
                    j--;
                    randomMult += 1;
                }
            }
            string text = "Starlight solidifies in the upper atmosphere!";
            Generating = false;
            if (Main.netMode == NetmodeID.Server)
                NetMessage.BroadcastChatMessage(NetworkText.FromLiteral(text), VoidPlayer.ChaosPink);
            else
                Main.NewText(text, VoidPlayer.ChaosPink);
        }
        public static void Generate()
        {
            ThreadPool.QueueUserWorkItem(DoGen, null);
        }
    }
}