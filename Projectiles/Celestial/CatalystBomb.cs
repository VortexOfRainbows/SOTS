using System;
using System.IO;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Dusts;
using System.Collections.Generic;
using SOTS.NPCs.Boss;

namespace SOTS.Projectiles.Celestial
{    
    public class CatalystBomb : ModProjectile 
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Catalyst Bomb");
			Main.projFrames[Projectile.type] = 2;
		}
        public override void SetDefaults()
        {
			Projectile.aiStyle = 0;
			Projectile.height = 34;
			Projectile.width = 38;
			Projectile.penetrate = -1;
			Projectile.friendly = true;
			Projectile.hostile = false;
			Projectile.timeLeft = 480;
			Projectile.tileCollide = false;
			Projectile.alpha = 255;
		}
        public override bool PreDraw(ref Color lightColor)
        {
			Texture2D texture = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
			Vector2 origin = new Vector2(texture.Width * 0.5f, texture.Height * 0.25f);
			for(int i = 1; i >= 0; i--)
            {
				int direction = i * 2 - 1;
				Rectangle frame = new Rectangle(0, (int)(texture.Height * 0.5f * i), texture.Width, texture.Height / 2);
				Vector2 offset = new Vector2(0, direction * 0.4f * (48 - Projectile.ai[1])).RotatedBy(Projectile.rotation);
				Main.spriteBatch.Draw(texture, Projectile.Center + offset - Main.screenPosition, frame, lightColor, Projectile.rotation, origin, Projectile.scale, SpriteEffects.None, 0f);
            }
            return false;
		}
		public override void PostDraw(Color lightColor)
		{
			Texture2D texture = (Texture2D)ModContent.Request<Texture2D>("SOTS/Projectiles/Celestial/SubspaceLingeringFlame");
			Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
			for (int i = 0; i < particleList.Count; i++)
			{
				Color color = new Color(75, 255, 30, 0);
				Vector2 drawPos = particleList[i].position - Main.screenPosition;
				color = Projectile.GetAlpha(color) * (0.35f + 0.65f * particleList[i].scale);
				for (int j = 0; j < 2; j++)
				{
					float x = Main.rand.NextFloat(-2f, 2f);
					float y = Main.rand.NextFloat(-2f, 2f);
					Main.spriteBatch.Draw(texture, drawPos + new Vector2(x, y), null, color, particleList[i].rotation, drawOrigin, particleList[i].scale * 1.15f, SpriteEffects.None, 0f);
				}
			}
		}
		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.immune[Projectile.owner] = 5;
        }
		public void Detonate(float explosionRadius)
		{
			int minTileX = (int)(Projectile.Center.X / 16f - (float)explosionRadius);
			int maxTileX = (int)(Projectile.Center.X / 16f + (float)explosionRadius);
			int minTileY = (int)(Projectile.Center.Y / 16f - (float)explosionRadius);
			int maxTileY = (int)(Projectile.Center.Y / 16f + (float)explosionRadius);
			if (minTileX < 0)
			{
				minTileX = 0;
			}
			if (maxTileX > Main.maxTilesX)
			{
				maxTileX = Main.maxTilesX;
			}
			if (minTileY < 0)
			{
				minTileY = 0;
			}
			if (maxTileY > Main.maxTilesY)
			{
				maxTileY = Main.maxTilesY;
			}
			for (int i = minTileX; i <= maxTileX; i++)
			{
				for (int j = minTileY; j <= maxTileY; j++)
				{
					float diffX = Math.Abs((float)i - Projectile.Center.X / 16f);
					float diffY = Math.Abs((float)j - Projectile.Center.Y / 16f);
					double distanceToTile = Math.Sqrt((double)(diffX * diffX + diffY * diffY));
					if (distanceToTile < (double)explosionRadius)
					{
						bool canKillTile = true;
						if (Main.tile[i, j] != null && Main.tile[i, j].HasTile)
						{
							canKillTile = true;
							if (Main.tileDungeon[(int)Main.tile[i, j ].TileType] || Main.tile[i, j ].TileType == 88 || Main.tile[i, j ].TileType == 21 || Main.tile[i, j ].TileType == 26 || Main.tile[i, j ].TileType == 107 || Main.tile[i, j ].TileType == 108 || Main.tile[i, j ].TileType == 111 || Main.tile[i, j ].TileType == 226 || Main.tile[i, j ].TileType == 237 || Main.tile[i, j ].TileType == 221 || Main.tile[i, j ].TileType == 222 || Main.tile[i, j ].TileType == 223 || Main.tile[i, j ].TileType == 211 || Main.tile[i, j ].TileType == 404)
							{
								canKillTile = false;
							}
							if (!Main.hardMode && Main.tile[i, j ].TileType == 58)
							{
								canKillTile = false;
							}
							if (!TileLoader.CanExplode(i, j))
							{
								canKillTile = false;
							}
							if (canKillTile)
							{
								WorldGen.KillTile(i, j, false, false, false);
								if (!Main.tile[i, j].HasTile && Main.netMode != 0)
								{
									NetMessage.SendData(17, -1, -1, null, 0, (float)i, (float)j, 0f, 0, 0, 0);
								}
							}
						}
					}
				}
			}
		}
		public override void Kill(int timeLeft)
        {
			Player player = Main.player[Projectile.owner];
			if(Projectile.ai[0] == 0)
			{
				for (int i = 0; i < particleList.Count; i++)
				{
					FireParticle particle = particleList[i];
					Dust dust = Dust.NewDustDirect(new Vector2(particle.position.X - 4, particle.position.Y - 4), 4, 4, ModContent.DustType<CopyDust4>());
					dust.noGravity = true;
					dust.velocity *= 1.25f;
					dust.scale *= 2.5f;
					dust.fadeIn = 0.1f;
					dust.color = new Color(50, 150, 50);
				}
				for (int i = 0; i < 360; i += 2)
				{
					if (Main.rand.NextBool(3))
					{
						Vector2 circularLocation = new Vector2(-10, 0).RotatedBy(MathHelper.ToRadians(i));
						circularLocation.Y *= 1.25f;
						Dust dust = Dust.NewDustDirect(new Vector2(Projectile.Center.X - 4, Projectile.Center.Y - 4), 4, 4, ModContent.DustType<CopyDust4>());
						dust.noGravity = true;
						dust.velocity *= 0.5f;
						dust.velocity += circularLocation;
						dust.scale *= 2.5f;
						dust.fadeIn = 0.1f;
						dust.color = new Color(50, 150, 50);
					}
				}
				for (int i = 0; i < 24; i++)
					Gore.NewGore(Projectile.GetSource_Death(), Projectile.position + new Vector2(Main.rand.NextFloat(12, 36), 0).RotatedBy(MathHelper.ToRadians(i * 15)), default(Vector2), Main.rand.Next(61, 64), 1.25f);
				if (player.ZoneUnderworldHeight)
				{
					Terraria.Audio.SoundEngine.PlaySound(SoundID.Item119, (int)Projectile.Center.X, (int)Projectile.Center.Y);
					if (!NPC.AnyNPCs(ModContent.NPCType<SubspaceSerpentHead>()))
					{
						NPC.SpawnOnPlayer(player.whoAmI, ModContent.NPCType<SubspaceSerpentHead>());
						for (int king = 0; king < 200; king++)
						{
							NPC npc = Main.npc[king];
							if (npc.type == ModContent.NPCType<SubspaceSerpentHead>())
							{
								npc.position.X = Projectile.Center.X - npc.width / 2;
								npc.position.Y = Projectile.Center.Y - npc.height / 2;
							}
						}
					}
				}
				else
				{
					Terraria.Audio.SoundEngine.PlaySound(SoundID.Item, (int)Projectile.Center.X, (int)Projectile.Center.Y, 14, 1.0f);
				}
			}
		}
		public override void SendExtraAI(BinaryWriter writer) 
		{
			writer.Write(Projectile.rotation);
			writer.Write(Projectile.timeLeft);
		}
		public override void ReceiveExtraAI(BinaryReader reader)
		{	
			Projectile.rotation = reader.ReadSingle();
			Projectile.timeLeft = reader.ReadInt32();
		}
		List<FireParticle> particleList = new List<FireParticle>();
		bool runOnce = true;
		float rotation = 3600; 
		int count = 0;
		float dist = 0;
		int direction = 0;
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
		public override void AI()
		{
			if(runOnce)
			{
				runOnce = false;
				Projectile.ai[1] = 48f;
				if (Projectile.velocity.X < 0)
					Projectile.direction = -1;
				Projectile.direction = 1;
			}
			Projectile.netUpdate = true;
			Player player = Main.player[Projectile.owner];
			Lighting.AddLight(Projectile.Center, (255 - Projectile.alpha) * 0.1f / 255f, (255 - Projectile.alpha) * 0.9f / 255f, (255 - Projectile.alpha) * 0.3f / 255f);
			if(Projectile.ai[0] == 0)
			{
				cataloguePos();
				bool flag = false;
				Projectile.alpha = 0;
				rotation *= 0.98f;
				rotation -= 5.5f;
				if(rotation < 300)
                {
					Projectile.ai[1] *= 0.99f;
					Projectile.ai[1] -= 0.1f;
					if (Projectile.ai[1] < 0)
						Projectile.ai[1] = 0;
					if(Projectile.ai[1] < 24)
					flag = true;
				}
				if (rotation < 0)
				{
					rotation = 0;
				}
				Projectile.rotation = MathHelper.ToRadians(rotation * Projectile.direction);
				Projectile.velocity *= 0.96f;
				if(flag)
				{
					dist += 0.9f;
					for (int i = 0; i < 360; i++)
					{
						Vector2 circular = new Vector2(dist, 0).RotatedBy(MathHelper.ToRadians(i));
						Vector2 rotational = new Vector2(0, -1.5f).RotatedBy(MathHelper.ToRadians(Main.rand.NextFloat(360f)));
						if (Main.rand.NextBool(40))
						{
							particleList.Add(new FireParticle(Projectile.Center + circular - rotational * 2, rotational, Main.rand.NextFloat(-3f, 3f), Main.rand.NextFloat(-2f, 2f), Main.rand.NextFloat(0.7f, 0.9f)));
						}
					}
					int num = count;
					if (num > 30)
						num = 30;
					if(Projectile.timeLeft % (20 - (int)(num * 0.5f)) == 0)
					{
						Detonate(dist / 16f);
						for (int i = 0; i < 360; i += 6)
						{
							if (Main.rand.NextBool(3))
							{
								Vector2 circularLocation = new Vector2(-12, 0).RotatedBy(MathHelper.ToRadians(i));
								circularLocation.Y *= 0.5f;
								Dust dust = Dust.NewDustDirect(new Vector2(Projectile.Center.X - 4, Projectile.Center.Y - 4), 4, 4, ModContent.DustType<CopyDust4>());
								dust.noGravity = true;
								dust.velocity *= 0.5f;
								dust.velocity += circularLocation * 0.35f;
								dust.scale *= 2.5f;
								dust.fadeIn = 0.1f;
								dust.color = new Color(50, 150, 50);
							}
						}
						count++;
						Terraria.Audio.SoundEngine.PlaySound(SoundID.NPCKilled, (int)Projectile.Center.X, (int)Projectile.Center.Y, 39, 0.95f, -0.4f);
						if (Main.myPlayer == Projectile.owner)
						{
							Vector2 circular = new Vector2(Main.rand.NextFloat(6f, 8f), 0).RotatedBy(MathHelper.ToRadians(Main.rand.NextFloat(360)));
							if (Main.rand.NextBool(3) && player.ZoneUnderworldHeight)
								Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, circular, ModContent.ProjectileType<PurgatoryGhost>(), 0, Projectile.knockBack, Projectile.owner, 0, Main.rand.Next(2) * 2 - 1);
							Vector2 perturbedSpeed = (circular.SafeNormalize(Vector2.Zero) * 2f * Main.rand.NextFloat(0.5f, 1.5f)).RotatedBy(MathHelper.ToRadians(Main.rand.NextFloat(360f)));
							Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, perturbedSpeed, ModContent.ProjectileType<PurgatoryLightning>(), 0, 1f, Main.myPlayer, Main.rand.Next(2));
						}
					}
				}
			}
		}
	}
}
	