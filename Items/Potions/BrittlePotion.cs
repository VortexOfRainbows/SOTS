using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.Potions
{
	public class BrittlePotion : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Brittle Potion");
			Tooltip.SetDefault("Getting hit surrounds you with ice shards");
		}
		public override void SetDefaults()
		{
			item.width = 20;
			item.height = 30;
            item.value = Item.sellPrice(0, 0, 2, 0);
			item.rare = 1;
			item.maxStack = 30;
            item.buffType = mod.BuffType("Brittle");   
            item.buffTime = 3600 * 4 + 300;  
            item.UseSound = SoundID.Item3;            
            item.useStyle = 2;        
            item.useTurn = true;
            item.useAnimation = 16;
            item.useTime = 16;
            item.consumable = true;       
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.BottledWater, 1);
			recipe.AddIngredient(null, "FragmentOfPermafrost", 1);
			recipe.AddIngredient(ItemID.Shiverthorn, 1);
			recipe.AddTile(13);
			recipe.SetResult(this, 1);
			recipe.AddRecipe();
			
			recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.BottledWater, 1);
			recipe.AddIngredient(null, "FragmentOfPermafrost", 1);
			recipe.AddIngredient(ItemID.Shiverthorn, 1);
			recipe.AddTile(13);
			recipe.SetResult(this, 1);
			recipe.AddRecipe();
		}
	}
}