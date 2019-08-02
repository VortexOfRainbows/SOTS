using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using Microsoft.Xna.Framework;

namespace SOTS.Items.Chess
{
	public class ElementalKing : ModItem
	{int Probe = -1;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Elemental King");
			Tooltip.SetDefault("Harness the power of the king!\nAll stats up!");
		}
		public override void SetDefaults()
		{
	
      
            item.width = 18;     
            item.height = 30;   
            item.value = 125000;
            item.rare = 10;
			item.accessory = true;
			item.expert = true;
			item.defense = 5;

		}
		
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			Probe++;
					if(Probe == 30)
				Projectile.NewProjectile(player.position.X, player.position.Y, 0, 0, mod.ProjectileType("FrightPawnOrb"), 50, 0, player.whoAmI);
			
					if(Probe == 60)
				Projectile.NewProjectile(player.position.X, player.position.Y, 0, 0, mod.ProjectileType("FrightBishopOrb"), 50, 0, player.whoAmI);
				
					if(Probe == 90)
				Projectile.NewProjectile(player.position.X, player.position.Y, 0, 0, mod.ProjectileType("FrightKnightOrb"), 50, 0, player.whoAmI);
				
					if(Probe == 120)
				Projectile.NewProjectile(player.position.X, player.position.Y, 0, 0, mod.ProjectileType("FrightRookOrb"), 50, 0, player.whoAmI);
			
					if(Probe == 150)
				Projectile.NewProjectile(player.position.X, player.position.Y, 0, 0, mod.ProjectileType("SightPawnOrb"), 50, 0, player.whoAmI);
				
					if(Probe == 180)
				Projectile.NewProjectile(player.position.X, player.position.Y, 0, 0, mod.ProjectileType("SightBishopOrb"), 50, 0, player.whoAmI);
				
					if(Probe == 210)
				Projectile.NewProjectile(player.position.X, player.position.Y, 0, 0, mod.ProjectileType("SightKnightOrb"), 50, 0, player.whoAmI);
				
					if(Probe == 240)
				Projectile.NewProjectile(player.position.X, player.position.Y, 0, 0, mod.ProjectileType("SightRookOrb"), 50, 0, player.whoAmI);
			
					if(Probe == 270)
				Projectile.NewProjectile(player.position.X, player.position.Y, 0, 0, mod.ProjectileType("MightPawnOrb"), 50, 0, player.whoAmI);
				
					if(Probe == 300)
				Projectile.NewProjectile(player.position.X, player.position.Y, 0, 0, mod.ProjectileType("MightBishopOrb"), 50, 0, player.whoAmI);
				
					if(Probe == 330)
				Projectile.NewProjectile(player.position.X, player.position.Y, 0, 0, mod.ProjectileType("MightKnightOrb"), 50, 0, player.whoAmI);
				
					if(Probe == 360)
					{
			
				Projectile.NewProjectile(player.position.X, player.position.Y, 0, 0, mod.ProjectileType("MightRookOrb"), 50, 0, player.whoAmI);
				Probe = 0;
					}
			
			
			  player.statLifeMax2 += 30;
			  player.meleeDamage += 0.05f;
			  player.magicDamage += 0.05f;
			  player.rangedDamage += 0.05f;
			  player.minionDamage += 0.05f;
			  player.thrownDamage += 0.05f;
			  player.thrownVelocity += 0.2f;
			  player.meleeSpeed += 0.2f;
			  player.magicCrit += 5;
			  player.meleeCrit += 5;
			  player.thrownCrit += 5;
			  player.rangedCrit += 5;
			  player.moveSpeed += 0.2f;
			  
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "SoulSingularity", 1280);
			recipe.AddIngredient(null, "KingTrinity", 5);
			recipe.AddIngredient(null, "ElementalPawn", 1);
			recipe.AddIngredient(null, "ElementalBishop", 1);
			recipe.AddIngredient(null, "ElementalKnight", 1);
			recipe.AddIngredient(null, "ElementalRook", 1);
			recipe.AddIngredient(null, "QueenHolyGuard", 1);
			recipe.SetResult(this, 1);
			recipe.AddRecipe();
		}
	}
}