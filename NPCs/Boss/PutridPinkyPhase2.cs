using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.NPCs.Boss
{	[AutoloadBossHead]
	public class PutridPinkyPhase2 : ModNPC
	{
		int expertModifier = 1;
		
		private float attackPhase {
			get => npc.ai[0];
			set => npc.ai[0] = value;
		}

		private float attackTimer {
			get => npc.ai[1];
			set => npc.ai[1] = value;
		}

		private float rotationSpeed {
			get => npc.ai[2];
			set => npc.ai[2] = value;
		}

		private float rotationDistance {
			get => npc.ai[3];
			set => npc.ai[3] = value;
		}
		
		private float fireToX = 0;
		private float fireToY = 0;
		private float followPlayer = 0;
		private float eyeReset = 0;
		
		public override void SendExtraAI(BinaryWriter writer) 
		{
			writer.Write(fireToX);
			writer.Write(fireToY);
			writer.Write(followPlayer);
			writer.Write(eyeReset);
		}
		public override void ReceiveExtraAI(BinaryReader reader)
		{	
			fireToX = reader.ReadSingle();
			fireToY = reader.ReadSingle();
			followPlayer = reader.ReadSingle();
			eyeReset = reader.ReadSingle();
		}
		public override void SetStaticDefaults()
		{
			
			DisplayName.SetDefault("Putrid Pinky");
		}
		public override void SetDefaults()
		{
			
            npc.aiStyle = -1;   
			npc.lifeMax = 5500;
            npc.damage = 37;   
            npc.defense = 0;   
            npc.knockBackResist = 0f;
            npc.width = 116;
            npc.height = 116;
            Main.npcFrameCount[npc.type] = 1;   
            npc.value = 150000;
            npc.npcSlots = 10f;
            npc.boss = true;
            npc.lavaImmune = true;
            npc.noGravity = true;
            npc.noTileCollide = false;
            npc.HitSound = SoundID.NPCHit1;
            npc.DeathSound = SoundID.NPCDeath5;
            npc.buffImmune[20] = true;
            music = MusicID.Boss3;
			bossBag = mod.ItemType("PinkyBag");
			
			//bossBag = mod.ItemType("BossBagBloodLord");
		}
		public override bool PreDraw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch, Color lightColor)
        {
            Texture2D texture = ModContent.GetTexture("SOTS/Projectiles/PutridVine");  
            
			for(int i = 0; i < 200; i++)
			{
				int alpha = 0;
				if(Main.npc[i].type == mod.NPCType("PutridHook") && Main.npc[i].active)
				{
					Vector2 position = npc.Center;
					Vector2 mountedCenter = Main.npc[i].Center;
					Microsoft.Xna.Framework.Rectangle? sourceRectangle = new Microsoft.Xna.Framework.Rectangle?();
					Vector2 origin = new Vector2((float)texture.Width * 0.5f, (float)texture.Height * 0.5f);
					float num1 = (float)texture.Height;
					Vector2 vector2_4 = mountedCenter - position;
					float rotation = (float)Math.Atan2((double)vector2_4.Y, (double)vector2_4.X) - 1.57f;
					bool flag = true;
					if (float.IsNaN(position.X) && float.IsNaN(position.Y))
						flag = false;
					if (float.IsNaN(vector2_4.X) && float.IsNaN(vector2_4.Y))
						flag = false;
					while (flag)
					{
						if ((double)vector2_4.Length() - texture.Height < (double)num1 + 1.0)
						{
							flag = false;
						}
						else
						{
							Vector2 vector2_1 = vector2_4;
							vector2_1.Normalize();
							position += vector2_1 * num1;
							vector2_4 = mountedCenter - position;
							Microsoft.Xna.Framework.Color color2 = Lighting.GetColor((int)position.X / 16, (int)((double)position.Y / 16.0));
							color2 = npc.GetAlpha(color2);
							color2.A = (byte)(255 - alpha);
							alpha += 10;
							Main.spriteBatch.Draw(texture, position - Main.screenPosition, sourceRectangle, color2, rotation, origin, 1f, SpriteEffects.None, 0.0f);
						}
					}
				}
			}
			return true;
        }
        public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
        {
            npc.lifeMax = (int)(npc.lifeMax * bossLifeScale * 0.7f);  //boss life scale in expertmode
            npc.damage = (int)(npc.damage * 1.2f);  //boss damage increase in expermode
        }
		
		public override bool PreAI()
		{
			if(Main.expertMode)
			{
				expertModifier = 2;
			}
			return true;
		}
		public override void PostDraw(SpriteBatch spriteBatch, Color drawColor)
		{
			Texture2D texture = ModContent.GetTexture("SOTS/NPCs/Boss/PutridPinkyEye");
			Vector2 drawOrigin = new Vector2(texture.Width/2, texture.Height/2);
			Vector2 drawPos = npc.Center - Main.screenPosition;
			
			float shootToX = fireToX - npc.Center.X;
			float shootToY = fireToY - npc.Center.Y;
			float distance = (float)System.Math.Sqrt((double)(shootToX * shootToX + shootToY * shootToY));

			distance = eyeReset * 1.2f * (npc.scale) / distance;
				  
			shootToX *= distance * 5;
			shootToY *= distance * 5;
			
			drawPos.X += shootToX;
			drawPos.Y += 4 + (shootToY * 2);

			if(npc.scale == 1)
			spriteBatch.Draw(texture, drawPos, null, drawColor, npc.rotation, drawOrigin, npc.scale, SpriteEffects.None, 0f);
		}
		public override void AI()
		{
			npc.TargetClosest(true);
			Player player  = Main.player[npc.target];
			
			float shootToX = player.Center.X - npc.Center.X;
			float shootToY = player.Center.Y - npc.Center.Y;
			float distance = (float)System.Math.Sqrt((double)(shootToX * shootToX + shootToY * shootToY));

			distance = 1f / distance;
				  
			shootToX *= distance * 5;
			shootToY *= distance * 5;
			if(attackPhase == 0)
			{
				eyeReset = 1;
				attackPhase = 1;
				attackTimer = 900;
				followPlayer = 1;
				
				if(Main.netMode != 1)
				npc.netUpdate = true;
			
				InitiateHooks();
				return;
			}
			if(attackPhase == 1)
			{
				eyeReset = eyeReset < 1 ? eyeReset + 0.08f : 1;
				attackTimer--;
				if(followPlayer == 1)
				{
					fireToX = player.Center.X;
					fireToY = player.Center.Y;
				}
				if(attackTimer == 600)
				{
					followPlayer = 0;
				}
				if(attackTimer == 570)
				{
					LaunchLaser(npc.Center, new Vector2(fireToX, fireToY));
					followPlayer = 1;
				}
				if(attackTimer == 540)
				{
					followPlayer = 0;
				}
				if(attackTimer == 510)
				{
					LaunchLaser(npc.Center, new Vector2(fireToX, fireToY));
					followPlayer = 1;
				}
				if(attackTimer == 480)
				{
					followPlayer = 0;
				}
				if(attackTimer == 450)
				{
					LaunchLaser(npc.Center, new Vector2(fireToX, fireToY));
					attackTimer = 900;
					followPlayer = 1;
				}
				rotationSpeed = 0.4f;
				rotationDistance += rotationDistance < 120 ? 1 : rotationDistance > 120 ? -1 : 0;
				
				if(Main.netMode != 1)
				npc.netUpdate = true;
			}
			for(int i = 0; i < 200; i++)
			{
				if(Main.npc[i].type == mod.NPCType("PutridHook") && Main.npc[i].active)
				{
					Main.npc[i].ai[0] = player.Center.X;
					Main.npc[i].ai[1] = player.Center.Y;
				}
			}
		}
		private void LaunchLaser(Vector2 fromArea, Vector2 toArea)
		{
			int damage = npc.damage / 2;
			if (Main.expertMode) 
			{
				damage = (int)(damage / Main.expertDamage);
			}
			Vector2 direction = fromArea - toArea;
			direction *= 6f;
			if(direction.Length() > 1500)
			{
				direction.Normalize();
				direction *= 1500;
			}
			if(Main.netMode != 1)
			Projectile.NewProjectile(fromArea.X, fromArea.Y, 0, 0, mod.ProjectileType("PinkLaser"), damage, 0, Main.myPlayer, toArea.X - direction.X, toArea.Y - direction.Y);
			//NetMessage.SendData(27, -1, -1, null, Probe);
			eyeReset = -0.8f;
		}
		private void InitiateHooks()
		{
			Player player  = Main.player[npc.target];
			if(Main.netMode != 1)
			{
				for(int i = 0; i < 12; i++)
				{
					int npcProj = NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, mod.NPCType("PutridHook"));	
					Main.npc[npcProj].ai[0] = player.Center.X;
					Main.npc[npcProj].ai[1] = player.Center.Y;
					Main.npc[npcProj].ai[2] = i;
					Main.npc[npcProj].netUpdate = true;
				}
			}
		}
		public override void HitEffect(int hitDirection, double damage) 
		{
			if (npc.life > 0) 
			{
				for (int i = 0; i < 10; i++) 
				{
					Dust dust = Dust.NewDustDirect(npc.position, npc.width, npc.height, 72, hitDirection, -1f, 100, new Color(100, 100, 100, 100), 1f);
					dust.noGravity = true;
				}
				return;
			}
		}
		public override void BossLoot(ref string name, ref int potionType)
		{ 
			SOTSWorld.downedPinky = true;
			potionType = ItemID.HealingPotion;
		
			if(Main.expertMode)
			{ 
				npc.DropBossBags();
			} 
			else 
			{
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("WormWoodCore"), 1); 
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemID.PinkGel, Main.rand.Next(20,50)); 
			}
		}
	}
}