using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using SOTS.Dusts;

namespace SOTS.Items.Pyramid
{
	public class AcediaPlatingWall : ModItem
	{
		public override void SetStaticDefaults() => this.SetResearchCost(400);
		public override void SetDefaults()
		{
			Item.width = 16;
			Item.height = 16;
			Item.maxStack = 999;
			Item.useTurn = true;
			Item.autoReuse = true;
			Item.useAnimation = 15;
			Item.useTime = 7;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.rare = ItemRarityID.Cyan;
			Item.consumable = true;
			Item.createWall = ModContent.WallType<UnsafeAcediaWallWall>();
		}
		public override void AddRecipes()
		{
			CreateRecipe(4).AddIngredient<AcediaPlating>(1).AddTile(TileID.WorkBenches).Register();
		}
	}
	public class AcediaPlatingWallWall : ModWall
	{
		public override void SetStaticDefaults()
		{
			Main.wallLargeFrames[Type] = (byte)2;
			Main.wallHouse[Type] = true;
			DustType = ModContent.DustType<AcedianDust>(); 
			ItemDrop/* tModPorter Note: Removed. Tiles and walls will drop the item which places them automatically. Use RegisterItemDrop to alter the automatic drop if necessary. */ = ModContent.ItemType<AcediaPlatingWall>();
			AddMapEntry(new Color(180, 64, 170));
		}
	}
	public class UnsafeAcediaWallWall : ModWall
	{
		public override void SetStaticDefaults()
		{
			Main.wallLargeFrames[Type] = (byte)2;
			Main.wallHouse[Type] = false;
			DustType = ModContent.DustType<AcedianDust>();
			ItemDrop/* tModPorter Note: Removed. Tiles and walls will drop the item which places them automatically. Use RegisterItemDrop to alter the automatic drop if necessary. */ = ModContent.ItemType<AcediaPlatingWall>();
			AddMapEntry(new Color(180, 64, 170));
		}
	}
}