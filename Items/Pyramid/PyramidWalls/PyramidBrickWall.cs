using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.Pyramid.PyramidWalls
{
	public class PyramidBrickWall : ModItem
	{
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
			CreateRecipe(1).AddIngredient(this, 4).AddTile(TileID.WorkBenches).ReplaceResult(ModContent.ItemType<PyramidBrick>());
		}
	}
	public class UnsafePyramidBrickWall : ModItem
	{
		public override string Texture => "SOTS/Items/Pyramid/PyramidWalls/PyramidBrickWall";
		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault("Changes the biome to pyramid when in front of\nAlso envokes the Pharaoh's Curse");
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
			ItemDrop = ModContent.ItemType<PyramidBrickWall>();
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
			ItemDrop = ModContent.ItemType<PyramidBrickWall>();
			AddMapEntry(new Color(75, 69, 27));
		}
	}
}