using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Dusts;
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
			NPCID.Sets.NPCBestiaryDrawModifiers drawModifiers = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
			{
				Hide = true
			};
			NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, drawModifiers);
		}
		public override void SetDefaults()
		{
            NPC.aiStyle =0; 
            NPC.lifeMax = 150;   
            NPC.damage = 45; 
            NPC.defense = 10;  
            NPC.knockBackResist = 0.5f;
            NPC.width = 45;
            NPC.height = 45;
			Main.npcFrameCount[NPC.type] = 1;  
            NPC.value = 0;
            NPC.npcSlots = 1f;
			NPC.dontCountMe = true;
			NPC.HitSound = null;
			NPC.DeathSound = null;
			NPC.lavaImmune = true;
			NPC.netAlways = true;
			NPC.noTileCollide = true;
			NPC.noGravity = true;
			NPC.dontTakeDamage = true;
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
						particle.position += NPC.velocity * 0.925f;
				}
			}
		}
        public override bool PreAI()
		{
			int parentID = (int)NPC.ai[0];
			if (parentID >= 0)
			{
				NPC npc2 = Main.npc[parentID];
				if (npc2.active && npc2.type == NPCType<PharaohsCurse>())
				{
					if(Main.netMode != NetmodeID.Server)
					{
						if (NPC.timeLeft == 2)
						{
							PharaohsCurse curse = npc2.ModNPC as PharaohsCurse;
							for (int j = 0; j < 40; j++)
							{
								Vector2 rotational = new Vector2(0, -Main.rand.NextFloat(1.05f, 3.5f)).RotatedBy(MathHelper.ToRadians(Main.rand.NextFloat(360f)));
								curse.foamParticleList1.Add(new CurseFoam(NPC.Center, rotational, 1.55f, true));
							}
						}
						PharaohsCurse.SpawnPassiveDust(Terraria.GameContent.TextureAssets.Npc[NPC.type].Value, NPC.Center, 1.0f * NPC.scale, foamParticleList1, 1, 0, 40, NPC.rotation);
						PharaohsCurse.SpawnPassiveDust((Texture2D)Request<Texture2D>("SOTS/NPCs/Boss/Curse/SmallGasFill"), NPC.Center + new Vector2(0, 10), 1.0f * NPC.scale, foamParticleList1, 1, 0, 100, NPC.rotation);
					}
				}
				else
				{
					NPC.active = false;
				}
			}
			catalogueParticles();
			return base.PreAI();
		}
        public override bool CanHitPlayer(Player target, ref int cooldownSlot)
        {
            return NPC.ai[3] > 90;
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
			NPC.velocity = Collision.AdvancedTileCollision(ignore, NPC.position, NPC.velocity, NPC.width, NPC.height, true, true);
		}
		public void ParticleExplosion(int amt = 160, bool quiet = false)
		{
			int parentID = (int)NPC.ai[0];
			if (parentID >= 0)
			{
				NPC npc2 = Main.npc[parentID];
				if (npc2.active && npc2.type == NPCType<PharaohsCurse>())
				{
					if (Main.netMode != NetmodeID.Server)
					{
						PharaohsCurse curse = npc2.ModNPC as PharaohsCurse;
						for (int j = 0; j < amt; j++)
						{
							float scale = Main.rand.NextFloat(0.5f, 1.5f);
							Vector2 rotational = new Vector2(0, -Main.rand.NextFloat(2.75f, 8.5f) / scale).RotatedBy(MathHelper.ToRadians(Main.rand.NextFloat(360f)));
							curse.foamParticleList1.Add(new CurseFoam(NPC.Center, rotational, 1.75f * scale, true));
						}
					}
				}
				else
				{
					NPC.active = false;
				}
			}
			if (!quiet)
				SOTSUtils.PlaySound(SoundID.Item62, (int)NPC.Center.X, (int)NPC.Center.Y, 1f, 0.2f);
		}
		public override void AI()
		{
			NPC.ai[3]++;
			Player player = Main.player[NPC.target];
			NPC.ai[1]++;
			if(NPC.ai[1] % 720 == 630 || NPC.ai[2] > 0) //do slam attack
			{
				NPC.velocity.X *= 0.5f;
				NPC.ai[2]++;
				if (NPC.ai[2] < 60)
				{
					float waveY = (float)Math.Sin(MathHelper.ToRadians(NPC.ai[2] * 4f));
					NPC.velocity.Y *= 0.875f;
					NPC.velocity.Y -= 0.4f * waveY;
					if (NPC.ai[2] == 40)
					{
						SOTSUtils.PlaySound(SoundID.Item15, (int)player.Center.X, (int)player.Center.Y, 1.33f, -0.05f);
					}
				}
				else
				{
					if (NPC.ai[2] == 60)
					{
						SOTSUtils.PlaySound(SoundID.Item96, (int)player.Center.X, (int)player.Center.Y, 1f, 0f);
						NPC.velocity.Y += 4.5f;
					}
					NPC.velocity.Y += 0.8f;
					//Check for tile collide
					Vector2 temp = NPC.velocity;
					NPC.velocity = Collision.AdvancedTileCollision(ignore, NPC.position, NPC.velocity, NPC.width, NPC.height, true, true);
					if (NPC.velocity != temp)
					{
						ParticleExplosion();
						if (Main.netMode != 1)
						{
							int damage = Common.GlobalNPCs.SOTSNPCs.GetBaseDamage(NPC) / 2;
							for (int i = 0; i < 6; i++)
							{
								Vector2 outWards = new Vector2(-2f, 0).RotatedBy(MathHelper.ToRadians(30 + i / 2 * 40));
								Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, outWards, ProjectileType<CurseWave>(), damage, 0f, Main.myPlayer, (int)NPC.ai[0], (i % 2 * 2 - 1) * 0.8f);
							}
						}
						NPC.active = false;
					}
				}
			}
			else
			{
				Vector2 rotatePos = new Vector2(160, 0).RotatedBy(MathHelper.ToRadians(NPC.ai[1]));
				Vector2 toPos = rotatePos + player.Center;
				Vector2 goToPos = NPC.Center - toPos;
				float length = goToPos.Length() + 0.1f;
				if (length > 12)
				{
					length = 12;
				}
				goToPos = goToPos.SafeNormalize(Vector2.Zero);
				NPC.velocity = goToPos * -length;
			}
		}
        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            return false;
        }
        public override void HitEffect(int hitDirection, double damage)
		{
			int parentID = (int)NPC.ai[0];
			if (parentID >= 0 && Main.netMode != NetmodeID.Server)
			{
				NPC npc2 = Main.npc[parentID];
				if (npc2.active && npc2.type == NPCType<PharaohsCurse>())
				{
					PharaohsCurse curse = npc2.ModNPC as PharaohsCurse;
					for (int j = 0; j < 40; j++)
					{
						Vector2 rotational = new Vector2(0, -Main.rand.NextFloat(1.05f, 3.5f)).RotatedBy(MathHelper.ToRadians(Main.rand.NextFloat(360f)));
						curse.foamParticleList1.Add(new CurseFoam(NPC.Center, rotational, 1.55f, true));
					}
				}
			}
			if (NPC.life > 0)
			{
				SOTSUtils.PlaySound(SoundID.NPCHit54, (int)NPC.Center.X, (int)NPC.Center.Y, 1.2f, -0.25f);
				int num = 0;
				while ((double)num < damage / (double)NPC.lifeMax * 60.0)
				{
					Dust.NewDust(NPC.position, NPC.width, NPC.height, DustType<CurseDust>(), (float)(2 * hitDirection), -2f);
					num++;											  
				}													  
			}														  
			else													  
			{														  
				for (int k = 0; k < 50; k++)						  
				{													  
					Dust.NewDust(NPC.position, NPC.width, NPC.height, DustType<CurseDust>(), (float)(2 * hitDirection), -2f);
				}
			}
		}
	}
}