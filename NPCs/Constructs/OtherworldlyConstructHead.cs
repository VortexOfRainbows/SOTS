using System;
using System.Diagnostics;
using System.Runtime.Remoting.Messaging;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoMod.RuntimeDetour;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
namespace SOTS.NPCs.Constructs
{
	public class OtherworldlyConstructHead : ModNPC
	{
		int timer = 0;
		int ai1 = 0;
		float dir = 0f;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Otherworldly Construct");
		}
		public override void SetDefaults()
		{
			npc.aiStyle = 0;
			npc.lifeMax = 300;  
			npc.damage = 40; 
			npc.defense = 14;  
			npc.knockBackResist = 0.1f;
			npc.width = 70;
			npc.height = 82;
			Main.npcFrameCount[npc.type] = 1;  
			npc.value = 9550;
			npc.npcSlots = 4f;
			npc.lavaImmune = true;
			npc.noGravity = true;
			npc.noTileCollide = false;
			npc.netAlways = true;
			npc.alpha = 0;
			npc.HitSound = SoundID.NPCHit4;
			npc.DeathSound = SoundID.NPCDeath14;
			npc.rarity = 5;
		}
		public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
		{
			dir = (float)Math.Atan2(aimTo.Y - npc.Center.Y, aimTo.X - npc.Center.X);
			npc.rotation = dir + (npc.spriteDirection - 1) * 0.5f * -MathHelper.ToRadians(180);
			return true;
		}
		public override void HitEffect(int hitDirection, double damage)
		{
			if (npc.life <= 0)
			{
				for (int k = 0; k < 20; k++)
				{
					Dust.NewDust(npc.position, npc.width, npc.height, 82, 2.5f * (float)hitDirection, -2.5f, 0, default(Color), 0.7f);
				}
				for (int i = 0; i < 30; i++)
				{
					int dust = Dust.NewDust(new Vector2(npc.position.X, npc.position.Y), npc.width, npc.height, mod.DustType("BigAetherDust"));
					Main.dust[dust].velocity *= 5f;
				}
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/OtherworldlyConstructGore2_1"), 1f);
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/OtherworldlyConstructGore2_2"), 1f);
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/OtherworldlyConstructGore2_3"), 1f);
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/OtherworldlyConstructGore2_4"), 1f);
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/OtherworldlyConstructGore2_5"), 1f);
				for (int i = 0; i < 9; i++)
					Gore.NewGore(npc.position, npc.velocity, Main.rand.Next(61, 64), 1f);
			}
		}
		Vector2 aimTo = new Vector2(-1, -1);
		public override bool PreAI()
		{
			Player player = Main.player[npc.target];
			npc.TargetClosest(true);
			if((aimTo.X == -1 && aimTo.Y == -1))
			{
				aimTo = npc.Center;
				return false;
			}
			aimTo = player.Center;
			return true;
		}
		bool flag = false;
		bool flag2 = false;
		Vector2 rotateVector = new Vector2(12, 0);
		public override void AI()
		{
			Player player = Main.player[npc.target];
			Vector2 dynamicAddition = new Vector2(0.4f, 0).RotatedBy(MathHelper.ToRadians(ai1));
			Lighting.AddLight(npc.Center, (255 - npc.alpha) * 0.45f / 155f, (255 - npc.alpha) * 0.25f / 155f, (255 - npc.alpha) * 0.45f / 155f);
			ai1++;
			if(npc.ai[0] < 0)
			{
				npc.velocity *= 0.98f;
				return;
			}
			npc.velocity = dynamicAddition;
			float dir2 = (float)Math.Atan2(aimTo.Y - npc.Center.Y, aimTo.X - npc.Center.X);
			Vector2 distanceToTarget = player.Center - npc.Center;
			Vector2 distanceToTarget2 = player.Center - npc.Center;

			distanceToTarget.Normalize();
			rotateVector += distanceToTarget * 1;
			rotateVector = new Vector2(12, 0).RotatedBy(rotateVector.ToRotation());
			int maxDistPlayer = 96;
			int maxDistNPC = 512;
			Vector2 added = new Vector2(3f, 0).RotatedBy(rotateVector.ToRotation());
			if (distanceToTarget2.Length() > 800f)
			{
				added = Vector2.Zero;
			}
			Vector2 newLocation = npc.Center + added + dynamicAddition;
			if((player.Center - newLocation).Length() < maxDistPlayer + 60)
			{
				flag = true;
			}
			if ((player.Center - newLocation).Length() > maxDistPlayer + 110)
			{
				flag = false;
			}
			if ((player.Center - newLocation).Length() < maxDistPlayer)
			{
				npc.velocity = -0.3f * added + -0.1f * dynamicAddition;
			}
			if(!flag)
			{
				npc.velocity = added + dynamicAddition;
			}
		}
		public override void PostAI()
		{
			Player player = Main.player[npc.target];
			Vector2 toPlayer = player.Center - npc.Center;
			Vector2 playerLoc = player.Center;
			if (toPlayer.Length() > 900 && npc.ai[0] < 270)
				return;
			if(toPlayer.Length() < 240)
			{
				float rot = toPlayer.ToRotation();
				Vector2 circular = new Vector2(240, 0).RotatedBy(rot);
				playerLoc = circular + npc.Center;
			}
			npc.ai[0]++;
			if(npc.ai[0] >= 270)
			{
				npc.velocity *= 0.25f;
				if(npc.ai[0] % 90 == 0)
				{
					int damage = npc.damage / 2;
					if (Main.expertMode)
					{
						damage = (int)(damage / Main.expertDamage);
					}
					npc.ai[1]++;
					if(npc.ai[1] < 4)
					{
						float locX = playerLoc.X + Main.rand.Next(-200, 201);
						float locY = playerLoc.Y + Main.rand.Next(-200, 201);
						bool inBlock = true;
						while (inBlock)
						{
							int i = (int)locX / 16;
							int j = (int)locY / 16;
							if (Main.tileSolid[Main.tile[i, j].type] && Main.tile[i, j].active() && !Main.tileSolidTop[Main.tile[i, j].type])
							{
								locX = playerLoc.X + Main.rand.Next(-200, 201);
								locY = playerLoc.Y + Main.rand.Next(-200, 201);
								inBlock = true;
							}
							else
							{
								inBlock = false;
								break;
							}
						}
						Main.PlaySound(2, (int)locX, (int)locY, 30, 0.2f);
						if (Main.netMode != 1)
							Projectile.NewProjectile(locX, locY, 0, 0, mod.ProjectileType("OtherworldlyTracer"), damage, 0f, Main.myPlayer, 571 - npc.ai[0], npc.whoAmI);
					}
				}
				if(npc.ai[1] >= 4)
				{
					npc.ai[1] = 0;
					npc.ai[0] = -90;
					Main.PlaySound(SoundID.Item92, npc.Center);
					for (int i = 0; i < Main.projectile.Length; i++)
					{
						Projectile proj = Main.projectile[i];
						if(proj.active && proj.type == mod.ProjectileType("OtherworldlyTracer") && proj.ai[1] == npc.whoAmI)
						{
							int damage = npc.damage / 2;
							if (Main.expertMode)
							{
								damage = (int)(damage / Main.expertDamage);
							}
							Vector2 toProj = proj.Center - npc.Center;
							toProj /= 30f;
							if (Main.netMode != 1)
								Projectile.NewProjectile(npc.Center.X, npc.Center.Y, toProj.X, toProj.Y, mod.ProjectileType("OtherworldlyBall"), damage, 0, Main.myPlayer);
						}
					}
					npc.velocity = -12 * toPlayer.SafeNormalize(new Vector2(0, 1));
				}
			}
		}
		public override void NPCLoot()
		{
			int n = NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, mod.NPCType("OtherworldlySpirit"));	
			Main.npc[n].velocity.Y = -10f;
			Main.npc[n].localAI[1] = -1;
			if (Main.netMode != 1)
				Main.npc[n].netUpdate = true;
			Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height,  mod.ItemType("FragmentOfOtherworld"), Main.rand.Next(4) + 4);
			if ((Main.expertMode || Main.rand.Next(2) == 0) && SOTSWorld.downedAdvisor) Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("TwilightShard"), 1);
		}	
	}
}