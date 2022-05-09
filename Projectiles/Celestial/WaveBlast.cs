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
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 18;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
		}
		public override bool PreDraw(ref Color lightColor)
		{
			Vector2 drawOrigin = new Vector2(Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value.Width * 0.5f, Projectile.height * 0.5f);
			if (!SOTS.Config.lowFidelityMode)
			{
				if ((int)Projectile.ai[0] == 1)
					for (int k = 0; k < Projectile.oldPos.Length; k++)
					{
						Vector2 drawPos = Projectile.oldPos[k] + drawOrigin + new Vector2(0f, Projectile.gfxOffY);
						float trailMult = ((float)(Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length);
						Draw(spriteBatch, trailMult, drawPos, !SOTS.Config.lowFidelityMode);
					}
			}
			Draw(spriteBatch, 1f, Projectile.Center, true);
			return false;
		}
		public void Draw(SpriteBatch spriteBatch, float alphaMult, Vector2 pos, bool outLine = true)
		{
			Texture2D texture = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
			Vector2 origin = new Vector2(texture.Width / 2, texture.Height / 2);
			Color color = new Color(100, 255, 100, 0);
			if ((int)Projectile.ai[0] == 1)
				color = new Color(185, 39, 23, 0) * 1.5f;
			int amt = 60;
			if (SOTS.Config.lowFidelityMode)
				amt = 90;
			if (outLine)
				for (int i = 0; i < 360; i += amt)
				{
					Vector2 circular = new Vector2(Main.rand.NextFloat(3.5f, 5), 0).RotatedBy(MathHelper.ToRadians(i));
					Main.spriteBatch.Draw(texture, pos + circular - Main.screenPosition, null, color * ((255f - Projectile.alpha) / 255f) * alphaMult, Projectile.rotation, origin, Projectile.scale * alphaMult, SpriteEffects.None, 0.0f);
				}
			color = new Color(50, 122, 50);
			if ((int)Projectile.ai[0] == 1)
				color = new Color(122, 50, 50);
			Main.spriteBatch.Draw(texture, pos - Main.screenPosition, null, color * ((255f - Projectile.alpha) / 255f) * alphaMult, Projectile.rotation, origin, Projectile.scale * alphaMult, SpriteEffects.None, 0.0f);
		}
		public override void SetDefaults()
        {
			Projectile.width = 40;
			Projectile.height = 14;
			Projectile.friendly = false;
			Projectile.timeLeft = 360;
			Projectile.penetrate = -1;
			Projectile.tileCollide = false;
			Projectile.hostile = true;
			Projectile.alpha = 0;
			Projectile.ignoreWater = true;
			Projectile.extraUpdates = 0;
		}
		public override void AI()
		{
			if(Projectile.ai[0] == 1)
            {
				Projectile.extraUpdates = 1;
            }
			float mult = 1;
			if (Projectile.timeLeft > 240)
				mult = ((Projectile.timeLeft - 240) / 120f);
			else
            {
				mult = 1.0f - 0.8f * (Projectile.timeLeft / 240f);
            }
			if(Projectile.timeLeft < 24)
            {
				Projectile.alpha += 20;
            }
			Vector2 velo = Projectile.velocity * mult;
			Projectile.position += velo;
			if (Projectile.ai[0] == 1)
			{
				Lighting.AddLight(Projectile.Center, (255 - Projectile.alpha) * 0.9f / 255f, (255 - Projectile.alpha) * 0.2f / 255f, (255 - Projectile.alpha) * 0.1f / 255f);
			}
			else
				Lighting.AddLight(Projectile.Center, (255 - Projectile.alpha) * 0.1f / 255f, (255 - Projectile.alpha) * 0.9f / 255f, (255 - Projectile.alpha) * 0.3f / 255f);
			Projectile.rotation = (float)Math.Atan2((double)Projectile.velocity.Y, (double)Projectile.velocity.X);
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
		