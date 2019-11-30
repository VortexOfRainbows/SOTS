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

namespace SOTS.Projectiles.Ores
{    
    public class GoldChakram : ModProjectile 
    {	
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Gold Chakram");
			ProjectileID.Sets.TrailCacheLength[projectile.type] = 12;  
			ProjectileID.Sets.TrailingMode[projectile.type] = 2;    
		}
        public override void SetDefaults()
        {
			projectile.height = 32;
			projectile.width = 32;
			projectile.penetrate = -1;
			projectile.ranged = true;
			projectile.tileCollide = true;
			projectile.timeLeft = 715;
			projectile.friendly = true;
			projectile.hostile = false;
			projectile.extraUpdates = 2;
		}
		public override void AI()
		{
			Player player = Main.player[projectile.owner];
			projectile.rotation += 0.35f;
			if(projectile.timeLeft < 700)
			{
				if(projectile.timeLeft > 610)
				{
					projectile.velocity *= 0.91f;
				}
				else
				{
					projectile.velocity = new Vector2(-22.5f, 0).RotatedBy(Math.Atan2(projectile.Center.Y - player.Center.Y, projectile.Center.X - player.Center.X));
					Vector2 toPlayer = player.Center - projectile.Center;
					float distance = toPlayer.Length();
					if(distance < 20)
					{
						projectile.Kill();
					}
				}
			}
		}
		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
            target.immune[projectile.owner] = 8;
			if(projectile.timeLeft < 690)
			{
				if(projectile.timeLeft > 620)
				{
					target.immune[projectile.owner] = 2;
				}
			}
		}
		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Vector2 drawOrigin = new Vector2(Main.projectileTexture[projectile.type].Width * 0.5f, projectile.height * 0.5f);
			for (int k = 0; k < projectile.oldPos.Length; k++) {
				Vector2 drawPos = projectile.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(0f, projectile.gfxOffY);
				Color color = projectile.GetAlpha(lightColor) * ((float)(projectile.oldPos.Length - k) / (float)projectile.oldPos.Length);
				spriteBatch.Draw(Main.projectileTexture[projectile.type], drawPos, null, color, projectile.rotation, drawOrigin, projectile.scale, SpriteEffects.None, 0f);
			}
			return true;
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
				projectile.timeLeft = projectile.timeLeft > 705 ? 705 : projectile.timeLeft;
				Main.PlaySound(SoundID.Item10, projectile.position);
			projectile.tileCollide = false;
			return false;
		}
	}
}
		