using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.Chess
{
	public class FrightBlade : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Fusion Blade");
			Tooltip.SetDefault("Fires out ranged and magic projectiles");
		}
		public override void SetDefaults()
		{

			item.damage = 35;
			item.melee = true;
			item.width = 36;
			item.height = 36;
			item.useTime = 40;
			item.useAnimation = 40;
			item.useStyle = 1;
			item.knockBack = 1;
			item.value = 125000;
			item.rare = 5;
			item.UseSound = SoundID.Item1;
			item.autoReuse = true;            
			item.shoot = 295; 
            item.shootSpeed = 12;


		}
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            int numberProjectiles = 2;  //This defines how many projectiles to shot

			
            for (int index = 0; index < numberProjectiles; ++index)
            {
                Vector2 vector2_1 = new Vector2((float)((double)player.position.X + (double)player.width * 0.5 + (double)(Main.rand.Next(201) * -player.direction) + ((double)Main.mouseX + (double)Main.screenPosition.X - (double)player.position.X)), (float)((double)player.position.Y + (double)player.height * 0.5 - 600.0));   //this defines the projectile width, direction and position
                vector2_1.X = (float)(((double)vector2_1.X + (double)player.Center.X) / 2.0) + (float)Main.rand.Next(-200, 201);
                vector2_1.Y -= (float)(100 * index);
                float num12 = (float)Main.mouseX + Main.screenPosition.X - vector2_1.X;
                float num13 = (float)Main.mouseY + Main.screenPosition.Y - vector2_1.Y;
                if ((double)num13 < 0.0) num13 *= -1f;
                if ((double)num13 < 20.0) num13 = 20f;
                float num14 = (float)Math.Sqrt((double)num12 * (double)num12 + (double)num13 * (double)num13);
                float num15 = item.shootSpeed / num14;
                float num16 = num12 * num15;
                float num17 = num13 * num15;
                float SpeedX = num16 + (float)Main.rand.Next(-40, 41) * 0.02f;  //this defines the projectile X position speed and randomnes
                float SpeedY = num17 + (float)Main.rand.Next(-40, 41) * 0.02f;  //this defines the projectile Y position speed and randomnes
                Projectile.NewProjectile(vector2_1.X, vector2_1.Y, SpeedX, SpeedY, type, damage, knockBack, Main.myPlayer, 0.0f, player.whoAmI);
            }
			
              int numberProjectiles2 = 9;
              for (int i = 0; i < numberProjectiles2; i++)
              {
                  Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(6)); // This defines the projectiles random spread . 30 degree spread.

				  Projectile.NewProjectile(position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, 85, damage, knockBack, player.whoAmI);
              }
            return false;
	}
	}
}