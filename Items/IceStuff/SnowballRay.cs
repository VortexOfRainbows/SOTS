using System;
using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.IceStuff
{
	public class SnowballRay : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Ice Ray");
			Tooltip.SetDefault("A ray of ice");
		}
		public override void SetDefaults()
		{
            item.damage = 24;  //gun damage
            item.ranged = true;   //its a gun so set this to true
            item.width = 60;     //gun image width
            item.height = 40;   //gun image  height
            item.useTime = 8;  //how fast 
            item.useAnimation = 8;
            item.useStyle = 5;    
            item.noMelee = true; //so the item's animation doesn't do damage
            item.knockBack = 1;
            item.value = 100000;
            item.rare = 4;
            item.UseSound = SoundID.Item11;
            item.autoReuse = true;
            item.shoot = 166; 
            item.shootSpeed = 32;
		

		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "AbsoluteBar", 16);
			recipe.AddIngredient(ItemID.SnowballCannon, 1);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}
