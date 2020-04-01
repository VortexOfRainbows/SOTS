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

namespace SOTS.Projectiles.Permafrost 
{    
    public class HypericeRocket : ModProjectile 
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Hyperice Rocket");
			
		}
		
        public override void SetDefaults()
        {
			projectile.CloneDefaults(14);
            aiType = 14; 
			projectile.penetrate = 1;
			projectile.width = 30;
			projectile.height = 18;
			projectile.alpha = 0;
			projectile.timeLeft = 640;


		}
		public override void Kill(int timeLeft)
		{
			for(int i = 0; i < 2; i++)
			{
			int goreIndex = Gore.NewGore(new Vector2(projectile.position.X, projectile.position.Y), default(Vector2), Main.rand.Next(61,64), 1f);	
			Main.gore[goreIndex].scale = 0.55f;
			}
			
            Main.PlaySound(SoundID.Item14, (int)(projectile.Center.X), (int)(projectile.Center.Y));
			
			if(Main.myPlayer == projectile.owner)
			{
				for (int i = 0; i < 4; i++)
				{
					Vector2 perturbedSpeed = new Vector2(projectile.velocity.X, projectile.velocity.Y).RotatedBy(MathHelper.ToRadians(15 - (10 * i)));
					Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, perturbedSpeed.X, perturbedSpeed.Y, mod.ProjectileType("IceCluster"), (int)(projectile.damage * 0.79f), 0, projectile.owner);
				}
			}
		}
		public override void AI()
		{
			/*
			Vector2 trailLoc = new Vector2(18, 0).RotatedBy(Math.Atan2(projectile.velocity.Y, projectile.velocity.X));
			int num1 = Dust.NewDust(new Vector2(projectile.Center.X - trailLoc.X - 2, projectile.Center.Y - trailLoc.Y - 2), 2, 2, 235);
			
			Main.dust[num1].noGravity = true;
			Main.dust[num1].velocity *= 0.1f;
			*/
			projectile.timeLeft -= Main.rand.Next(40);
			projectile.timeLeft -= Math.Abs((int)(projectile.velocity.X * 0.8f));
			projectile.timeLeft -= Math.Abs((int)(projectile.velocity.Y * 0.8f));
			projectile.rotation = (float)Math.Atan2((double)projectile.velocity.Y, (double)projectile.velocity.X);
		}
	}
}
		
			