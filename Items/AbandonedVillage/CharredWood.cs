using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.AbandonedVillage
{
	public class CharredWoodTile : ModTile
	{
		public override void SetStaticDefaults()
		{
			//Main.tileBrick[Type] = true;
			Main.tileSolid[Type] = true;
			Main.tileMergeDirt[Type] = true;
			Main.tileBlockLight[Type] = true;
			Main.tileLighted[Type] = false;
			//Main.tileBlendAll[Type] = true;
			DustType = 122; //boreal wood
			AddMapEntry(new Color(105, 82, 61));
		}
	}
	public class CharredWood : ModItem
	{
		public override void SetStaticDefaults()
		{
			this.SetResearchCost(100);
		}
		public override void SetDefaults()
		{
			Item.CloneDefaults(ItemID.StoneBlock);
			Item.width = 24;
			Item.height = 18;
			Item.createTile = ModContent.TileType<CharredWoodTile>();
		}
	}
	public class CharredWoodWallTile : ModWall
	{
		public override void SetStaticDefaults()
		{
			Main.wallHouse[Type] = true;
			DustType = 122;
			////ItemDrop/* tModPorter Note: Removed. Tiles and walls will drop the item which places them automatically. Use RegisterItemDrop to alter the automatic drop if necessary. */ = ModContent.ItemType<CharredWoodWall>();
			AddMapEntry(new Color(67, 49, 34));
		}
	}
	public class CharredWoodWall : ModItem
	{
		public override void SetStaticDefaults()
		{
			this.SetResearchCost(400);
		}
		public override void SetDefaults()
		{
			Item.CloneDefaults(ItemID.StoneWall);
			Item.width = 28;
			Item.height = 28;
			Item.rare = ItemRarityID.Blue;
			Item.createWall = ModContent.WallType<CharredWoodWallTile>();
		}
	}
}