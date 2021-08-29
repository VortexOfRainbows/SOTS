using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
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
			
			DisplayName.SetDefault("Nature Construct");
		}
		public override void SetDefaults()
		{
			npc.aiStyle = 0;
			npc.lifeMax = 125;  
			npc.damage = 20; 
			npc.defense = 6;  
			npc.knockBackResist = 0.1f;
			npc.width = 120;
			npc.height = 70;
			Main.npcFrameCount[npc.type] = 3;  
			npc.value = 3330;
			npc.npcSlots = 3f;
			npc.boss = false;
			npc.lavaImmune = false;
			npc.noGravity = false;
			npc.noTileCollide = false;
			npc.netAlways = true;
			npc.HitSound = SoundID.NPCHit4;
			npc.DeathSound = SoundID.NPCDeath14;
			npc.rarity = 5;
		}
		public override void PostDraw(SpriteBatch spriteBatch, Color drawColor)
		{
			Player player = Main.player[npc.target];
			Texture2D texture = ModContent.GetTexture("SOTS/NPCs/Constructs/NatureConstructHead");
			Texture2D texture2 = ModContent.GetTexture("SOTS/NPCs/Constructs/NatureConstructHeadGlow");
			Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
			Vector2 drawPos = npc.Center - Main.screenPosition + new Vector2(0f, npc.gfxOffY);
			if(npc.frame.Y == 70) //frame 2
				drawPos.Y -= 4;
			if(npc.frame.Y == 140) //frame 3
				drawPos.Y -= 2;
			bool flip = false;
			Vector2 toPlayer = player.Center - npc.Center;
			if(Math.Abs(MathHelper.WrapAngle(toPlayer.ToRotation())) <= MathHelper.ToRadians(90))
            {
				flip = true;
            }
			float bonusDir = !flip ? MathHelper.ToRadians(180) : 0;
			spriteBatch.Draw(texture, drawPos, null, drawColor, dir - bonusDir, drawOrigin, npc.scale, flip ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0f);
			spriteBatch.Draw(texture2, drawPos, null, Color.White, dir - bonusDir, drawOrigin, npc.scale, flip ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0f);
		}
		public override void HitEffect(int hitDirection, double damage)
		{
			ai2 = 0;
			if (npc.life <= 0)
			{
				for (int k = 0; k < 20; k++)
				{
					Dust.NewDust(npc.position, npc.width, npc.height, 82, 2.5f * (float)hitDirection, -2.5f, 0, default(Color), 0.7f);
				}
				for(int i = 0; i < 30; i ++)
				{
					int dust = Dust.NewDust(new Vector2(npc.position.X, npc.position.Y), npc.width, npc.height, mod.DustType("BigNatureDust"));
					Main.dust[dust].velocity *= 5f;
				}
				for(int i = 1; i < 8; i++)
					Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/NatureConstructGore" + i), 1f);
				for(int i = 0; i < 9; i++)
					Gore.NewGore(npc.position, npc.velocity, Main.rand.Next(61,64), 1f);	
			}
		}
		public override void FindFrame(int frameHeight) 
		{
			float speed = Math.Abs(npc.velocity.X * 0.7f);
			if (speed > 1.67f)
				speed = 1.67f;
			else if (speed <= 0.1f)
            {
				speed = 0;
				npc.frame.Y = 0;
            }
			npc.frameCounter += speed;
			if (npc.frameCounter > 10f) 
			{
				npc.frame.Y = (npc.frame.Y + frameHeight);
				if(npc.frame.Y >= frameHeight * 3)
				{
					npc.frame.Y = 0;
				}
				npc.frameCounter = 0;
			}
		}
		int shootingAI = 0;
		public override void AI()
		{
			Player player = Main.player[npc.target];
			if(delay <= (Main.expertMode ? 30 : 0) && !canSpell)
			{
				delay = 270 + Main.rand.Next(120);
				if(Main.netMode != 1)
					npc.netUpdate = true;
				canSpell = true;
			}
			if(canSpell)
			{
				shootingAI++;
				float sinPercent = (float)Math.Sin(MathHelper.ToRadians(shootingAI));
				if(shootingAI == 90)
				{
					if (Main.netMode != 1)
					{
						int damage = npc.damage / 2;
						if (Main.expertMode)
						{
							damage = (int)(damage / Main.expertDamage);
						}
						Vector2 circular2 = new Vector2(40, 0).RotatedBy(dir);
						for (int i = 0; i < 5; i++)
						{
							Vector2 circular = new Vector2(12, 0).RotatedBy(dir + MathHelper.ToRadians((i - 2) * 12.5f));
							Projectile.NewProjectile(npc.Center + circular2, circular, mod.ProjectileType("NatureBolt"), damage, 0, Main.myPlayer, Main.rand.NextFloat(30f + i * 15f, 40f + i * 20f), npc.target);
						}
					}
					npc.velocity.Y -= 2.0f;
					Main.PlaySound(SoundID.Item, (int)npc.Center.X, (int)npc.Center.Y, 92, 1.1f, -0.1f);
                }
				if(sinPercent <= 0)
                {
					shootingAI = 0;
					canSpell = false;
					if (Main.netMode != 1)
						npc.netUpdate = true;
				}
				Vector2 playerLocation = player.Center * (1 - sinPercent) + new Vector2(npc.Center.X, npc.Center.Y - 128) * sinPercent;
				Vector2 toPlayer = playerLocation - npc.Center;
				dir = toPlayer.ToRotation();
				speedMod = 0.9f - sinPercent;
				if(speedMod < 0)
					speedMod = 0;
				npc.velocity.X *= 1 - sinPercent;
			}
			else
            {
				speedMod = 1f;
				dir = (player.Center - npc.Center).ToRotation();
				delay--;
			}
			if(npc.velocity.X == 0 && npc.velocity.Y == 0)
			{
				npc.aiStyle = 3;
				aiType = 73;
				initiateSpeed = -1;
			}
			else if (npc.velocity.Y == 0 && ai2 == 0)
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
				if(player.Center.X > npc.Center.X + 12)
				{
					npc.velocity.X = 1.1f * npc.scale * (float)(ai2 / 30f * speedMod);
					npc.spriteDirection = 1;
				}
				if(player.Center.X < npc.Center.X - 12)
				{
					npc.velocity.X = -1.1f * npc.scale * (float)(ai2 / 30f * speedMod);
					npc.spriteDirection = -1;
				}
			}
		}
		public override void NPCLoot()
		{
			int n = NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, mod.NPCType("NatureSpirit"));	
			Main.npc[n].velocity.Y = -10f;
			Main.npc[n].netUpdate = true;
			Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height,  mod.ItemType("FragmentOfNature"), Main.rand.Next(4) + 4);	
		}	
	}
}