using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Dusts;
using SOTS.Items.Furniture.AncientGold;
using SOTS.Items.Furniture.Earthen;
using SOTS.Items.Potions;
using SOTS.Items.Pyramid;
using SOTS.Items.Slime;
using SOTS.NPCs;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ObjectData;
using static Terraria.ModLoader.ModContent;

namespace SOTS.Items.AbandonedVillage
{
	internal class AVPots : ModTile
    {
        public override void PostDraw(int i, int j, SpriteBatch spriteBatch)
        {
            float uniquenessCounter = Main.GlobalTimeWrappedHourly * -100 + (i + j) * 5;
            Tile tile = Main.tile[i, j];
            Texture2D texture = Mod.Assets.Request<Texture2D>("Items/AbandonedVillage/AVPotsGlow").Value;
            Rectangle frame = new Rectangle(tile.TileFrameX, tile.TileFrameY, 16, 16);
            Color color;
            color = WorldGen.paintColor((int)Main.tile[i, j].TileColor) * (100f / 255f);
            color.A = 0;
            float alphaMult = 0.55f + 0.45f * (float)Math.Sin(MathHelper.ToRadians(uniquenessCounter));
            Vector2 zero = new Vector2(Main.offScreenRange, Main.offScreenRange);
            if (Main.drawToScreen)
            {
                zero = Vector2.Zero;
            }
            for (int k = 0; k < 3; k++)
            {
                Vector2 pos = new Vector2((i * 16 - (int)Main.screenPosition.X), (j * 16 - (int)Main.screenPosition.Y)) + zero;
                Vector2 offset = new Vector2(Main.rand.NextFloat(-1, 1f), Main.rand.NextFloat(-1, 1f)) * 0.15f * k;
                Main.spriteBatch.Draw(texture, pos + offset, frame, color * alphaMult * 0.75f, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
            }
        }
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
            LocalizedText name = CreateMapEntryName();
            AddMapEntry(new Color(158, 130, 116), name);
            DustType = DustID.Iron;
        }
        public override bool CreateDust(int i, int j, ref int type)
        {
            int potType = Main.tile[i, j].TileFrameY / 36; //0, 1, 2
            if(potType == 0)
            {
                type = DustID.Iron;
            }
            if(potType == 1)
            {
                type = DustID.Titanium;
            }
            if(potType == 2)
            {
                type = DustID.BorealWood;
                if (Main.rand.NextBool(3))
                    type = DustType<SootDust>();
            }
            return base.CreateDust(i, j, ref type);
        }
        public override void NumDust(int i, int j, bool fail, ref int num)
        {
            num = 7;
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
        public void DoGore(int i, int j, int type)
        {
            EntitySource_TileBreak GetSource()
            {
                return new EntitySource_TileBreak(i, j);
            }
            Vector2 spawnPos = new Vector2(i * 16, j * 16);
            int[] ValidGoreTypes = new int[8];
            int[] ValidGoreChances = new int[8];
            if (type == 0)
            {
                ValidGoreTypes = new int[] { 1, 2, 3, 4, 6, 7, 8, 10, 11 };
                ValidGoreChances = new int[] { 3, 3, 4, 4, 5, 5, 5, 5, 5 };
            }
            if (type == 1)
            {
                ValidGoreTypes = new int[] { 5, 6, 7, 8, 2, 3, 4, 10, 11 };
                ValidGoreChances = new int[] { 3, 3, 4, 4, 5, 5, 5, 5, 5 };
            }
            if (type == 2)
            {
                ValidGoreTypes = new int[] { 9, 10, 11, 2, 3, 4, 6, 7, 8 };
                ValidGoreChances = new int[] { 3, 3, 4, 4, 5, 5, 5, 5, 5 };
            }
            if (type == 3)
            {
                ValidGoreTypes = new int[] { 13, 12, 14, 15, 16, 17, 19, 22 };
                ValidGoreChances = new int[] { 1, 3, 3, 3, 5, 5, 5, 5 };
            }
            if (type == 4)
            {
                ValidGoreTypes = new int[] { 18, 16, 17, 15, 19, 12, 14, 22 };
                ValidGoreChances = new int[] { 1, 3, 3, 3, 5, 5, 5, 5 };
            }
            if (type == 5)
            {
                ValidGoreTypes = new int[] { 20, 21, 22, 12, 14, 15, 16, 17, 19 };
                ValidGoreChances = new int[] { 2, 2, 3, 5, 5, 5, 5, 5, 5 };
            }
            if (type == 6)
            {
                ValidGoreTypes = new int[] { 23, 24, 25, 26, 28, 32 };
                ValidGoreChances = new int[] { 2, 2, 2, 2, 5, 5 };
            }
            if (type == 7)
            {
                ValidGoreTypes = new int[] { 27, 27, 27, 29, 28, 25, 26, 32 };
                ValidGoreChances = new int[] { 2, 4, 4, 2, 2, 5, 5, 5 };
            }
            if (type == 8)
            {
                ValidGoreTypes = new int[] { 30, 31, 32, 25, 26, 28 };
                ValidGoreChances = new int[] { 2, 2, 2, 5, 5, 5 };
            }
            for(int k = 0; k < ValidGoreTypes.Length; k++)
            {
                int gType = ValidGoreTypes[k];
                int gChance = ValidGoreChances[k];
                if(Main.rand.NextBool(gChance) || Main.rand.NextBool(8))
                {
                    Gore.NewGore(GetSource(), spawnPos + new Vector2(Main.rand.NextFloat(16), Main.rand.NextFloat(16)), default, ModGores.GoreType("Gores/Pots/AVPotGore" + gType), 1f);
                    if (gChance >= 5 && Main.rand.NextBool(4))
                    {
                        break;
                    }
                }
            }
        }
        public void PotDrops(int i, int j, int frameX, int frameY)
        {
            SOTSTile.TryDroppingSwallowedPenny(i, j, Type);

            int goreType = frameX / 36 + frameY / 36 * 3;
            DoGore(i, j, goreType);
            bool earth = goreType == 0 || goreType == 1 || goreType == 2;
            bool steel = goreType == 3 || goreType == 4 || goreType == 5;
            bool wooden = goreType == 6 || goreType == 7 || goreType == 8;
            if (wooden)
            {
                SOTSUtils.PlaySound(new SoundStyle("SOTS/Sounds/Tiles/WoodBreaking"), new Vector2(i, j) * 16, 0.7f, -0.2f, 0.1f);
            }
            else
            {
                SoundEngine.PlaySound(SoundID.Shatter, new Vector2(i, j) * 16);
            }

            int chanceForPortal = 750;
            if (Main.rand.NextBool(chanceForPortal))
            {
                if (Main.netMode != NetmodeID.MultiplayerClient)
                    Projectile.NewProjectile(new EntitySource_TileBreak(i, j), (i * 16 + 16), (j * 16 + 16), 0.0f, -12f, ProjectileID.CoinPortal, 0, 0.0f, Main.myPlayer, 0.0f, 0.0f);
            }
            else if (WorldGen.genRand.NextBool(18) || (Main.rand.NextBool(35) && Main.expertMode))
            {
                int type2 = WorldGen.genRand.Next(13);
                if (type2 == 0)
                    Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 16, 16, ItemID.TrapsightPotion, 1, false, 0, false, false);
                if (type2 == 1)
                    Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 16, 16, ItemID.IronskinPotion, 1, false, 0, false, false);
                if (type2 == 2)
                    Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 16, 16, ItemID.HunterPotion, 1, false, 0, false, false);
                if (type2 == 3)
                    Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 16, 16, ItemID.GillsPotion, 1, false, 0, false, false);
                if (type2 == 4)
                    Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 16, 16, ItemID.SpelunkerPotion, 1, false, 0, false, false);
                if (type2 == 5)
                    Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 16, 16, ItemID.ThornsPotion, 1, false, 0, false, false);
                if (type2 == 6)
                    Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 16, 16, ItemID.EndurancePotion, 1, false, 0, false, false);
                if (type2 == 7)
                    Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 16, 16, ItemID.RegenerationPotion, 1, false, 0, false, false);
                if (type2 == 8)
                    Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 16, 16, ItemID.HeartreachPotion, 1, false, 0, false, false);
                if (type2 == 9)
                    Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 16, 16, ItemID.MiningPotion, 1, false, 0, false, false);
                if (type2 == 10)
                    Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 16, 16, ItemID.BuilderPotion, 1, false, 0, false, false);
                if (type2 == 11)
                    Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 16, 16, ItemID.SwiftnessPotion, 1, false, 0, false, false);
                if (type2 == 12)
                    Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 16, 16, ItemType<NightmarePotion>(), 1, false, 0, false, false);
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
                    int torchType = !wooden ? ItemType<EarthenPlatingTorch>() : (WorldGen.crimson ? ItemID.CrimsonTorch : ItemID.CorruptTorch);
                    int Stack = Main.rand.Next(2, 6);
                    if (Main.expertMode)
                        Stack += Main.rand.Next(1, 7);
                    if ((int)Main.tile[i, j].LiquidAmount > 0)
                        Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 16, 16, ItemID.Glowstick, Stack, false, 0, false, false);
                    else
                        Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 16, 16, torchType, Stack, false, 0, false, false);
                }
                else if (num3 == 3)
                {
                    int Stack = Main.rand.Next(20, 31);
                    int Type = ItemID.MusketBall;
                    if (Main.hardMode)
                        Type = ItemID.ExplodingBullet;
                    Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 16, 16, Type, Stack, false, 0, false, false);
                }
                else if (num3 == 4)
                {
                    int Type = ItemID.RestorationPotion;
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
                    int Type = ItemID.UnholyArrow;
                    Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 16, 16, Type, Stack, false, 0, false, false);
                }
                else if (num3 == 6 && Main.rand.NextBool(5))
                {
                    int Stack = Main.rand.Next(5, 11);
                    Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 16, 16, ItemID.Chain, Stack, false, 0, false, false);
                }
                else
                {
                    float num11 = 240 + WorldGen.genRand.Next(-100, 101);
                    float num12 = num11 * (float)(1.0 + Main.rand.Next(-20, 21) * 0.01);
                    if (Main.rand.NextBool(4))
                        num12 *= (float)(1.0 + Main.rand.Next(5, 11) * 0.01);
                    if (Main.rand.NextBool(8))
                        num12 *= (float)(1.0 + Main.rand.Next(10, 21) * 0.01);
                    if (Main.rand.NextBool(12))
                        num12 *= (float)(1.0 + Main.rand.Next(20, 41) * 0.01);
                    if (Main.rand.NextBool(16))
                        num12 *= (float)(1.0 + Main.rand.Next(40, 81) * 0.01);
                    if (Main.rand.NextBool(20))
                        num12 *= (float)(1.0 + Main.rand.Next(50, 101) * 0.01);
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
            {
                Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 16, 16, ItemType<OldKey>(), 1, false, 0, false, false);
            }
            else if(Main.rand.NextBool(40))
            {
                Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 16, 16, ItemType<AncientSteelBar>(), Main.rand.Next(3, 6), false, 0, false, false);
            }
            else if (Main.rand.NextBool(20))
            {
                Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 16, 16, ItemType<Peanut>(), Main.rand.Next(3, 6), false, 0, false, false);
            }
        }
    }
    internal class AVPot : ModItem
	{
		public override void SetDefaults()
		{
			Item.CloneDefaults(ItemID.DartTrap);
			Item.width = 30;
			Item.height = 26;
			Item.createTile = TileType<AVPots>();
			Item.value = 0;
		}
	}
}