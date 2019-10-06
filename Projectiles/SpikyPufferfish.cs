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

namespace SOTS.Projectiles 
{    
    public class SpikyPufferfish : ModProjectile 
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Spiky Pufferfish");
			
		}
		
        public override void SetDefaults()
        {
			projectile.CloneDefaults(3);
            aiType = 3; 
			projectile.penetrate = 1;
			projectile.width = 20;
			projectile.height = 20;
			projectile.alpha = 0;
			projectile.timeLeft = 640;


		}
		public override void Kill(int timeLeft)
		{
			for(int i = 0; i < 5; i++)
			{
			int goreIndex = Gore.NewGore(new Vector2(projectile.position.X, projectile.position.Y), default(Vector2), Main.rand.Next(61,64), 1f);	
			Main.gore[goreIndex].scale = 0.45f;
			}
            Main.PlaySound(SoundID.Item14, (int)(projectile.Center.X), (int)(projectile.Center.Y));
			
			Vector2 projVelocity1 = new Vector2(projectile.velocity.X, projectile.velocity.Y).RotatedBy(MathHelper.ToRadians(Main.rand.Next(20)));
			Vector2 projVelocity2 = new Vector2(projectile.velocity.X, projectile.velocity.Y).RotatedBy(MathHelper.ToRadians(-Main.rand.Next(20)));
		
			if(Main.myPlayer == projectile.owner)
			{
				if(Main.rand.Next(2) == 0)
				{
				Vector2 projVelocity3 = new Vector2(projectile.velocity.X, projectile.velocity.Y).RotatedBy(MathHelper.ToRadians(Main.rand.Next(35)));
				Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, projVelocity3.X * (float)(Main.rand.Next(50,61) * 0.02f), projVelocity3.Y * (float)(Main.rand.Next(50,61) * 0.02f), 14, (int)(projectile.damage * 0.6f), projectile.knockBack, Main.myPlayer);
				}
			
				if(Main.rand.Next(3) == 0)
				{
				Vector2 projVelocity4 = new Vector2(projectile.velocity.X, projectile.velocity.Y).RotatedBy(MathHelper.ToRadians(-Main.rand.Next(35)));
				Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, projVelocity4.X * (float)(Main.rand.Next(50,61) * 0.02f), projVelocity4.Y * (float)(Main.rand.Next(50,61) * 0.02f), 14, (int)(projectile.damage * 0.6f), projectile.knockBack, Main.myPlayer);
				}
			
				Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, projVelocity1.X * (float)(Main.rand.Next(50,61) * 0.02f), projVelocity1.Y * (float)(Main.rand.Next(50,61) * 0.02f), 14, (int)(projectile.damage * 0.6f), projectile.knockBack, Main.myPlayer);
				
				Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, projVelocity2.X * (float)(Main.rand.Next(50,61) * 0.02f), projVelocity2.Y * (float)(Main.rand.Next(50,61) * 0.02f), 14, (int)(projectile.damage * 0.6f), projectile.knockBack, Main.myPlayer);
			}
					
		}
		public override void AI()
		{
			projectile.timeLeft -= Main.rand.Next(40);
			projectile.timeLeft -= Math.Abs((int)(projectile.velocity.X * 0.8f));
			projectile.timeLeft -= Math.Abs((int)(projectile.velocity.Y * 0.8f));
			
		}
	}
}
		
			