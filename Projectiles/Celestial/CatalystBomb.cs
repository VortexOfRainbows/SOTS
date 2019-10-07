using System;
using System.IO;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace SOTS.Projectiles.Celestial
{    
    public class CatalystBomb : ModProjectile 
    {	int count = 0;
		int dis = 24;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Catalyst Bomb");
			
		}
		
        public override void SetDefaults()
        {
			projectile.aiStyle = 0;
			projectile.height = 20;
			projectile.width = 20;
			projectile.penetrate = -1;
			projectile.friendly = true;
			projectile.hostile = false;
			projectile.timeLeft = 240;
			projectile.tileCollide = false;
			projectile.alpha = 255;
		}
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.immune[projectile.owner] = 5;
        }
		public void Detonate(float explosionRadius)
		{
				int minTileX = (int)(projectile.position.X / 16f - (float)explosionRadius);
				int maxTileX = (int)(projectile.position.X / 16f + (float)explosionRadius);
				int minTileY = (int)(projectile.position.Y / 16f - (float)explosionRadius);
				int maxTileY = (int)(projectile.position.Y / 16f + (float)explosionRadius);
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
				bool canKillWalls = false;
				for (int x = minTileX; x <= maxTileX; x++)
				{
					for (int y = minTileY; y <= maxTileY; y++)
					{
						float diffX = Math.Abs((float)x - projectile.position.X / 16f);
						float diffY = Math.Abs((float)y - projectile.position.Y / 16f);
						double distance = Math.Sqrt((double)(diffX * diffX + diffY * diffY));
						if (distance < (double)explosionRadius && Main.tile[x, y] != null && Main.tile[x, y].wall == 0)
						{
							canKillWalls = false;
							break;
						}
					}
				}
				for (int i = minTileX; i <= maxTileX; i++)
				{
					for (int j = minTileY; j <= maxTileY; j++)
					{
						float diffX = Math.Abs((float)i - projectile.position.X / 16f);
						float diffY = Math.Abs((float)j - projectile.position.Y / 16f);
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
							if (canKillTile)
							{
								for (int x = i - 1; x <= i + 1; x++)
								{
									for (int y = j - 1; y <= j + 1; y++)
									{
										if(Main.tile[x, y] != null && Main.tile[x, y].wall > 0)
										{
											if (Main.tile[x, y].wall == 0 && Main.netMode != 0)
											{
												NetMessage.SendData(17, -1, -1, null, 2, (float)x, (float)y, 0f, 0, 0, 0);
											}
										}
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
				Detonate(10f);
				Main.PlaySound(SoundID.Item119, (int)(projectile.Center.X), (int)(projectile.Center.Y));
				if(player.ZoneUnderworldHeight)
				{
					if(!NPC.AnyNPCs(mod.NPCType("SubspaceSerpentHead")))
					{
						NPC.SpawnOnPlayer(player.whoAmI, mod.NPCType("SubspaceSerpentHead"));
						for(int king = 0; king < 200; king++)
						{
							NPC npc = Main.npc[king];
							if(npc.type == mod.NPCType("SubspaceSerpentHead"))
							{
								npc.position.X = projectile.Center.X - npc.width/2;
								npc.position.Y = projectile.Center.Y - npc.height/2;
							}
						}
					}
				}
			}
		}
		public override void AI()
		{
			Player player = Main.player[projectile.owner];
			Lighting.AddLight(projectile.Center, (255 - projectile.alpha) * 0.1f / 255f, (255 - projectile.alpha) * 0.9f / 255f, (255 - projectile.alpha) * 0.3f / 255f);
			if(projectile.ai[0] == 0)
			{
					projectile.rotation++;
					
					projectile.alpha = 0;
					projectile.velocity *= 0.96f;
					if(projectile.timeLeft % 3 == 0 && projectile.timeLeft <= 180)
					{
					dis++;
					Vector2 stormPos = new Vector2(dis * 3, 0).RotatedBy(MathHelper.ToRadians(count * 24));
					
					if(Main.myPlayer == projectile.owner)
					{
						int shard = Projectile.NewProjectile(projectile.Center.X - stormPos.X, projectile.Center.Y - stormPos.Y, 0, 0, projectile.type, projectile.damage, projectile.knockBack, player.whoAmI);
						Main.projectile[shard].ai[0] = 1;
						Main.projectile[shard].ai[1] = projectile.whoAmI;
						Main.projectile[shard].timeLeft = 120;
						Main.projectile[shard].rotation = (float)(MathHelper.ToRadians(180) + Math.Atan2(stormPos.Y, stormPos.X));
					}
					
					count++;
					}
			}
			else
			{
				if(projectile.timeLeft > 110)
				{
					if(projectile.timeLeft % 3 == 0)
					{
						int num1 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), 20, 20, 107);
						Main.dust[num1].noGravity = true;
						Main.dust[num1].velocity *= 0.1f;
						Main.dust[num1].scale *= 2f;
					}
					Detonate(4f);
					projectile.rotation += MathHelper.ToRadians(18);
					//projectile.velocity = new Vector2(1, 0).RotatedBy(projectile.rotation);
				}
				else
				{
					Detonate(2f);
					projectile.velocity = new Vector2(18, 0).RotatedBy(projectile.rotation);
					float distX = projectile.Center.X - Main.projectile[(int)projectile.ai[1]].Center.X;
					float distY = projectile.Center.Y - Main.projectile[(int)projectile.ai[1]].Center.Y;
					double dist = Math.Sqrt(distX * distX + distY * distY);
					if(dist < 32)
					{
						projectile.velocity *= 0.0f;
						if(projectile.timeLeft % 5 == 0)
						{
							int num1 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), 20, 20, 107);
							Main.dust[num1].noGravity = true;
							Main.dust[num1].velocity *= 0.1f;
							Main.dust[num1].scale *= 2f;
						}
					}
					else
					{
						if(projectile.timeLeft % 2 == 0)
						{
							int num1 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), 20, 20, 107);
							Main.dust[num1].noGravity = true;
							Main.dust[num1].velocity *= 0.1f;
							Main.dust[num1].scale *= 2f;
						}
					}
				}
				if(!Main.projectile[(int)projectile.ai[1]].active || Main.projectile[(int)projectile.ai[1]].ai[1] != 0 || Main.projectile[(int)projectile.ai[1]].type != projectile.type)
				{
					projectile.Kill();
				}
			}
		}
	}
}
	