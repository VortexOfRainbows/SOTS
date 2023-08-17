using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.Pyramid.PyramidWalls
{
	public class PyramidBrickWall : ModItem
	{
		public override void SetStaticDefaults() => this.SetResearchCost(400);
		public override void SetDefaults()
		{
			Item.CloneDefaults(ItemID.StoneWall);
			Item.width = 28;
			Item.height = 28;
			Item.rare = ItemRarityID.Blue;
			Item.createWall = ModContent.WallType<PyramidBrickWallWall>();
		}
		public override void AddRecipes()
		{
			CreateRecipe(4).AddIngredient(ModContent.ItemType<PyramidBrick>(), 1).AddTile(TileID.WorkBenches).Register();
			Recipe.Create(ModContent.ItemType<PyramidBrick>()).AddIngredient(this, 4).AddTile(TileID.WorkBenches).Register();
		}
	}
	public class UnsafePyramidBrickWall : ModItem
	{
		public override string Texture => "SOTS/Items/Pyramid/PyramidWalls/PyramidBrickWall";
		public override void SetStaticDefaults()
		{
			this.SetResearchCost(400);
		}
		public override void SetDefaults()
		{
			Item.CloneDefaults(ItemID.StoneWall);
			Item.width = 28;
			Item.height = 28;
			Item.rare = ItemRarityID.Red;
			Item.createWall = ModContent.WallType<UnsafePyramidBrickWallWall>();
		}
	}
	public class PyramidBrickWallWall : ModWall
	{
		public override void SetStaticDefaults()
		{
			Main.wallHouse[Type] = true;
			DustType = 32;
			//ItemDrop/* tModPorter Note: Removed. Tiles and walls will drop the item which places them automatically. Use RegisterItemDrop to alter the automatic drop if necessary. */ = ModContent.ItemType<PyramidBrickWall>();
			AddMapEntry(new Color(75, 69, 27));
		}
	}
	public class UnsafePyramidBrickWallWall : ModWall
	{
		public override string Texture => "SOTS/Items/Pyramid/PyramidWalls/PyramidBrickWallWall";
		public override void SetStaticDefaults()
		{
			Main.wallHouse[Type] = false;
			DustType = 32;
			//ItemDrop/* tModPorter Note: Removed. Tiles and walls will drop the item which places them automatically. Use RegisterItemDrop to alter the automatic drop if necessary. */ = ModContent.ItemType<PyramidBrickWall>();
			AddMapEntry(new Color(75, 69, 27));
		}
	}
}