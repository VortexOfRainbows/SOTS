using Microsoft.Xna.Framework;
using SOTS.Dusts;
using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;
using static Terraria.ModLoader.ModContent;

namespace SOTS.Items.Otherworld
{
	// This example shows how to have a tile that is cut by weapons, like vines and grass.
	// This example also shows how to spawn a projectile on death like Beehive and Boulder trap.
	internal class SkyPots : ModTile
	{
		public override void SetDefaults()
		{
			Main.tileSpelunker[Type] = true;
			Main.tileFrameImportant[Type] = true;
			Main.tileCut[Type] = true;
			TileObjectData.newTile.CopyFrom(TileObjectData.Style2x2);
			TileObjectData.newTile.StyleHorizontal = true;
			TileObjectData.newTile.Origin = new Point16(0, 1);
			TileObjectData.newTile.StyleWrapLimit = 3;
			TileObjectData.newTile.RandomStyleRange = 9;
			TileObjectData.newTile.LavaDeath = false;
			TileObjectData.addTile(Type);
            dustType = DustType<AvaritianDust>();
        }
        public override void NumDust(int i, int j, bool fail, ref int num)
        {
            num = 8;
        }
		public override void KillMultiTile(int i, int j, int frameX, int frameY)
		{
			//Projectile.NewProjectile((float)(k * 16) + 15.5f, (float)(num4 * 16 + 16), 0f, 0f, 99, 70, 10f, Main.myPlayer, 0f, 0f);
			if (!WorldGen.gen)
			{
                //Projectile.NewProjectile((i + 1.5f) * 16f, (j + 1.5f) * 16f, 0f, 0f, ProjectileID.Boulder, 70, 10f, Main.myPlayer, 0f, 0f);
                PotDrops(i, j, frameX, frameY);
                //Item.NewItem(i * 16, j * 16, 32, 32, ItemType<Pots>());
            }

		}
        public void PotDrops(int i, int j, int frameX, int frameY)
        {
            Main.PlaySound(SoundID.Shatter, i * 16, j * 16, 1, 1f, 0.0f);
            Gore.NewGore(new Vector2((float)(i * 16), (float)(j * 16)), default, mod.GetGoreSlot("Gores/SkyPotGore1"), 1f);
            Gore.NewGore(new Vector2((float)(i * 16), (float)(j * 16)), default, mod.GetGoreSlot("Gores/SkyPotGore2"), 1f);
            int num = 0;
            if(frameY < 36)
            {
                num = 1;
                if(frameX >= 36)
                {
                    num = 2;
                    if(frameX >= 72)
                    {
                        num = 3;
                    }
                }
            }
            else if (frameY < 72)
            {
                num = 4;
            }
            else
            {
                num = 5;
            }
            if(num == 1) //star pot
            {
                if(Main.rand.NextBool(2))
                   Item.NewItem(i * 16, j * 16, 16, 16, ItemID.Star, 1, false, 0, false, false);
                Gore.NewGore(new Vector2((float)(i * 16), (float)(j * 16)), default, mod.GetGoreSlot("Gores/SkyPotGore6"), 1f);
                if (Main.rand.NextBool(2))
                    Gore.NewGore(new Vector2((float)(i * 16), (float)(j * 16)), default, mod.GetGoreSlot("Gores/SkyPotGore3"), 1f);
                if (Main.rand.NextBool(2))
                    Gore.NewGore(new Vector2((float)(i * 16), (float)(j * 16)), default, mod.GetGoreSlot("Gores/SkyPotGore4"), 1f);
                if (Main.rand.NextBool(2))
                    Gore.NewGore(new Vector2((float)(i * 16), (float)(j * 16)), default, mod.GetGoreSlot("Gores/SkyPotGore5"), 1f);
            }
            if (num == 2) //heart pot
            {
                if (Main.rand.NextBool(2))
                    Item.NewItem(i * 16, j * 16, 16, 16, ItemID.Heart, 1, false, 0, false, false);
                Gore.NewGore(new Vector2((float)(i * 16), (float)(j * 16)), default, mod.GetGoreSlot("Gores/SkyPotGore7"), 1f);
                if (Main.rand.NextBool(2))
                    Gore.NewGore(new Vector2((float)(i * 16), (float)(j * 16)), default, mod.GetGoreSlot("Gores/SkyPotGore3"), 1f);
                if (Main.rand.NextBool(2))
                    Gore.NewGore(new Vector2((float)(i * 16), (float)(j * 16)), default, mod.GetGoreSlot("Gores/SkyPotGore4"), 1f);
                if (Main.rand.NextBool(2))
                    Gore.NewGore(new Vector2((float)(i * 16), (float)(j * 16)), default, mod.GetGoreSlot("Gores/SkyPotGore5"), 1f);
            }
            if (num == 3) //void pot?
            {
                Gore.NewGore(new Vector2((float)(i * 16), (float)(j * 16)), default, mod.GetGoreSlot("Gores/SkyPotGore8"), 1f);
                if (Main.rand.NextBool(2))
                    Gore.NewGore(new Vector2((float)(i * 16), (float)(j * 16)), default, mod.GetGoreSlot("Gores/SkyPotGore3"), 1f);
                if (Main.rand.NextBool(2))
                    Gore.NewGore(new Vector2((float)(i * 16), (float)(j * 16)), default, mod.GetGoreSlot("Gores/SkyPotGore4"), 1f);
                if (Main.rand.NextBool(2))
                    Gore.NewGore(new Vector2((float)(i * 16), (float)(j * 16)), default, mod.GetGoreSlot("Gores/SkyPotGore5"), 1f);
            }
            if (num == 4)
            {
                if (Main.rand.NextBool(2))
                    Gore.NewGore(new Vector2((float)(i * 16), (float)(j * 16)), default, mod.GetGoreSlot("Gores/SkyPotGore9"), 1f);
                if (Main.rand.NextBool(2))
                    Gore.NewGore(new Vector2((float)(i * 16), (float)(j * 16)), default, mod.GetGoreSlot("Gores/SkyPotGore10"), 1f);
                if (Main.rand.NextBool(2))
                    Gore.NewGore(new Vector2((float)(i * 16), (float)(j * 16)), default, mod.GetGoreSlot("Gores/SkyPotGore11"), 1f);
                if (Main.rand.NextBool(4))
                    Gore.NewGore(new Vector2((float)(i * 16), (float)(j * 16)), default, mod.GetGoreSlot("Gores/SkyPotGore3"), 1f);
                if (Main.rand.NextBool(4))
                    Gore.NewGore(new Vector2((float)(i * 16), (float)(j * 16)), default, mod.GetGoreSlot("Gores/SkyPotGore4"), 1f);
                if (Main.rand.NextBool(4))
                    Gore.NewGore(new Vector2((float)(i * 16), (float)(j * 16)), default, mod.GetGoreSlot("Gores/SkyPotGore5"), 1f);
            }
            if (num == 5)
            {
                if (Main.rand.NextBool(2))
                    Gore.NewGore(new Vector2((float)(i * 16), (float)(j * 16)), default, mod.GetGoreSlot("Gores/SkyPotGore12"), 1f);
                if (Main.rand.NextBool(2))
                    Gore.NewGore(new Vector2((float)(i * 16), (float)(j * 16)), default, mod.GetGoreSlot("Gores/SkyPotGore13"), 1f);
                if (Main.rand.NextBool(2))
                    Gore.NewGore(new Vector2((float)(i * 16), (float)(j * 16)), default, mod.GetGoreSlot("Gores/SkyPotGore14"), 1f);
                if (Main.rand.NextBool(4))
                    Gore.NewGore(new Vector2((float)(i * 16), (float)(j * 16)), default, mod.GetGoreSlot("Gores/SkyPotGore3"), 1f);
                if (Main.rand.NextBool(4))
                    Gore.NewGore(new Vector2((float)(i * 16), (float)(j * 16)), default, mod.GetGoreSlot("Gores/SkyPotGore4"), 1f);
                if (Main.rand.NextBool(4))
                    Gore.NewGore(new Vector2((float)(i * 16), (float)(j * 16)), default, mod.GetGoreSlot("Gores/SkyPotGore5"), 1f);
            }



            int maxValue = 350;
            if (Main.rand.Next(maxValue) == 0)
            {
                if (Main.netMode != NetmodeID.MultiplayerClient)
                    Projectile.NewProjectile((float)(i * 16 + 16), (float)(j * 16 + 16), 0.0f, -12f, ProjectileID.CoinPortal, 0, 0.0f, Main.myPlayer, 0.0f, 0.0f);
            }
            else if (WorldGen.genRand.Next(40) == 0 && Main.wallDungeon[(int)Main.tile[i, j].wall])
                Item.NewItem(i * 16, j * 16, 16, 16, 327, 1, false, 0, false, false);
            else if (WorldGen.genRand.Next(35) == 0 || Main.rand.Next(35) == 0 && Main.expertMode)
            {
                int type = Main.rand.Next(4);
                if (type == 0)
                {
                    int num8 = WorldGen.genRand.Next(10);
                    if (num8 == 0)
                        Item.NewItem(i * 16, j * 16, 16, 16, 292, 1, false, 0, false, false);
                    if (num8 == 1)
                        Item.NewItem(i * 16, j * 16, 16, 16, 298, 1, false, 0, false, false);
                    if (num8 == 2)
                        Item.NewItem(i * 16, j * 16, 16, 16, 299, 1, false, 0, false, false);
                    if (num8 == 3)
                        Item.NewItem(i * 16, j * 16, 16, 16, 290, 1, false, 0, false, false);
                    if (num8 == 4)
                        Item.NewItem(i * 16, j * 16, 16, 16, 2322, 1, false, 0, false, false);
                    if (num8 == 5)
                        Item.NewItem(i * 16, j * 16, 16, 16, 2324, 1, false, 0, false, false);
                    if (num8 == 6)
                        Item.NewItem(i * 16, j * 16, 16, 16, 2325, 1, false, 0, false, false);
                    if (num8 >= 7)
                        Item.NewItem(i * 16, j * 16, 16, 16, 2350, 1, false, 0, false, false);
                }
                else if (type == 1)
                {
                    int num8 = WorldGen.genRand.Next(11);
                    if (num8 == 0)
                        Item.NewItem(i * 16, j * 16, 16, 16, 289, 1, false, 0, false, false);
                    if (num8 == 1)
                        Item.NewItem(i * 16, j * 16, 16, 16, 298, 1, false, 0, false, false);
                    if (num8 == 2)
                        Item.NewItem(i * 16, j * 16, 16, 16, 299, 1, false, 0, false, false);
                    if (num8 == 3)
                        Item.NewItem(i * 16, j * 16, 16, 16, 290, 1, false, 0, false, false);
                    if (num8 == 4)
                        Item.NewItem(i * 16, j * 16, 16, 16, 303, 1, false, 0, false, false);
                    if (num8 == 5)
                        Item.NewItem(i * 16, j * 16, 16, 16, 291, 1, false, 0, false, false);
                    if (num8 == 6)
                        Item.NewItem(i * 16, j * 16, 16, 16, 304, 1, false, 0, false, false);
                    if (num8 == 7)
                        Item.NewItem(i * 16, j * 16, 16, 16, 2322, 1, false, 0, false, false);
                    if (num8 == 8)
                        Item.NewItem(i * 16, j * 16, 16, 16, 2329, 1, false, 0, false, false);
                    if (num8 >= 9)
                        Item.NewItem(i * 16, j * 16, 16, 16, 2350, 1, false, 0, false, false);
                }
                else if (type == 2)
                {
                    int num8 = WorldGen.genRand.Next(15);
                    if (num8 == 0)
                        Item.NewItem(i * 16, j * 16, 16, 16, 296, 1, false, 0, false, false);
                    if (num8 == 1)
                        Item.NewItem(i * 16, j * 16, 16, 16, 295, 1, false, 0, false, false);
                    if (num8 == 2)
                        Item.NewItem(i * 16, j * 16, 16, 16, 299, 1, false, 0, false, false);
                    if (num8 == 3)
                        Item.NewItem(i * 16, j * 16, 16, 16, 302, 1, false, 0, false, false);
                    if (num8 == 4)
                        Item.NewItem(i * 16, j * 16, 16, 16, 303, 1, false, 0, false, false);
                    if (num8 == 5)
                        Item.NewItem(i * 16, j * 16, 16, 16, 305, 1, false, 0, false, false);
                    if (num8 == 6)
                        Item.NewItem(i * 16, j * 16, 16, 16, 301, 1, false, 0, false, false);
                    if (num8 == 7)
                        Item.NewItem(i * 16, j * 16, 16, 16, 302, 1, false, 0, false, false);
                    if (num8 == 8)
                        Item.NewItem(i * 16, j * 16, 16, 16, 297, 1, false, 0, false, false);
                    if (num8 == 9)
                        Item.NewItem(i * 16, j * 16, 16, 16, 304, 1, false, 0, false, false);
                    if (num8 == 10)
                        Item.NewItem(i * 16, j * 16, 16, 16, 2322, 1, false, 0, false, false);
                    if (num8 == 11)
                        Item.NewItem(i * 16, j * 16, 16, 16, 2323, 1, false, 0, false, false);
                    if (num8 == 12)
                        Item.NewItem(i * 16, j * 16, 16, 16, 2327, 1, false, 0, false, false);
                    if (num8 == 13)
                        Item.NewItem(i * 16, j * 16, 16, 16, 2329, 1, false, 0, false, false);
                    if (num8 == 14)
                        Item.NewItem(i * 16, j * 16, 16, 16, 2350, 1, false, 0, false, false);
                }
                else
                {
                    int num8 = WorldGen.genRand.Next(14);
                    if (num8 == 0)
                        Item.NewItem(i * 16, j * 16, 16, 16, 296, 1, false, 0, false, false);
                    if (num8 == 1)
                        Item.NewItem(i * 16, j * 16, 16, 16, 295, 1, false, 0, false, false);
                    if (num8 == 2)
                        Item.NewItem(i * 16, j * 16, 16, 16, 293, 1, false, 0, false, false);
                    if (num8 == 3)
                        Item.NewItem(i * 16, j * 16, 16, 16, 288, 1, false, 0, false, false);
                    if (num8 == 4)
                        Item.NewItem(i * 16, j * 16, 16, 16, 294, 1, false, 0, false, false);
                    if (num8 == 5)
                        Item.NewItem(i * 16, j * 16, 16, 16, 297, 1, false, 0, false, false);
                    if (num8 == 6)
                        Item.NewItem(i * 16, j * 16, 16, 16, 304, 1, false, 0, false, false);
                    if (num8 == 7)
                        Item.NewItem(i * 16, j * 16, 16, 16, 305, 1, false, 0, false, false);
                    if (num8 == 8)
                        Item.NewItem(i * 16, j * 16, 16, 16, 301, 1, false, 0, false, false);
                    if (num8 == 9)
                        Item.NewItem(i * 16, j * 16, 16, 16, 302, 1, false, 0, false, false);
                    if (num8 == 10)
                        Item.NewItem(i * 16, j * 16, 16, 16, 288, 1, false, 0, false, false);
                    if (num8 == 11)
                        Item.NewItem(i * 16, j * 16, 16, 16, 300, 1, false, 0, false, false);
                    if (num8 == 12)
                        Item.NewItem(i * 16, j * 16, 16, 16, 2323, 1, false, 0, false, false);
                    if (num8 == 13)
                        Item.NewItem(i * 16, j * 16, 16, 16, 2326, 1, false, 0, false, false);
                }
            }
            else if (Main.netMode == 2 && Main.rand.Next(30) == 0)
            {
                Item.NewItem(i * 16, j * 16, 16, 16, 2997, 1, false, 0, false, false);
            }
            else if(Main.rand.Next(50) == 0 || (num == 3 && Main.rand.Next(10) == 0))
            {
                Item.NewItem(i * 16, j * 16, 16, 16, mod.ItemType("DigitalCornSyrup"), 1, false, 0, false, false);
            }
            else
            {
                int num8 = Main.rand.Next(8);
                if (Main.expertMode)
                    --num8;
                if (num8 == 0 && Main.player[(int)Player.FindClosest(new Vector2((float)(i * 16), (float)(j * 16)), 16, 16)].statLife < Main.player[(int)Player.FindClosest(new Vector2((float)(i * 16), (float)(j * 16)), 16, 16)].statLifeMax2)
                {
                    Item.NewItem(i * 16, j * 16, 16, 16, ItemID.Heart, 1, false, 0, false, false);
                    if (Main.rand.Next(2) == 0)
                        Item.NewItem(i * 16, j * 16, 16, 16, ItemID.Heart, 1, false, 0, false, false);
                    if (Main.expertMode)
                    {
                        if (Main.rand.Next(2) == 0)
                            Item.NewItem(i * 16, j * 16, 16, 16, ItemID.Heart, 1, false, 0, false, false);
                        if (Main.rand.Next(2) == 0)
                            Item.NewItem(i * 16, j * 16, 16, 16, ItemID.Heart, 1, false, 0, false, false);
                    }
                }
                else if (num8 == 1 && Main.player[(int)Player.FindClosest(new Vector2((float)(i * 16), (float)(j * 16)), 16, 16)].statMana < Main.player[(int)Player.FindClosest(new Vector2((float)(i * 16), (float)(j * 16)), 16, 16)].statManaMax2)
                    Item.NewItem(i * 16, j * 16, 16, 16, ItemID.Star, 1, false, 0, false, false);
                else if (num8 == 2)
                {
                    int Stack = Main.rand.Next(2, 6);
                    if (Main.expertMode)
                        Stack += Main.rand.Next(1, 7);
                    if ((int)Main.tile[i, j].liquid > 0)
                        Item.NewItem(i * 16, j * 16, 16, 16, 282, Stack, false, 0, false, false);
                    else
                        Item.NewItem(i * 16, j * 16, 16, 16, 8, Stack, false, 0, false, false);
                }
                else if (num8 == 3)
                {
                    int Stack = Main.rand.Next(10, 21);
                    int Type = ItemID.MeteorShot;
                    Item.NewItem(i * 16, j * 16, 16, 16, Type, Stack, false, 0, false, false);
                }
                else if (num8 == 4)
                {
                    int Type = ItemID.HealingPotion;
                    int Stack = 1;
                    if (Main.expertMode && Main.rand.Next(3) != 0)
                        ++Stack;
                    if(NPC.downedMechBoss1 || NPC.downedMechBoss2 || NPC.downedMechBoss3)
                    {
                        if (Main.rand.Next(5) < 2)
                        {
                            Type = ItemID.GreaterHealingPotion;
                        }
                        else if(NPC.downedMoonlord)
                        {
                            Type = ItemID.SuperHealingPotion;
                        }
                    }

                    Item.NewItem(i * 16, j * 16, 16, 16, Type, Stack, false, 0, false, false);
                }
                else if (num8 == 5)
                {
                    int Stack = Main.rand.Next(10, 21);
                    int Type = ItemID.JestersArrow;
                    Item.NewItem(i * 16, j * 16, 16, 16, Type, Stack, false, 0, false, false);
                }
                else if (num8 == 6 && Main.rand.Next(5) == 0)
                {
                    int Stack = Main.rand.Next(20, 41);
                    Item.NewItem(i * 16, j * 16, 16, 16, ItemID.SilkRope, Stack, false, 0, false, false);
                }
                else
                {
                    float num11 = (float)(200 + WorldGen.genRand.Next(-100, 101));
                    float num12 = num11 * (float)(1.0 + (double)Main.rand.Next(-20, 21) * 0.01);
                    if (Main.rand.Next(4) == 0)
                        num12 *= (float)(1.0 + (double)Main.rand.Next(5, 11) * 0.01);
                    if (Main.rand.Next(8) == 0)
                        num12 *= (float)(1.0 + (double)Main.rand.Next(10, 21) * 0.01);
                    if (Main.rand.Next(12) == 0)
                        num12 *= (float)(1.0 + (double)Main.rand.Next(20, 41) * 0.01);
                    if (Main.rand.Next(16) == 0)
                        num12 *= (float)(1.0 + (double)Main.rand.Next(40, 81) * 0.01);
                    if (Main.rand.Next(20) == 0)
                        num12 *= (float)(1.0 + (double)Main.rand.Next(50, 101) * 0.01);
                    if (Main.expertMode)
                        num12 *= 2.5f;
                    if (Main.expertMode && Main.rand.Next(2) == 0)
                        num12 *= 1.25f;
                    if (Main.expertMode && Main.rand.Next(3) == 0)
                        num12 *= 1.5f;
                    if (Main.expertMode && Main.rand.Next(4) == 0)
                        num12 *= 1.75f;
                    float num13 = num12;
                    if (NPC.downedBoss1)
                        num13 *= 1.1f;
                    if (NPC.downedBoss2)
                        num13 *= 1.1f;
                    if (NPC.downedBoss3)
                        num13 *= 1.1f;
                    if (NPC.downedMechBoss1)
                        num13 *= 1.1f;
                    if (NPC.downedMechBoss2)
                        num13 *= 1.1f;
                    if (NPC.downedMechBoss3)
                        num13 *= 1.1f;
                    if (NPC.downedPlantBoss)
                        num13 *= 1.1f;
                    if (NPC.downedQueenBee)
                        num13 *= 1.1f;
                    if (NPC.downedGolemBoss)
                        num13 *= 1.1f;
                    if (NPC.downedPirates)
                        num13 *= 1.1f;
                    if (NPC.downedGoblins)
                        num13 *= 1.1f;
                    if (NPC.downedFrost)
                        num13 *= 1.1f;
                    while ((int)num13 > 0)
                    {
                        if ((double)num13 > 1000000.0)
                        {
                            int Stack = (int)((double)num13 / 1000000.0);
                            if (Stack > 50 && Main.rand.Next(2) == 0)
                                Stack /= Main.rand.Next(3) + 1;
                            if (Main.rand.Next(2) == 0)
                                Stack /= Main.rand.Next(3) + 1;
                            num13 -= (float)(1000000 * Stack);
                            Item.NewItem(i * 16, j * 16, 16, 16, ItemID.PlatinumCoin, Stack, false, 0, false, false);
                        }
                        else if ((double)num13 > 10000.0)
                        {
                            int Stack = (int)((double)num13 / 10000.0);
                            if (Stack > 50 && Main.rand.Next(2) == 0)
                                Stack /= Main.rand.Next(3) + 1;
                            if (Main.rand.Next(2) == 0)
                                Stack /= Main.rand.Next(3) + 1;
                            num13 -= (float)(10000 * Stack);
                            Item.NewItem(i * 16, j * 16, 16, 16, 73, Stack, false, 0, false, false);
                        }
                        else if ((double)num13 > 100.0)
                        {
                            int Stack = (int)((double)num13 / 100.0);
                            if (Stack > 50 && Main.rand.Next(2) == 0)
                                Stack /= Main.rand.Next(3) + 1;
                            if (Main.rand.Next(2) == 0)
                                Stack /= Main.rand.Next(3) + 1;
                            num13 -= (float)(100 * Stack);
                            Item.NewItem(i * 16, j * 16, 16, 16, 72, Stack, false, 0, false, false);
                        }
                        else
                        {
                            int Stack = (int)num13;
                            if (Stack > 50 && Main.rand.Next(2) == 0)
                                Stack /= Main.rand.Next(3) + 1;
                            if (Main.rand.Next(2) == 0)
                                Stack /= Main.rand.Next(4) + 1;
                            if (Stack < 1)
                                Stack = 1;
                            num13 -= (float)Stack;
                            Item.NewItem(i * 16, j * 16, 16, 16, 71, Stack, false, 0, false, false);
                        }
                    }
                }
            }
        }
    }
    internal class Pots : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Pots");
		}

		public override void SetDefaults()
		{
			item.CloneDefaults(ItemID.DartTrap);
			item.width = 24;
			item.height = 26;
			item.createTile = TileType<SkyPots>();
			item.value = 0;
		}
	}
}