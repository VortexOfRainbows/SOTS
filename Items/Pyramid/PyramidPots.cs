using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Dusts;
using SOTS.Items.Potions;
using SOTS.Items.Pyramid;
using SOTS.Items.Pyramid.AncientGold;
using SOTS.NPCs;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;
using static Terraria.ModLoader.ModContent;

namespace SOTS.Items.Pyramid
{
	internal class PyramidPots : ModTile
	{
		public override void SetStaticDefaults()
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
            ModTranslation name = CreateMapEntryName();
            name.SetDefault("Pot");
            AddMapEntry(new Color(150, 130, 20), name);
            DustType = 32;
        }
        public override void NumDust(int i, int j, bool fail, ref int num)
        {
            num = 8;
        }
		public override void KillMultiTile(int i, int j, int frameX, int frameY)
		{
			if (!WorldGen.gen)
			{
                PotDrops(i, j, frameX, frameY);
            }
		}
        public void PotDrops(int i, int j, int frameX, int frameY)
        {
            Terraria.Audio.SoundEngine.PlaySound(SoundID.Shatter, i * 16, j * 16, 1, 1f, 0.0f);
            int num = 0;
            bool snakePot = false;
            if (frameY < 36)
            {
                num = 1; //gold
                if (frameX < 36)
                    snakePot = true;
            }
            else if (frameY < 72)
            {
                num = 2; //mixed Pots
                if (frameX < 36)
                {
                    num = 3; //mostly gold
                }
            }
            else
            {
                num = 1; //gold
                if (frameX < 72)
                {
                    num = 4; //sandstone
                }
            }
            if(num == 1) //gold
            {
                if (Main.rand.NextBool(3))
                    Gore.NewGore(new EntitySource_TileBreak(i, j), new Vector2((float)(i * 16), (float)(j * 16)), default, ModGores.GoreType("Gores/Pots/PyramidPotGore1"), 1f);
                if (Main.rand.NextBool(3))
                    Gore.NewGore(new EntitySource_TileBreak(i, j), new Vector2((float)(i * 16), (float)(j * 16)), default, ModGores.GoreType("Gores/Pots/PyramidPotGore2"), 1f);
                if (Main.rand.NextBool(3))
                    Gore.NewGore(new EntitySource_TileBreak(i, j), new Vector2((float)(i * 16), (float)(j * 16)), default, ModGores.GoreType("Gores/Pots/PyramidPotGore3"), 1f);
                else if (Main.rand.NextBool(3))
                    Gore.NewGore(new EntitySource_TileBreak(i, j), new Vector2((float)(i * 16), (float)(j * 16)), default, ModGores.GoreType("Gores/Pots/PyramidPotGore4"), 1f);
                if (Main.rand.NextBool(5))
                    Gore.NewGore(new EntitySource_TileBreak(i, j), new Vector2((float)(i * 16), (float)(j * 16)), default, ModGores.GoreType("Gores/Pots/PyramidPotGore5"), 1f);
                else if (Main.rand.NextBool(5))
                    Gore.NewGore(new EntitySource_TileBreak(i, j), new Vector2((float)(i * 16), (float)(j * 16)), default, ModGores.GoreType("Gores/Pots/PyramidPotGore6"), 1f);
                else if (Main.rand.NextBool(5))
                    Gore.NewGore(new EntitySource_TileBreak(i, j), new Vector2((float)(i * 16), (float)(j * 16)), default, ModGores.GoreType("Gores/Pots/PyramidPotGore7"), 1f);
            }
            if (num == 2) //mixed Pots
            {
                if (Main.rand.NextBool(3))
                    Gore.NewGore(new EntitySource_TileBreak(i, j), new Vector2((float)(i * 16), (float)(j * 16)), default, ModGores.GoreType("Gores/Pots/PyramidPotGore14"), 1f);
                if (Main.rand.NextBool(3))
                    Gore.NewGore(new EntitySource_TileBreak(i, j), new Vector2((float)(i * 16), (float)(j * 16)), default, ModGores.GoreType("Gores/Pots/PyramidPotGore15"), 1f);
                if (Main.rand.NextBool(4))
                    Gore.NewGore(new EntitySource_TileBreak(i, j), new Vector2((float)(i * 16), (float)(j * 16)), default, ModGores.GoreType("Gores/Pots/PyramidPotGore16"), 1f);
                if (Main.rand.NextBool(10))
                    Gore.NewGore(new EntitySource_TileBreak(i, j), new Vector2((float)(i * 16), (float)(j * 16)), default, ModGores.GoreType("Gores/Pots/PyramidPotGore3"), 1f);
                else if (Main.rand.NextBool(10))
                    Gore.NewGore(new EntitySource_TileBreak(i, j), new Vector2((float)(i * 16), (float)(j * 16)), default, ModGores.GoreType("Gores/Pots/PyramidPotGore5"), 1f);
                if (Main.rand.NextBool(4))
                    Gore.NewGore(new EntitySource_TileBreak(i, j), new Vector2((float)(i * 16), (float)(j * 16)), default, ModGores.GoreType("Gores/Pots/PyramidPotGore11"), 1f);
                else if (Main.rand.NextBool(3))
                    Gore.NewGore(new EntitySource_TileBreak(i, j), new Vector2((float)(i * 16), (float)(j * 16)), default, ModGores.GoreType("Gores/Pots/PyramidPotGore10"), 1f);
            }
            if (num == 3) //mostly gold
            {
                if (Main.rand.NextBool(3))
                    Gore.NewGore(new EntitySource_TileBreak(i, j), new Vector2((float)(i * 16), (float)(j * 16)), default, ModGores.GoreType("Gores/Pots/PyramidPotGore12"), 1f);
                else if (Main.rand.NextBool(4))
                    Gore.NewGore(new EntitySource_TileBreak(i, j), new Vector2((float)(i * 16), (float)(j * 16)), default, ModGores.GoreType("Gores/Pots/PyramidPotGore13"), 1f);
                if (Main.rand.NextBool(3))
                    Gore.NewGore(new EntitySource_TileBreak(i, j), new Vector2((float)(i * 16), (float)(j * 16)), default, ModGores.GoreType("Gores/Pots/PyramidPotGore14"), 1f);
                if (Main.rand.NextBool(8))
                    Gore.NewGore(new EntitySource_TileBreak(i, j), new Vector2((float)(i * 16), (float)(j * 16)), default, ModGores.GoreType("Gores/Pots/PyramidPotGore1"), 1f);
                else if (Main.rand.NextBool(8))
                    Gore.NewGore(new EntitySource_TileBreak(i, j), new Vector2((float)(i * 16), (float)(j * 16)), default, ModGores.GoreType("Gores/Pots/PyramidPotGore2"), 1f);
                if (Main.rand.NextBool(5))
                    Gore.NewGore(new EntitySource_TileBreak(i, j), new Vector2((float)(i * 16), (float)(j * 16)), default, ModGores.GoreType("Gores/Pots/PyramidPotGore11"), 1f);
                if (Main.rand.NextBool(5))
                    Gore.NewGore(new EntitySource_TileBreak(i, j), new Vector2((float)(i * 16), (float)(j * 16)), default, ModGores.GoreType("Gores/Pots/PyramidPotGore10"), 1f);
            }
            if (num == 4) //sandstone
            {
                if (Main.rand.NextBool(3))
                    Gore.NewGore(new EntitySource_TileBreak(i, j), new Vector2((float)(i * 16), (float)(j * 16)), default, ModGores.GoreType("Gores/Pots/PyramidPotGore8"), 1f);
                if (Main.rand.NextBool(3))
                    Gore.NewGore(new EntitySource_TileBreak(i, j), new Vector2((float)(i * 16), (float)(j * 16)), default, ModGores.GoreType("Gores/Pots/PyramidPotGore9"), 1f);
                else if (Main.rand.NextBool(3))
                    Gore.NewGore(new EntitySource_TileBreak(i, j), new Vector2((float)(i * 16), (float)(j * 16)), default, ModGores.GoreType("Gores/Pots/PyramidPotGore10"), 1f);
                if (Main.rand.NextBool(4))
                    Gore.NewGore(new EntitySource_TileBreak(i, j), new Vector2((float)(i * 16), (float)(j * 16)), default, ModGores.GoreType("Gores/Pots/PyramidPotGore11"), 1f);
                if (Main.rand.NextBool(6))
                    Gore.NewGore(new EntitySource_TileBreak(i, j), new Vector2((float)(i * 16), (float)(j * 16)), default, ModGores.GoreType("Gores/Pots/PyramidPotGore14"), 1f);
                else if (Main.rand.NextBool(5))
                    Gore.NewGore(new EntitySource_TileBreak(i, j), new Vector2((float)(i * 16), (float)(j * 16)), default, ModGores.GoreType("Gores/Pots/PyramidPotGore16"), 1f);
            }



            int chanceForPortal = 1000;
            if(num == 1)
                chanceForPortal = 100;
            else if(num == 2)
                chanceForPortal = 150;
            else if(num == 3)
                chanceForPortal = 250;
            if (Main.rand.NextBool(chanceForPortal))
            {
                if (Main.netMode != NetmodeID.MultiplayerClient)
                    Projectile.NewProjectile(new EntitySource_TileBreak(i, j), (float)(i * 16 + 16), (float)(j * 16 + 16), 0.0f, -12f, ProjectileID.CoinPortal, 0, 0.0f, Main.myPlayer, 0.0f, 0.0f);
            }
            else if (WorldGen.genRand.NextBool(40) && Main.wallDungeon[(int)Main.tile[i, j].WallType])
                Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 16, 16, ItemID.GoldenKey, 1, false, 0, false, false);
            else if (WorldGen.genRand.NextBool(18) || (Main.rand.NextBool(35) && Main.expertMode))
            {
                int type2 = WorldGen.genRand.Next(13);
                if (type2 == 0)
                    Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 16, 16, ItemID.TrapsightPotion, 1, false, 0, false, false);
                if (type2 == 1)
                    Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 16, 16, ItemID.SummoningPotion, 1, false, 0, false, false);
                if (type2 == 2)
                    Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 16, 16, ItemID.HunterPotion, 1, false, 0, false, false);
                if (type2 == 3)
                    Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 16, 16, ItemID.WrathPotion, 1, false, 0, false, false);
                if (type2 == 4)
                    Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 16, 16, ItemID.RagePotion, 1, false, 0, false, false);
                if (type2 == 5)
                    Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 16, 16, ItemID.ThornsPotion, 1, false, 0, false, false);
                if (type2 == 6)
                    Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 16, 16, ItemID.BattlePotion, 1, false, 0, false, false);
                if (type2 == 7)
                    Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 16, 16, ItemID.RegenerationPotion, 1, false, 0, false, false);
                if (type2 == 8)
                    Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 16, 16, ItemID.HeartreachPotion, 1, false, 0, false, false);
                if (type2 == 9)
                    Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 16, 16, ItemID.MiningPotion, 1, false, 0, false, false);
                if (type2 == 10)
                    Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 16, 16, ItemID.BuilderPotion, 1, false, 0, false, false);
                if (type2 == 11)
                    Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 16, 16, ItemType<RoughskinPotion>(), 1, false, 0, false, false);
                if (type2 == 12)
                    Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 16, 16, ItemType<SoulAccessPotion>(), 1, false, 0, false, false);
            }
            else if (Main.netMode == NetmodeID.Server && Main.rand.NextBool(30))
            {
                Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 16, 16, ItemID.WormholePotion, 1, false, 0, false, false);
            }
            else
            {
                int num3 = Main.rand.Next(9);
                if (num3 == 0 && Main.player[(int)Player.FindClosest(new Vector2((float)(i * 16), (float)(j * 16)), 16, 16)].statLife < Main.player[(int)Player.FindClosest(new Vector2((float)(i * 16), (float)(j * 16)), 16, 16)].statLifeMax2)
                {
                    Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 16, 16, ItemID.Heart, 1, false, 0, false, false);
                    if (Main.rand.NextBool(2))
                        Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 16, 16, ItemID.Heart, 1, false, 0, false, false);
                    if (Main.expertMode)
                    {
                        if (Main.rand.NextBool(2))
                            Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 16, 16, ItemID.Heart, 1, false, 0, false, false);
                        if (Main.rand.NextBool(2))
                            Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 16, 16, ItemID.Heart, 1, false, 0, false, false);
                    }
                }
                else if (num3 == 1 && Main.player[(int)Player.FindClosest(new Vector2((float)(i * 16), (float)(j * 16)), 16, 16)].statMana < Main.player[(int)Player.FindClosest(new Vector2((float)(i * 16), (float)(j * 16)), 16, 16)].statManaMax2)
                    Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 16, 16, ItemID.Star, 1, false, 0, false, false);
                else if (num3 == 2)
                {
                    int Stack = Main.rand.Next(2, 6);
                    if (Main.expertMode)
                        Stack += Main.rand.Next(1, 7);
                    if ((int)Main.tile[i, j].LiquidAmount > 0)
                        Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 16, 16, ItemID.Glowstick, Stack, false, 0, false, false);
                    else
                        Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 16, 16, ItemType<AncientGoldTorch>(), Stack, false, 0, false, false);
                }
                else if (num3 == 3)
                {
                    int Stack = Main.rand.Next(20, 31);
                    int Type = ItemID.MusketBall;
                    if (Main.hardMode)
                        Type = ItemID.GoldenBullet;
                    Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 16, 16, Type, Stack, false, 0, false, false);
                }
                else if (num3 == 4)
                {
                    int Type = ItemID.HealingPotion;
                    int Stack = 1;
                    if (Main.expertMode && !Main.rand.NextBool(3))
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
                    Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 16, 16, Type, Stack, false, 0, false, false);
                }
                else if (num3 == 5)
                {
                    int Stack = Main.rand.Next(20, 31);
                    int Type = ItemID.FlamingArrow;
                    Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 16, 16, Type, Stack, false, 0, false, false);
                }
                else if (num3 == 6 && Main.rand.NextBool(5))
                {
                    int Stack = Main.rand.Next(20, 41);
                    Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 16, 16, ItemID.SilkRope, Stack, false, 0, false, false);
                }
                else
                {
                    float num11 = (float)(240 + WorldGen.genRand.Next(-100, 101));
                    float num12 = num11 * (float)(1.0 + (double)Main.rand.Next(-20, 21) * 0.01);
                    if (Main.rand.NextBool(4))
                        num12 *= (float)(1.0 + (double)Main.rand.Next(5, 11) * 0.01);
                    if (Main.rand.NextBool(8))
                        num12 *= (float)(1.0 + (double)Main.rand.Next(10, 21) * 0.01);
                    if (Main.rand.NextBool(12))
                        num12 *= (float)(1.0 + (double)Main.rand.Next(20, 41) * 0.01);
                    if (Main.rand.NextBool(16))
                        num12 *= (float)(1.0 + (double)Main.rand.Next(40, 81) * 0.01);
                    if (Main.rand.NextBool(20))
                        num12 *= (float)(1.0 + (double)Main.rand.Next(50, 101) * 0.01);
                    if (Main.expertMode)
                        num12 *= 2.5f;
                    if (Main.expertMode && Main.rand.NextBool(2))
                        num12 *= 1.25f;
                    if (Main.expertMode && Main.rand.NextBool(3))
                        num12 *= 1.5f;
                    if (Main.expertMode && Main.rand.NextBool(4))
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
                            if (Stack > 50 && Main.rand.NextBool(2))
                                Stack /= Main.rand.Next(3) + 1;
                            if (Main.rand.NextBool(2))
                                Stack /= Main.rand.Next(3) + 1;
                            num13 -= (float)(1000000 * Stack);
                            Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 16, 16, ItemID.PlatinumCoin, Stack, false, 0, false, false);
                        }
                        else if ((double)num13 > 10000.0)
                        {
                            int Stack = (int)((double)num13 / 10000.0);
                            if (Stack > 50 && Main.rand.NextBool(2))
                                Stack /= Main.rand.Next(3) + 1;
                            if (Main.rand.NextBool(2))
                                Stack /= Main.rand.Next(3) + 1;
                            num13 -= (float)(10000 * Stack);
                            Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 16, 16, 73, Stack, false, 0, false, false);
                        }
                        else if ((double)num13 > 100.0)
                        {
                            int Stack = (int)((double)num13 / 100.0);
                            if (Stack > 50 && Main.rand.NextBool(2))
                                Stack /= Main.rand.Next(3) + 1;
                            if (Main.rand.NextBool(2))
                                Stack /= Main.rand.Next(3) + 1;
                            num13 -= (float)(100 * Stack);
                            Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 16, 16, 72, Stack, false, 0, false, false);
                        }
                        else
                        {
                            int Stack = (int)num13;
                            if (Stack > 50 && Main.rand.NextBool(2))
                                Stack /= Main.rand.Next(3) + 1;
                            if (Main.rand.NextBool(2))
                                Stack /= Main.rand.Next(4) + 1;
                            if (Stack < 1)
                                Stack = 1;
                            num13 -= (float)Stack;
                            Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 16, 16, 71, Stack, false, 0, false, false);
                        }
                    }
                }
            }
            if(snakePot && Main.rand.NextBool(4))
            {
                if(Main.netMode != NetmodeID.MultiplayerClient)
                {
                    for (int amount = 2 + Main.rand.Next(3); amount > 0; amount--)
                    {
                        int npcSpawn = NPC.NewNPC(new EntitySource_TileBreak(i, j), i * 16 + 16, j * 16 + 16, NPCType<Snake>());
                        Main.npc[npcSpawn].velocity.X += Main.rand.NextFloat(-1.6f, 1.6f);
                        Main.npc[npcSpawn].velocity.Y -= Main.rand.NextFloat(5.25f, 7.5f);
                        Main.npc[npcSpawn].netUpdate = true;
                    }
                }
            }
            else if(snakePot)
            {
                Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 16, 16, ItemType<Snakeskin>(), Main.rand.Next(2) + 1, false, 0, false, false);
            }
            else if(Main.rand.NextBool(40))
            {
                Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 16, 16, ItemType<JuryRiggedDrill>(), Main.rand.Next(4) + 4, false, 0, false, false);
            }
        }
    }
    internal class PyramidPot : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Pyramid Pot");
		}
		public override void SetDefaults()
		{
			Item.CloneDefaults(ItemID.DartTrap);
			Item.width = 28;
			Item.height = 24;
			Item.createTile = TileType<PyramidPots>();
			Item.value = 0;
		}
	}
}