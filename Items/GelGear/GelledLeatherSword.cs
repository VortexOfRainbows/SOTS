using System;
using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.GelGear
{
	public class GelledLeatherSword : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Leather Sword");
			Tooltip.SetDefault("");
		}
		public override void SetDefaults()
		{
            item.damage = 9;  //gun damage
            item.melee = true;   //its a gun so set this to true
            item.width = 60;     //gun image width
            item.height = 60;   //gun image  height
            item.useTime = 32;  //how fast 
            item.useAnimation = 14;
            item.useStyle = 1;    

            item.knockBack = 1;
            item.value = 1000;
            item.rare = 4;
            item.UseSound = SoundID.Item1;
            item.autoReuse = true;
			item.shoot = 22; 
            item.shootSpeed = 9;

			
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "GelBar", 8);
			recipe.AddIngredient(ItemID.Leather, 12);
			recipe.AddIngredient(null, "SlimeyFeather", 8);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
		
	}
}
