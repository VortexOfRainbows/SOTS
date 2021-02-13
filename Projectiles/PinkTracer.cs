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
    public class PinkTracer : ModProjectile 
    {	
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Putrid Tracer");
			ProjectileID.Sets.TrailCacheLength[projectile.type] = 12;  
			ProjectileID.Sets.TrailingMode[projectile.type] = 1;    
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
        public override void SetDefaults()
        {
			projectile.height = 22;
			projectile.width = 22;
			projectile.friendly = false;
			projectile.penetrate = -1;
			projectile.timeLeft = 330;
			projectile.tileCollide = false;
			projectile.hostile = true;
			projectile.magic = false;
			projectile.ranged = false;
			projectile.netImportant = true;
		}
		public override void Kill(int timeLeft)
        {
			for(int i = 0; i < 360; i += 24)
			{
				Vector2 circularLocation = new Vector2(-8, 0).RotatedBy(MathHelper.ToRadians(i));
				
				int num1 = Dust.NewDust(projectile.Center + circularLocation - new Vector2(5), 0, 0, 72);
				Main.dust[num1].noGravity = true;
				Main.dust[num1].velocity = circularLocation * 0.45f;
			}
		}
		public override void AI()
		{
			int num1 = Dust.NewDust(new Vector2(projectile.Center.X - 1, projectile.Center.Y - 1), 2, 2, 72);
			Main.dust[num1].noGravity = true;
			Main.dust[num1].velocity *= 0.1f;
			
			projectile.rotation += 0.1f;
		}
	}
}
		