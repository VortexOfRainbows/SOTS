using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Items.Banners;
using SOTS.Items.Pyramid;
using SOTS.Projectiles.Pyramid;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace SOTS.NPCs.Boss.Curse
{
	public class SmallGas : ModNPC
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Pharaoh's Curse");
		}
		public override void SetDefaults()
		{
            npc.aiStyle = 0; 
            npc.lifeMax = 150;   
            npc.damage = 45; 
            npc.defense = 10;  
            npc.knockBackResist = 0.5f;
            npc.width = 45;
            npc.height = 45;
			Main.npcFrameCount[npc.type] = 1;  
            npc.value = 0;
            npc.npcSlots = 1f;
			npc.dontCountMe = true;
			npc.HitSound = null;
			npc.DeathSound = null;
			npc.lavaImmune = true;
			npc.netAlways = true;
			npc.buffImmune[BuffID.OnFire] = true;
			npc.buffImmune[BuffID.Frostburn] = true;
			npc.noTileCollide = true;
			npc.noGravity = true;
			npc.dontTakeDamage = true;
		}
		public List<CurseFoam> foamParticleList1 = new List<CurseFoam>();
		public void catalogueParticles()
		{
			for (int i = 0; i < foamParticleList1.Count; i++)
			{
				CurseFoam particle = foamParticleList1[i];
				particle.Update();
				if (!particle.active)
				{
					particle = null;
					foamParticleList1.RemoveAt(i);
					i--;
				}
				else
				{
					particle.Update();
					if (!particle.active)
					{
						particle = null;
						foamParticleList1.RemoveAt(i);
						i--;
					}
					else if (!particle.noMovement)
						particle.position += npc.velocity * 0.925f;
				}
			}
		}
        public override bool PreAI()
		{
			int parentID = (int)npc.ai[0];
			if (parentID >= 0)
			{
				NPC npc2 = Main.npc[parentID];
				if (npc2.active && npc2.type == NPCType<PharaohsCurse>())
				{
					if(Main.netMode != NetmodeID.Server)
					{
						if (npc.timeLeft == 2)
						{
							PharaohsCurse curse = npc2.modNPC as PharaohsCurse;
							for (int j = 0; j < 40; j++)
							{
								Vector2 rotational = new Vector2(0, -Main.rand.NextFloat(1.05f, 3.5f)).RotatedBy(MathHelper.ToRadians(Main.rand.NextFloat(360f)));
								curse.foamParticleList1.Add(new CurseFoam(npc.Center, rotational, 1.55f, true));
							}
						}
						PharaohsCurse.SpawnPassiveDust(Main.npcTexture[npc.type], npc.Center, 1.0f * npc.scale, foamParticleList1, 1, 0, 40, npc.rotation);
						PharaohsCurse.SpawnPassiveDust(GetTexture("SOTS/NPCs/Boss/Curse/SmallGasFill"), npc.Center + new Vector2(0, 10), 1.0f * npc.scale, foamParticleList1, 1, 0, 100, npc.rotation);
					}
				}
				else
				{
					npc.active = false;
				}
			}
			catalogueParticles();
			return base.PreAI();
		}
        public override bool CanHitPlayer(Player target, ref int cooldownSlot)
        {
            return npc.ai[3] > 90;
        }
        bool[] ignore;
		public override void PostAI()
		{
			if (ignore == null)
			{
				ignore = new bool[Main.tileSolid.Length];
				for (int i = 0; i < ignore.Length; i++)
				{
					ignore[i] = true;
				}
				ignore[TileType<TrueSandstoneTile>()] = false;
				ignore[TileType<AncientGoldGateTile>()] = false;
			}
			npc.velocity = Collision.AdvancedTileCollision(ignore, npc.position, npc.velocity, npc.width, npc.height, true, true);
		}
		public void ParticleExplosion(int amt = 160, bool quiet = false)
		{
			int parentID = (int)npc.ai[0];
			if (parentID >= 0)
			{
				NPC npc2 = Main.npc[parentID];
				if (npc2.active && npc2.type == NPCType<PharaohsCurse>())
				{
					if (Main.netMode != NetmodeID.Server)
					{
						PharaohsCurse curse = npc2.modNPC as PharaohsCurse;
						for (int j = 0; j < amt; j++)
						{
							float scale = Main.rand.NextFloat(0.5f, 1.5f);
							Vector2 rotational = new Vector2(0, -Main.rand.NextFloat(2.75f, 8.5f) / scale).RotatedBy(MathHelper.ToRadians(Main.rand.NextFloat(360f)));
							curse.foamParticleList1.Add(new CurseFoam(npc.Center, rotational, 1.75f * scale, true));
						}
					}
				}
				else
				{
					npc.active = false;
				}
			}
			if (!quiet)
				Main.PlaySound(SoundID.Item14, (int)npc.Center.X, (int)npc.Center.Y);
		}
		public override void AI()
		{
			npc.ai[3]++;
			Player player = Main.player[npc.target];
			npc.ai[1]++;
			if(npc.ai[1] % 720 == 630 || npc.ai[2] > 0) //do slam attack
			{
				npc.ai[2]++;
				if (npc.ai[2] < 60)
				{
					float waveY = (float)Math.Sin(MathHelper.ToRadians(npc.ai[2] * 4f));
					npc.velocity.Y *= 0.875f;
					npc.velocity.Y -= 0.4f * waveY;
					if (npc.ai[2] == 40)
					{
						Main.PlaySound(2, (int)player.Center.X, (int)player.Center.Y, 15, 1.33f, -0.05f);
					}
				}
				else
				{
					if (npc.ai[2] == 60)
					{
						Main.PlaySound(2, (int)player.Center.X, (int)player.Center.Y, 96, 1f, 0f);
						npc.velocity.Y += 4.5f;
					}
					npc.velocity.Y += 0.8f;
					//Check for tile collide
					Vector2 temp = npc.velocity;
					npc.velocity = Collision.AdvancedTileCollision(ignore, npc.position, npc.velocity, npc.width, npc.height, true, true);
					if (npc.velocity != temp)
					{
						ParticleExplosion();
						if (Main.netMode != 1)
						{
							int damage = npc.damage / 2;
							if (Main.expertMode)
							{
								damage = (int)(damage / Main.expertDamage);
							}
							for (int i = 0; i < 6; i++)
							{
								Vector2 outWards = new Vector2(-2f, 0).RotatedBy(MathHelper.ToRadians(30 + i / 2 * 40));
								Projectile.NewProjectile(npc.Center, outWards, ProjectileType<CurseWave>(), damage, 0f, Main.myPlayer, (int)npc.ai[0], (i % 2 * 2 - 1) * 0.8f);
							}
						}
						npc.active = false;
					}
				}
			}
			else
			{
				Vector2 rotatePos = new Vector2(160, 0).RotatedBy(MathHelper.ToRadians(npc.ai[1]));
				Vector2 toPos = rotatePos + player.Center;
				Vector2 goToPos = npc.Center - toPos;
				float length = goToPos.Length() + 0.1f;
				if (length > 12)
				{
					length = 12;
				}
				goToPos = goToPos.SafeNormalize(Vector2.Zero);
				npc.velocity = goToPos * -length;
			}
		}
		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			return false;
		}
		public override void HitEffect(int hitDirection, double damage)
		{
			int parentID = (int)npc.ai[0];
			if (parentID >= 0 && Main.netMode != NetmodeID.Server)
			{
				NPC npc2 = Main.npc[parentID];
				if (npc2.active && npc2.type == NPCType<PharaohsCurse>())
				{
					PharaohsCurse curse = npc2.modNPC as PharaohsCurse;
					for (int j = 0; j < 40; j++)
					{
						Vector2 rotational = new Vector2(0, -Main.rand.NextFloat(1.05f, 3.5f)).RotatedBy(MathHelper.ToRadians(Main.rand.NextFloat(360f)));
						curse.foamParticleList1.Add(new CurseFoam(npc.Center, rotational, 1.55f, true));
					}
				}
			}
			if (npc.life > 0)
			{
				Main.PlaySound(3, (int)npc.Center.X, (int)npc.Center.Y, 54, 1.2f, -0.25f);
				int num = 0;
				while ((double)num < damage / (double)npc.lifeMax * 60.0)
				{
					Dust.NewDust(npc.position, npc.width, npc.height, mod.DustType("CurseDust"), (float)(2 * hitDirection), -2f);
					num++;
				}
			}
			else
			{
				for (int k = 0; k < 50; k++)
				{
					Dust.NewDust(npc.position, npc.width, npc.height, mod.DustType("CurseDust"), (float)(2 * hitDirection), -2f);
				}
			}
		}
	}
}