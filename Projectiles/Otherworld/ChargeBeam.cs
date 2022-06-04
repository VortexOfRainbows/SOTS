using log4net.Util;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Projectiles.Base;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Projectiles.Otherworld
{
	public class ChargeBeam : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Charge Ball");
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 5;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 1;
		}
		public override void SetDefaults()
		{
			Projectile.width = 38;
			Projectile.height = 38;
			Projectile.hostile = true;
			Projectile.friendly = false;
			Projectile.timeLeft = 300;
			Projectile.tileCollide = false;
			Projectile.penetrate = -1;
			Projectile.scale = 1f;
			Projectile.extraUpdates = 1;
			Projectile.alpha = 255;
		}
		public override void ModifyDamageHitbox(ref Rectangle hitbox)
		{
			float width = Projectile.width * Projectile.scale;
			float height = Projectile.width * Projectile.scale;
			width += 2;
			height += 2;
			hitbox = new Rectangle((int)(Projectile.Center.X - width/2), (int)(Projectile.Center.Y - height/2), (int)width, (int)height);
		}
		public override bool PreDraw(ref Color lightColor)
		{
			Texture2D texture = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
			Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
			for (int k = 0; k < Projectile.oldPos.Length; k++)
			{
				Color color = new Color(110, 110, 110, 0);
				Vector2 drawPos = Projectile.oldPos[k] - Main.screenPosition + drawOrigin;
				color = Projectile.GetAlpha(color) * ((Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length) * 0.5f;
				for (int j = 0; j < 5; j++)
				{
					float length = Main.rand.Next(-10, 11) * 0.3f * (1 + Projectile.scale) * (1 + Projectile.scale);
					Vector2 xy = new Vector2(length, 0).RotatedBy(MathHelper.ToRadians(Main.rand.Next(360)));
					Main.spriteBatch.Draw(texture, drawPos + new Vector2(xy.X, xy.Y), null, color, Projectile.rotation, drawOrigin, Projectile.scale * (Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length, SpriteEffects.None, 0f);
				}
			}
			return false;
		}
		public override void PostDraw(Color lightColor)
		{
			Texture2D texture = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
			Color color = new Color(110, 110, 110, 0);
			Vector2 drawOrigin = new Vector2(Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value.Width * 0.5f, Projectile.height * 0.5f);
			for (int k = 0; k < 6; k++)
			{
				float x = Main.rand.Next(-10, 11) * 0.1f;
				float y = Main.rand.Next(-10, 11) * 0.1f;
				Main.spriteBatch.Draw(texture, new Vector2((float)(Projectile.Center.X - (int)Main.screenPosition.X) + x, (float)(Projectile.Center.Y - (int)Main.screenPosition.Y) + y), null, color * (1f - (Projectile.alpha / 255f)), Projectile.rotation, drawOrigin, Projectile.scale, SpriteEffects.None, 0f);
			}
			base.PostDraw(spriteBatch, drawColor);
		}
        public override bool ShouldUpdatePosition()
        {
			return false;
        }
		bool runOnce = true;
        public override void AI()
		{
			if(runOnce)
            {
				runOnce = false;
				Projectile.scale = 0;
				Projectile.alpha = 0;
            }
			Lighting.AddLight(Projectile.Center, 0.25f, 0.45f, 0.75f);
			if(Projectile.timeLeft > 200)
            {
				if(Projectile.timeLeft % 20 == 0)
				{
					Terraria.Audio.SoundEngine.PlaySound(2, (int)Projectile.Center.X, (int)Projectile.Center.Y, 15, 0.7f);
					for (int k = 0; k < 360; k += 10)
					{
						Vector2 circularLocation = new Vector2(-38 * Projectile.scale, 0).RotatedBy(MathHelper.ToRadians(k));
						circularLocation += 0.5f * new Vector2(Main.rand.Next(-1, 2), Main.rand.Next(-1, 2));
						int type = DustID.Electric;
						int num1 = Dust.NewDust(new Vector2(Projectile.Center.X + circularLocation.X - 4, Projectile.Center.Y + circularLocation.Y - 4), 4, 4, type);
						Main.dust[num1].noGravity = true;
						Main.dust[num1].scale *= 1f + 0.166f * Projectile.scale;
						Main.dust[num1].velocity = -circularLocation * 0.07f;
					}
				}
				Projectile.scale += 0.015f;
            }
			else
            {
				if(Projectile.timeLeft == 200)
				{
					Terraria.Audio.SoundEngine.PlaySound(2, (int)(Projectile.Center.X), (int)(Projectile.Center.Y), 94);
					if (Main.netMode != 1)
                    {
						int numberProjectiles = 3;
						for (int i = 0; i < numberProjectiles; i++)
						{
							Vector2 perturbedSpeed = new Vector2(Projectile.velocity.X, Projectile.velocity.Y).RotatedBy(MathHelper.ToRadians((i - 1f) * 3f));
							Projectile.NewProjectile(Projectile.Center.X, Projectile.Center.Y, perturbedSpeed.X, perturbedSpeed.Y, Mod.Find<ModProjectile>("SpiralDeathBeam").Type, Projectile.damage, 1f, Main.myPlayer, (i * 360f / numberProjectiles));
						}
					}
                }
				Projectile.scale -= 0.0075f;
            }
		}
	}
}