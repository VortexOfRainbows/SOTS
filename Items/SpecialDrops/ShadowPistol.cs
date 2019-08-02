using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using Microsoft.Xna.Framework;

namespace SOTS.Items.SpecialDrops
{
	public class ShadowPistol : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Shadow Pistol");
			Tooltip.SetDefault("Fits on your hand");
		}
		public override void SetDefaults()
		{
			item.ranged = true;
			item.damage = 32;
			item.width = 30;
			item.height = 18;
			item.useTime = 34;
			item.useAnimation = 34;
			item.useStyle = 5;
			item.knockBack = 2;
			item.value = 30000;
			item.rare = 5;
			item.UseSound = SoundID.Item11;
			item.autoReuse = true;            
			item.shoot = mod.ProjectileType("RainbeamStaffProj");  
            item.shootSpeed = 12;
		}
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
          {
			  
              int numberProjectiles = 2;
              for (int i = 0; i < numberProjectiles; i++)
              {
                  Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(12)); // This defines the projectiles random spread . 30 degree spread.
                  Projectile.NewProjectile(position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, type, damage, knockBack, player.whoAmI);

              }
              return false; 
			  
	}
	}
}