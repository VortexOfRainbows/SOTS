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
using SOTS.Dusts;
using SOTS.Projectiles.Blades;

namespace SOTS.Projectiles.Lightning
{    
    public class VorpalLightning2 : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			ProjectileID.Sets.DrawScreenCheckFluff[Type] = 3200;
			DisplayName.SetDefault("Vorpal Lightning");
		}
		public override void SetDefaults()
		{
			Projectile.width = 24;
			Projectile.height = 24;
			Projectile.friendly = false;
			Projectile.DamageType = ModContent.GetInstance<Void.VoidMelee>();
			Projectile.timeLeft = 3600;
			Projectile.tileCollide = false;
			Projectile.penetrate = -1;
			Projectile.alpha = 100;
			Projectile.scale = 1f;
			Projectile.localNPCHitCooldown = 10;
			Projectile.usesLocalNPCImmunity = true;
		}
		public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
		{
			if (Projectile.alpha >= 150 || !Projectile.friendly)
			{
				return false;
			}
			float width = Projectile.width;
			float height = Projectile.height;
			for (int i = 0; i < trailPos.Length; i++)
			{
				Vector2 pos = trailPos[i];
				projHitbox = new Rectangle((int)pos.X - (int)width / 2, (int)pos.Y - (int)height / 2, (int)width, (int)height);
				if (projHitbox.Intersects(targetHitbox))
				{
					return true;
				}
                if (trailPos[i].Equals(Vector2.Zero))
                {
					return false;
                }
			}
			return false;
		}
		Vector2[] trailPos = new Vector2[250];
		public override bool PreDraw(ref Color lightColor)
		{
			if (runOnce)
				return false;
			Texture2D texture = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
			Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
			Vector2 previousPosition = Projectile.Center;
			Color color = new Color(140, 200, 140, 0) * ((255 - Projectile.alpha) / 255f);
			for (int k = 0; k < trailPos.Length; k++)
			{
				if (trailPos[k] == Vector2.Zero)
				{
					return false;
				}
				float scale = Projectile.scale * 1.5f + 2.0f * (k / (float)trailPos.Length);
				Vector2 drawPos;
				Vector2 currentPos = trailPos[k];
				Vector2 betweenPositions = previousPosition - currentPos;
				float max = betweenPositions.Length() / (texture.Width * scale * 0.25f);
				for (int i = 0; i < max; i++)
				{
					drawPos = previousPosition + -betweenPositions * (i / max) - Main.screenPosition;
					for (int j = 0; j < 2; j++)
					{
						if (trailPos[k] != Projectile.Center)
							Main.spriteBatch.Draw(texture, drawPos + Main.rand.NextVector2Circular(j * 0.75f, j * 0.75f), null, color, betweenPositions.ToRotation() + MathHelper.ToRadians(90), drawOrigin, scale, SpriteEffects.None, 0f);
					}
				}
				previousPosition = currentPos;
			}
			return false;
		}
		public override bool ShouldUpdatePosition()
		{
			return false;
		}
		bool runOnce = true;
		Vector2 addPos = Vector2.Zero;
		Vector2 originalVelo = Vector2.Zero;
		Vector2 originalPos = Vector2.Zero;
		int[] randStorage = new int[250];
		int dist = 250;
		public override void AI()
		{
			if (runOnce)
			{
				Vector2 positionToReach = new Vector2(Projectile.ai[0], Projectile.ai[1]);
				Projectile.position += Projectile.velocity.SafeNormalize(Vector2.Zero) * 24;
				SOTSUtils.PlaySound(SoundID.Item94, (int)positionToReach.X, (int)positionToReach.Y, 1.05f, 0.2f);
				for (int i = 0; i < randStorage.Length; i++)
				{
					randStorage[i] = Main.rand.Next(-60, 61);
				}
				for (int i = 0; i < trailPos.Length; i++)
				{
					trailPos[i] = Vector2.Zero;
				}
				originalVelo = Projectile.velocity.SafeNormalize(Vector2.Zero) * 12f;
				originalPos = positionToReach;
				Vector2 temp = originalPos;
				addPos = Projectile.Center;
				for (int i = 0; i < dist; i++)
				{
					Vector2 attemptToPosition = originalPos - addPos;
					Vector2 nextVelocity = new Vector2(originalVelo.Length(), 0).RotatedBy(attemptToPosition.ToRotation() + MathHelper.ToRadians(randStorage[i]));
					addPos += nextVelocity;
					trailPos[i] = addPos;
					nextVelocity = Vector2.Lerp(nextVelocity, originalVelo, 0.66f);
					if(i % 10 == 2)
                    {
						float scaleFactor = (float)Math.Sqrt(0.5f * i);
						for (int j = 0; j < 30; j++)
                        {
							Vector2 circular = new Vector2(24 * (0.5f + 0.125f * scaleFactor), 0).RotatedBy(MathHelper.ToRadians(j * 12));
							circular.X *= 0.5f;
							circular = circular.RotatedBy(nextVelocity.ToRotation());

							Dust dust = Dust.NewDustDirect(addPos + circular, 0, 0, ModContent.DustType<CopyDust4>());
							dust.noGravity = true;
							dust.velocity *= 0.25f;
							dust.velocity += nextVelocity * (0.5f + 0.125f * scaleFactor);
							dust.scale *= 1.5f + 0.065f * scaleFactor;
							dust.fadeIn = 0.1f;
							dust.color = Color.Lerp(VorpalThrow.VorpalColor1, VorpalThrow.VorpalColor2, Main.rand.NextFloat(1f));
							dust.alpha = 100;
						}
                    }
					if (trailPos[i].Distance(originalPos) < 24)
					{
						break;
					}
				}
				originalPos = temp;
				runOnce = false;
			}
			Projectile.alpha += 4;
			if (Projectile.alpha >= 255)
				Projectile.Kill();
			Projectile.scale *= 0.974f;
			Projectile.friendly = true;
		}
	}
}
		