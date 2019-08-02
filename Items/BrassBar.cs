using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items
{
	public class BrassBar : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Brass Bar");
			Tooltip.SetDefault("");
		}
		public override void SetDefaults()
		{

			item.width = 30;
			item.height = 24;
			item.value = 125;
			item.rare = 1;
			item.maxStack = 99;
		}
		public override void AddRecipes()
		{
			ModRecipe a = new ModRecipe(mod);
			a.AddIngredient(ItemID.SandBlock, 24);
			a.AddIngredient(ItemID.CopperBar, 3);
			a.AddIngredient(ItemID.SilverBar, 1);
			a.AddTile(TileID.Furnaces);
			a.SetResult(this, 4);
			a.AddRecipe();
			
			ModRecipe b = new ModRecipe(mod);
			b.AddIngredient(ItemID.SandBlock, 24);
			b.AddIngredient(ItemID.TinBar, 3);
			b.AddIngredient(ItemID.SilverBar, 1);
			b.AddTile(TileID.Furnaces);
			b.SetResult(this, 4);
			b.AddRecipe();
			
			ModRecipe c = new ModRecipe(mod);
			c.AddIngredient(ItemID.SandBlock, 24);
			c.AddIngredient(ItemID.TinBar, 3);
			c.AddIngredient(ItemID.TungstenBar, 1);
			c.AddTile(TileID.Furnaces);
			c.SetResult(this, 4);
			c.AddRecipe();
			
			ModRecipe d = new ModRecipe(mod);
			d.AddIngredient(ItemID.SandBlock, 24);
			d.AddIngredient(ItemID.CopperBar, 3);
			d.AddIngredient(ItemID.TungstenBar, 1);
			d.AddTile(TileID.Furnaces);
			d.SetResult(this, 4);
			d.AddRecipe();
		}
	}
}