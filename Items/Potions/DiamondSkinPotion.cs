using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;



namespace SOTS.Items.Potions
{
	public class DiamondSkinPotion : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Diamond Skin Potion");
			
			Tooltip.SetDefault("Grants 15 defense, reduces incoming damage by 15%, and increases speed by 15%");
		}
		public override void SetDefaults()
		{
			item.width = 20;
			item.height = 32;
			item.value = 25500;
			item.rare = 4;
			item.maxStack = 30;
            item.buffType = mod.BuffType("DiamondSkin");    //this is where you put your Buff name
            item.buffTime = 72030;  
            item.UseSound = SoundID.Item3;                //this is the sound that plays when you use the item
            item.useStyle = 2;                 //this is how the item is holded when used
            item.useTurn = true;
            item.useAnimation = 16;
            item.useTime = 16;
            item.consumable = true;       
			
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.BottledWater, 2);
			recipe.AddIngredient(null, "JewelFish", 1);
			recipe.AddIngredient(ItemID.IronskinPotion, 1);
			recipe.AddIngredient(ItemID.EndurancePotion, 1);
			recipe.AddIngredient(ItemID.Fireblossom, 1);
			recipe.AddTile(13);
			recipe.SetResult(this, 4);
			recipe.AddRecipe();
}
	}
}