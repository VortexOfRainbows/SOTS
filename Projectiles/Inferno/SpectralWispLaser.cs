using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Projectiles.Celestial;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Projectiles.Inferno
{
	public class SpectralWispLaser : ModProjectile
	{
		public override void SetStaticDefaults() 
		{
			DisplayName.SetDefault("Spectral Wisp Laser");
		}
		public override void SetDefaults() 
		{
			Projectile.width = 16;
			Projectile.height = 16;
			Projectile.timeLeft = 150;
			Projectile.penetrate = -1;
			Projectile.hostile = false;
			Projectile.friendly = false;
			Projectile.tileCollide = false;
			Projectile.ignoreWater = true;
			Projectile.alpha = 0;
			Projectile.extraUpdates = 1;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 30;
			Projectile.minion = false;
			Projectile.DamageType = ModContent.GetInstance<Void.VoidSummon>();
		}
		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			Projectile.localNPCImmunity[target.whoAmI] = Projectile.localNPCHitCooldown;
			target.immune[Projectile.owner] = 0;
		}
		bool green = false;
        public override bool PreAI()
        {
			if(Projectile.ai[0] == -1)
			{
				Projectile.ai[0] = 29;
				SOTSUtils.PlaySound(SoundID.Item15, (int)Projectile.Center.X, (int)Projectile.Center.Y, 1f, 0.25f);
				green = true;
            }
            return base.PreAI();
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
		bool runOnce = false;
		int counter = 0;
		public override void AI() 
		{
			Projectile.rotation += MathHelper.ToRadians(8);
			if(Projectile.ai[0] == 0)
			{
				SOTSUtils.PlaySound(SoundID.Item15, (int)Projectile.Center.X, (int)Projectile.Center.Y, 1f, 0.25f);
			}
			if(Projectile.ai[0] == 29)
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
			if (Projectile.ai[0] < 30)
				Projectile.ai[0]++;
			else
				counter++;
			if (runOnce)
			{
				SOTSUtils.PlaySound(SoundID.Item94, (int)Projectile.Center.X, (int)Projectile.Center.Y, 0.75f, 0.4f);
				Laser();
				runOnce = false;
				//Projectile.friendly = true;
				Projectile.friendly = true;
            }
			if(counter >= 5)
			{
				//Projectile.friendly = false;
				Projectile.friendly = false;
			}
			Lighting.AddLight(Projectile.Center, (255 - Projectile.alpha) * 0.8f / 255f, (255 - Projectile.alpha) * 0.8f / 255f, (255 - Projectile.alpha) * 0.8f / 255f);
			if (Main.netMode != NetmodeID.Server)
				cataloguePos();
		}
		public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox) 
		{
			if(Projectile.friendly || Projectile.hostile)
				for(int i = 0; i < posList.Count; i += 16)
				{
					if (i > posList.Count - 1)
						return false;
					Rectangle rect = new Rectangle((int)posList[i].X - 8, (int)posList[i].Y - 8, 8, 8);
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
				color = green ? new Color(104, 229, 101, 0) : new Color(30, 60, 225, 0);
				Vector2 drawPos = particleList[i].position - Main.screenPosition;
				color = Projectile.GetAlpha(color) * (0.35f + 0.65f * particleList[i].scale);
				for (int j = 0; j < 2; j++)
				{
					float x = Main.rand.NextFloat(-0.5f, 0.5f);
					float y = Main.rand.NextFloat(-0.5f, 0.5f);
					Main.spriteBatch.Draw(texture, drawPos + new Vector2(x, y), null, color, particleList[i].rotation, drawOrigin, particleList[i].scale * 1.1f, SpriteEffects.None, 0f);
				}
			}
			if(Projectile.ai[0] < 30)
			{
				Texture2D texture2 = green ? (Texture2D)ModContent.Request<Texture2D>("SOTS/Projectiles/Inferno/GreenWispIndicator") : (Texture2D)ModContent.Request<Texture2D>("SOTS/Projectiles/Inferno/SansIndicator");
				Vector2 drawOrigin2 = new Vector2(0, 2);
				for(int j = 0; j < 450; j++)
				{
					Vector2 offset = Projectile.velocity.SafeNormalize(Vector2.Zero) * j;
					float alphaMult = 0.1f + ((455 - j) / 455f);
					for (int i = -1; i < 2; i += 2)
					{
						Vector2 circular = new Vector2(0, i * 0.33f * (30 - Projectile.ai[0])).RotatedBy(Projectile.velocity.ToRotation());
						Main.spriteBatch.Draw(texture2, Projectile.Center + offset + circular - Main.screenPosition, new Rectangle(0, 0, 2, 4),  (green ? new Color(120, 205, 120) : new Color(150, 150, 255)) * (Projectile.ai[0] / 30f) * alphaMult, Projectile.velocity.ToRotation(), drawOrigin2, 0.5f, SpriteEffects.None, 0f);
					}
					int x = (int)(Projectile.Center.X + offset.X) / 16;
					int y = (int)(Projectile.Center.Y + offset.Y) / 16;
					Tile tile = Framing.GetTileSafely(x, y);
					if (!WorldGen.InWorld(x, y, 20) || (tile.HasTile && !Main.tileSolidTop[tile.TileType] && Main.tileSolid[tile.TileType]))
					{
						break;
					}
				}
				if(!green)
				{
					texture = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
					drawOrigin = new Vector2(texture.Width / 2, texture.Height / 2);
					float scale = 1f;
					for (int j = 0; j < 2; j++)
					{
						for (int i = 0; i < 4; i++)
						{
							float x = Main.rand.NextFloat(-1f, 1f);
							float y = Main.rand.NextFloat(-1f, 1f);
							Main.spriteBatch.Draw(texture, Projectile.Center + new Vector2(x, y) - Main.screenPosition, null, new Color(100, 100, 175, 0), Projectile.rotation * (1 + j * 0.33f), drawOrigin, scale * (Projectile.scale * 1.25f + Projectile.ai[0] * 0.0125f) * (1 - j * 0.33f), SpriteEffects.None, 0f);
						}
					}
				}
			}
			return false;
		}
    }
}