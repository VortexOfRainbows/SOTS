using SOTS.Items.Chaos;
using Microsoft.Xna.Framework;
using System;
using System.Threading;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;
using SOTS.Void;
using Terraria.ID;
using SOTS.Helpers;

namespace SOTS.WorldgenHelpers
{
    internal sealed class PhaseWorldgenHelper
    {
        public static bool Generating { get; private set; }
        public static bool ClearPrevious = false;
        public static string GetStatus()
        {
            return Language.GetTextValue("Mods.SOTS.WorldGeneration.GetStatus");
        }
        private static void ClearPreviousGen()
        {
            int amtDestroyed = 0;
            for (int i = 10; i < Main.maxTilesX - 10; i++)
            {
                for (int j = 10; j < Main.worldSurface * 0.35f + 40; j++)
                {
                    Tile tile = Main.tile[i, j];
                    if (tile.HasTile && tile.TileType == ModContent.TileType<PhaseOreTile>())
                    {
                        tile.HasTile = false;
                        amtDestroyed++;
                    }
                }
            }
            if (Main.netMode == NetmodeID.Server)
                Terraria.Chat.ChatHelper.BroadcastChatMessage(NetworkText.FromLiteral(amtDestroyed.ToString()), ColorHelper.ChaosPink);
            else
                Main.NewText(amtDestroyed, ColorHelper.ChaosPink);
        }
        private static void DoGen(object state)
        {
            /*if (Main.netMode == NetmodeID.Server)
                Terraria.Chat.ChatHelper.BroadcastChatMessage(NetworkText.FromLiteral("Gen Start 2"), ColorHelpers.ChaosPink);
            else
                Main.NewText("Gen Start 2", ColorHelpers.ChaosPink);*/
            Generating = true;
            if (ClearPrevious)
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
            for (int i = 1; i < total; i++)
            {
                if (i != total / 2)
                {
                    worldPercent = spread * i;
                    float randomMinMax = 20 * randomMult;
                    int xPos = (int)(MathHelper.Lerp(40 + randomMinMax, Main.maxTilesX - 40 - randomMinMax, worldPercent) + WorldGen.genRand.NextFloat(-randomMinMax, randomMinMax));
                    int max = (int)(Main.worldSurface * 0.25f);
                    int yPos = WorldGen.genRand.Next(80 < max ? 80 : max - 10, max);
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
                int max = (int)(Main.worldSurface * 0.3f);
                int yPos = WorldGen.genRand.Next(80 < max ? 80 : max - 10, max);
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
            string text = Language.GetTextValue("Mods.SOTS.WorldGeneration.Atmosphere");
            Generating = false;
            if (Main.netMode == NetmodeID.Server)
                Terraria.Chat.ChatHelper.BroadcastChatMessage(NetworkText.FromLiteral(text), ColorHelper.ChaosPink);
            else
                Main.NewText(text, ColorHelper.ChaosPink);
        }
        public static void Generate()
        {
            /*if (Main.netMode == NetmodeID.Server)
                Terraria.Chat.ChatHelper.BroadcastChatMessage(NetworkText.FromLiteral("Gen Start 1"), ColorHelpers.ChaosPink);
            else
                Main.NewText("Gen Start 1", ColorHelpers.ChaosPink);*/
            ThreadPool.QueueUserWorkItem(DoGen, null);
            /*if (Main.netMode == NetmodeID.Server)
                Terraria.Chat.ChatHelper.BroadcastChatMessage(NetworkText.FromLiteral("Gen End"), ColorHelpers.ChaosPink);
            else
                Main.NewText("Gen End", ColorHelpers.ChaosPink);*/
        }
    }
}