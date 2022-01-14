using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using SOTS.Dusts;

namespace SOTS.Items.Pyramid.PyramidWalls
{
	public class MalditeWall : ModItem
	{
		public override void SetDefaults()
		{
			item.CloneDefaults(ItemID.StoneWall);
			item.width = 28;
			item.height = 28;
			item.rare = ItemRarityID.Blue;
			item.createWall = ModContent.WallType<MalditeWallWall>();
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<Maldite>(), 1);
			recipe.AddTile(TileID.WorkBenches);
			recipe.SetResult(this, 4);
			recipe.AddRecipe();

			recipe = new ModRecipe(mod);
			recipe.AddIngredient(this, 4);
			recipe.AddTile(TileID.WorkBenches);
			recipe.SetResult(ModContent.ItemType<Maldite>(), 1);
			recipe.AddRecipe();
		}
	}
	public class UnsafeMalditeWall : ModItem
	{
		public override string Texture => "SOTS/Items/Pyramid/PyramidWalls/MalditeWall";
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
			item.createWall = ModContent.WallType<UnsafeMalditeWallWall>();
		}
	}
	public class MalditeWallWall : ModWall
	{
		public override void SetDefaults()
		{
			Main.wallHouse[Type] = true;
			dustType = ModContent.DustType<CurseDust3>();
			drop = ModContent.ItemType<MalditeWall>();
			AddMapEntry(new Color(22, 15, 30));
		}
	}
	public class UnsafeMalditeWallWall : ModWall
	{
		public override bool Autoload(ref string name, ref string texture)
		{
			texture = "SOTS/Items/Pyramid/PyramidWalls/MalditeWallWall";
			return base.Autoload(ref name, ref texture);
		}
		public override void SetDefaults()
		{
			Main.wallHouse[Type] = false;
			dustType = ModContent.DustType<CurseDust3>();
			drop = ModContent.ItemType<MalditeWall>();
			AddMapEntry(new Color(41, 34, 51));
		}
	}
}