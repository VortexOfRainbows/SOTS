using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Projectiles.Celestial;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Projectiles.Inferno
{
	public class LesserWispLaser : ModProjectile
	{
		public override void SetStaticDefaults() 
		{
			DisplayName.SetDefault("Lesser Wisp Laser");
		}
		public override void SetDefaults() 
		{
			Projectile.width = 16;
			Projectile.height = 16;
			Projectile.timeLeft = 180;
			Projectile.DamageType = DamageClass.Magic;
			Projectile.penetrate = -1;
			Projectile.hostile = false;
			Projectile.friendly = false;
			Projectile.tileCollide = false;
			Projectile.ignoreWater = true;
			Projectile.alpha = 0;
		}
		List<Vector2> posList = new List<Vector2>();
		List<FireParticle> particleList = new List<FireParticle>();
		public void cataloguePos()
		{
			for (int i = 0; i < particleList.Count; i++)
			{
				FireParticle particle = particleList[i];
				particle.Update();
				if (!particle.active)
				{
					particleList.RemoveAt(i);
					i--;
				}
			}
		}
		public void Laser()
        {
			Vector2 current = Projectile.Center;
			Vector2 velo = Projectile.velocity.SafeNormalize(Vector2.Zero);
			for(int i = 0; i <= 360; i++)
            {
				float scaleMult = 0.4f + 0.9f * ((360 - i) / 360f);
				current += velo * 1.25f;
				posList.Add(current);
				if(Main.netMode != NetmodeID.Server)
				{
					for (int j = 0; j < (SOTS.Config.lowFidelityMode ? 1 : 1 + Main.rand.Next(2)); j++)
					{
						Vector2 rotational = new Vector2(0, -Main.rand.NextFloat(0.5f + j * 1.75f + 0.5f * scaleMult)).RotatedBy(MathHelper.ToRadians(Main.rand.NextFloat(360f)));
						particleList.Add(new FireParticle(current, rotational, Main.rand.NextFloat(-3f, 3f), Main.rand.NextFloat(-2f, 2f), Main.rand.NextFloat(0.8f, 1.3f) * (1f - j * 0.25f) * scaleMult));
					}
				}
				int x = (int)current.X / 16;
				int y = (int)current.Y / 16;
				Tile tile = Framing.GetTileSafely(x, y);
				if (!WorldGen.InWorld(x, y, 20) || (tile.HasTile && !Main.tileSolidTop[tile.TileType] && Main.tileSolid[tile.TileType]))
				{
					break;
				}
			}
        }
        public override bool PreAI()
        {
			if(Projectile.ai[1] == -1)
            {
				sans = true;
            }
			else
            {
				sans = false;
            }
            return base.PreAI();
        }
        bool sans = false;
		bool runOnce = false;
		int counter = 0;
		public override void AI() 
		{
			Projectile.rotation += MathHelper.ToRadians(8);
			if(Projectile.ai[0] == 0)
			{
				Terraria.Audio.SoundEngine.PlaySound(2, (int)(Projectile.Center.X), (int)(Projectile.Center.Y), 15, 1f, 0.25f);
			}
			if(Projectile.ai[0] == 39)
			{
				if (Main.netMode != NetmodeID.Server)
				{
					for (int j = 0; j < (SOTS.Config.lowFidelityMode ? 24 : 36); j++)
					{
						Vector2 rotational = new Vector2(0, -Main.rand.NextFloat(1.5f, 4)).RotatedBy(MathHelper.ToRadians(Main.rand.NextFloat(360f)));
						particleList.Add(new FireParticle(Projectile.Center, rotational, Main.rand.NextFloat(-3f, 3f), Main.rand.NextFloat(-2f, 2f), Main.rand.NextFloat(1f, 1.5f)));
					}
				}
				runOnce = true;
			}
			if (Projectile.ai[0] < 40)
				Projectile.ai[0]++;
			else
				counter++;
			if (runOnce)
			{
				Terraria.Audio.SoundEngine.PlaySound(2, (int)Projectile.Center.X, (int)Projectile.Center.Y, 94, 0.75f, 0.4f);
				Laser();
				runOnce = false;
				//Projectile.friendly = true;
				Projectile.hostile = true;
            }
			if(counter >= 5)
			{
				//Projectile.friendly = false;
				Projectile.hostile = false;
			}
			Lighting.AddLight(Projectile.Center, (255 - Projectile.alpha) * 0.8f / 255f, (255 - Projectile.alpha) * 0.8f / 255f, (255 - Projectile.alpha) * 0.8f / 255f);
			cataloguePos();
		}
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.immune[Projectile.owner] = 6;
        }
		public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox) 
		{
			if(Projectile.friendly || Projectile.hostile)
				for(int i = 0; i < posList.Count; i += 8)
				{
					Rectangle rect = new Rectangle((int)posList[i].X - 4, (int)posList[i].Y - 4, 8, 8);
					if(targetHitbox.Intersects(rect))
					{
						return true;
					}
				}
			return false;
		}
        public override bool ShouldUpdatePosition()
        {
			return false;
        }
        public override bool PreDraw(ref Color lightColor)
		{
			Texture2D texture = (Texture2D)ModContent.Request<Texture2D>("SOTS/Projectiles/Celestial/SubspaceLingeringFlame");
			Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
			Color color;
			for (int i = 0; i < particleList.Count; i++)
			{
				color = sans ? new Color(30, 60, 225, 0) : new Color(235, 60, 0, 0);
				Vector2 drawPos = particleList[i].position - Main.screenPosition;
				color = Projectile.GetAlpha(color) * (0.35f + 0.65f * particleList[i].scale);
				for (int j = 0; j < 2; j++)
				{
					float x = Main.rand.NextFloat(-0.5f, 0.5f);
					float y = Main.rand.NextFloat(-0.5f, 0.5f);
					Main.spriteBatch.Draw(texture, drawPos + new Vector2(x, y), null, color, particleList[i].rotation, drawOrigin, particleList[i].scale * 1.1f, SpriteEffects.None, 0f);
				}
			}
			if(Projectile.ai[0] < 40)
			{
				Texture2D texture2 = sans ? (Texture2D)ModContent.Request<Texture2D>("SOTS/Projectiles/Inferno/SansIndicator") : (Texture2D)ModContent.Request<Texture2D>("SOTS/Projectiles/Inferno/WispIndicator");
				Vector2 drawOrigin2 = new Vector2(0, 2);
				for(int j = 0; j < 450; j++)
				{
					Vector2 offset = Projectile.velocity.SafeNormalize(Vector2.Zero) * j;
					float alphaMult = 0.1f + ((455 - j) / 455f);
					for (int i = -1; i < 2; i += 2)
					{
						Vector2 circular = new Vector2(0, i * 0.25f * (40 - Projectile.ai[0])).RotatedBy(Projectile.velocity.ToRotation());
						Main.spriteBatch.Draw(texture2, Projectile.Center + offset + circular - Main.screenPosition, new Rectangle(0, 0, 2, 4), (sans ? new Color(150, 150, 255) : new Color(255, 100, 100)) * (Projectile.ai[0] / 40f) * alphaMult, Projectile.velocity.ToRotation(), drawOrigin2, 0.5f, SpriteEffects.None, 0f);
					}
					int x = (int)(Projectile.Center.X + offset.X) / 16;
					int y = (int)(Projectile.Center.Y + offset.Y) / 16;
					Tile tile = Framing.GetTileSafely(x, y);
					if (!WorldGen.InWorld(x, y, 20) || (tile.HasTile && !Main.tileSolidTop[tile.TileType] && Main.tileSolid[tile.TileType]))
					{
						break;
					}
				}
				texture = sans ? (Texture2D)ModContent.Request<Texture2D>("SOTS/Projectiles/Inferno/SansBones") : Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
				drawOrigin = new Vector2(texture.Width / 2, texture.Height / 2);
				float scale = sans ? 0.75f : 1f;
				for (int j = 0; j < 3; j++)
				{
					for (int i = 0; i < 4; i++)
					{
						float x = Main.rand.NextFloat(-1f, 1f);
						float y = Main.rand.NextFloat(-1f, 1f);
						Main.spriteBatch.Draw(texture, Projectile.Center + new Vector2(x, y) - Main.screenPosition, null, sans ? new Color(100, 100, 175, 0) : new Color(150, 100, 100, 0), Projectile.rotation * (1 + j * 0.33f), drawOrigin, scale * (Projectile.scale * 1.25f + Projectile.ai[0] * 0.01f) * (sans ? (1.2f - j * 0.4f) : (1 - j * 0.33f)), SpriteEffects.None, 0f);
					}
				}
			}
			return false;
		}
        public override void PostDraw(Color lightColor)
        {
			if(sans && Projectile.ai[0] < 40)
			{
				Texture2D texture = (Texture2D)ModContent.Request<Texture2D>("SOTS/Projectiles/Inferno/BlasfartsGreatestCreation");
				Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
				Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, null, Color.White, Projectile.velocity.ToRotation() - MathHelper.ToRadians(90), drawOrigin, 1.15f, SpriteEffects.None, 0f);
			}
			base.PostDraw(spriteBatch, lightColor);
        }
    }
}