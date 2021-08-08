using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;
using Microsoft.Xna.Framework;
using Terraria.ObjectData;
using Terraria.DataStructures;
using Terraria.Enums;

namespace SOTS.Items.Pyramid
{
	public class OvergrownPyramidBlock : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Overgrown Pyramid Brick");
			Tooltip.SetDefault("");
		}
		public override void SetDefaults()
		{
			item.width = 16;
			item.height = 16;
			item.maxStack = 999;
			item.useTurn = true;
			item.autoReuse = true;
			item.useAnimation = 15;
			item.useTime = 10;
			item.useStyle = 1;
			item.rare = 5;
			item.consumable = true;
			item.createTile = mod.TileType("OvergrownPyramidTileSafe");
		}
	}
	public class OvergrownPyramidTileSafe : ModTile
	{
		public override void SetDefaults()
		{
			TileID.Sets.NeedsGrassFraming[Type] = true;
			TileID.Sets.NeedsGrassFramingDirt[Type] = ModContent.TileType<PyramidSlabTile>();
			Main.tileSolid[Type] = true;
			Main.tileMergeDirt[Type] = false;
			Main.tileBlockLight[Type] = true;
			Main.tileLighted[Type] = true;
			drop = mod.ItemType("OvergrownPyramidBlock");
			AddMapEntry(new Color(40, 160, 100));
			mineResist = 1.5f;
			soundType = 21;
			soundStyle = 2;
			dustType = DustID.Grass;
		}
		public override void RandomUpdate(int i, int j)
		{
			if (!Main.rand.NextBool(8))
				if (!Main.tile[i, j - 1].active())
				{
					WorldGen.PlaceTile(i, j - 1, mod.TileType("CursedGrass"), true, false, -1, Main.rand.Next(12));
					Main.tile[i, j - 1].color(Main.tile[i, j].color());
					NetMessage.SendTileSquare(-1, i, j - 1, 3, TileChangeType.None);
				}
				else if (Main.rand.NextBool(8))
					GrowCurseVine(i, j);
			base.RandomUpdate(i, j);
		}
		public static void GrowCurseVine(int i, int j)
		{
			if (!Main.tile[i, j + 1].active() && !Main.tile[i, j + 1].lava())
			{
				var flag9 = false;
				for (var VineY = j; VineY > j - 10; VineY--)
				{
					if (Main.tile[i, VineY].bottomSlope())
					{
						flag9 = false;
						break;
					}
					if (Main.tile[i, VineY].active() && !Main.tile[i, VineY].bottomSlope())
					{
						flag9 = true;
						break;
					}
				}

				if (flag9)
				{
					var num47 = i;
					var num48 = j + 1;
					if(Main.tile[num47, num48].liquid == 0)
					{
						Main.tile[num47, num48].type = (ushort)ModContent.TileType<CursedVine>();
						Main.tile[num47, num48].active(true);
						Main.tile[num47, num48].color(Main.tile[i, j].color());
						WorldGen.SquareTileFrame(num47, num48, true);
						if (Main.netMode == NetmodeID.Server)
						{
							NetMessage.SendTileSquare(-1, num47, num48, 3, TileChangeType.None);
						}
					}
				}
			}
		}
		public override bool CanExplode(int i, int j)
		{
			return true;
		}
		public override bool Slope(int i, int j)
		{
			if (SOTSWorld.downedCurse)
				return true;

			return true;
		}
	}
	public class OvergrownPyramidTile : ModTile
	{
		public override void SetDefaults()
		{
			//Main.tileMerge[Type][ModContent.TileType<PyramidSlabTile>()] = true;
			TileID.Sets.NeedsGrassFraming[Type] = true;
			TileID.Sets.NeedsGrassFramingDirt[Type] = ModContent.TileType<PyramidSlabTile>();
			Main.tileSolid[Type] = true;
			Main.tileMergeDirt[Type] = false;
			Main.tileBlockLight[Type] = true;
			Main.tileLighted[Type] = true;
			drop = mod.ItemType("OvergrownPyramidBlock");
			AddMapEntry(new Color(40, 160, 100));
			mineResist = 1.5f;
			minPick = 180;
			soundType = 21;
			soundStyle = 2;
			dustType = DustID.Grass;
		}
        public override void RandomUpdate(int i, int j)
		{
			if (!Main.rand.NextBool(8))
				if (!Main.tile[i, j - 1].active())
				{
					WorldGen.PlaceTile(i, j - 1, mod.TileType("CursedGrass"), true, false, -1, Main.rand.Next(12));
					Main.tile[i, j - 1].color(Main.tile[i, j].color());
					NetMessage.SendTileSquare(-1, i, j - 1, 3, TileChangeType.None);
				}
			else if (Main.rand.NextBool(8))
				GrowCurseVine(i, j);
			base.RandomUpdate(i, j);
        }
		public static void GrowCurseVine(int i, int j)
		{
			if (!Main.tile[i, j + 1].active() && !Main.tile[i, j + 1].lava())
			{
				var flag9 = false;
				for (var VineY = j; VineY > j - 10; VineY--)
				{
					if (Main.tile[i, VineY].bottomSlope())
					{
						flag9 = false;
						break;
					}
					if (Main.tile[i, VineY].active() && !Main.tile[i, VineY].bottomSlope())
					{
						flag9 = true;
						break;
					}
				}

				if (flag9)
				{
					var num47 = i;
					var num48 = j + 1;
					if (Main.tile[num47, num48].liquid == 0)
					{
						Main.tile[num47, num48].type = (ushort)ModContent.TileType<CursedVine>();
						Main.tile[num47, num48].active(true);
						Main.tile[num47, num48].color(Main.tile[i, j].color());
						WorldGen.SquareTileFrame(num47, num48, true);
						if (Main.netMode == 2)
						{
							NetMessage.SendTileSquare(-1, num47, num48, 3, TileChangeType.None);
						}
					}
				}
			}
		}
        public override bool CanExplode(int i, int j)
		{
			return false;
		}
		public override bool Slope(int i, int j)
		{
			if (SOTSWorld.downedCurse)
				return true;

			return false;
		}
	}
	public class CursedGrass : ModTile
	{
		public override void SetDefaults()
		{
			Main.tileFrameImportant[Type] = true;
			Main.tileLavaDeath[Type] = true;
			Main.tileCut[Type] = true;
			Main.tileSolid[Type] = false;
			Main.tileBlockLight[Type] = false;
			Main.tileLighted[Type] = false;
			AddMapEntry(new Color(40, 180, 120));
			soundType = SoundID.Grass;
			soundStyle = 2;
			dustType = DustID.Grass;
			TileObjectData.newTile.CopyFrom(TileObjectData.Style1x1);
			TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.SolidTile, TileObjectData.newTile.Width, 0);
			TileObjectData.newTile.CoordinateHeights = new int[] { 20 };
			TileObjectData.newTile.StyleHorizontal = true;
			TileObjectData.newTile.DrawYOffset = 2;
			TileObjectData.addTile(Type);
		}
        public override void NumDust(int i, int j, bool fail, ref int num)
        {
			num--;
            base.NumDust(i, j, fail, ref num);
        }
	}
	public class CursedVine : ModTile
	{
		public override void SetDefaults()
		{
			Main.tileFrameImportant[Type] = false;
			Main.tileLavaDeath[Type] = true;
			Main.tileCut[Type] = true;
			Main.tileSolid[Type] = false;
			Main.tileBlockLight[Type] = false;
			Main.tileLighted[Type] = false;
			AddMapEntry(new Color(40, 180, 120));
			soundType = SoundID.Grass;
			soundStyle = 2;
			dustType = DustID.Grass;
			TileObjectData.newTile.CopyFrom(TileObjectData.Style1x1); 
			TileObjectData.newTile.AnchorTop = new AnchorData(AnchorType.AlternateTile, TileObjectData.newTile.Width, 0);
			TileObjectData.newTile.AnchorBottom = AnchorData.Empty;
			TileObjectData.newTile.Origin = new Point16(0, 0);
			TileObjectData.newTile.CoordinateHeights = new[] { 16 };
			TileObjectData.newTile.DrawYOffset = -2;
			TileObjectData.newTile.AnchorAlternateTiles = new int[]
			{
				ModContent.TileType<OvergrownPyramidTile>(),
				ModContent.TileType<CursedVine>(),
			};
			TileObjectData.addTile(Type);
		}
		public override void NumDust(int i, int j, bool fail, ref int num)
		{
			num--;
			base.NumDust(i, j, fail, ref num);
		}
        public override bool TileFrame(int i, int j, ref bool resetFrame, ref bool noBreak)
        {
			if (!Main.tile[i, j - 1].active() || !(Main.tile[i, j - 1].type == mod.TileType("CursedVine") || Main.tile[i, j - 1].type == mod.TileType("OvergrownPyramidTile") || Main.tile[i, j - 1].type == mod.TileType("OvergrownPyramidTileSafe")))
				WorldGen.KillTile(i, j, false, false, false);
            return base.TileFrame(i, j, ref resetFrame, ref noBreak);
        }
        public override void RandomUpdate(int i, int j)
		{
			if(Main.rand.NextBool(8))
				OvergrownPyramidTile.GrowCurseVine(i, j);
		}
	}
}