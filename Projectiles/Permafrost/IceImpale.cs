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
using SOTS.Void;

namespace SOTS.Projectiles.Permafrost
{    
    public class IceImpale : ModProjectile 
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("IceImpale");
		}
        public override void SetDefaults()
        {
			projectile.friendly = true;
			projectile.tileCollide = true;
			projectile.penetrate = -1;
			projectile.width = 58;
			projectile.height = 46;
			projectile.alpha = 255;
			projectile.timeLeft = 640;


		} 
		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
			Player player = Main.player[projectile.owner];
			VoidPlayer voidPlayer = VoidPlayer.ModPlayer(player);
            target.immune[projectile.owner] = 8;
			Projectile.NewProjectile(target.Center.X, target.Center.Y, 0, 0, mod.ProjectileType("HealProj"), 2, 0, projectile.owner, 0.85f, 4);
		}
		public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough)
        {
            width = 30;
            height = 30;
            fallThrough = true;
            return true;
        }
		public override void Kill(int timeLeft)
        {
			if(projectile.owner == Main.myPlayer)
			{
				Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, 0, 0, mod.ProjectileType("IcePulse"), projectile.damage, 0, projectile.owner);
			}
		}
		public override void AI()
		{
			projectile.alpha -= 20;
			projectile.rotation = (float)Math.Atan2((double)projectile.velocity.Y, (double)projectile.velocity.X);
			projectile.spriteDirection = 1;
			if(projectile.rotation > MathHelper.ToRadians(180))
			{
				projectile.rotation -= MathHelper.ToRadians(180);
				projectile.spriteDirection = -1;
			}
			
			if(projectile.timeLeft % 4 == 0)
			{
				for(int i = 0; i < 360; i += 20)
				{
					Vector2 circularLocation = new Vector2(20, 0).RotatedBy(MathHelper.ToRadians(i));
					int num1 = Dust.NewDust(new Vector2(projectile.Center.X + circularLocation.X - 4, projectile.Center.Y + circularLocation.Y - 4), 4, 4, 67);
					Main.dust[num1].noGravity = true;
					Main.dust[num1].velocity = projectile.velocity.RotatedBy(MathHelper.ToRadians(180));
				}
			}
			/*
			Vector2 trailLoc = new Vector2(18, 0).RotatedBy(Math.Atan2(projectile.velocity.Y, projectile.velocity.X));
			int num1 = Dust.NewDust(new Vector2(projectile.Center.X - trailLoc.X - 2, projectile.Center.Y - trailLoc.Y - 2), 2, 2, 235);
			
			Main.dust[num1].noGravity = true;
			Main.dust[num1].velocity *= 0.1f;
			*/
		}
	}
}
		
			