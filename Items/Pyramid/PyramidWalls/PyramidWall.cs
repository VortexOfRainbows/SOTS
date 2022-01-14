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
			item.CloneDefaults(ItemID.StoneWall);
			item.width = 28;
			item.height = 28;
			item.rare = ItemRarityID.Blue;
			item.createWall = ModContent.WallType<PyramidWallWall>();
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<PyramidSlab>(), 1);
			recipe.AddTile(TileID.WorkBenches);
			recipe.SetResult(this, 4);
			recipe.AddRecipe();
			
			recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<PyramidWall>(), 4);
			recipe.AddTile(TileID.WorkBenches);
			recipe.SetResult(ModContent.ItemType<PyramidSlab>(), 1);
			recipe.AddRecipe();
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
			item.CloneDefaults(ItemID.StoneWall);
			item.width = 28;
			item.height = 28;
			item.rare = ItemRarityID.Red;
			item.createWall = ModContent.WallType<UnsafePyramidWallWall>();
		}
	}
	public class PyramidWallWall : ModWall
	{
		public override void SetDefaults()
		{
			Main.wallHouse[Type] = true;
			Main.wallLargeFrames[Type] = (byte)1;
			dustType = 32;
			drop = ModContent.ItemType<PyramidWall>();
			AddMapEntry(new Color(89, 81, 38));
		}
	}
	public class UnsafePyramidWallWall : ModWall
	{
		public override bool Autoload(ref string name, ref string texture)
		{
			texture = "SOTS/Items/Pyramid/PyramidWalls/PyramidWallWall";
			return base.Autoload(ref name, ref texture);
		}
		public override void SetDefaults()
		{
			Main.wallLargeFrames[Type] = (byte)1;
			Main.wallHouse[Type] = false;
			dustType = 32;
			drop = ModContent.ItemType<PyramidWall>();
			AddMapEntry(new Color(89, 81, 38));
		}
	}
}