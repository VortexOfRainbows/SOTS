using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Void;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using SOTS.Dusts;

namespace SOTS.Projectiles.Anomaly
{
	public class UltraLaser : ModProjectile
	{
		public override void SetStaticDefaults() 
		{
			ProjectileID.Sets.DrawScreenCheckFluff[Type] = 2000;
		}
		public override void SetDefaults() 
		{
			Projectile.width = 8;
			Projectile.height = 8;
			Projectile.timeLeft = 60;
			Projectile.penetrate = -1;
			Projectile.hostile = true;
			Projectile.friendly = false;
			Projectile.tileCollide = false;
			Projectile.ignoreWater = true;
		}
        public override bool ShouldUpdatePosition()
        {
			return false;
		}
		Vector2 finalPosition = Vector2.Zero;
		bool hasInit = false;
		float scaleMult = 1f;
		public override void AI() 
		{
			if(!hasInit)
			{
				Color color = ColorHelpers.VoidAnomaly;
				color.A = 0;
				SOTSUtils.PlaySound(SoundID.Item92, (int)Projectile.Center.X, (int)Projectile.Center.Y, 1.1f, -0.4f);
				for (int i = 20; i > 0; i--)
				{
					Dust dust = Dust.NewDustDirect(Projectile.Center - new Vector2(12, 4), 20, 0, ModContent.DustType<CopyDust4>(), 0, 0, 0, color, 1.4f);
					dust.noGravity = true;
					dust.velocity *= 1.5f;
					dust.velocity -= Projectile.velocity * Main.rand.NextFloat(1f, 2f);
					dust.fadeIn = 0.2f;
				}
			}
			InitializeLaser();
		}
		public void InitializeLaser()
		{
			Color color = ColorHelpers.VoidAnomaly;
			color.A = 0;
			Vector2 startingPosition = Projectile.Center;
			Projectile.velocity = Projectile.velocity.SafeNormalize(Vector2.Zero);
			for (int b = 0; b < 800; b++)
			{
				startingPosition += Projectile.velocity * 2.5f;
				finalPosition = startingPosition;
				int i = (int)startingPosition.X / 16;
				int j = (int)startingPosition.Y / 16;
				if (WorldgenHelpers.SOTSWorldgenHelper.TrueTileSolid(i, j))
				{
					break;
				}
				bool extra = !hasInit;
				int chance = SOTS.Config.lowFidelityMode ? 100 : 70;
				if (Main.rand.NextBool(chance) || extra && b > 10)
				{
					Dust dust = Dust.NewDustDirect(finalPosition - new Vector2(5, 5), 0, 0, ModContent.DustType<CopyDust4>(), 0, 0, 0, color, 0.8f);
					dust.noGravity = true;
					if (!extra)
					{
						dust.velocity = dust.velocity * 0.25f;
					}
					else
					{
						dust.velocity *= 1.2f;
						dust.velocity += Projectile.velocity * Main.rand.NextFloat(4f, 7f);
						dust.scale *= 1.4f;
					}
					dust.fadeIn = 0.2f;
					dust.velocity.X += Main.rand.NextFloat(-3, 3f);
				}
			}
			if(!hasInit)
			{
				for (int i = 20; i > 0; i--)
				{
					Dust dust = Dust.NewDustDirect(finalPosition - new Vector2(12, 4), 20, 0, ModContent.DustType<CopyDust4>(), 0, 0, 0, color, 1.5f);
					dust.noGravity = true;
					dust.velocity *= 1.0f;
					dust.velocity += Projectile.velocity * Main.rand.NextFloat(4f, 7f);
					dust.fadeIn = 0.2f;
				}
			}
			else if(!Main.rand.NextBool(3))
			{
				Dust dust = Dust.NewDustDirect(finalPosition - new Vector2(12, 4), 20, 0, ModContent.DustType<CopyDust4>(), 0, 0, 0, color, 1.5f);
				dust.noGravity = true;
				dust.velocity = dust.velocity * 0.2f + Projectile.velocity * Main.rand.NextFloat(0.1f, 1.0f);
				dust.fadeIn = 0.2f;
			}
			hasInit = true;
		}
		public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox) 
		{
			float point = 0f;
			if(Projectile.timeLeft > 30)
			{
				if (Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), Projectile.Center, finalPosition, 20f * scaleMult * Projectile.scale, ref point))
				{
					return true;
				}
			}
			return false;
			//return Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), Projectile.Center, endPoint, 8f, ref point);
		}
		public void Draw(SpriteBatch spriteBatch)
		{
			if (!hasInit)
				return;
			float alphaScale = Projectile.timeLeft / 60f;
			Texture2D texture = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
			Vector2 origin = new Vector2(texture.Width / 2, texture.Height / 2);
			Vector2 toEnd = finalPosition - Projectile.Center;
			float maxLength = toEnd.Length() / texture.Height * 4;
			Color color;
			for (int j = 0; j < 12; j++)
			{
				Vector2 offset = Vector2.Zero;
				float percent = 0f;
				color = new Color(120, 100, 130, 0);
				for (float i = 0; i < maxLength; i++)
				{
					if(j != 0)
						offset = new Vector2(2 * (float)Math.Sin(MathHelper.ToRadians(i * 12)), 0).RotatedBy(j * MathHelper.TwoPi / 12f + MathHelper.ToRadians(SOTSWorld.GlobalCounter));
					if (percent < 1)
						percent += 0.1f;
					if (!SOTS.Config.lowFidelityMode || (int)(i % 2) == 0)
					{
						Vector2 position = Vector2.Lerp(Projectile.Center, finalPosition, i / maxLength);
						//float radians = MathHelper.ToRadians((Math.Abs(i) + ColorHelpers.soulColorCounter) * 2);
						//Color color = ColorHelpers.pastelAttempt(radians);
						//color.A = 0;
						//float mult = 1;
						//float sinusoid = 1.0f + (0.1f + 0.1f * (float)Math.Sin(MathHelper.ToRadians(Math.Abs(i) * 16 + ColorHelpers.soulColorCounter * 4f))) * Projectile.scale;
						//float scale = Projectile.scale * scaleMult * sinusoid * (1 - 0.9f * (float)Math.Abs(i) / maxLength);
						Vector2 drawPos = position - Main.screenPosition;
						spriteBatch.Draw(texture, drawPos + offset, null, color * alphaScale * percent, Projectile.velocity.ToRotation() + MathHelper.PiOver2, origin, new Vector2(1.0f * (0.5f + (float)Math.Sqrt(percent)), 0.5f), SpriteEffects.None, 0f);
					}
				}
			}
			return;
		}
        public override bool PreDraw(ref Color lightColor)
		{
			Draw(Main.spriteBatch);
			return false;
		}
        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
			VoidPlayer.VoidBurn(Mod, target, 10, 420);
		}
    }
}