using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using SOTS.Items.Invidia;

namespace SOTS.Items.Invidia
{
	public class OvergrownEvostoneBrick : ModItem
	{
		public override void SetStaticDefaults()
		{
			this.SetResearchCost(100);
		}
		public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.StoneBlock);
            Item.rare = ItemRarityID.Green;
			Item.createTile = ModContent.TileType<OvergrownEvostoneBrickTile>();
		}
	}
	public class OvergrownEvostoneBrickTile : ModTile
	{
		public override void SetStaticDefaults()
		{
			TileID.Sets.NeedsGrassFraming[Type] = true;
			TileID.Sets.NeedsGrassFramingDirt[Type] = ModContent.TileType<EvostoneBrickTile>();
			Main.tileSolid[Type] = true;
			Main.tileBrick[Type] = true;
			Main.tileMergeDirt[Type] = false;
			Main.tileBlockLight[Type] = true;
			Main.tileLighted[Type] = true;
			AddMapEntry(new Color(91, 153, 59));
			MineResist = 1.5f;
			HitSound = SoundID.Tink;
			DustType = DustID.Grass;
		}
		//public override void RandomUpdate(int i, int j)
		//{
		//	if (!Main.rand.NextBool(8))
		//		if (!Main.tile[i, j - 1].HasTile)
		//		{
		//			Tile tile = Main.tile[i, j - 1];
		//			WorldGen.PlaceTile(i, j - 1, ModContent.TileType<CursedGrass>(), true, false, -1, Main.rand.Next(12));
		//			tile.TileColor = Main.tile[i, j].TileColor;
		//			NetMessage.SendTileSquare(-1, i, j - 1, 3, TileChangeType.None);
		//		}
		//		else if (Main.rand.NextBool(8))
		//			GrowCurseVine(i, j);
		//	base.RandomUpdate(i, j);
		//}
		//public static void GrowCurseVine(int i, int j)
		//{
		//	if (!Main.tile[i, j + 1].HasTile && Main.tile[i, j+1].LiquidAmount == 0)
		//	{
		//		var flag9 = false;
		//		for (var VineY = j; VineY > j - 10; VineY--)
		//		{
		//			if (Main.tile[i, VineY].BottomSlope)
		//			{
		//				flag9 = false;
		//				break;
		//			}
		//			if (Main.tile[i, VineY].HasTile && !Main.tile[i, VineY].BottomSlope)
		//			{
		//				flag9 = true;
		//				break;
		//			}
		//		}

		//		if (flag9)
		//		{
		//			var num47 = i;
		//			var num48 = j + 1;
		//			if(Main.tile[num47, num48].LiquidAmount == 0)
		//			{
		//				Tile tile = Main.tile[num47, num48];
		//				tile.TileType = (ushort)ModContent.TileType<CursedVine>();
		//				tile.HasTile = true;
		//				tile.TileColor = Main.tile[i, j].TileColor;
		//				WorldGen.SquareTileFrame(num47, num48, true);
		//				if (Main.netMode == NetmodeID.Server)
		//				{
		//					NetMessage.SendTileSquare(-1, num47, num48, 3, TileChangeType.None);
		//				}
		//			}
		//		}
		//	}
		//}
		public override bool CanExplode(int i, int j)
		{
			return true;
		}
		public override bool Slope(int i, int j)
		{
			return true;
		}
	}
	//public class CursedGrass : ModTile
	//{
	//	public override void SetStaticDefaults()
	//	{
	//		Main.tileFrameImportant[Type] = true;
	//		Main.tileLavaDeath[Type] = true;
	//		Main.tileCut[Type] = true;
	//		Main.tileSolid[Type] = false;
	//		Main.tileBlockLight[Type] = false;
	//		Main.tileLighted[Type] = false;
	//		AddMapEntry(new Color(50, 115, 29));
	//		HitSound = SoundID.Grass;
	//		DustType = DustID.Grass;
	//		TileObjectData.newTile.CopyFrom(TileObjectData.Style1x1);
	//		TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.SolidTile, TileObjectData.newTile.Width, 0);
	//		TileObjectData.newTile.CoordinateHeights = new int[] { 20 };
	//		TileObjectData.newTile.StyleHorizontal = true;
	//		TileObjectData.newTile.DrawYOffset = 2;
	//		TileObjectData.addTile(Type);
	//	}
 //       public override void NumDust(int i, int j, bool fail, ref int num)
 //       {
	//		num--;
 //           base.NumDust(i, j, fail, ref num);
 //       }
	//}
	//public class CursedVine : ModTile
	//{
	//	public override void SetStaticDefaults()
	//	{
	//		Main.tileFrameImportant[Type] = false;
	//		Main.tileLavaDeath[Type] = true;
	//		Main.tileCut[Type] = true;
	//		Main.tileSolid[Type] = false;
	//		Main.tileBlockLight[Type] = false;
	//		Main.tileLighted[Type] = false;
	//		AddMapEntry(new Color(50, 115, 29));
	//		HitSound = SoundID.Grass;
	//		DustType = DustID.Grass;
	//		TileObjectData.newTile.CopyFrom(TileObjectData.Style1x1); 
	//		TileObjectData.newTile.AnchorTop = new AnchorData(AnchorType.AlternateTile, TileObjectData.newTile.Width, 0);
	//		TileObjectData.newTile.AnchorBottom = AnchorData.Empty;
	//		TileObjectData.newTile.Origin = new Point16(0, 0);
	//		TileObjectData.newTile.CoordinateHeights = new[] { 16 };
	//		TileObjectData.newTile.DrawYOffset = -2;
	//		TileObjectData.newTile.AnchorAlternateTiles = new int[]
	//		{
	//			ModContent.TileType<OvergrownPyramidTile>(),
	//			ModContent.TileType<CursedVine>(),
	//		};
	//		TileObjectData.addTile(Type);
	//	}
	//	public override void NumDust(int i, int j, bool fail, ref int num)
	//	{
	//		num--;
	//		base.NumDust(i, j, fail, ref num);
	//	}
 //       public override bool TileFrame(int i, int j, ref bool resetFrame, ref bool noBreak)
 //       {
	//		if (!Main.tile[i, j - 1].HasTile || !(Main.tile[i, j - 1].TileType == ModContent.TileType<CursedVine>() || Main.tile[i, j - 1].TileType == ModContent.TileType<OvergrownPyramidTile>() || Main.tile[i, j - 1].TileType == ModContent.TileType<OvergrownPyramidTileSafe>()))
	//			WorldGen.KillTile(i, j, false, false, false);
 //           return base.TileFrame(i, j, ref resetFrame, ref noBreak);
 //       }
 //       public override void RandomUpdate(int i, int j)
	//	{
	//		if(Main.rand.NextBool(8))
	//			OvergrownPyramidTile.GrowCurseVine(i, j);
	//	}
	//}
}