using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
// If you are using c# 6, you can use: "using static Terraria.Localization.GameCulture;" which would mean you could just write "DisplayName.AddTranslation(German, "");"
using Terraria.Localization;
using Microsoft.Xna.Framework;

namespace SOTS.Items.Pyramid
{
	public class PyramidBrick : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Pyramid Brick");
			Tooltip.SetDefault("A slab from an ancient burial site, it may be hard to break");
		}
		public override void SetDefaults()
		{
			item.width = 16;
			item.height = 16;
			item.maxStack = 999;
			item.useTurn = true;
			item.autoReuse = true;
			item.useAnimation = 15;
			item.useTime = 10;
			item.useStyle = 1;
			item.rare = ItemRarityID.LightRed;
			item.consumable = true;
			item.createTile = mod.TileType("PyramidBrickTile");
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.SandstoneBrick, 1);
			recipe.AddTile(TileID.Autohammer);
			recipe.SetResult(this, 1);
			recipe.AddRecipe();
			
			recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.SandstoneBrick, 1);
			recipe.AddTile(TileID.LunarCraftingStation);
			recipe.SetResult(this, 1);
			recipe.AddRecipe();
			
			recipe = new ModRecipe(mod);
			recipe.AddIngredient(this, 1);
			recipe.AddTile(TileID.WorkBenches);
			recipe.SetResult(ItemID.SandstoneBrick, 1);
			recipe.AddRecipe();
		}
	}
	public class PyramidBrickTile : ModTile
	{
		public override void SetDefaults()
		{
			Main.tileMerge[Type][ModContent.TileType<OvergrownPyramidTile>()] = true;
			Main.tileMerge[Type][ModContent.TileType<OvergrownPyramidTileSafe>()] = true;
			Main.tileBrick[Type] = true;
			Main.tileSolid[Type] = true;
			Main.tileMergeDirt[Type] = true;
			Main.tileBlockLight[Type] = true;
			Main.tileLighted[Type] = true;
			drop = ModContent.ItemType<PyramidBrick>();
			AddMapEntry(new Color(190, 160, 95));
			mineResist = 1.0f;
			minPick = 0;
			soundType = SoundID.Tink;
			soundStyle = 2;
			dustType = 32;
		}
	}
}