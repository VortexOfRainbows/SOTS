using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;
using Microsoft.Xna.Framework;

namespace SOTS.Items.GelGear
{
	public class Wormwood : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Wormwood");
			Tooltip.SetDefault("");
		}

		public override void SetDefaults()
		{
			item.width = 28;
			item.height = 22;
			item.maxStack = 999;
			item.useTurn = true;
			item.autoReuse = true;
			item.useAnimation = 15;
			item.useTime = 15;
			item.useStyle = 1;
			item.rare = 1;
			item.consumable = true;
			item.createTile = mod.TileType("WormwoodTile");
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.Wood, 2);
			recipe.AddIngredient(null, "GelBar", 1);
			recipe.AddTile(TileID.WorkBenches);
			recipe.SetResult(this, 2);
			recipe.AddRecipe();
		}
	}
	public class WormwoodTile : ModTile
	{
		public override void SetDefaults()
		{
			Main.tileSolid[Type] = true;
			Main.tileMergeDirt[Type] = true;
			Main.tileBlockLight[Type] = true;
			Main.tileLighted[Type] = true;
			drop = mod.ItemType("Wormwood");
			AddMapEntry(new Color(140, 70, 20));
			dustType = 7; //dynasty wood dust
		}
	}
}