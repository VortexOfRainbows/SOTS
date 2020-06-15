using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items
{
	public class ShurikenSword : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Shuriken Slicer");
			Tooltip.SetDefault("");
		}
		public override void SetDefaults()
		{

			item.damage = 13;
			item.melee = true;
			item.width = 32;
			item.height = 32;
			item.useTime = 20;
			item.useAnimation = 20;
			item.useStyle = 1;
			item.knockBack = 3.5f;
			item.value = Item.sellPrice(0, 0, 25, 0);
			item.rare = 2;
			item.UseSound = SoundID.Item1;
			item.autoReuse = false;            
			item.shoot = 3; 
            item.shootSpeed = 24;
			item.crit = 6;

		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.IronBar, 18);
			recipe.AddIngredient(null, "Goblinsteel", 12);
			recipe.AddIngredient(ItemID.Shuriken, 50);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
			
			recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.LeadBar, 18);
			recipe.AddIngredient(null, "Goblinsteel", 12);
			recipe.AddIngredient(ItemID.Shuriken, 50);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}