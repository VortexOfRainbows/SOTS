using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;



namespace SOTS.Items.Potions
{
	public class FastBrew : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Fast Brew");
			
			Tooltip.SetDefault("Temporary fast use");
		}
		public override void SetDefaults()
		{
			item.width = 12;
			item.height = 32;
			item.value = 62500;
			item.rare = 8;
			item.maxStack = 30;
            item.buffType = mod.BuffType("Rapidity");    //this is where you put your Buff name
            item.buffTime = 7800;  
            item.UseSound = SoundID.Item3;                //this is the sound that plays when you use the item
            item.useStyle = 2;                 //this is how the item is holded when used
            item.useTurn = true;
            item.useAnimation = 60;
            item.useTime = 60;
            item.consumable = true;       
			
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.BottledWater, 3);
			recipe.AddIngredient(null, "Plasmawhale", 1);
			recipe.AddIngredient(null, "ParticleFish", 1);
			recipe.AddIngredient(ItemID.SpecularFish, 1);
			recipe.AddIngredient(ItemID.Moonglow, 1);
			recipe.AddIngredient(ItemID.Blinkroot, 1);
			recipe.AddTile(13);
			recipe.SetResult(this, 3);
			recipe.AddRecipe();
}
	}
}