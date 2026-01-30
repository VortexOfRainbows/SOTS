using Microsoft.Xna.Framework;
using SOTS.Dusts;
using SOTS.Items.AbandonedVillage;
using SOTS.Items.Conduit;
using SOTS.Items.Potions;
using SOTS.Items.Void;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ObjectData;
using static Terraria.ModLoader.ModContent;

namespace SOTS.Items.Invidia
{
	internal class EvostonePots : ModTile
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
            AddMapEntry(new Color(46, 63, 77), Language.GetText("MapObject.Pot"));
            DustType = ModContent.DustType<EvostoneDust>();
        }
        public override void NumDust(int i, int j, bool fail, ref int num)
        {
            num = 8;
        }
        public override bool CanDrop(int i, int j)
        {
            return false;
        }
        public override void KillMultiTile(int i, int j, int frameX, int frameY)
		{
			if (!WorldGen.gen)
			{
                PotDrops(i, j, frameX, frameY);
            }
		}
        public void PotGore(int i, int j, int frameX, int frameY)
        {
            Vector2 position = new Vector2(i * 16, j * 16);
            int goreType = frameX / 36 + frameY / 36 * 3;
            EntitySource_TileBreak GetSource()
            {
                return new EntitySource_TileBreak(i, j);
            }
            Vector2 spawnPos = new Vector2(i * 16, j * 16);
            int[] ValidGoreTypes = new int[8];
            int[] ValidGoreChances = new int[8];
            if (goreType == 0)
            {
                ValidGoreTypes = [1, 2, 3, 4, 5, 9, 24, 18, 19];
                ValidGoreChances = [2, 2, 2, 4, 4, 5, 5, 5, 6];
            }
            if (goreType == 1)
            {
                ValidGoreTypes = [4, 5, 6, 25, 9, 24, 18, 19, 8];
                ValidGoreChances = [2, 2, 2, 4, 4, 4, 5, 5, 10];
            }
            if (goreType == 2)
            {
                ValidGoreTypes = [7, 8, 9, 4, 7, 9, 8, 9, 18, 18];
                ValidGoreChances = [2, 2, 2, 4, 4, 4, 5, 5, 6, 6];
            }
            if (goreType == 3)
            {
                ValidGoreTypes = [10, 11, 12, 7, 8, 17, 18];
                ValidGoreChances = [2, 2, 2, 4, 4, 5, 6];
            }
            if (goreType == 4)
            {
                ValidGoreTypes = [13, 14, 15, 18, 14, 7, 7];
                ValidGoreChances = [2, 2, 2, 4, 6, 6, 6];
            }
            if (goreType == 5)
            {
                ValidGoreTypes = [16, 17, 18, 19, 14, 7, 24];
                ValidGoreChances = [2, 2, 2, 5, 5, 6, 6];
            }
            if (goreType == 6)
            {
                ValidGoreTypes = [18, 19, 20, 24, 22, 7, 17, 18, 18];
                ValidGoreChances = [2, 2, 2, 4, 5, 6, 6, 6, 6];
            }
            if (goreType == 7)
            {
                ValidGoreTypes = [21, 22, 6, 24, 25, 7, 9, 19];
                ValidGoreChances = [2, 2, 2, 4, 5, 6, 6, 6];
            }
            if (goreType == 8)
            {
                ValidGoreTypes = [23, 24, 25, 17, 18, 7];
                ValidGoreChances = [2, 2, 2, 5, 6, 6];
            }
            for (int k = 0; k < ValidGoreTypes.Length; k++)
            {
                int gType = ValidGoreTypes[k];
                int gChance = ValidGoreChances[k];
                if (Main.rand.NextBool(gChance) || Main.rand.NextBool(8))
                {
                    Gore.NewGore(GetSource(), spawnPos + new Vector2(Main.rand.NextFloat(16), Main.rand.NextFloat(16)), default, ModGores.GoreType("Gores/Pots/EvostonePotGore" + gType), 1f);
                    if (gChance >= 5 && Main.rand.NextBool(3))
                    {
                        break;
                    }
                }
            }
        }
        public void PotDrops(int i, int j, int frameX, int frameY)
        {
            Vector2 position = new Vector2(i * 16, j * 16);
            SOTSTile.TryDroppingSwallowedPenny(i, j, Type);
            SoundEngine.PlaySound(SoundID.Shatter, new Vector2(i, j) * 16);
            PotGore(i, j, frameX, frameY);
            int chanceForPortal = 200;
            if (Main.rand.NextBool(chanceForPortal))
            {
                if (Main.netMode != NetmodeID.MultiplayerClient)
                    Projectile.NewProjectile(new EntitySource_TileBreak(i, j), (float)(i * 16 + 16), (float)(j * 16 + 16), 0.0f, -12f, ProjectileID.CoinPortal, 0, 0.0f, Main.myPlayer, 0.0f, 0.0f);
            }
            else if (WorldGen.genRand.NextBool(18) || (Main.rand.NextBool(35) && Main.expertMode))
            {
                int[] potionTypes = [ItemID.IronskinPotion, ItemID.BattlePotion, ItemID.ObsidianSkinPotion, ItemID.CalmingPotion, ItemID.EndurancePotion, ItemID.RegenerationPotion, 
                    ItemID.HeartreachPotion, ItemID.LifeforcePotion, ItemID.InfernoPotion, ItemType<BluefirePotion>(), ItemType<VigorPotion>(), ItemType<VigorPotion>()];
                Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 16, 16, potionTypes[Main.rand.Next(potionTypes.Length)], 1, false, 0, false, false);
            }
            else if (Main.netMode == NetmodeID.Server && Main.rand.NextBool(30))
            {
                Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 16, 16, ItemID.WormholePotion, 1, false, 0, false, false);
            }
            else
            {
                int num3 = Main.rand.Next(9);
                if (num3 == 0 && Main.player[(int)Player.FindClosest(position, 16, 16)].statLife < Main.player[(int)Player.FindClosest(position, 16, 16)].statLifeMax2)
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
                else if (num3 == 1 && Main.player[Player.FindClosest(position, 16, 16)].statMana < Main.player[Player.FindClosest(position, 16, 16)].statManaMax2)
                    Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 16, 16, ItemID.Star, 1, false, 0, false, false);
                else if (num3 == 2)
                {
                    int Stack = Main.rand.Next(2, 6);
                    if (Main.expertMode)
                        Stack += Main.rand.Next(1, 7);
                    if (Main.tile[i, j].LiquidAmount > 0)
                        Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 16, 16, ItemID.SpelunkerGlowstick, Stack, false, 0, false, false);
                    else
                        Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 16, 16, ItemID.DemonTorch, Stack, false, 0, false, false);
                }
                else if (num3 == 3)
                {
                    Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 16, 16, ItemType<SkipBullet>(), Main.rand.Next(20, 31), false, 0, false, false);
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
                            Type = ItemID.GreaterHealingPotion;
                        else if(NPC.downedMoonlord)
                            Type = ItemID.SuperHealingPotion;
                    }
                    Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 16, 16, Type, Stack, false, 0, false, false);
                }
                else if (num3 == 5)
                {
                    Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 16, 16, ItemType<SkipArrow>(), Main.rand.Next(20, 31), false, 0, false, false);
                }
                else if (num3 == 6 && Main.rand.NextBool(5))
                {
                    int Stack = Main.rand.Next(20, 41);
                    Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 16, 16, ItemID.Chain, Stack, false, 0, false, false);
                }
                else
                {
                    float num11 = 400 + WorldGen.genRand.Next(-100, 101);
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
            if(Main.rand.NextBool(40))
                Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 16, 16, ItemType<OldKey>(), 1, false, 0, false, false);
            else if (Main.rand.NextBool(50))
                Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 16, 16, ItemID.MeteoriteBar, Main.rand.Next(1, 4), false, 0, false, false);
            else if (Main.rand.NextBool(30))
                Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 16, 16, ItemType<PetalSalad>(), Main.rand.Next(1, 6), false, 0, false, false);
        }
    }
    internal class EvostonePotItem : ModItem
	{
		public override void SetDefaults()
		{
			Item.CloneDefaults(ItemID.DartTrap);
			Item.width = 28;
			Item.height = 28;
			Item.createTile = TileType<EvostonePots>();
			Item.value = 0;
		}
	}
}