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

namespace SOTS.Projectiles.Inferno
{    
    public class SharangaBolt : ModProjectile 
    {
		int helixRot = 0;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Sharanga Bolt");
			ProjectileID.Sets.TrailCacheLength[projectile.type] = 12;  
			ProjectileID.Sets.TrailingMode[projectile.type] = 0;    
		}
		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Vector2 drawOrigin = new Vector2(Main.projectileTexture[projectile.type].Width * 0.5f, projectile.height * 0.5f);
			for (int k = 0; k < projectile.oldPos.Length; k++) {
				Vector2 drawPos = projectile.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(0f, projectile.gfxOffY);
				Color color = projectile.GetAlpha(lightColor) * ((float)(projectile.oldPos.Length - k) / (float)projectile.oldPos.Length);
				spriteBatch.Draw(Main.projectileTexture[projectile.type], drawPos, null, color * 0.8f, projectile.rotation, drawOrigin, projectile.scale, SpriteEffects.None, 0f);
			}
			return true;
		}
        public override void SetDefaults()
        {
			projectile.CloneDefaults(1);
            aiType = 1;
			projectile.penetrate = 3;
			projectile.alpha = 0;
			projectile.width = 14;
			projectile.height = 38;


		}
		public override void Kill(int timeLeft)
		{
			Player player = Main.player[projectile.owner];
			if(projectile.owner == Main.myPlayer)
			{
				Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, 0, 0, mod.ProjectileType("SharangaBlast"), projectile.damage, 0, Main.myPlayer);
			}
		}
		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			Player player = Main.player[projectile.owner];
            target.immune[projectile.owner] = 10;
			target.AddBuff(BuffID.OnFire, 60, false);
			if(projectile.owner == Main.myPlayer && projectile.penetrate != 1)
			{
				Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, 0, 0, mod.ProjectileType("SharangaBlast"), projectile.damage, 0, Main.myPlayer);
			}
		}
		public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough) 
		{
			width = 8;
			height = 16;
			fallThrough = true;
			return true;
		}
		public override void AI()
		{
			float rad = (float)Math.Atan2((double)projectile.velocity.Y, (double)projectile.velocity.X);
			//Vector2 curve = new Vector2(8f,0).RotatedBy(MathHelper.ToRadians(helixRot * 5f));
			Vector2 curve2 = new Vector2(8f,0).RotatedBy(MathHelper.ToRadians(helixRot * 15f));
			helixRot ++;
			
			Vector2 helixPos3 = new Vector2(18f + curve2.X, 0).RotatedBy(rad + MathHelper.ToRadians(90));
			Vector2 helixPos4 = new Vector2(18f + curve2.X, 0).RotatedBy(rad - MathHelper.ToRadians(90));
			/*
			Vector2 helixPos1 = projectile.Center + new Vector2(curve.X, 0).RotatedBy(rad + MathHelper.ToRadians(90));
			//Vector2 helixPos2 = projectile.Center + new Vector2(curve.X, 0).RotatedBy(rad - MathHelper.ToRadians(90));
			
			int num1 = Dust.NewDust(new Vector2(helixPos1.X - 4, helixPos1.Y - 4), 4, 4, 269);
			Main.dust[num1].noGravity = true;
			Main.dust[num1].velocity *= 0.15f;
			Main.dust[num1].scale = 1.5f;
			
			//num1 = Dust.NewDust(new Vector2(helixPos2.X - 4, helixPos2.Y - 4), 4, 4, 269);
			//Main.dust[num1].noGravity = true;
			//Main.dust[num1].velocity *= 0.15f;
			//Main.dust[num1].scale = 1.5f
			*/
			int num2 = Dust.NewDust(new Vector2(projectile.Center.X + helixPos3.X - 4, projectile.Center.Y + helixPos3.Y - 4), 4, 4, 6);
			Main.dust[num2].noGravity = true;
			Main.dust[num2].velocity = helixPos3 * 0.125f;
			Main.dust[num2].scale = 2;
			
			num2 = Dust.NewDust(new Vector2(projectile.Center.X + helixPos4.X - 4, projectile.Center.Y + helixPos4.Y - 4), 4, 4, 6);
			Main.dust[num2].noGravity = true;
			Main.dust[num2].velocity = helixPos4 * 0.125f;
			Main.dust[num2].scale = 2;
		}
	}
}
		
			