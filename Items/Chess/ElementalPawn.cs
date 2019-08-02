using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using Microsoft.Xna.Framework;

namespace SOTS.Items.Chess
{
	public class ElementalPawn : ModItem
	{int Probe = -1;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Elemental Pawn");
			Tooltip.SetDefault("Harness the power of the pawns!");
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
				
				Projectile.NewProjectile(player.position.X, player.position.Y, 0, 0, mod.ProjectileType("FrightPawnOrb"), 50, 0, player.whoAmI);
			}
			if(Probe == 240)
			{
				
				Projectile.NewProjectile(player.position.X, player.position.Y, 0, 0, mod.ProjectileType("MightPawnOrb"), 50, 0, player.whoAmI);
			}
			if(Probe == 360)
			{
				Probe = 0;
				Projectile.NewProjectile(player.position.X, player.position.Y, 0, 0, mod.ProjectileType("SightPawnOrb"), 50, 0, player.whoAmI);
			}
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "SoulSingularity", 160);
			recipe.AddIngredient(null, "KingTrinity", 1);
			recipe.AddIngredient(null, "FrightBlade", 1);
			recipe.AddIngredient(null, "MightyAquarius", 1);
			recipe.AddIngredient(null, "SightChalice", 1);
			recipe.SetResult(this, 1);
			recipe.AddRecipe();
		}
	}
}