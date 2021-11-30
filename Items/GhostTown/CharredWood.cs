using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.GhostTown
{
	public class CharredWoodTile : ModTile
	{
		public override void SetDefaults()
		{
			Main.tileSolid[Type] = true;
			Main.tileMergeDirt[Type] = true;
			Main.tileBlockLight[Type] = true;
			Main.tileLighted[Type] = false;
			Main.tileBlendAll[Type] = true;
			dustType = 122; //boreal wood
			drop = ModContent.ItemType<CharredWood>();
			AddMapEntry(new Color(105, 82, 61));
		}
	}
	public class CharredWood : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Charred Wood");
			Tooltip.SetDefault("'Too burnt to be used'");
		}
		public override void SetDefaults()
		{
			item.CloneDefaults(ItemID.StoneBlock);
			item.width = 24;
			item.height = 18;
			item.createTile = ModContent.TileType<CharredWoodTile>();
		}
	}
	public class CharredWoodWallTile : ModWall
	{
		public override void SetDefaults()
		{
			Main.wallHouse[Type] = true;
			dustType = 122;
			drop = ModContent.ItemType<CharredWoodWall>();
			AddMapEntry(new Color(110, 81, 46));
		}
	}
	public class CharredWoodWall : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Charred Wood Wall");
			Tooltip.SetDefault("");
		}
		public override void SetDefaults()
		{
			item.CloneDefaults(ItemID.StoneWall);
			item.width = 28;
			item.height = 28;
			item.rare = ItemRarityID.Blue;
			item.createWall = ModContent.WallType<CharredWoodWallTile>();
		}
	}
}