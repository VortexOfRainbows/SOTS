using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using Microsoft.Xna.Framework;

namespace SOTS.Items.PLunar
{
	public class MoonLordGun : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Moon Lord Finger Gun");
			Tooltip.SetDefault("The most powerful item in the universe IS your fingertips\nFires compressed Moonlords");
		}
		public override void SetDefaults()
		{
            item.damage = 6666666;  //gun damage
            item.width = 60;     //gun image width
            item.height = 54;   //gun image  height
            item.useTime = 15;  //how fast 
            item.useAnimation = 15;
            item.useStyle = 5;    
            item.noMelee = false; //so the item's animation doesn't do damage
            item.knockBack = 69;
            item.value = 0;
            item.rare = -12;
            item.UseSound = SoundID.Item12;
            item.autoReuse = false;
            item.shoot = mod.ProjectileType("CompressedMoonLord"); 
            item.shootSpeed = 35;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "NightmareFuel", 50);
			recipe.AddIngredient(null, "NightmareManipulator", 1);
			recipe.AddTile(TileID.LunarCraftingStation);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}
