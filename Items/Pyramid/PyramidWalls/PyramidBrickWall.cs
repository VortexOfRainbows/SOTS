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
			item.CloneDefaults(ItemID.StoneWall);
			item.width = 28;
			item.height = 28;
			item.rare = ItemRarityID.Blue;
			item.createWall = ModContent.WallType<PyramidBrickWallWall>();
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<PyramidBrick>(), 1);
			recipe.AddTile(TileID.WorkBenches);
			recipe.SetResult(this, 4);
			recipe.AddRecipe();
			recipe = new ModRecipe(mod);
			recipe.AddIngredient(this, 4);
			recipe.AddTile(TileID.WorkBenches);
			recipe.SetResult(ModContent.ItemType<PyramidBrick>(), 1);
			recipe.AddRecipe();
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
			item.CloneDefaults(ItemID.StoneWall);
			item.width = 28;
			item.height = 28;
			item.rare = ItemRarityID.Red;
			item.createWall = ModContent.WallType<UnsafePyramidBrickWallWall>();
		}
	}
	public class PyramidBrickWallWall : ModWall
	{
		public override void SetDefaults()
		{
			Main.wallHouse[Type] = true;
			dustType = 32;
			drop = ModContent.ItemType<PyramidBrickWall>();
			AddMapEntry(new Color(75, 69, 27));
		}
	}
	public class UnsafePyramidBrickWallWall : ModWall
	{
		public override bool Autoload(ref string name, ref string texture)
		{
			texture = "SOTS/Items/Pyramid/PyramidWalls/PyramidBrickWallWall";
			return base.Autoload(ref name, ref texture);
		}
		public override void SetDefaults()
		{
			Main.wallHouse[Type] = false;
			dustType = 32;
			drop = ModContent.ItemType<PyramidBrickWall>();
			AddMapEntry(new Color(75, 69, 27));
		}
	}
}