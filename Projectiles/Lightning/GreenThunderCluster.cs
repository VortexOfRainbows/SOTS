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

namespace SOTS.Projectiles.Lightning
{    
    public class GreenThunderCluster : ModProjectile 
    {	float distance = 30f;  
		int rotation = 0;
		
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Thunder Cluster");
			
		}
		
        public override void SetDefaults()
        {
			projectile.CloneDefaults(263);
            aiType = 263; 
			projectile.height = 43;
			projectile.width = 43;
			projectile.penetrate = 24;
			projectile.friendly = true;
			projectile.timeLeft = 60;
			projectile.tileCollide = true;
			projectile.magic = true;
			projectile.hostile = false;
		}
		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			if (projectile.velocity.X != oldVelocity.X)
			{
				projectile.velocity.X = -oldVelocity.X;
			}
			if (projectile.velocity.Y != oldVelocity.Y)
			{
				projectile.velocity.Y = -oldVelocity.Y;
			}
			return false;
		}
		public override void AI()
		{
			Vector2 circularLocation = new Vector2(projectile.velocity.X -distance, projectile.velocity.Y).RotatedBy(MathHelper.ToRadians(rotation));
			rotation += 15;
			distance -= 0.5f;
			projectile.scale *= 0.98f;
			projectile.alpha++;
			
			Player player  = Main.player[projectile.owner];
			
			int num1 = Dust.NewDust(new Vector2(projectile.Center.X + circularLocation.X - 4, projectile.Center.Y + circularLocation.Y - 4), 4, 4, 107);
			Main.dust[num1].noGravity = true;
			Main.dust[num1].velocity *= 0.1f;
		}
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.immune[projectile.owner] = 15;
        }
		public override void Kill(int timeLeft)
		{
			Player player = Main.player[projectile.owner];
			SOTSPlayer modPlayer = (SOTSPlayer)player.GetModPlayer(mod, "SOTSPlayer");
		
			Vector2 cursorArea = Main.MouseWorld;
		
			float shootToX = cursorArea.X - projectile.Center.X;
			float shootToY = cursorArea.Y - projectile.Center.Y;
			float distance = (float)System.Math.Sqrt((double)(shootToX * shootToX + shootToY * shootToY));

			distance = 6.25f / distance;
	   
			shootToX *= distance * 5;
			shootToY *= distance * 5;
	   
			Main.PlaySound(SoundID.Item94, (int)(projectile.Center.X), (int)(projectile.Center.Y));
				
			if(projectile.owner == Main.myPlayer)
				Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, shootToX, shootToY, mod.ProjectileType("GreenLightning"), projectile.damage, projectile.knockBack, Main.myPlayer, 0, 5f);
		}
	}
}
		