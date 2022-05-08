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
			Item.width = 24;
			Item.height = 28;
            Item.value = Item.sellPrice(0, 0, 2, 0);
			Item.rare = 3;
			Item.maxStack = 30;
            Item.buffType = mod.BuffType("Assassination");   
            Item.buffTime = 3600 * 12 + 30;  
            Item.UseSound = SoundID.Item3;            
            Item.useStyle = 2;        
            Item.useTurn = true;
            Item.useAnimation = 16;
            Item.useTime = 16;
            Item.consumable = true;       
			
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