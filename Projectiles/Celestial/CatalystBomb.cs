using System;
using System.IO;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Dusts;
using System.Collections.Generic;

namespace SOTS.Projectiles.Celestial
{    
    public class CatalystBomb : ModProjectile 
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Catalyst Bomb");
			Main.projFrames[projectile.type] = 2;
		}
        public override void SetDefaults()
        {
			projectile.aiStyle = 0;
			projectile.height = 34;
			projectile.width = 38;
			projectile.penetrate = -1;
			projectile.friendly = true;
			projectile.hostile = false;
			projectile.timeLeft = 480;
			projectile.tileCollide = false;
			projectile.alpha = 255;
		}
        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
			Texture2D texture = Main.projectileTexture[projectile.type];
			Vector2 origin = new Vector2(texture.Width * 0.5f, texture.Height * 0.25f);
			for(int i = 1; i >= 0; i--)
            {
				int direction = i * 2 - 1;
				Rectangle frame = new Rectangle(0, (int)(texture.Height * 0.5f * i), texture.Width, texture.Height / 2);
				Vector2 offset = new Vector2(0, direction * 0.4f * (48 - projectile.ai[1])).RotatedBy(projectile.rotation);
				spriteBatch.Draw(texture, projectile.Center + offset - Main.screenPosition, frame, lightColor, projectile.rotation, origin, projectile.scale, SpriteEffects.None, 0f);
            }
            return false;
		}
		public override void PostDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Texture2D texture = ModContent.GetTexture("SOTS/Projectiles/Celestial/SubspaceLingeringFlame");
			Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
			for (int i = 0; i < particleList.Count; i++)
			{
				Color color = new Color(75, 255, 30, 0);
				Vector2 drawPos = particleList[i].position - Main.screenPosition;
				color = projectile.GetAlpha(color) * (0.35f + 0.65f * particleList[i].scale);
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
            target.immune[projectile.owner] = 5;
        }
		public void Detonate(float explosionRadius)
		{
			int minTileX = (int)(projectile.Center.X / 16f - (float)explosionRadius);
			int maxTileX = (int)(projectile.Center.X / 16f + (float)explosionRadius);
			int minTileY = (int)(projectile.Center.Y / 16f - (float)explosionRadius);
			int maxTileY = (int)(projectile.Center.Y / 16f + (float)explosionRadius);
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
					float diffX = Math.Abs((float)i - projectile.Center.X / 16f);
					float diffY = Math.Abs((float)j - projectile.Center.Y / 16f);
					double distanceToTile = Math.Sqrt((double)(diffX * diffX + diffY * diffY));
					if (distanceToTile < (double)explosionRadius)
					{
						bool canKillTile = true;
						if (Main.tile[i, j] != null && Main.tile[i, j].active())
						{
							canKillTile = true;
							if (Main.tileDungeon[(int)Main.tile[i, j].type] || Main.tile[i, j].type == 88 || Main.tile[i, j].type == 21 || Main.tile[i, j].type == 26 || Main.tile[i, j].type == 107 || Main.tile[i, j].type == 108 || Main.tile[i, j].type == 111 || Main.tile[i, j].type == 226 || Main.tile[i, j].type == 237 || Main.tile[i, j].type == 221 || Main.tile[i, j].type == 222 || Main.tile[i, j].type == 223 || Main.tile[i, j].type == 211 || Main.tile[i, j].type == 404)
							{
								canKillTile = false;
							}
							if (!Main.hardMode && Main.tile[i, j].type == 58)
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
								if (!Main.tile[i, j].active() && Main.netMode != 0)
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
			Player player = Main.player[projectile.owner];
			if(projectile.ai[0] == 0)
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
						Dust dust = Dust.NewDustDirect(new Vector2(projectile.Center.X - 4, projectile.Center.Y - 4), 4, 4, ModContent.DustType<CopyDust4>());
						dust.noGravity = true;
						dust.velocity *= 0.5f;
						dust.velocity += circularLocation;
						dust.scale *= 2.5f;
						dust.fadeIn = 0.1f;
						dust.color = new Color(50, 150, 50);
					}
				}
				for (int i = 0; i < 24; i++)
					Gore.NewGore(projectile.position + new Vector2(Main.rand.NextFloat(12, 36), 0).RotatedBy(MathHelper.ToRadians(i * 15)), default(Vector2), Main.rand.Next(61, 64), 1.25f);
				if (player.ZoneUnderworldHeight)
				{
					Main.PlaySound(SoundID.Item119, (int)projectile.Center.X, (int)projectile.Center.Y);
					if (!NPC.AnyNPCs(mod.NPCType("SubspaceSerpentHead")))
					{
						NPC.SpawnOnPlayer(player.whoAmI, mod.NPCType("SubspaceSerpentHead"));
						for (int king = 0; king < 200; king++)
						{
							NPC npc = Main.npc[king];
							if (npc.type == mod.NPCType("SubspaceSerpentHead"))
							{
								npc.position.X = projectile.Center.X - npc.width / 2;
								npc.position.Y = projectile.Center.Y - npc.height / 2;
							}
						}
					}
				}
				else
				{
					Main.PlaySound(SoundID.Item, (int)projectile.Center.X, (int)projectile.Center.Y, 14, 1.0f);
				}
			}
		}
		public override void SendExtraAI(BinaryWriter writer) 
		{
			writer.Write(projectile.rotation);
			writer.Write(projectile.timeLeft);
		}
		public override void ReceiveExtraAI(BinaryReader reader)
		{	
			projectile.rotation = reader.ReadSingle();
			projectile.timeLeft = reader.ReadInt32();
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
				projectile.ai[1] = 48f;
				if (projectile.velocity.X < 0)
					projectile.direction = -1;
				projectile.direction = 1;
			}
			projectile.netUpdate = true;
			Player player = Main.player[projectile.owner];
			Lighting.AddLight(projectile.Center, (255 - projectile.alpha) * 0.1f / 255f, (255 - projectile.alpha) * 0.9f / 255f, (255 - projectile.alpha) * 0.3f / 255f);
			if(projectile.ai[0] == 0)
			{
				cataloguePos();
				bool flag = false;
				projectile.alpha = 0;
				rotation *= 0.98f;
				rotation -= 5.5f;
				if(rotation < 300)
                {
					projectile.ai[1] *= 0.99f;
					projectile.ai[1] -= 0.1f;
					if (projectile.ai[1] < 0)
						projectile.ai[1] = 0;
					if(projectile.ai[1] < 24)
					flag = true;
				}
				if (rotation < 0)
				{
					rotation = 0;
				}
				projectile.rotation = MathHelper.ToRadians(rotation * projectile.direction);
				projectile.velocity *= 0.96f;
				if(flag)
				{
					dist += 0.9f;
					for (int i = 0; i < 360; i++)
					{
						Vector2 circular = new Vector2(dist, 0).RotatedBy(MathHelper.ToRadians(i));
						Vector2 rotational = new Vector2(0, -1.5f).RotatedBy(MathHelper.ToRadians(Main.rand.NextFloat(360f)));
						if (Main.rand.NextBool(40))
						{
							particleList.Add(new FireParticle(projectile.Center + circular - rotational * 2, rotational, Main.rand.NextFloat(-3f, 3f), Main.rand.NextFloat(-2f, 2f), Main.rand.NextFloat(0.7f, 0.9f)));
						}
					}
					int num = count;
					if (num > 30)
						num = 30;
					if(projectile.timeLeft % (20 - (int)(num * 0.5f)) == 0)
					{
						Detonate(dist / 16f);
						for (int i = 0; i < 360; i += 6)
						{
							if (Main.rand.NextBool(3))
							{
								Vector2 circularLocation = new Vector2(-12, 0).RotatedBy(MathHelper.ToRadians(i));
								circularLocation.Y *= 0.5f;
								Dust dust = Dust.NewDustDirect(new Vector2(projectile.Center.X - 4, projectile.Center.Y - 4), 4, 4, ModContent.DustType<CopyDust4>());
								dust.noGravity = true;
								dust.velocity *= 0.5f;
								dust.velocity += circularLocation * 0.35f;
								dust.scale *= 2.5f;
								dust.fadeIn = 0.1f;
								dust.color = new Color(50, 150, 50);
							}
						}
						count++;
						Main.PlaySound(SoundID.NPCKilled, (int)projectile.Center.X, (int)projectile.Center.Y, 39, 0.95f, -0.4f);
						if (Main.myPlayer == projectile.owner)
						{
							Vector2 circular = new Vector2(Main.rand.NextFloat(6f, 8f), 0).RotatedBy(MathHelper.ToRadians(Main.rand.NextFloat(360)));
							if (Main.rand.NextBool(3) && player.ZoneUnderworldHeight)
								Projectile.NewProjectile(projectile.Center, circular, ModContent.ProjectileType<PurgatoryGhost>(), 0, projectile.knockBack, projectile.owner, 0, Main.rand.Next(2) * 2 - 1);
							Vector2 perturbedSpeed = (circular.SafeNormalize(Vector2.Zero) * 2f * Main.rand.NextFloat(0.5f, 1.5f)).RotatedBy(MathHelper.ToRadians(Main.rand.NextFloat(360f)));
							Projectile.NewProjectile(projectile.Center, perturbedSpeed, ModContent.ProjectileType<PurgatoryLightning>(), 0, 1f, Main.myPlayer, Main.rand.Next(2));
						}
					}
				}
			}
		}
	}
}
	