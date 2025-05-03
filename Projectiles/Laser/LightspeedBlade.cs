using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Dusts;
using SOTS.Items.Planetarium.Furniture;
using SOTS.Projectiles.Blades;
using SOTS.Projectiles.Permafrost;
using SOTS.Void;
using System;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Projectiles.Laser
{
	public class LightspeedBlade : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 240;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
		}
		public override void SetDefaults()
		{
			Projectile.width = 48;
			Projectile.height = 48;
			Projectile.friendly = true;
			Projectile.DamageType = ModContent.GetInstance<VoidMelee>();
			Projectile.extraUpdates = 16;
			Projectile.timeLeft = 6000;
			Projectile.tileCollide = false;
			Projectile.penetrate = -1;
		}
		public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
		{
			width = 16;
			height = 16;
			return true;
		}
		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			PreKillUpdate();
			return false;
		}
		public override bool PreDraw(ref Color lightColor)
		{
			Texture2D texture = Mod.Assets.Request<Texture2D>("Projectiles/Laser/LightspeedTrail").Value;
			Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
			for (int k = 0; k < Projectile.oldPos.Length; k++)
			{
				Color color = Color.Black;
				Vector2 drawPos = Projectile.oldPos[k] - Main.screenPosition + drawOrigin;
				color = Projectile.GetAlpha(color) * ((Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length) * 0.5f;
				for (int j = 0; j < 7; j++)
				{
					float x = Main.rand.Next(-10, 11) * 0.15f;
					float y = Main.rand.Next(-10, 11) * 0.15f;
					if(!Projectile.oldPos[k].Equals(Projectile.position))
					{
						Main.spriteBatch.Draw(texture, drawPos + new Vector2(x, y), null, color, Projectile.rotation, drawOrigin, (Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length, SpriteEffects.None, 0f);
					}
				}
			}
			return false;
		}
		public override void PostDraw(Color lightColor)
		{
			Texture2D texture = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
			Color color = Color.Black;
			Vector2 drawOrigin = new Vector2(Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value.Width * 0.5f, Projectile.height * 0.5f);
			for (int k = 0; k < 7; k++)
			{
				float x = Main.rand.Next(-10, 11) * 0.15f;
				float y = Main.rand.Next(-10, 11) * 0.15f;
				if (Projectile.ai[0] != 1)
					Main.spriteBatch.Draw(texture, new Vector2((float)(Projectile.Center.X - (int)Main.screenPosition.X) + x, (float)(Projectile.Center.Y - (int)Main.screenPosition.Y) + y), null, color * (1f - (Projectile.alpha / 255f)), Projectile.rotation, drawOrigin, 1f, SpriteEffects.None, 0f);
			}
		}
		private int inititate = 0;
		public override void AI()
		{
			Player player = Main.player[Projectile.owner];
			if(this is not LightspeedKingblade)
				Lighting.AddLight(Projectile.Center, 1f, 0.4f, 0.4f);
			else
				Lighting.AddLight(Projectile.Center, 0.6f, 0.3f, 0.5f);
			if (inititate == 0)
            {
				inititate++;
				SOTSUtils.PlaySound(SoundID.Item60, player.Center);
			}
			if(!Projectile.velocity.Equals(new Vector2(0, 0)))
				Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.ToRadians(45);

			if(Projectile.velocity.X > 0)
			{
				if(Projectile.Center.X > Projectile.ai[1])
				{
					Projectile.tileCollide = true;
				}
			}
			if (Projectile.velocity.X < 0)
			{
				if (Projectile.Center.X < Projectile.ai[1])
				{
					Projectile.tileCollide = true;
				}
			}

			if (Projectile.ai[0] == 1)
			{
				Projectile.alpha += 2;
				Projectile.friendly = false;
				if (Projectile.alpha >= 255)
				{
					Projectile.Kill();
				}
			}
		}
		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			target.immune[Projectile.owner] = 0;
			PreKillUpdate();
		}
		public void PreKillUpdate()
		{
			Projectile.ai[0] = 1;
			Projectile.velocity *= 0;
			Projectile.friendly = false;
			if (Projectile.owner == Main.myPlayer)
			{
				Projectile.netUpdate = true;
				Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center.X, Projectile.Center.Y, 0, 0, ModContent.ProjectileType<VoidRing>(), Projectile.damage, 0, Main.myPlayer, 0, 0, this is LightspeedKingblade ? -1 : 0);
			}
		}
	}
	public class LightspeedKingblade : LightspeedBlade
	{
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 150;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
        }
        public override void SetDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 150;
            base.SetDefaults();
            Projectile.width = Projectile.height = 74;
			Projectile.extraUpdates -= 4;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = SOTSUtils.WhitePixel;
            Vector2 drawOrigin = new Vector2(0, texture.Height * 0.5f);
            Color c1 = KingSlash.Red;
            Color c2 = KingSlash.Blue;
            Color c3 = KingSlash.SecondBlue;
            for (int j = -1; j <= 1; j += 2)
            {
                Vector2 previous = Projectile.Center;
                for (int k = 0; k < Projectile.oldPos.Length; k++)
                {
                    if (Projectile.oldPos[k] == Vector2.Zero || Projectile.oldPos[k] == Projectile.position)
                        continue;
                    float percent =(Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length;
                    Color color2 = Color.Lerp(c1, c2, 0.5f + 0.5f * MathF.Sin(percent * MathF.PI * 5) );
					percent *= MathF.Min(1, k / 9f);
                    Vector2 drawPos = Projectile.oldPos[k] + Projectile.Size / 2;
                    drawPos += new Vector2(0, MathF.Sin(MathHelper.ToRadians(k * 12)) * 12 * percent * j).RotatedBy(Projectile.oldRot[k]);
                    Vector2 toPrev = previous - drawPos;
                    Main.spriteBatch.Draw(texture, drawPos - Main.screenPosition, null, color2 * percent, toPrev.ToRotation(), drawOrigin, new Vector2(toPrev.Length() / texture.Width, percent * 4.5f), SpriteEffects.None, 0f);
                    Main.spriteBatch.Draw(texture, drawPos - Main.screenPosition, null, c3 * percent, toPrev.ToRotation(), drawOrigin, new Vector2(toPrev.Length() / texture.Width, percent * 1.5f), SpriteEffects.None, 0f);
                    previous = drawPos;
                }
            }
            return false;
        }
        public override void PostDraw(Color lightColor)
        {
			if (Projectile.ai[0] == 1 || Projectile.timeLeft >= 6000 - Projectile.extraUpdates)
				return;
			Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
			Color color = new Color(100, 100, 100, 0);
            Vector2 drawOrigin = new Vector2(TextureAssets.Projectile[Projectile.type].Value.Width * 0.5f, Projectile.height * 0.5f);
			Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, null, Color.White * (1f - (Projectile.alpha / 255f)), Projectile.rotation, drawOrigin, 1f, SpriteEffects.None, 0f);
            for (int k = 0; k < 6; k++)
            {
				Vector2 circular = new Vector2(4, 0).RotatedBy(MathHelper.ToRadians(SOTSWorld.GlobalCounter * 2.5f + k * 60));
				Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition + circular, null, color * (1f - (Projectile.alpha / 255f)) * 0.5f, Projectile.rotation, drawOrigin, 1f, SpriteEffects.None, 0f);
            }
        }
        public override void PostAI()
        {
            if (Projectile.ai[0] == 1 || Projectile.timeLeft >= 6000 - Projectile.extraUpdates)
                return;
            float percent = Main.rand.NextFloat(0.9f) * Main.rand.NextFloat();
			if(percent > 0.1f)
            {
                Dust dust = PixelDust.Spawn(Projectile.Center - Projectile.velocity.SafeNormalize(Vector2.Zero) * 28, 0, 0, Main.rand.NextVector2Circular(0.7f, 0.7f) - Projectile.velocity * 0.1f,
                Color.Lerp(KingSlash.Red, KingSlash.Blue, Main.rand.NextFloat(0.9f) * Main.rand.NextFloat(0.9f)), 4);
                dust.scale = Main.rand.NextFloat(1.0f, 1.5f);
            }
        }
    }
}