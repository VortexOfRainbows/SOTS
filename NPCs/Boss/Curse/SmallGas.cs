using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Items.Banners;
using SOTS.Items.Pyramid;
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
		public override void AI()
		{
			npc.ai[3]++;
			Player player = Main.player[npc.target];
			npc.ai[1]++;
			if(npc.ai[1] % 720 == 630 || npc.ai[2] > 0) //do slam attack
            {
				npc.ai[2]++;
				npc.velocity *= 0.1f;
				npc.velocity.Y += 2.4f;
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