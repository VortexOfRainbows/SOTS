using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;



namespace SOTS.Items.SpectreCog
{
	public class ReanimationMaterial : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Reanimation Material");
			Main.RegisterItemAnimation(item.type, new DrawAnimationVertical(5, 8));
			Tooltip.SetDefault("Has the power to create... life?");
		}
		public override void SetDefaults()
		{

			item.width = 57;
			item.height = 7;
			item.value = 0;
			item.rare = 10;
			item.maxStack = 999;
			 ItemID.Sets.ItemNoGravity[item.type] = true; 


			
		}
		
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.Cog, 12);
			recipe.AddIngredient(ItemID.Ectoplasm, 24);
			recipe.AddIngredient(null, "NightmareFuel", 3);
			recipe.AddTile(TileID.LunarCraftingStation);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}