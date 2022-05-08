using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace SOTS.Items.Slime
{
	public class Wormwood : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Goopwood");
			Tooltip.SetDefault("");
		}
		public override void SetDefaults()
		{
			Item.width = 28;
			Item.height = 22;
			Item.maxStack = 999;
			Item.useTurn = true;
			Item.autoReuse = true;
			Item.useAnimation = 15;
			Item.useTime = 10;
			Item.useStyle = 1;
			Item.rare = 1;
			Item.consumable = true;
			Item.createTile = mod.TileType("WormwoodTile");
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddRecipeGroup(RecipeGroupID.Wood, 2);
			recipe.AddIngredient(ItemID.Gel, 5);
			recipe.AddTile(TileID.Solidifier);
			recipe.SetResult(this, 2);
			recipe.AddRecipe();
		}
	}
	public class WormwoodTile : ModTile
	{
		public override void SetDefaults()
		{
			Main.tileSolid[Type] = true;
			Main.tileBrick[Type] = true;
			Main.tileMergeDirt[Type] = true;
			Main.tileBlockLight[Type] = true;
			Main.tileLighted[Type] = true;
			drop = mod.ItemType("Wormwood");
			AddMapEntry(new Color(140, 70, 20));
			dustType = 7; //dynasty wood dust
		}
	}
}