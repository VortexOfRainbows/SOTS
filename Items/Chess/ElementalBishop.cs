using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using Microsoft.Xna.Framework;

namespace SOTS.Items.Chess
{
	public class ElementalBishop : ModItem
	{int Probe = -1;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Elemental Bishop");
			Tooltip.SetDefault("Harness the power of the bishops!");
		}
		public override void SetDefaults()
		{
	
      
            item.width = 18;     
            item.height = 30;   
            item.value = 125000;
            item.rare = 10;
			item.accessory = true;

		}
		
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			Probe++;
			if(Probe == 120)
			{
				
				Projectile.NewProjectile(player.position.X, player.position.Y, 0, 0, mod.ProjectileType("FrightBishopOrb"), 50, 0, player.whoAmI);
			}
			if(Probe == 240)
			{
				
				Projectile.NewProjectile(player.position.X, player.position.Y, 0, 0, mod.ProjectileType("MightBishopOrb"), 50, 0, player.whoAmI);
			}
			if(Probe == 360)
			{
				Probe = 0;
				Projectile.NewProjectile(player.position.X, player.position.Y, 0, 0, mod.ProjectileType("SightBishopOrb"), 50, 0, player.whoAmI);
			}
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "SoulSingularity", 320);
			recipe.AddIngredient(null, "KingTrinity", 2);
			recipe.AddIngredient(null, "FrightBattery", 1);
			recipe.AddIngredient(null, "MightyTrail", 1);
			recipe.AddIngredient(null, "SightHeal", 1);
			recipe.SetResult(this, 1);
			recipe.AddRecipe();
		}
	}
}