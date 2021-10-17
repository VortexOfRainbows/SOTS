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
using SOTS.Buffs;

namespace SOTS.Projectiles.Celestial
{    
    public class WaveBlast : ModProjectile 
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Wave Blast");
			ProjectileID.Sets.TrailCacheLength[projectile.type] = 18;
			ProjectileID.Sets.TrailingMode[projectile.type] = 0;
		}
		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Vector2 drawOrigin = new Vector2(Main.projectileTexture[projectile.type].Width * 0.5f, projectile.height * 0.5f);
			if (!SOTS.Config.lowFidelityMode)
			{
				if ((int)projectile.ai[0] == 1)
					for (int k = 0; k < projectile.oldPos.Length; k++)
					{
						Vector2 drawPos = projectile.oldPos[k] + drawOrigin + new Vector2(0f, projectile.gfxOffY);
						float trailMult = ((float)(projectile.oldPos.Length - k) / (float)projectile.oldPos.Length);
						Draw(spriteBatch, trailMult, drawPos, !SOTS.Config.lowFidelityMode);
					}
			}
			Draw(spriteBatch, 1f, projectile.Center, true);
			return false;
		}
		public void Draw(SpriteBatch spriteBatch, float alphaMult, Vector2 pos, bool outLine = true)
		{
			Texture2D texture = Main.projectileTexture[projectile.type];
			Vector2 origin = new Vector2(texture.Width / 2, texture.Height / 2);
			Color color = new Color(100, 255, 100, 0);
			if ((int)projectile.ai[0] == 1)
				color = new Color(255, 100, 100, 0);
			int amt = 60;
			if (SOTS.Config.lowFidelityMode)
				amt = 90;
			if (outLine)
				for (int i = 0; i < 360; i += amt)
				{
					Vector2 circular = new Vector2(Main.rand.NextFloat(3.5f, 5), 0).RotatedBy(MathHelper.ToRadians(i));
					Main.spriteBatch.Draw(texture, pos + circular - Main.screenPosition, null, color * ((255f - projectile.alpha) / 255f) * alphaMult, projectile.rotation, origin, projectile.scale * alphaMult, SpriteEffects.None, 0.0f);
				}
			color = new Color(50, 122, 50);
			if ((int)projectile.ai[0] == 1)
				color = new Color(122, 50, 50);
			Main.spriteBatch.Draw(texture, pos - Main.screenPosition, null, color * ((255f - projectile.alpha) / 255f) * alphaMult, projectile.rotation, origin, projectile.scale * alphaMult, SpriteEffects.None, 0.0f);
		}
		public override void SetDefaults()
        {
			projectile.width = 40;
			projectile.height = 14;
			projectile.friendly = false;
			projectile.timeLeft = 360;
			projectile.penetrate = -1;
			projectile.tileCollide = false;
			projectile.hostile = true;
			projectile.alpha = 0;
			projectile.ignoreWater = true;
			projectile.extraUpdates = 0;
		}
		public override void AI()
		{
			if(projectile.ai[0] == 1)
            {
				projectile.extraUpdates = 1;
            }
			float mult = 1;
			if (projectile.timeLeft > 240)
				mult = ((projectile.timeLeft - 240) / 120f);
			else
            {
				mult = 1.0f - 0.8f * (projectile.timeLeft / 240f);
            }
			if(projectile.timeLeft < 24)
            {
				projectile.alpha += 20;
            }
			Vector2 velo = projectile.velocity * mult;
			projectile.position += velo;
			Lighting.AddLight(projectile.Center, (255 - projectile.alpha) * 0.1f / 255f, (255 - projectile.alpha) * 0.9f / 255f, (255 - projectile.alpha) * 0.3f / 255f);
			projectile.rotation = (float)Math.Atan2((double)projectile.velocity.Y, (double)projectile.velocity.X);
		}
        public override bool ShouldUpdatePosition()
        {
			return false;
        }
        public override void OnHitPlayer(Player target, int damage, bool crit)
		{
			target.AddBuff(ModContent.BuffType<AbyssalInferno>(), 60, false);
		}
	}
}
		