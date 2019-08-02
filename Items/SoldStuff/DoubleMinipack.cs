using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace SOTS.Items.SoldStuff
{
	[AutoloadEquip(EquipType.Wings)]
	public class DoubleMinipack : ModItem
	{ int direction = 0;
		public override void SetStaticDefaults()
		{	
		DisplayName.SetDefault("Double Minipack");
			Tooltip.SetDefault("Boost it up, again");
		}

		public override void SetDefaults()
		{
			item.width = 46;
			item.height = 46;
			item.value = 1250000;
			item.rare = 6;
			item.accessory = true;
		}
		//these wings use the same values as the solar wings
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			
			player.wingTimeMax = 155;
			if(player.gravDir == 1f)
			{
				
				
				 if (player.velocity.Y > 1f && player.velocity.Y < 6f) 
            {
				
				Projectile.NewProjectile((player.Center.X) + 9, (player.Center.Y) + 24, Main.rand.Next(-1, 2), 21, mod.ProjectileType("FireProj"), 17, 0, player.whoAmI);
			
			
				Projectile.NewProjectile((player.Center.X) - 13, (player.Center.Y) + 24, Main.rand.Next(-1, 2), 21, mod.ProjectileType("FireProj"), 17, 0, player.whoAmI);
				
				}
				
				
			 if (player.velocity.Y < -6f )
            {
				
				Projectile.NewProjectile((player.Center.X) + 9, player.Center.Y, Main.rand.Next(-2, 3), 7, 14, 12, 0, player.whoAmI);
				Projectile.NewProjectile((player.Center.X) + 9, (player.Center.Y) + 17, Main.rand.Next(-1, 2), 7, mod.ProjectileType("FireProj"), 17, 0, player.whoAmI);
			
						
					Projectile.NewProjectile((player.Center.X) - 13, player.Center.Y, Main.rand.Next(-2, 3), 7, 14, 12, 0, player.whoAmI);
					Projectile.NewProjectile((player.Center.X) - 13, (player.Center.Y) + 17, Main.rand.Next(-1, 2), 7, mod.ProjectileType("FireProj"), 17, 0, player.whoAmI);

		}
		}
		}

		public override void VerticalWingSpeeds(Player player, ref float ascentWhenFalling, ref float ascentWhenRising,
			ref float maxCanAscendMultiplier, ref float maxAscentMultiplier, ref float constantAscend)
		{
			ascentWhenFalling = 0.85f;
			ascentWhenRising = 0.5f;
			maxCanAscendMultiplier = 0.135f;
			maxAscentMultiplier = 3f;
			constantAscend = 0.135f;
		}

		public override void HorizontalWingSpeeds(Player player, ref float speed, ref float acceleration)
		{
			speed = 9f;
			acceleration *= 1.9f;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "Minipack", 2);

			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}