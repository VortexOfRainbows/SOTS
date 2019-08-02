using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;



namespace SOTS.Items.Potions
{
	public class CrimCruptPotion : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Crim Crupt Perfume");
			
			Tooltip.SetDefault("Smell like the demons!\nHeal from most crimson and corruption enemies\nYou can still die if your health is less than how much they heal");
		}
		public override void SetDefaults()
		{
			item.width = 28;
			item.height = 30;
			item.value = 10500;
			item.rare = 4;
			item.maxStack = 30;
            item.buffType = mod.BuffType("CrimCruptSmell");    //this is where you put your Buff name
            item.buffTime = 10800;  
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
			recipe.AddIngredient(null, "RottingBloodKoi", 1);
			recipe.AddIngredient(ItemID.FallenStar, 1);
			recipe.AddIngredient(ItemID.Deathweed, 1);
			recipe.AddTile(13);
			recipe.SetResult(this);
			recipe.AddRecipe();
}
	}
}