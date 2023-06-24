using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Items.Fragments;
using SOTS.Projectiles.Nature;
using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.NPCs.Constructs
{
	public class NatureConstruct : ModNPC
	{
		int initiateSpeed = 1;
		int ai2 = 30;
		float dir;
		float speedMod = 1f;
		bool canSpell = false;
		private float delay = 360;
		public override void SendExtraAI(BinaryWriter writer) 
		{
			writer.Write(delay);
			writer.Write(dir);
			writer.Write(canSpell);
		}
		public override void ReceiveExtraAI(BinaryReader reader)
		{	
			delay = reader.ReadSingle();
			dir = reader.ReadSingle();
			canSpell = reader.ReadBoolean();
		}
        public override void SetStaticDefaults()
		{
			Main.npcFrameCount[NPC.type] = 3;
			NPCID.Sets.MPAllowedEnemies[Type] = true;
		}
        public override void SetDefaults()
		{
			NPC.aiStyle =0;
			NPC.lifeMax = 125;  
			NPC.damage = 20; 
			NPC.defense = 6;  
			NPC.knockBackResist = 0.1f;
			NPC.width = 120;
			NPC.height = 70;
			NPC.value = 3330;
			NPC.npcSlots = 3f;
			NPC.boss = false;
			NPC.lavaImmune = false;
			NPC.noGravity = false;
			NPC.noTileCollide = false;
			NPC.netAlways = true;
			NPC.HitSound = SoundID.NPCHit4;
			NPC.DeathSound = SoundID.NPCDeath14;
			NPC.rarity = 5;
		}
        public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
			Player player = Main.player[NPC.target];
			Texture2D texture = (Texture2D)ModContent.Request<Texture2D>("SOTS/NPCs/Constructs/NatureConstructHead");
			Texture2D texture2 = (Texture2D)ModContent.Request<Texture2D>("SOTS/NPCs/Constructs/NatureConstructHeadGlow");
			Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
			Vector2 drawPos = NPC.Center - screenPos + new Vector2(0f, NPC.gfxOffY);
			if(NPC.frame.Y == 70) //frame 2
				drawPos.Y -= 4;
			if(NPC.frame.Y == 140) //frame 3
				drawPos.Y -= 2;
			bool flip = false;
			Vector2 toPlayer = player.Center - NPC.Center;
			if(Math.Abs(MathHelper.WrapAngle(toPlayer.ToRotation())) <= MathHelper.ToRadians(90))
            {
				flip = true;
            }
			float bonusDir = !flip ? MathHelper.ToRadians(180) : 0;
			spriteBatch.Draw(texture, drawPos, null, drawColor, dir - bonusDir, drawOrigin, NPC.scale, flip ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0f);
			spriteBatch.Draw(texture2, drawPos, null, Color.White, dir - bonusDir, drawOrigin, NPC.scale, flip ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0f);
		}
		public override void HitEffect(int hitDirection, double damage)
		{
			if (Main.netMode == NetmodeID.Server)
				return;
			ai2 = 0;
			if (NPC.life <= 0)
			{
				for (int k = 0; k < 20; k++)
				{
					Dust.NewDust(NPC.position, NPC.width, NPC.height, 82, 2.5f * (float)hitDirection, -2.5f, 0, default(Color), 0.7f);
				}
				for(int i = 1; i < 8; i++)
					Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, ModGores.GoreType("Gores/NatureConstructGore" + i), 1f);
				for(int i = 0; i < 9; i++)
					Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Main.rand.Next(61,64), 1f);	
			}
		}
		public override void FindFrame(int frameHeight) 
		{
			float speed = Math.Abs(NPC.velocity.X * 0.7f);
			if (speed > 1.67f)
				speed = 1.67f;
			else if (speed <= 0.1f)
            {
				speed = 0;
				NPC.frame.Y = 0;
            }
			NPC.frameCounter += speed;
			if (NPC.frameCounter > 10f) 
			{
				NPC.frame.Y = (NPC.frame.Y + frameHeight);
				if(NPC.frame.Y >= frameHeight * 3)
				{
					NPC.frame.Y = 0;
				}
				NPC.frameCounter = 0;
			}
		}
		int shootingAI = 0;
		public override void AI()
		{
			Player player = Main.player[NPC.target];
			if(delay <= (Main.expertMode ? 30 : 0) && !canSpell)
			{
				delay = 270 + Main.rand.Next(120);
				if(Main.netMode != 1)
					NPC.netUpdate = true;
				canSpell = true;
			}
			if(canSpell)
			{
				shootingAI++;
				float sinPercent = (float)Math.Sin(MathHelper.ToRadians(shootingAI));
				if(shootingAI == 90)
				{
					Vector2 circular2 = new Vector2(40, 0).RotatedBy(dir);
					for (int i = 0; i < 20; i++)
					{
						int dust3 = Dust.NewDust(NPC.Center + circular2 * 0.6f, 0, 0, 267);
						Dust dust4 = Main.dust[dust3];
						dust4.velocity *= 1.2f;
						dust4.velocity += circular2 * Main.rand.NextFloat(0.1f, 0.2f);
						dust4.color = new Color(64, 178, 77);
						dust4.noGravity = true;
						dust4.fadeIn = 0.1f;
						dust4.scale *= 2.0f;
					}
					if (Main.netMode != 1)
					{
						int damage = NPC.GetBaseDamage() / 2;
						for (int i = 0; i < 5; i++)
						{
							Vector2 circular = new Vector2(12, 0).RotatedBy(dir + MathHelper.ToRadians((i - 2) * 12.5f));
							Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center + circular2, circular, ModContent.ProjectileType<NatureBolt>(), damage, 0, Main.myPlayer, Main.rand.NextFloat(30f + i * 15f, 40f + i * 20f), NPC.target);
						}
					}
					NPC.velocity.Y -= 2.0f;
					SOTSUtils.PlaySound(SoundID.Item92, (int)NPC.Center.X, (int)NPC.Center.Y, 1.1f, -0.1f);
                }
				if(sinPercent <= 0)
                {
					shootingAI = 0;
					canSpell = false;
					if (Main.netMode != NetmodeID.MultiplayerClient)
						NPC.netUpdate = true;
				}
				Vector2 playerLocation = player.Center * (1 - sinPercent) + new Vector2(NPC.Center.X, NPC.Center.Y - 128) * sinPercent;
				Vector2 toPlayer = playerLocation - NPC.Center;
				dir = toPlayer.ToRotation();
				speedMod = 0.9f - sinPercent;
				if(speedMod < 0)
					speedMod = 0;
				NPC.velocity.X *= 1 - sinPercent;
			}
			else
            {
				speedMod = 1f;
				dir = (player.Center - NPC.Center).ToRotation();
				delay--;
			}
			if(NPC.velocity.X == 0 && NPC.velocity.Y == 0)
			{
				NPC.aiStyle =3;
				AIType = 73;
				initiateSpeed = -1;
			}
			else if (NPC.velocity.Y == 0 && ai2 == 0)
			{
				ai2 = 1;
			}
			if(ai2 >= 1)
			{
				ai2++;
			}
			if(initiateSpeed == -1 && ai2 >= 5)
			{
				if(ai2 >= 30)
					ai2 = 30;
				if(player.Center.X > NPC.Center.X + 12)
				{
					NPC.velocity.X = 1.1f * NPC.scale * (float)(ai2 / 30f * speedMod);
					NPC.spriteDirection = 1;
				}
				if(player.Center.X < NPC.Center.X - 12)
				{
					NPC.velocity.X = -1.1f * NPC.scale * (float)(ai2 / 30f * speedMod);
					NPC.spriteDirection = -1;
				}
			}
		}
        public override void OnKill()
		{
			int n = NPC.NewNPC(NPC.GetSource_Death(), (int)NPC.Center.X, (int)NPC.Center.Y, ModContent.NPCType<NatureSpirit>());
			Main.npc[n].velocity.Y = -10f;
			Main.npc[n].netUpdate = true;
		}
		public override void ModifyNPCLoot(NPCLoot npcLoot)
		{
			npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<FragmentOfNature>(), 1, 4, 7));
		}
	}
}