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

namespace SOTS.Projectiles.Minions
{    
    public class PearlescentCore : ModProjectile 
    {	
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Pearlescent Core");
			ProjectileID.Sets.TrailCacheLength[projectile.type] = 3;  
			ProjectileID.Sets.TrailingMode[projectile.type] = 2;    
		}
		
        public override void SetDefaults()
        {
			projectile.width = 20;
			projectile.height = 20;
            Main.projFrames[projectile.type] = 1;
			projectile.penetrate = -1;
			projectile.friendly = false;
			projectile.timeLeft = 960;
			projectile.tileCollide = false;
			projectile.hostile = false;
			projectile.minion = true;
			projectile.magic = true;
			projectile.alpha = 0;
            projectile.netImportant = true;
            projectile.minionSlots = 0f;
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
	}
}
		