using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace SOTS.Items.Pyramid.PyramidWalls
{
	public class PyramidWall : ModItem
	{
		public override void SetDefaults()
		{
			Item.CloneDefaults(ItemID.StoneWall);
			Item.width = 28;
			Item.height = 28;
			Item.rare = ItemRarityID.Blue;
			Item.createWall = ModContent.WallType<PyramidWallWall>();
		}
		public override void AddRecipes()
		{
			CreateRecipe(4).AddIngredient(ModContent.ItemType<PyramidSlab>(), 1).AddTile(TileID.WorkBenches).Register();
			CreateRecipe(1).AddIngredient(ModContent.ItemType<PyramidWall>(), 4).AddTile(TileID.WorkBenches).ReplaceResult(ModContent.ItemType<PyramidSlab>());
		}
	}
	public class UnsafePyramidWall : ModItem
	{
		public override string Texture => "SOTS/Items/Pyramid/PyramidWalls/PyramidWall";
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
			Item.createWall = ModContent.WallType<UnsafePyramidWallWall>();
		}
	}
	public class PyramidWallWall : ModWall
	{
		public override void SetStaticDefaults()
		{
			Main.wallHouse[Type] = true;
			Main.wallLargeFrames[Type] = (byte)1;
			DustType = 32;
			ItemDrop = ModContent.ItemType<PyramidWall>();
			AddMapEntry(new Color(89, 81, 38));
		}
	}
	public class UnsafePyramidWallWall : ModWall
	{
		public override string Texture => "SOTS/Items/Pyramid/PyramidWalls/PyramidWallWall";
		public override void SetStaticDefaults()
		{
			Main.wallLargeFrames[Type] = (byte)1;
			Main.wallHouse[Type] = false;
			DustType = 32;
			ItemDrop = ModContent.ItemType<PyramidWall>();
			AddMapEntry(new Color(89, 81, 38));
		}
	}
}