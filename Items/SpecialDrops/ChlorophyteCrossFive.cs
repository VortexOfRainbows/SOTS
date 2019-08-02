using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using Microsoft.Xna.Framework;

namespace SOTS.Items.SpecialDrops
{
	public class ChlorophyteCrossFive : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Chlorophyte Cross Four");
			Tooltip.SetDefault("Fires out 4 homing, bouncing, arrows");
		}
		public override void SetDefaults()
		{

			item.damage = 60;
			item.ranged = true;
			item.width = 34;
			item.height = 58;
			item.useTime = 30;
			item.useAnimation = 30;
			item.useStyle = 5;
			item.knockBack = 6;
			item.value = 10000;
			item.rare = 7;
			item.UseSound = SoundID.Item5;
			item.autoReuse = true;            
			item.shoot = mod.ProjectileType("HomingArrow"); 
            item.shootSpeed = 15.25f;

		}
		
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
          {
			  
              int numberProjectiles = 4;
              for (int i = 0; i < numberProjectiles; i++)
              {
                  Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(10)); // This defines the projectiles random spread . 30 degree spread.
                  Projectile.NewProjectile(position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, type, damage, knockBack, player.whoAmI);

              }
              return false; 
			  
	}
	}
}