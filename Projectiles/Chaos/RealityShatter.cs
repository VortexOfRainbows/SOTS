using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Void;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace SOTS.Projectiles.Chaos
{
	public class RealityShatter : ModProjectile
	{
		public override void SetStaticDefaults() 
		{
			DisplayName.SetDefault("Shattering Laser");
		}
		public override void SetDefaults() 
		{
			Projectile.width = 8;
			Projectile.height = 8;
			Projectile.timeLeft = 40;
			Projectile.penetrate = -1;
			Projectile.hostile = false;
			Projectile.friendly = true;
			Projectile.DamageType = DamageClass.Melee;
			Projectile.tileCollide = false;
			Projectile.ignoreWater = true;
			Projectile.localNPCHitCooldown = 30;
			Projectile.usesLocalNPCImmunity = true;
		}
        public override bool ShouldUpdatePosition()
        {
			return false;
        }
		bool runOnce = true;
		float scaleMult = 1f;
		int counter = 0;
		public override void AI() 
		{
			if(runOnce)
			{
				SOTSUtils.PlaySound(SoundID.Item92, (int)Projectile.Center.X, (int)Projectile.Center.Y, 1.1f, 0.4f);
			}
			float bonus = Projectile.ai[0];
			if (bonus > 8)
				bonus = 8;
			counter++;
			if(Projectile.ai[1] >= 0 && Projectile.ai[0] > 0)
			{
				NPC presumedTarget = Main.npc[(int)Projectile.ai[1]];
				if (presumedTarget.CanBeChasedBy())
				{
					if (counter == (int)(12 + bonus))
					{
						if (Main.myPlayer == Projectile.owner)
						{
							Projectile.NewProjectile(Projectile.GetSource_FromThis(), presumedTarget.Center, Projectile.velocity.RotatedBy(MathHelper.ToRadians(Main.rand.NextFloat(-45, 45))), ModContent.ProjectileType<RealityShatter>(), Projectile.damage, Projectile.knockBack, Main.myPlayer, Projectile.ai[0] - 1, (int)Projectile.ai[1]);
						}
					}
				}
				else
					Projectile.ai[1] = -1;
			}
			scaleMult = (0.8f + 0.05f * bonus) * Projectile.timeLeft / 40f;
			runOnce = false;
		}
		public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox) 
		{
			float point = 0f;
			if(Projectile.timeLeft > 20)
			{
				Vector2 finalPoint = Projectile.Center + Projectile.velocity.SafeNormalize(Vector2.Zero) * (maxDistance);
				if (Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), Projectile.Center, finalPoint, 12f * scaleMult * Projectile.scale, ref point))
				{
					return true;
				}
				finalPoint = Projectile.Center - Projectile.velocity.SafeNormalize(Vector2.Zero) * (maxDistance);
				if (Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), Projectile.Center, finalPoint, 12f * scaleMult * Projectile.scale, ref point))
				{
					return true;
				}
			}
			return false;
			//return Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), Projectile.Center, endPoint, 8f, ref point);
		}
		public const float maxDistance = 150;
		public void Draw(SpriteBatch spriteBatch, int type)
		{
			if (runOnce)
				return;
			float alphaScale = 1f;
			Texture2D texture = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
			Vector2 origin = new Vector2(texture.Width / 2, texture.Height / 2);
			Texture2D textureBlack = (Texture2D)ModContent.Request<Texture2D>("SOTS/Projectiles/Chaos/BlackLuxLaser");
			Vector2 originBlack = new Vector2(textureBlack.Width / 2, textureBlack.Height / 2);
			float length = texture.Width * 0.5f;
			Vector2 unit = Projectile.velocity.SafeNormalize(Vector2.Zero);
			float windUp = counter / 9f;
			float maxLength = maxDistance / length;
			float min = MathHelper.Lerp(-maxLength, maxLength, MathHelper.Clamp(windUp * 0.33f, 0, 1));
			float max = MathHelper.Lerp(-maxLength, maxLength, MathHelper.Clamp(windUp, 0, 1));
			int count = 0;
			for (float i = min; i <= max; i++)
			{
				count++;
				Vector2 position = Projectile.Center + unit * length * i;
				float radians = MathHelper.ToRadians((Math.Abs(i) + VoidPlayer.soulColorCounter) * 3 + Projectile.whoAmI * 60);
				Color color = VoidPlayer.pastelAttempt(radians);
				color.A = 0;
				float mult = 1;
				float sinusoid = 1.0f + (0.1f + 0.1f * (float)Math.Sin(MathHelper.ToRadians(Math.Abs(i) * 16 + VoidPlayer.soulColorCounter * 4f))) * Projectile.scale;
				float scale = Projectile.scale * scaleMult * sinusoid * (0.3f + 0.7f * (float)Math.Sin(MathHelper.Lerp(0, MathHelper.Pi, count / (max - min))));
				Vector2 drawPos = position - Main.screenPosition;
				if (type == 1 && i != min && i != max)
					spriteBatch.Draw(texture, drawPos, null, color * alphaScale * mult, Projectile.velocity.ToRotation(), origin, new Vector2(1f, scale), SpriteEffects.None, 0f);
				else if(type == 0)
					spriteBatch.Draw(textureBlack, drawPos, null, Color.White * alphaScale * mult, Projectile.velocity.ToRotation(), originBlack , new Vector2(1f, scale), SpriteEffects.None, 0f);
			}
			return;
		}
        public override bool PreDraw(ref Color lightColor)
		{
			Draw(Main.spriteBatch, 1);
			DrawBlack(Main.spriteBatch);
			return false;
		}
        public void DrawBlack(SpriteBatch spriteBatch)
        {
			Draw(spriteBatch, 0);
        }
	}
}