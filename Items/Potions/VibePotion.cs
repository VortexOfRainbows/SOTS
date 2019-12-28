using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;

namespace SOTS.Items.Potions
{
	public class VibePotion : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Vibe Potion");
			
			Tooltip.SetDefault("Increases attack speed by 20% while not moving and by 5% while moving");
		}
		public override void SetDefaults()
		{
			item.width = 24;
			item.height = 30;
            item.value = Item.sellPrice(0, 0, 2, 0);
			item.rare = 2;
			item.maxStack = 30;
            item.buffType = mod.BuffType("GoodVibes");   
            item.buffTime = 10000; //around 3 minutes
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
			recipe.AddIngredient(null, "FragmentOfNature", 1);
			recipe.AddIngredient(ItemID.Daybloom, 1);
			recipe.AddTile(13);
			recipe.SetResult(this, 1);
			recipe.AddRecipe();
		}
	}
}