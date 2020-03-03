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
    public class Razorwater : ModProjectile 
    {
		
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Razorwater");
			ProjectileID.Sets.TrailCacheLength[projectile.type] = 6;  
			ProjectileID.Sets.TrailingMode[projectile.type] = 0;    
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
			projectile.width = 62;
			projectile.height = 62;
			projectile.friendly = true;
			projectile.timeLeft = 3600;
			projectile.penetrate = -1;
			projectile.tileCollide = false;
			projectile.magic = false;
			projectile.melee = true;
			projectile.alpha = 140;
			projectile.ai[1] = -1;
		}
		public override void AI()
		{
			projectile.rotation -= 0.37f;
			if(projectile.ai[1] != -1)
			{
				Projectile proj = Main.projectile[(int)projectile.ai[1]];
				if(proj.active && proj.type == mod.ProjectileType("Zeppelin") && proj.owner == projectile.owner)
				{
					projectile.position.X = proj.Center.X - projectile.width/2;
					projectile.position.Y = proj.Center.Y - projectile.height/2;
					projectile.timeLeft = 6;
				}
			}
		}
	}
}
		