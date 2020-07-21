using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.Vibrant
{
	public class EchoDisk : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Echo Disk");
			Tooltip.SetDefault("");
		}
		public override void SetDefaults()
		{
			item.CloneDefaults(279);
			item.damage = 10;
			item.useTime = 27;
			item.useAnimation = 27;
			item.ranged = true;
			item.thrown = false;
			item.value = Item.sellPrice(0, 0, 80, 0);
			item.rare = ItemRarityID.Blue;
			item.width = 26;
			item.height = 26;
			item.maxStack = 1;
			item.autoReuse = true;            
			item.shoot = mod.ProjectileType("EchoDisk"); 
            item.shootSpeed = 9.5f;
			item.knockBack = 3f;
			item.consumable = false;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "VeryGlowyMushroom", 1);
			recipe.AddRecipeGroup("IronBar", 12);
			recipe.SetResult(this);
			recipe.AddTile(TileID.Anvils);
			recipe.AddRecipe();
		}
	}
}