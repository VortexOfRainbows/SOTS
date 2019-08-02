using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;



namespace SOTS.Items.Potions
{
	public class SubatomicJuice : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Subatomic Juice");
			
			Tooltip.SetDefault("Fish out anything");
		}
		public override void SetDefaults()
		{
			item.width = 16;
			item.height = 32;
			item.value = 7500;
			item.rare = 6;
			item.maxStack = 30;
            item.buffType = mod.BuffType("SubatomicFishing");    //this is where you put your Buff name
            item.buffTime = 36000;  
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
			recipe.AddIngredient(ItemID.BottledWater, 1);
			recipe.AddIngredient(null, "ParticleFish", 1);
			recipe.AddIngredient(null, "RainbowCrate", 1);
			recipe.AddIngredient(ItemID.Blinkroot, 1);
			recipe.AddIngredient(ItemID.Deathweed, 1);
			recipe.AddIngredient(ItemID.Moonglow, 1);
			recipe.AddIngredient(ItemID.Daybloom, 1);
			recipe.AddTile(13);
			recipe.SetResult(this, 1);
			recipe.AddRecipe();
			
			ModRecipe recipe2 = new ModRecipe(mod);
			recipe2.AddIngredient(ItemID.BottledWater, 1);
			recipe2.AddIngredient(null, "ParticleFish", 1);
			recipe2.AddIngredient(ItemID.GravityGlobe, 1);
			recipe2.AddIngredient(ItemID.Blinkroot, 1);
			recipe2.AddIngredient(ItemID.Deathweed, 1);
			recipe2.AddIngredient(ItemID.Moonglow, 1);
			recipe2.AddIngredient(ItemID.Daybloom, 1);
			recipe2.AddTile(13);
			recipe2.SetResult(this, 1);
			recipe2.AddRecipe();
}
	}
}