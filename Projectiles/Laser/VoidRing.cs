using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Dusts;
using SOTS.Projectiles.Blades;
using SOTS.Void;
using System;
using Terraria;
using Terraria.ModLoader;

namespace SOTS.Projectiles.Laser
{
	public class VoidRing : ModProjectile
	{
		public override void SetDefaults()
		{
			Projectile.width = 96;
			Projectile.height = 96;
			Projectile.friendly = true;
			Projectile.DamageType = ModContent.GetInstance<VoidMelee>();
			Projectile.timeLeft = 60;
			Projectile.tileCollide = false;
			Projectile.penetrate = -1;
			Projectile.localNPCHitCooldown = 15;
			Projectile.usesLocalNPCImmunity = true;
		}
        public override void ModifyDamageHitbox(ref Rectangle hitbox)
        {
			if (Projectile.ai[2] == -1)
			{
                hitbox.X -= 4;
				hitbox.Y -= 4;
				hitbox.Width += 8;
                hitbox.Height += 8;
			}
        }
		public override bool PreDraw(ref Color lightColor)
		{
			Texture2D texture = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
			Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
            if (Projectile.ai[2] == -1)
            {
                texture = SOTSUtils.WhitePixel;
				drawOrigin = Vector2.UnitY;
				float p1 = Projectile.timeLeft / 60f;
                float p = MathF.Sqrt(p1) * 0.7f + 0.3f * p1;
                Color c1 = KingSlash.Red * p;
                Color c2 = KingSlash.Blue * p;
				Color c3 = KingSlash.SecondBlue * p;
                for (int j = -1; j <= 1; j += 2)
                {
					Vector2 previous = Projectile.Center;
					float size = j == -1 ? 12 : 10;
					for (int k = -1; k < 120; k++)
					{
						float percent = k / 80f;
						Color c = Color.Lerp(c1, c2, MathF.Sin(percent * MathF.PI * 8) * 0.5f + 0.5f) * 2f;
                        float cos = MathF.Cos(MathHelper.ToRadians(k * 24)) * size * j * p;
						Vector2 circularPos = new Vector2(Projectile.ai[0] * 2 + cos, 0).RotatedBy(MathHelper.ToRadians(k * 3f + SOTSWorld.GlobalCounter * 1.5f * j)) * p;
						if(k != -1)
						{
                            Vector2 toPrev = previous - circularPos;
                            Vector2 drawPos = Projectile.Center + circularPos - Main.screenPosition;
                            Main.spriteBatch.Draw(texture, drawPos, null, c, toPrev.ToRotation(), drawOrigin, new Vector2(toPrev.Length() / texture.Width * 1.2f, 3), SpriteEffects.None, 0f);
                            Main.spriteBatch.Draw(texture, drawPos, null, c3, toPrev.ToRotation(), drawOrigin, new Vector2(toPrev.Length() / texture.Width, 1), SpriteEffects.None, 0f);
                        }
                        previous = circularPos;
                    }
                }
                return false;
            }
            Color color = Color.Black;
            color = Projectile.GetAlpha(color) * 0.1667f;
            for (int k = 0; k < 120; k++)
            {
                float circularLength = MathF.Cos(MathHelper.ToRadians(k * 18)) * 4;
				Vector2 circularPos = new Vector2(Projectile.ai[0] * 2 + circularLength, 0).RotatedBy(MathHelper.ToRadians(k * 3) + Projectile.rotation);
                Vector2 drawPos = Projectile.Center + circularPos - Main.screenPosition;
				for (int j = 0; j < 3; j++)
				{
					float x = Main.rand.Next(-10, 11) * 0.1f;
					float y = Main.rand.Next(-10, 11) * 0.1f;
					Main.spriteBatch.Draw(texture, drawPos + new Vector2(x, y), null, color, Projectile.rotation, drawOrigin, 1f, SpriteEffects.None, 0f);
				}
			}
			return false;
		}
		public override void AI()
		{
			if (Projectile.ai[2] == -1)
			{
				if (Projectile.ai[0] <= 0)
				{
					for(int i = 0; i < 20; i++)
                    {
                        Dust dust = PixelDust.Spawn(Projectile.Center, 0, 0, Main.rand.NextVector2Circular(11f, 11f),
                        Color.Lerp(KingSlash.Red, KingSlash.Blue, Main.rand.NextFloat(0.9f) * Main.rand.NextFloat(0.9f)), 3);
						dust.scale = Main.rand.NextFloat(1.5f, 2.5f);
                    }
                }
				Projectile.ai[0] = MathHelper.Lerp(Projectile.ai[0], 32, 0.14f);
				return;
			}
			Lighting.AddLight(Projectile.Center, 1f, 0.4f, 0.4f);
			Projectile.rotation += MathHelper.ToRadians(2);
			Projectile.alpha += 4;
			if (Projectile.timeLeft < 12)
			{
				Projectile.ai[0] -= 2;
				if(Projectile.ai[0] <= 0)
				{
					Projectile.Kill();
				}
			}
			else if (Projectile.ai[1] == 0)
			{
				Projectile.ai[0]++;
				if(Projectile.ai[0] >= 22)
				{
					Projectile.ai[1] = 1;
				}
			}
			if (Projectile.ai[1] == 1)
			{
				Projectile.ai[0]--;
				if (Projectile.ai[0] <= 16)
				{
					Projectile.ai[1] = 0;
				}
			}
		}
	}
}