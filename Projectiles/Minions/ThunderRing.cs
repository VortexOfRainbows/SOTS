using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Void;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Projectiles.Minions
{
	public class ThunderRing : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Thunder Ring");
		}
		public override void SetDefaults()
		{
			projectile.width = 48;
			projectile.height = 48;
			projectile.friendly = true;
			projectile.melee = false;
			projectile.timeLeft = 40;
			projectile.tileCollide = false;
			projectile.penetrate = -1;
			projectile.localNPCHitCooldown = 40;
			projectile.usesLocalNPCImmunity = true;
		}
		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			projectile.localNPCImmunity[target.whoAmI] = projectile.localNPCHitCooldown;
			target.immune[projectile.owner] = 0;
		}
		float[] variance = new float[20];
		bool runOnce = true;
		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			if (runOnce)
				return false;
			Texture2D texture = Main.projectileTexture[projectile.type];
			Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
			Vector2 previousPosition = new Vector2(projectile.ai[0] + this.variance[19], 0).RotatedBy(MathHelper.ToRadians(19 * 18) + projectile.rotation) + projectile.Center;
			float scale = projectile.scale * 1.15f;
			for (int k = 0; k < 20; k++)
			{
				float variance = this.variance[k];
				Vector2 circularPos = new Vector2(projectile.ai[0] + variance, 0).RotatedBy(MathHelper.ToRadians(k * 18) + projectile.rotation) + projectile.Center;
				Vector2 betweenPositions = previousPosition - circularPos;
				float max = betweenPositions.Length() / (texture.Width * scale * 0.2f);
				for (int i = 0; i < max; i++)
				{
					Vector2 drawPos = previousPosition + -betweenPositions * (i / max) - Main.screenPosition;
					for (int j = 0; j < 2; j++)
					{
						float x = Main.rand.Next(-10, 11) * 0.125f * scale;
						float y = Main.rand.Next(-10, 11) * 0.125f * scale;
						if (!previousPosition.Equals(projectile.Center))
							Main.spriteBatch.Draw(texture, drawPos + new Vector2(x, y), null, VoidPlayer.OtherworldColor * 0.8f * ((255 - projectile.alpha) / 255f), betweenPositions.ToRotation() + MathHelper.ToRadians(90), drawOrigin, scale, SpriteEffects.None, 0f);
					}
				}
				previousPosition = circularPos;
			}
			return false;
		}
		public override void AI()
		{
			if(runOnce)
            {
				projectile.alpha = 30;
				projectile.ai[0] = 24;
				for (int i = 0; i < 20; i++)
                {
					variance[i] = Main.rand.NextFloat(4) * ((i % 2) * 2 - 1);
				}
				runOnce = false;
			}
			for (int i = 0; i < 20; i++)
			{
				variance[i] += Main.rand.NextFloat(-4, 5);
				if(Math.Abs(variance[i]) > 8)
                {
					variance[i] *= 0.9f;
				}
			}
			projectile.scale *= 0.99f;
			Lighting.AddLight(projectile.Center, VoidPlayer.OtherworldColor.R / 255f, VoidPlayer.OtherworldColor.G / 255f, VoidPlayer.OtherworldColor.B / 255f);
			projectile.alpha += 6;
		}
	}
}