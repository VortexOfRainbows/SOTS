using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;



namespace SOTS.Items.Potions
{
	public class ShrinkerJuice : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Shrinker Juice");
			
			Tooltip.SetDefault("Makes you take no damage from sources that do less than 35 damage plus 10% of your current health");
		}
		public override void SetDefaults()
		{
			item.width = 20;
			item.height = 34;
			item.value = 12500;
			item.rare = 4;
			item.maxStack = 30;
            item.buffType = mod.BuffType("Pluto");    //this is where you put your Buff name
            item.buffTime = 7200;  
            item.UseSound = SoundID.Item3;                //this is the sound that plays when you use the item
            item.useStyle = 2;                 //this is how the item is holded when used
            item.useTurn = true;
            item.useAnimation = 17;
            item.useTime = 17;
            item.consumable = true;       
			
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.BottledWater, 2);
			recipe.AddIngredient(null, "TinyPlanetFish", 1);
			recipe.AddIngredient(null, "EmptyPlanetariumOrb", 4);
			recipe.AddIngredient(ItemID.Blinkroot, 1);
			recipe.AddIngredient(ItemID.Daybloom, 1);
			recipe.AddTile(13);
			recipe.SetResult(this, 2);
			recipe.AddRecipe();
}
	}
}