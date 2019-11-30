using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;

namespace SOTS.Items.Potions
{
	public class RoughskinPotion : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Roughskin Potion");
			
			Tooltip.SetDefault("Increases defense by 4 and damage by 4%");
		}
		public override void SetDefaults()
		{
			item.width = 20;
			item.height = 30;
            item.value = Item.sellPrice(0, 0, 2, 0);
			item.rare = 2;
			item.maxStack = 30;
            item.buffType = mod.BuffType("Roughskin");   
            item.buffTime = 19000;  
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
			recipe.AddIngredient(null, "Snakeskin", 3);
			recipe.AddIngredient(ItemID.Blinkroot, 1);
			recipe.AddTile(13);
			recipe.SetResult(this, 1);
			recipe.AddRecipe();
		}
	}
}