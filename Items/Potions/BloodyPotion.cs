using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;



namespace SOTS.Items.Potions
{
	public class BloodyPotion : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Blood Scale Potion");
			
			Tooltip.SetDefault("Temporary lifesteal");
		}
		public override void SetDefaults()
		{
			item.width = 20;
			item.height = 36;
			item.value = 17500;
			item.rare = 6;
			item.maxStack = 30;
            item.buffType = mod.BuffType("BloodySapping");    //this is where you put your Buff name
            item.buffTime = 21600;  
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
			recipe.AddIngredient(null, "FacelessDemonFish", 1);
			recipe.AddIngredient(null, "ObsidianScale", 1);
			recipe.AddIngredient(ItemID.Moonglow, 1);
			recipe.AddTile(13);
			recipe.SetResult(this);
			recipe.AddRecipe();
}
	}
}