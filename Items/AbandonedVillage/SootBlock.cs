using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.AbandonedVillage
{
	public class SootBlockTile : ModTile
	{
		public override void SetStaticDefaults()
		{
			Main.tileBrick[Type] = true;
			Main.tileSolid[Type] = true;
			Main.tileMergeDirt[Type] = true;
			Main.tileBlockLight[Type] = true;
			Main.tileLighted[Type] = false;
			Main.tileBlendAll[Type] = true;
			DustType = 38; //mud
			ItemDrop = ModContent.ItemType<SootBlock>();
			AddMapEntry(new Color(57, 50, 44));
		}
	}
	public class SootBlock : ModItem
	{
		public override void SetStaticDefaults() => this.SetResearchCost(100);
		public override void SetDefaults()
		{
			Item.CloneDefaults(ItemID.StoneBlock);
			Item.createTile = ModContent.TileType<SootBlockTile>();
		}
	}
	public class SootWallTile : ModWall
	{
		public override void SetStaticDefaults()
		{
			Main.wallHouse[Type] = true;
			DustType = 38;
			ItemDrop = ModContent.ItemType<SootWall>();
			AddMapEntry(new Color(34, 29, 24));
		}
	}
	public class SootWall : ModItem
	{
		public override void SetStaticDefaults() => this.SetResearchCost(400);
		public override void SetDefaults()
		{
			Item.CloneDefaults(ItemID.StoneWall);
			Item.width = 28;
			Item.height = 28;
			Item.rare = ItemRarityID.Blue;
			Item.createWall = ModContent.WallType<SootWallTile>();
		}
	}
}