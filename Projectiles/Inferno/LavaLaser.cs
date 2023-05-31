using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Dusts;
using SOTS.Void;
using System;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Projectiles.Inferno
{
	public class LavaLaser : ModProjectile
	{
        public override void SendExtraAI(BinaryWriter writer)
        {
			writer.Write(hasHit);
        }
        public override void ReceiveExtraAI(BinaryReader reader)
        {
			hasHit = reader.ReadBoolean();
        }
        public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Lava Laser");
		}
		public override void SetDefaults()
		{
			Projectile.width = 16;
			Projectile.height = 16;
			Projectile.hostile = true;
			Projectile.friendly = false;
			Projectile.DamageType = DamageClass.Magic;
			Projectile.extraUpdates = 10;
			Projectile.timeLeft = 1200;
			Projectile.tileCollide = true;
			Projectile.penetrate = -1;
		}
        Vector2[] trailPos = new Vector2[200];
		public void cataloguePos()
		{
			Vector2 current = Projectile.Center;
			for (int i = 0; i < trailPos.Length; i++)
			{
				Vector2 previousPosition = trailPos[i];
				trailPos[i] = current;
				current = previousPosition;
			}
		}
		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			triggerUpdate();
			return false;
		}
		public void TrailPreDraw(SpriteBatch spriteBatch)
		{
			Texture2D texture = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
			Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
			Vector2 previousPosition = Projectile.Center;
			for (int k = 0; k < trailPos.Length; k++)
			{
				float scale = Projectile.scale * 1.25f * (trailPos.Length - k) / (float)trailPos.Length;
				if (trailPos[k] == Vector2.Zero)
				{
					break;
				}
				Color color = ColorHelpers.InfernoColorAttemptDegrees(k);
				color.A = 0;
				Vector2 drawPos = trailPos[k] - Main.screenPosition;
				Vector2 currentPos = trailPos[k];
				Vector2 betweenPositions = previousPosition - currentPos;
				color = color * ((trailPos.Length - k) / (float)trailPos.Length) * 0.33f;
				float max = betweenPositions.Length() / (texture.Width * scale);
				for (int i = 0; i < max; i++)
				{
					drawPos = previousPosition + -betweenPositions * (i / max) - Main.screenPosition;
					if (trailPos[k] != Projectile.Center)
						spriteBatch.Draw(texture, drawPos, null, color, betweenPositions.ToRotation(), drawOrigin, scale, SpriteEffects.None, 0f);
				}
				previousPosition = currentPos;
			}
		}
		public override bool PreDraw(ref Color lightColor)
		{
			TrailPreDraw(Main.spriteBatch);
			return false;
		}
		bool hasHit = false;
		bool runOnce = true;
		public override void AI()
		{
			cataloguePos();
			Player player = Main.player[Projectile.owner];
			Lighting.AddLight(Projectile.Center, ColorHelpers.Inferno1.ToVector3());
			if (runOnce)
			{
				runOnce = false;
				//Terraria.Audio.SoundEngine.PlaySound(SoundID.Item, (int)Projectile.Center.X, (int)Projectile.Center.Y, 60, 0.8f, -0.1f);
			}
			if(Main.rand.NextBool(120))
            {
				int dust2 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, ModContent.DustType<CopyDust4>());
				Dust dust = Main.dust[dust2];
				dust.color = ColorHelpers.InfernoColorAttempt(Main.rand.NextFloat(1));
				dust.noGravity = true;
				dust.fadeIn = 0.1f;
				dust.scale *= 2f;
				dust.velocity *= 0.8f;
				dust.alpha = 125;
			}
			if (!Projectile.velocity.Equals(new Vector2(0, 0)))
				Projectile.rotation = Projectile.velocity.ToRotation();

			if (hasHit)
			{
				if (Projectile.timeLeft > 240)
					Projectile.timeLeft = 240;
			}
			else
			{
				float sin = (float)Math.Sin(MathHelper.ToRadians(Projectile.ai[1] * 1.1f)) * Projectile.ai[0];
				Projectile.Center += new Vector2(0, sin).RotatedBy(Projectile.velocity.ToRotation());
				Projectile.ai[1]++;
			}
		}
		public override void OnHitPlayer(Player target, int damage, bool crit)
		{
			target.AddBuff(BuffID.OnFire, 600, false);
		}
		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			target.immune[Projectile.owner] = 0;
			triggerUpdate();
		}
		public void triggerUpdate()
		{
			hasHit = true;
			Projectile.velocity *= 0;
			Projectile.friendly = false;
			if (Projectile.owner == Main.myPlayer)
			{
				Projectile.netUpdate = true;
				//Projectile.NewProjectile(Projectile.Center.X, Projectile.Center.Y, 0, 0, ModContent.ProjectileType<VibrantRing>(), Projectile.damage, Projectile.knockBack * 0.1f, Main.myPlayer);
			}
		}
	}
}