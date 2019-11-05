using System;
using System.IO;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace SOTS.Projectiles.Celestial
{    
    public class CelestialArrow : ModProjectile 
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Celestial Rocket");
			
		}
		
        public override void SetDefaults()
        {
			projectile.CloneDefaults(14);
            aiType = 14; 
			projectile.penetrate = 1;
			projectile.width = 22;
			projectile.height = 24;
			projectile.alpha = 100;
			projectile.timeLeft = 20;
			projectile.friendly = true;


		}
		public override void Kill(int timeLeft)
		{
			if(projectile.friendly)
			{
				for(int i = 0; i < 360; i += 24)
				{
				Vector2 circularLocation = new Vector2(-15, 0).RotatedBy(MathHelper.ToRadians(i));
				
				int num1 = Dust.NewDust(new Vector2(projectile.Center.X + circularLocation.X - 4, projectile.Center.Y + circularLocation.Y - 4), 4, 4, 56);
				Main.dust[num1].noGravity = true;
				Main.dust[num1].velocity *= 0.1f;
				
				circularLocation = new Vector2(-21, 0).RotatedBy(MathHelper.ToRadians(i));
				
				num1 = Dust.NewDust(new Vector2(projectile.Center.X + circularLocation.X - 4, projectile.Center.Y + circularLocation.Y - 4), 4, 4, 173);
				Main.dust[num1].noGravity = true;
				Main.dust[num1].velocity *= 0.1f;
				
				}
				
				if(Main.myPlayer == projectile.owner)
				{
					for (int i = 0; i < 2; i++)
					{
					  Vector2 perturbedSpeed = new Vector2(projectile.velocity.X, projectile.velocity.Y).RotatedBy(MathHelper.ToRadians(2.5f - (5 * i)));
					  
					  if(i == 1) {
					  Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, perturbedSpeed.X, perturbedSpeed.Y, mod.ProjectileType("PurpleArrow"), projectile.damage, 0, projectile.owner);
					  } else {
					  Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, perturbedSpeed.X, perturbedSpeed.Y, mod.ProjectileType("BlueArrow"), projectile.damage, 0, projectile.owner);
					  }
					}
				}
			}
		}
		public override void AI()
		{
			projectile.alpha += 4;
			/*
			Vector2 trailLoc = new Vector2(18, 0).RotatedBy(Math.Atan2(projectile.velocity.Y, projectile.velocity.X));
			int num1 = Dust.NewDust(new Vector2(projectile.Center.X - trailLoc.X - 2, projectile.Center.Y - trailLoc.Y - 2), 2, 2, 235);
			
			Main.dust[num1].noGravity = true;
			Main.dust[num1].velocity *= 0.1f;
			*/
			projectile.rotation = (float)Math.Atan2((double)projectile.velocity.Y, (double)projectile.velocity.X) + (MathHelper.ToRadians(90f));
		}
	}
}
		
			