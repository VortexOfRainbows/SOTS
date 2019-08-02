using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using Microsoft.Xna.Framework;

namespace SOTS.Items.SpecialDrops
{
	public class PrimordialMismatch : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Primordial Mismatch");
			Tooltip.SetDefault("A mismatch of destruction");
		}
		public override void SetDefaults()
		{

			item.damage = 22;
			item.melee = true;
			item.width = 68;
			item.height = 68;
			item.useTime = 36;
			item.useAnimation = 36;
			item.useStyle = 1;
			item.knockBack = 6;
			item.value = 50000;
			item.rare = 11;
			item.UseSound = SoundID.Item1;
			item.autoReuse = true;            
			item.shoot = 304; 
            item.shootSpeed = 24;

		}
		
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
          {
			  
              int numberProjectiles = 3;
              for (int i = 0; i < numberProjectiles; i++)
              {
                  Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(18)); // This defines the projectiles random spread . 30 degree spread.
                  Projectile.NewProjectile(position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, type, damage, knockBack, player.whoAmI);
				  Projectile.NewProjectile(position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, 307, damage, knockBack, player.whoAmI);
              }
              return false; 
			  
	}
	}
}