using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using Microsoft.Xna.Framework;

namespace SOTS.Items
{
	public class TinyWatermelon : ModItem
	{int Probe = -1;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Tiny Watermelon");
			Tooltip.SetDefault("");
		}
		public override void SetDefaults()
		{
	
      
            item.width = 34;     
            item.height = 34;   
            item.value = 125000;
            item.rare = 5;
			item.expert = true;
			item.accessory = true;
			item.defense = 2;

		}
		
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			Probe++;
			if(Probe == 5)
			{
				Probe = 0;
				
				Projectile.NewProjectile(player.position.X, player.position.Y, 0, 0, mod.ProjectileType("TinyWatermelonProj"), 22, 0, player.whoAmI);
			}
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null,"WaterMelon", 1);
			recipe.AddIngredient(null,"TinyPlanet", 1);
			recipe.AddIngredient(null,"CoreOfExpertise", 3);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}