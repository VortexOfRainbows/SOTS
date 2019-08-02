using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;



namespace SOTS.Items.Potions
{
	public class HoneyElixir : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Honey Elixir");
			
			Tooltip.SetDefault("Heal an additional 30 health every 15 seconds");
		}
		public override void SetDefaults()
		{
			item.width = 16;
			item.height = 34;
			item.value = 12500;
			item.rare = 3;
			item.maxStack = 30;
            item.buffType = mod.BuffType("HoneyFinned");    //this is where you put your Buff name
            item.buffTime = 15000;  
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
			recipe.AddIngredient(ItemID.Honeyfin, 2);
			recipe.AddIngredient(ItemID.Moonglow, 1);
			recipe.AddIngredient(ItemID.Daybloom, 1);
			recipe.AddTile(13);
			recipe.SetResult(this, 2);
			recipe.AddRecipe();
}
	}
}