using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Items.Fragments;
using SOTS.Items.Slime;
using SOTS.WorldgenHelpers;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.GameContent;
using Terraria.GameContent.ObjectInteractions;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace SOTS.Items
{
	public class PeanutBush : ModItem
	{
		public override void SetStaticDefaults()
		{
			this.SetResearchCost(1);
		}
		public override void SetDefaults()
		{
			Item.CloneDefaults(ItemID.StoneBlock);
			Item.Size = new Vector2(34, 30);
			Item.rare = ItemRarityID.Blue;
			Item.createTile = ModContent.TileType<PeanutBushTile>();
		}
		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(ItemID.Acorn, 1).AddIngredient<Peanut>(10).AddIngredient<FragmentOfNature>(10).Register();
		}
	}
	public class PeanutBushTile : ModTile
	{
		public override void SetStaticDefaults()
		{
			MineResist = 0.01f;
			Main.tileNoFail[Type] = true;
			Main.tileSolid[Type] = false;
			Main.tileFrameImportant[Type] = true;
			Main.tileLavaDeath[Type] = true;
			TileObjectData.newTile.CopyFrom(TileObjectData.StyleDye);
			TileObjectData.newTile.CoordinateWidth = 52;
			TileObjectData.newTile.CoordinatePadding = 2;
			TileObjectData.newTile.CoordinateHeights = new int[1] { 40 };
			TileObjectData.newTile.DrawYOffset = -22;
			TileObjectData.newTile.DrawFlipHorizontal = false;
			TileObjectData.newTile.RandomStyleRange = 3;
			TileObjectData.addTile(Type);
			DustType = DustID.WoodFurniture;
			LocalizedText name = CreateMapEntryName();
			AddMapEntry(new Color(113, 173, 37), name);
		}
        public override bool CanPlace(int i, int j)
        {
			for(int i2 = i - 1; i2 <= i + 1; i2++)
			{
				for (int j2 = j - 1; j2 <= j + 1; j2++)
				{
					Tile tile = Framing.GetTileSafely(i2, j2);
					if(tile.TileType == Type)
                    {
						return false;
                    }
				}
			}
            return true;
        }
        public override bool CreateDust(int i, int j, ref int type)
        {
			if (Main.rand.NextBool(3))
				type = DustID.Grass;
            return true;
		}
		public override void NumDust(int i, int j, bool fail, ref int num)
		{
			num = 12;
		}
        public override void RandomUpdate(int i, int j)
        {
			if(WorldGen.InWorld(i, j, 20))
				AttemptToGrowPeanuts(i, j);
        }
		public static void AttemptToGrowPeanuts(int i, int j)
		{
			for (int y = 2; y <= 7; y++)
			{
				for (int x = -3; x <= 3; x++)
				{
					Tile tileToConvert = Main.tile[i + x, j + y];
					if(tileToConvert.HasTile && (Main.tile[i + x, j + y - 1].TileType != ModContent.TileType<PeanutBushTile>() || !Main.tile[i + x, j + y - 1].HasTile))
					{
						bool PeanutAdjecent = (y == 2 && x == 0)
							|| (Main.tile[i + x - 1, j + y].TileType == ModContent.TileType<PeanutOreTile>() && Main.tile[i + x - 1, j + y].HasTile)
							|| (Main.tile[i + x + 1, j + y].TileType == ModContent.TileType<PeanutOreTile>() && Main.tile[i + x + 1, j + y].HasTile)
							|| (Main.tile[i + x, j + y - 1].TileType == ModContent.TileType<PeanutOreTile>() && Main.tile[i + x, j + y - 1].HasTile)
							|| (Main.tile[i + x, j + y + 1].TileType == ModContent.TileType<PeanutOreTile>() && Main.tile[i + x, j + y + 1].HasTile);
						int fartherAway = 2;
						if (x == -3 || x == 3)
						{
							fartherAway += 2;
						}
						if (y >= 6)
							fartherAway += 2;
						if ((PeanutAdjecent || WorldGen.genRand.NextBool(50)) && (tileToConvert.TileType == TileID.Dirt || (tileToConvert.TileType == TileID.Grass && Main.tile[i + x, j + y - 1].HasTile && WorldGen.genRand.NextBool(2))))
						{
							if (WorldGen.genRand.NextBool(y * 2 + Math.Abs(x) * 2 + fartherAway))
							{
								tileToConvert.TileType = (ushort)ModContent.TileType<PeanutOreTile>();
								WorldGen.SquareTileFrame(i + x, j + y);
								if (Main.netMode != NetmodeID.SinglePlayer)
									NetMessage.SendTileSquare(-1, i + x, j + y, 3, TileChangeType.None);
								return;
							}
						}
					}
				}
			}
		}
        public override IEnumerable<Item> GetItemDrops(int i, int j)
        {
			yield return new Item(ModContent.ItemType<PeanutBush>());
        }
    }
}