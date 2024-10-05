using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Dusts;
using SOTS.Helpers;
using SOTS.Void;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Projectiles.Earth.Glowmoth
{    
    public class GlowBombOrb : ModProjectile 
    {
		public const int TrailLength = 20;
		public override void SetStaticDefaults()
		{
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = TrailLength;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
		}
        public override void SetDefaults()
        {
			Projectile.penetrate = -1;
			Projectile.friendly = false;
			Projectile.hostile = true;
			Projectile.alpha = 0;
			Projectile.width = 40;
			Projectile.height = 40;
			Projectile.ignoreWater = true;
			Projectile.tileCollide = false;
			Projectile.timeLeft = 900;
			Projectile.alpha = 255;
			Projectile.extraUpdates = 1;
		}
		public override void ModifyDamageHitbox(ref Rectangle hitbox)
		{
			int width = 40;
			float scaleMult = Projectile.ai[1] / 5f + 0.5f;
			if (Projectile.ai[0] < 0)
			{
				width = (int)(width * scaleMult);
			}
			hitbox = new Rectangle((int)Projectile.Center.X - width / 2, (int)Projectile.Center.Y - width / 2, width, width);
			base.ModifyDamageHitbox(ref hitbox);
		}
		public void DrawTrail()
		{
			Texture2D texture = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
			Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
			float scaleMult = Projectile.ai[1] / 5f + 0.5f;
			float alphaMult = 1.2f;
			for (int k = 0; k < TrailLength; k++)
			{
				float scale = (TrailLength - k) / (float)TrailLength;
				Vector2 drawPos = Projectile.oldPos[k] + Projectile.Size / 2 - Main.screenPosition;
				Color color = Projectile.GetAlpha(new Color(100, 100, 110, 0)) * scale;
				Main.spriteBatch.Draw(texture, drawPos, null, color * 0.3f * alphaMult, Projectile.rotation, drawOrigin, Projectile.scale * scaleMult * scale, SpriteEffects.None, 0f);
			}
		}
		public override bool PreDraw(ref Color lightColor)
		{
			Texture2D texture = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
			Color color = new Color(100, 100, 110, 0);
			Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
			float scaleMult = Projectile.ai[1] / 8f + 0.75f;
			DrawTrail();
			float alphaMult = 1f;
			for (int k = 0; k < 2; k++)
			{
				Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, null, Projectile.GetAlpha(color) * alphaMult, Projectile.rotation, drawOrigin, Projectile.scale * scaleMult, SpriteEffects.None, 0f);
			}
			return false;
		}
		bool runOnce = true;
		public override void AI()
		{
			if (runOnce)
			{
				DustOut();
				SOTSUtils.PlaySound(SoundID.Item98, (int)Projectile.Center.X, (int)Projectile.Center.Y, 0.6f, -0.4f);
                if (Projectile.ai[0] == 4)
				Projectile.scale = 0.05f;
				Projectile.alpha = 0;
				runOnce = false;
			}
			else if (Projectile.scale < 1f)
				Projectile.scale += 0.05f;
			else 
				Projectile.scale = 1f;
			Projectile.rotation += Projectile.velocity.Length() * 0.05f;
			Projectile.velocity.Y += 0.1f;
			if(Projectile.Center.Y > Projectile.ai[0] && Projectile.timeLeft < 870)
            {
				Projectile.tileCollide = true;
            }
			if(Projectile.timeLeft < 900)
			{
				Dust dust;
				if(Projectile.timeLeft % 15 == 0)
				{
					for (int i = 0; i < 360; i += 20)
					{
						Vector2 circularLocation = new Vector2(24, 0).RotatedBy(MathHelper.ToRadians(i));
						circularLocation.X *= 0.5f;
						circularLocation = circularLocation.RotatedBy(Projectile.velocity.ToRotation());
						dust = Dust.NewDustDirect(new Vector2(Projectile.Center.X + circularLocation.X - 5, Projectile.Center.Y + circularLocation.Y - 5), 0, 0, ModContent.DustType<Dusts.CopyDust4>());
						dust.velocity = -circularLocation * 0.075f;
						dust.velocity -= Projectile.velocity * 0.5f;
						dust.color = ColorHelper.VibrantColorGradient(Main.rand.NextFloat(180, 360), true);
						dust.noGravity = true;
						dust.fadeIn = 0.1f;
						dust.scale = 1.0f;
						dust.alpha = 80;
					}
				}
				dust = Dust.NewDustDirect(new Vector2(Projectile.Center.X - 5, Projectile.Center.Y - 5), 0, 0, ModContent.DustType<Dusts.PixelDust>());
				dust.velocity *= 0.1f;
				dust.velocity -= Projectile.velocity * Main.rand.NextFloat(0.5f, 1f);
				dust.color = ColorHelper.VibrantColorGradient(Main.rand.NextFloat(180, 360), true);
				dust.noGravity = true;
				dust.fadeIn = 12f;
				dust.scale = 1;
			}
		}
        public override bool ShouldUpdatePosition()
        {
			return true;
        }
        public override void OnKill(int timeLeft)
		{
			if(Main.netMode != NetmodeID.MultiplayerClient)
			{
				int direction = Math.Sign(Projectile.velocity.X);
				int start = direction;
				int end = direction;
				if (Projectile.velocity.X == 0)
                {
					start = -1;
					end = 1;
                }
				if(Projectile.ai[1] > 0)
					for (int i = start; i <= end; i += 2)
					{
						Vector2 shootUp = new Vector2(2 * i, -Projectile.ai[1] * 1.25f - 4.25f);
						Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, shootUp, ModContent.ProjectileType<GlowBombOrb>(), Projectile.damage, 1f, Main.myPlayer, Projectile.position.Y, Projectile.ai[1] - 1);
					}
				else
                {
					Projectile.velocity = Vector2.Zero;
					DustOut();
                }
				int extra = 3;
				if (Projectile.ai[1] == 4 && Main.expertMode)
					extra = 6;
				for (int i = -1; i <= 1; i++)
				{
					Vector2 shootUp = new Vector2(0, -4);
					float Ypos = Projectile.position.Y + Projectile.height - 8;
					if (i == 0)
						shootUp = new Vector2(0, -3.9f);
					Projectile.NewProjectile(Projectile.GetSource_FromThis(), new Vector2(16 * i + Projectile.Center.X, Ypos), shootUp, ModContent.ProjectileType<GlowBombShard>(), Projectile.damage, 1f, Main.myPlayer, Ypos, (Projectile.ai[1] * 4 + extra) * i);
				}
			}
		}
		public void DustOut()
		{
			for (int i = 0; i < 360; i += 10)
			{
				Vector2 circularLocation = new Vector2(8 * Main.rand.NextFloat(0.5f, 1f), 0).RotatedBy(MathHelper.ToRadians(i) + Projectile.rotation);
				Dust dust;
				if(!Main.rand.NextBool(3))
				{
					dust = Dust.NewDustDirect(new Vector2(Projectile.Center.X + circularLocation.X - 4, Projectile.Center.Y + circularLocation.Y - 4), 4, 4, ModContent.DustType<Dusts.PixelDust>());
					dust.velocity *= 0.5f;
					dust.velocity += Projectile.velocity * 1.0f * (0.5f + Projectile.ai[1] / 10f) * Main.rand.NextFloat(0.1f, 6f);
					dust.color = ColorHelper.VibrantColorGradient(Main.rand.NextFloat(180, 360), true);
					dust.noGravity = true;
					dust.fadeIn = 8f;
					dust.scale = 1;
				}
                else
                {
					dust = Dust.NewDustDirect(new Vector2(Projectile.Center.X + circularLocation.X - 4, Projectile.Center.Y + circularLocation.Y - 4), 4, 4, ModContent.DustType<Dusts.CopyDust4>());
					dust.velocity = circularLocation * 0.5f;
					dust.velocity += Projectile.velocity * 1.0f * (0.25f + Projectile.ai[1] / 5f);
					dust.color = ColorHelper.VibrantColorGradient(Main.rand.NextFloat(180, 360), true);
					dust.noGravity = true;
					dust.fadeIn = 0.1f;
					dust.scale *= 1.5f;
				}
			}
		}
	}
	public class GlowBombShard : ModProjectile
	{
        public override string Texture => "SOTS/Projectiles/Earth/Glowmoth/IlluminantStaffShard";
		public override void SetDefaults()
		{
			Projectile.penetrate = -1;
			Projectile.friendly = false;
			Projectile.hostile = true;
			Projectile.alpha = 0;
			Projectile.width = 14;
			Projectile.height = 14;
			Projectile.ignoreWater = true;
			Projectile.tileCollide = false;
			Projectile.timeLeft = 50;
			Projectile.alpha = 255;
			Projectile.extraUpdates = 0;
		}
		public override bool PreDraw(ref Color lightColor)
		{
			Texture2D texture = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
			Color color = new Color(140, 140, 150, 0);
			Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
			float scaleMult = 1f;
			float alphaMult = 1f;
			float betweenPositions = Math.Abs(Projectile.ai[0] - Projectile.position.Y);
			float yScale = (betweenPositions - 10) * 0.5f;
			for (int k = 0; k < 3; k++)
			{
				float yOffset = 0;
				Rectangle frame = new Rectangle(0, 0, 10, 10);
				if(k == 1)
                {
					yOffset = -1;
					frame = new Rectangle(10, 0, 2, 10);
				}
				if (k == 2)
				{
					yOffset = betweenPositions;
					frame = new Rectangle(12, 0, 10, 10);
				}
				Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition + new Vector2(0, yOffset), frame, Projectile.GetAlpha(color) * alphaMult, Projectile.rotation, k == 1 ? new Vector2(0, texture.Height * 0.5f) : drawOrigin, Projectile.scale * scaleMult * new Vector2(k == 1 ? yScale : 1, 1), SpriteEffects.None, 0f);
			}
			return false;
		}
		bool runOnce = true;
		public override void AI()
		{
			if (runOnce)
			{
				DustOut();
				//SOTSUtils.PlaySound(SoundID.Item25, (int)Projectile.Center.X, (int)Projectile.Center.Y, 0.5f, -0.2f);
				Projectile.scale = 0.1f;
				Projectile.alpha = 0;
				runOnce = false;
			}
			else if (Projectile.scale < 1f)
				Projectile.scale += 0.25f;
			if(Projectile.scale > 1)
				Projectile.scale = 1f;
			Projectile.rotation = MathHelper.PiOver2;
			Projectile.velocity.Y += 0.15f;
			if(Projectile.timeLeft == 44)
			{
				if (Main.netMode != NetmodeID.MultiplayerClient && Projectile.ai[1] != 0)
				{
					int direction = Math.Sign(Projectile.ai[1]);
					Projectile.NewProjectile(Projectile.GetSource_FromThis(), new Vector2(direction * 16 + Projectile.Center.X, Projectile.ai[0]), new Vector2(0, -4), ModContent.ProjectileType<GlowBombShard>(), Projectile.damage, 1f, Main.myPlayer, Projectile.ai[0], Projectile.ai[1] - direction);
				}
			}
			if(Projectile.timeLeft <= 13)
            {
				Projectile.alpha += 20;
				if(Projectile.alpha > 100)
					Projectile.hostile = false;
            }
		}
		public override bool ShouldUpdatePosition()
		{
			return true;
		}
		public void DustOut()
		{
			for (int i = 0; i < 16; i++)
			{
				Dust dust;
				if (i % 5 != 0)
				{
					dust = Dust.NewDustDirect(new Vector2(Projectile.Center.X - 5, Projectile.Center.Y - 5), 0, 0, ModContent.DustType<Dusts.PixelDust>());
					dust.velocity *= 0.6f;
					dust.velocity += new Vector2(0, -Main.rand.NextFloat(1, 10));
					dust.color = ColorHelper.VibrantColorGradient(Main.rand.NextFloat(180, 360), true);
					dust.noGravity = true;
					dust.fadeIn = 8f;
					dust.scale = 1;
				}
				else
				{
					dust = Dust.NewDustDirect(new Vector2(Projectile.Center.X - 5, Projectile.Center.Y - 5), 0, 0, ModContent.DustType<Dusts.CopyDust4>());
					dust.velocity *= 0.75f;
					dust.color = ColorHelper.VibrantColorGradient(Main.rand.NextFloat(180, 360), true);
					dust.noGravity = true;
					dust.fadeIn = 0.1f;
					dust.scale *= 1.15f;
				}
			}
		}
	}
	public class GlowSparkle : ModProjectile
	{
		public const int TrailLength = 20;
		public override void SetStaticDefaults()
		{
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = TrailLength;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
		}
		public override void SetDefaults()
		{
			Projectile.penetrate = -1;
			Projectile.friendly = false;
			Projectile.hostile = false;
			Projectile.alpha = 0;
			Projectile.width = 34;
			Projectile.height = 34;
			Projectile.ignoreWater = true;
			Projectile.tileCollide = false;
			Projectile.timeLeft = 900;
			Projectile.alpha = 0;
			Projectile.extraUpdates = 0;
		}
		public override void ModifyDamageHitbox(ref Rectangle hitbox)
		{
			int width = 32;
			hitbox = new Rectangle((int)Projectile.Center.X - width / 2, (int)Projectile.Center.Y - width / 2, width, width);
		}
		public void DrawTrail()
		{
			Texture2D texture2 = ModContent.Request<Texture2D>("SOTS/Projectiles/Earth/Glowmoth/MothMinionTrail").Value;
			Vector2 trailOrigin = new Vector2(texture2.Width - 6, texture2.Height * 0.5f);
			for (int k = 1; k < TrailLength - 1; k++)
			{
				if (Projectile.oldPos[k + 1].Distance(Projectile.position) > 160)
				{
					break;
				}
				for (int j = 0; j < 2; j++)
				{
					float scaleMult = ((Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length) * (1.0f - j * 0.2f);
					Vector2 toNextPosition = Projectile.oldPos[k] - Projectile.oldPos[k + 1];
					Vector2 drawPos = Projectile.oldPos[k] + Projectile.Size / 2 - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY);
					Color color = new Color(60, 80, 70, 0) * scaleMult * 0.5f;
					Main.EntitySpriteDraw(texture2, drawPos + toNextPosition.SafeNormalize(Vector2.Zero) * 3 * j, null, color, toNextPosition.ToRotation(), trailOrigin, new Vector2(toNextPosition.Length() / texture2.Width * 2, Projectile.scale * (1.0f) * scaleMult), SpriteEffects.None, 0);
				}
			}
		}
		public override bool PreDraw(ref Color lightColor)
		{
			Texture2D texture = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
			Color color = new Color(100, 110, 105, 0);
			Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
			float sinusoid = (float)Math.Sin(Math.Sqrt(Projectile.ai[1] / 60f) * MathHelper.Pi);
			float scaleMult = 1.25f * sinusoid;
			DrawTrail();
			float alphaMult = 1f * sinusoid;
			if (Projectile.ai[0] != -1)
				alphaMult = 0;
			for (int k = 0; k < 3; k++)
			{
				float scale2 = 1f + k * 0.1f;
				float alpha2 = 1.25f - 0.6f * k;
				Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, null, Projectile.GetAlpha(color) * alphaMult * alpha2, Projectile.rotation, drawOrigin, Projectile.scale * scaleMult * scale2, SpriteEffects.None, 0f);
			}
			return false;
		}
		public override void AI()
		{
			Vector2 destination = new Vector2(Projectile.ai[0], Projectile.ai[1]);
            if (Projectile.ai[0] != -1)
			{
				Vector2 toDestination = destination - Projectile.Center;
				float distTo = toDestination.Length();
				toDestination = toDestination.SafeNormalize(Vector2.Zero) * (4.5f + distTo / 40f);
				Projectile.velocity = Vector2.Lerp(Projectile.velocity, toDestination, 0.06f);
				Projectile.rotation = Projectile.velocity.ToRotation();
				if (distTo < 8)
				{
					Projectile.velocity *= 0.0f;
					Projectile.ai[0] = -1;
					Projectile.ai[1] = 0;
					if (Main.netMode == NetmodeID.Server)
						Projectile.netUpdate = true;
					Projectile.hostile = true;
				}
				else if(Main.rand.NextBool(20))
				{
					Vector2 outward = Main.rand.NextVector2CircularEdge(32, 32) * (0.08f + distTo / 720f);
					Dust dust = Dust.NewDustDirect(destination - new Vector2(5, 5) + outward, 0, 0, ModContent.DustType<Dusts.PixelDust>());
					dust.velocity *= distTo / 640f;
					dust.velocity -= outward * 0.1f;
					dust.color = ColorHelper.VibrantColorGradient(Main.rand.NextFloat(180), true) * 0.5f;
					dust.noGravity = true;
					dust.fadeIn = 8f;
					dust.scale = 1.5f;
				}
			}
			else
            {
                if(Projectile.ai[1] <= 0)
				{
					SOTSUtils.PlaySound(SoundID.Item30, (int)Projectile.Center.X, (int)Projectile.Center.Y, 0.4f, -0.1f);
					StarDust(Projectile.Center);
					DustOut();
				}
				Projectile.ai[1]++;
				if (Projectile.ai[1] > 23)
					Projectile.hostile = false;
				if (Projectile.ai[1] > 60)
					Projectile.Kill();
            }
		}
		public void StarDust(Vector2 center)
		{
			Vector2 startingLocation;
			float degrees = Main.rand.NextFloat(360);
			for (int j = 0; j < 5; j++)
			{
				Vector2 offset = new Vector2(0, 5).RotatedBy(MathHelper.ToRadians(j * 90) + Projectile.rotation);
				for (int i = -4; i <= 4; i++)
				{
					degrees += 360f / 28f;
					startingLocation = new Vector2(i, 10 - Math.Abs(i) * 2).RotatedBy(MathHelper.ToRadians(j * 90) + Projectile.rotation);
					Vector2 velo = offset + startingLocation;
					Dust dust = Dust.NewDustPerfect(center + velo * 0.4f, ModContent.DustType<CopyDust4>());
					dust.noGravity = true;
					dust.velocity *= 0.3f;
					dust.scale = 1.4f;
					dust.fadeIn = 0.1f;
					dust.alpha = 100;
					dust.color = ColorHelper.VibrantColorGradient(Main.rand.NextFloat(180), true);
					dust.velocity += velo * 0.15f;
				}
			}
		}
		public override bool ShouldUpdatePosition()
		{
			return true;
		}
		public override void OnKill(int timeLeft)
		{

		}
		public void DustOut()
		{
			for (int i = 0; i < 360; i += 40)
			{
				Vector2 circularLocation = new Vector2(8 * Main.rand.NextFloat(0.5f, 1f), 0).RotatedBy(MathHelper.ToRadians(i) + Projectile.rotation);
				Dust dust = Dust.NewDustDirect(new Vector2(Projectile.Center.X + circularLocation.X - 4, Projectile.Center.Y + circularLocation.Y - 4), 4, 4, ModContent.DustType<Dusts.PixelDust>());
				dust.velocity *= 0.2f;
				dust.velocity += circularLocation * Main.rand.NextFloat(0.5f, 1f);
				dust.color = ColorHelper.VibrantColorGradient(Main.rand.NextFloat(180), true);
				dust.noGravity = true;
				dust.fadeIn = 9f;
				dust.scale = 2;
			}
		}
	}
}