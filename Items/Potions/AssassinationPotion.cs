using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.Potions
{
	public class AssassinationPotion : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Assassination Potion");
			
			Tooltip.SetDefault("Execute enemies below 20 health or 10% health");
		}
		public override void SetDefaults()
		{
			item.width = 24;
			item.height = 30;
            item.value = Item.sellPrice(0, 0, 2, 0);
			item.rare = 3;
			item.maxStack = 30;
            item.buffType = mod.BuffType("Assassination");   
            item.buffTime = 3600 * 12 + 30;  
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
			recipe.AddIngredient(null, "TwilightGel", 10);
			recipe.AddIngredient(null, "FragmentOfOtherworld", 1);
			recipe.AddIngredient(ItemID.Fireblossom, 1);
			recipe.AddTile(13);
			recipe.SetResult(this, 1);
			recipe.AddRecipe();
		}
	}
}