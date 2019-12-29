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
    public class DeathBlade : ModProjectile 
    {	int bounce = 8;
		int wait = 1;              
		float Speed = 1f;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Death's Touch");
			ProjectileID.Sets.TrailCacheLength[projectile.type] = 7;  
			ProjectileID.Sets.TrailingMode[projectile.type] = 1;    
		}
		
        public override void SetDefaults()
        {
			projectile.magic = true;
			projectile.friendly = true;
			projectile.width = 66;
			projectile.height = 46;
			projectile.timeLeft = 240;
			projectile.penetrate = 9;
			projectile.alpha = 100;
			projectile.tileCollide = false;
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
		public override void AI()
		{ 
			Player player = Main.player[projectile.owner];
			Lighting.AddLight(projectile.Center, (255 - projectile.alpha) * 0.1f / 255f, (255 - projectile.alpha) * 0.9f / 255f, (255 - projectile.alpha) * 0.3f / 255f);
			projectile.rotation += 0.3f;
			
			if(player.whoAmI == Main.myPlayer)
			{
				projectile.netUpdate = true;
				Vector2 cursorArea = Main.MouseWorld;
				float dX = 0f;
				float dY = 0f;
				float distance = 0;
				float speed = 1f;
					
					
				if(projectile.timeLeft > 192 && projectile.timeLeft < 224 && projectile.penetrate >= 9)
				{
					dX = cursorArea.X - projectile.Center.X;
					dY = cursorArea.Y - projectile.Center.Y;
					distance = (float)Math.Sqrt((double)(dX * dX + dY * dY));
					speed /= distance;
					projectile.velocity *= 0.9725f;
					projectile.velocity += new Vector2(dX * speed, dY * speed);
				}
			}
			if(projectile.timeLeft <= 20)
			{
				projectile.alpha += 7;
				projectile.velocity *= 0.915f;
			}
			
		}
		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
			Player player = Main.player[projectile.owner];
			projectile.velocity *= 0.25f;
			projectile.netUpdate = true;
			if(target.life <= 2500)
			{
            target.immune[projectile.owner] = 12;
			}
			else
			{
            target.immune[projectile.owner] = 9;
			}
		}
		public override void Kill(int timeLeft)
		{
			for(int i = 0; i < 360; i += 8)
			{
			Vector2 circularLocation = new Vector2(-28, 0).RotatedBy(MathHelper.ToRadians(i));
			
			int num1 = Dust.NewDust(new Vector2(projectile.Center.X + circularLocation.X - 4, projectile.Center.Y + circularLocation.Y - 4), 4, 4, 107);
			Main.dust[num1].noGravity = true;
			Main.dust[num1].velocity = circularLocation * 0.5f;
			
			}
		}
	}
}
		