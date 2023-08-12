using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Void;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace SOTS.Projectiles.Chaos
{
	public class DogmaLaser : ModProjectile
	{
		public override void SetStaticDefaults() 
		{
			// DisplayName.SetDefault("Hyperlight Beam");
			ProjectileID.Sets.DrawScreenCheckFluff[Type] = 2400;
		}

		public override void SetDefaults() 
		{
			Projectile.width = 8;
			Projectile.height = 8;
			Projectile.timeLeft = 140;
			Projectile.penetrate = -1;
			Projectile.hostile = true;
			Projectile.friendly = false;
			Projectile.tileCollide = false;
			Projectile.ignoreWater = true;
		}
		float counter = 0;
        public override bool ShouldUpdatePosition()
        {
			return false;
        }
		public const float windUpAngle = 80f;
		public const float windUpLength = 90f;
		bool runOnce = true;
		Vector2 ogVelo = Vector2.Zero;
		float scaleMult = 1f;
		public override void AI() 
		{
			if(runOnce)
            {
				ogVelo = Projectile.velocity;
				runOnce = false;
            }
			float angleToTraverse = Projectile.ai[0];
			if(counter < windUpLength)
			{
				scaleMult = Projectile.ai[1] + 1;
			}
			else
            {
				float timeLeftMult = (Projectile.timeLeft - 2) / 51f;
				if (timeLeftMult < 0)
					timeLeftMult = 0;
				Projectile.scale = (float)Math.Sqrt(timeLeftMult) * 0.5f + 0.5f * timeLeftMult;
            }
			counter++;
			float lerp = counter / windUpAngle;
			if (lerp > 1f)
				lerp = 1;
			float angle = angleToTraverse * (float)Math.Pow(1 - lerp, 1.5f) * 0.8f + 0.2f * (1 - lerp);
			Projectile.velocity = ogVelo.RotatedBy(MathHelper.ToRadians(angle));
			if(counter == windUpLength + 1)// && Projectile.knockBack == -1)
			{
				//Terraria.Audio.SoundEngine.PlaySound(SoundLoader.CustomSoundType, (int)Projectile.Center.X, (int)Projectile.Center.Y, mod.GetSoundSlot(SoundType.Custom, "Sounds/Enemies/LuxBeann"), 1.6f, -0.1f);
				SOTSUtils.PlaySound(SoundID.Item94, (int)Projectile.Center.X, (int)Projectile.Center.Y, 1.1f, 0.1f);
			}
			if (counter > windUpLength)
			{
				Vector2 position = Projectile.Center;
				for (float i = 40; i <= maxDistance; i += 8)
				{
					if (Main.rand.NextBool(1000) || (counter == windUpLength + 1 && Main.rand.NextBool(4)))
					{
						position = Projectile.Center + Projectile.velocity.SafeNormalize(Vector2.Zero) * i;
						int dust2 = Dust.NewDust(position - new Vector2(12, 12), 16, 16, ModContent.DustType<Dusts.CopyDust4>());
						Dust dust = Main.dust[dust2];
						dust.velocity *= 2f;
						dust.velocity += Projectile.velocity * 0.2f;
						dust.color = ColorHelpers.pastelAttempt(Main.rand.NextFloat(6.28f), true);
						dust.noGravity = true;
						dust.alpha = 90;
						dust.fadeIn = 0.1f;
						dust.scale *= 2.5f * Projectile.scale;
					}
				}
			}
		}
		public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox) 
		{
			float point = 0f;
			Vector2 finalPoint = Projectile.Center + Projectile.velocity.SafeNormalize(Vector2.Zero) * maxDistance;
			if(counter > windUpLength && counter < 110)
			{
				if (Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), Projectile.Center, finalPoint, 24f * scaleMult * Projectile.scale, ref point))
				{
					return true;
				}
			}
			return false;
			//return Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), Projectile.Center, endPoint, 8f, ref point);
		}
		public const float maxDistance = 3200;
        public override bool PreDraw(ref Color lightColor)
        {
			if (runOnce)
				return false;
			Player player = Main.player[Projectile.owner];
			float alphaScale = 0.5f;
			float lerp = counter / windUpLength * 0.5f;
			float scalingFactor = 0.5f;
			if (lerp > 0.5f)
			{
				scalingFactor = 1.2f;
				alphaScale = 0.2f + 0.8f * (float)Math.Sqrt(Projectile.scale);
				lerp = 1f;
			}
			else
			{
				lerp = lerp * lerp * 2;
				scalingFactor += 0.8f * lerp;
			}				
			if(counter > windUpLength - 20 && counter <= windUpLength)
            {
				float otherMult = (counter - windUpLength + 20) / 20f;
				alphaScale = 0.1f + 0.4f * (1 - otherMult);
				scalingFactor += 0.2f * otherMult;
			}
			Texture2D texture = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
			Vector2 origin = new Vector2(texture.Width / 2, texture.Height / 2);
			float length = texture.Width * 0.5f * scalingFactor;
			Vector2 unit = Projectile.velocity.SafeNormalize(Vector2.Zero);
			Vector2 position = Projectile.Center;
			float maxLength = maxDistance / length * lerp;
			for (float i = 0; i <= maxLength; i++)
			{
				if (!SOTS.Config.lowFidelityMode || i % 2 == 0)
				{
					float radians = MathHelper.ToRadians((i + ColorHelpers.soulColorCounter) * 2);
					Color color = ColorHelpers.pastelAttempt(radians);
					color.A = 0;
					float mult = 1 - (i / maxLength);
					float sinusoid = 1;
					if (lerp >= 1)
					{
						mult = 1;
						if (i > maxLength - 40)
						{
							mult = 1 - (i - maxLength + 40) / 40f;
						}
						sinusoid = 1.0f + (0.5f + 0.5f * (float)Math.Sin(MathHelper.ToRadians(i * 16 + ColorHelpers.soulColorCounter * 4f))) * Projectile.scale;
					}
					float scale = Projectile.scale * scalingFactor * scaleMult * sinusoid;
					Vector2 drawPos = position - Main.screenPosition;
					Main.spriteBatch.Draw(texture, drawPos, null, color * alphaScale * mult, Projectile.velocity.ToRotation(), origin, new Vector2(scalingFactor, scale), SpriteEffects.None, 0f);
				}
				position += unit * length;
			}
			return false;
		}
	}
}