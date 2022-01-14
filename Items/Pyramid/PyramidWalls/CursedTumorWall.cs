using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using SOTS.Dusts;

namespace SOTS.Items.Pyramid.PyramidWalls
{
	public class CursedTumorWall : ModItem
	{
		public override void SetDefaults()
		{
			item.CloneDefaults(ItemID.StoneWall);
			item.width = 28;
			item.height = 28;
			item.rare = ItemRarityID.Blue;
			item.createWall = ModContent.WallType<CursedTumorWallWall>();
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<CursedTumor>(), 1);
			recipe.AddTile(TileID.WorkBenches);
			recipe.SetResult(this, 4);
			recipe.AddRecipe();

			recipe = new ModRecipe(mod);
			recipe.AddIngredient(this, 4);
			recipe.AddTile(TileID.WorkBenches);
			recipe.SetResult(ModContent.ItemType<CursedTumor>(), 1);
			recipe.AddRecipe();
		}
	}
	public class UnsafeCursedTumorWall : ModItem
	{
        public override string Texture => "SOTS/Items/Pyramid/PyramidWalls/CursedTumorWall";
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
			item.createWall = ModContent.WallType<UnsafeCursedTumorWallWall>();
		}
	}
	public class CursedTumorWallWall : ModWall
	{
		public override void SetDefaults()
		{
			Main.wallHouse[Type] = true;
			dustType = ModContent.DustType<CurseDust3>();
			drop = ModContent.ItemType<CursedTumorWall>();
			soundType = SoundID.NPCHit;
			soundStyle = 1;
			AddMapEntry(new Color(49, 33, 75));
		}
	}
	public class UnsafeCursedTumorWallWall : ModWall
	{
        public override bool Autoload(ref string name, ref string texture)
        {
			texture = "SOTS/Items/Pyramid/PyramidWalls/CursedTumorWallWall";
			return base.Autoload(ref name, ref texture);
        }
        public override void SetDefaults()
		{
			Main.wallHouse[Type] = false;
			dustType = ModContent.DustType<CurseDust3>();
			drop = ModContent.ItemType<CursedTumorWall>();
			soundType = SoundID.NPCHit;
			soundStyle = 1;
			AddMapEntry(new Color(49, 33, 75));
		}
	}
}