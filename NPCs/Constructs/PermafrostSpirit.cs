using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;


namespace SOTS.NPCs.Constructs
{
	public class PermafrostSpirit : ModNPC
	{	
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Permafrost Spirit");
			NPCID.Sets.TrailCacheLength[npc.type] = 5;  
			NPCID.Sets.TrailingMode[npc.type] = 0;
		}
		public override void SendExtraAI(BinaryWriter writer)
		{
			writer.Write(phase);
			writer.Write(counter);
		}
		public override void ReceiveExtraAI(BinaryReader reader)
		{
			phase = reader.ReadInt32();
			counter = reader.ReadInt32();
		}
		public override void SetDefaults()
		{
			npc.aiStyle = 10;
            npc.lifeMax = 700; 
            npc.damage = 60; 
            npc.defense = 0;   
            npc.knockBackResist = 0f;
            npc.width = 58;
            npc.height = 58;
			Main.npcFrameCount[npc.type] = 1;   
            npc.value = 35075;
            npc.npcSlots = 4f;
            npc.boss = false;
            npc.lavaImmune = true;
            npc.noGravity = true;
            npc.noTileCollide = true;
            npc.HitSound = SoundID.NPCHit54;
            npc.DeathSound = SoundID.NPCDeath6;
            npc.netAlways = false;
			npc.rarity = 2;
		}
		public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
		{
			npc.damage = 80;
			npc.lifeMax = 1200;
		}
		private int InitiateHealth = 2000;
		private float ExpertHealthMult = 1.5f;
		
		int phase = 1;
		int counter = 0;
		public void SpellLaunch(Vector2 velocity)
		{
			if(npc.ai[3] <= 0)
			{
				int damage = npc.damage / 2;
				if (Main.expertMode) 
				{
					damage = (int)(damage / Main.expertDamage);
				}
				if(Main.netMode != 1)
				Projectile.NewProjectile(npc.Center.X, npc.Center.Y, velocity.X, velocity.Y,  mod.ProjectileType("PermafrostSpike"), damage, 0, Main.myPlayer, 180 - npc.ai[1]);
			}
			else
			{
				int damage = (int)npc.ai[3] / 2;
				if (Main.expertMode) 
				{
					damage = (int)(damage / Main.expertDamage);
				}
				if(Main.netMode != 1)
				Projectile.NewProjectile(npc.Center.X, npc.Center.Y, velocity.X, velocity.Y,  mod.ProjectileType("PermafrostSpike"), damage, 0, Main.myPlayer, 180 - npc.ai[1]);
			}
			//Main.PlaySound(SoundID.Item92, (int)(npc.Center.X), (int)(npc.Center.Y));
		}
		public override void AI()
		{
			Lighting.AddLight(npc.Center, (255 - npc.alpha) * 0.15f / 255f, (255 - npc.alpha) * 0.25f / 255f, (255 - npc.alpha) * 0.65f / 255f);
			Player player = Main.player[npc.target];
			if(phase == 3)
			{
				npc.dontTakeDamage = false;
				if (Main.netMode != 1)
				{
					npc.netUpdate = true;
				}
				if ((int)npc.ai[0] % 270 == 269)
				{
					npc.ai[1]++;
					if ((int)npc.ai[1] % 10 == 0)
					{
						Vector2 toPlayer = player.Center - npc.Center;
						toPlayer.Normalize();
						toPlayer *= Main.rand.Next(6,10);
						toPlayer.X += Main.rand.Next(-1, 2);
						toPlayer.Y += Main.rand.Next(-1, 2);
						SpellLaunch(toPlayer);
					}
					if (npc.ai[1] >= 180)
					{
						Main.PlaySound(SoundID.Item46, (int)(npc.Center.X), (int)(npc.Center.Y));
						npc.ai[1] = 0;
						npc.ai[0]++;
					}
				}
				else
				{
					npc.ai[0]++;
				}
				Vector2 circleGen = new Vector2(45, 0).RotatedBy(MathHelper.ToRadians(npc.ai[0]));
				Vector2 rotatePos = new Vector2(-360, 0).RotatedBy(MathHelper.ToRadians(circleGen.X + 90));


				Vector2 rotateAround = new Vector2(npc.ai[1], 0).RotatedBy(MathHelper.ToRadians(npc.ai[1] * 2));

				Vector2 toCircle = rotateAround + rotatePos + player.Center - npc.Center;
				toCircle.Normalize();
				toCircle *= 4f;

				npc.velocity = toCircle;
			}
			if(phase == 2)
			{
				if (Main.netMode != 1)
				{
					npc.netUpdate = true;
				}
				npc.dontTakeDamage = false;
				npc.aiStyle = -1;
				npc.ai[0] = 0;
				npc.ai[1] = 0;
				phase = 3;
			}
			else if(phase == 1)
			{
				counter++;
			}
			if(Main.player[npc.target].dead)
			{
				counter++;
			}
			if(counter >= 1200)
			{
				if (Main.netMode != 1)
				{
					npc.netUpdate = true;
				}
				phase = 1;
				npc.aiStyle = -1;
				npc.velocity.Y -= 0.014f;
				npc.dontTakeDamage = true;
			}
			int dust2 = Dust.NewDust(new Vector2(npc.position.X, npc.position.Y), npc.width, npc.height, 267);
			Dust dust = Main.dust[dust2];
			dust.color = new Color(45, 96, 114);
			dust.noGravity = true;
			dust.fadeIn = 0.1f;
			dust.scale *= 2f;
		}
		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Texture2D texture = Main.npcTexture[npc.type];
			Vector2 drawOrigin = new Vector2(Main.npcTexture[npc.type].Width * 0.5f, npc.height * 0.5f);
			for (int k = 0; k < npc.oldPos.Length; k++) {
				Vector2 drawPos = npc.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(0f, npc.gfxOffY);
				Color color = npc.GetAlpha(lightColor) * ((float)(npc.oldPos.Length - k) / (float)npc.oldPos.Length);
				spriteBatch.Draw(texture, drawPos, null, color * 0.5f, npc.rotation, drawOrigin, npc.scale, SpriteEffects.None, 0f);
			}
			return false;
		}	
		public override void HitEffect(int hitDirection, double damage)
		{
			if (npc.life <= 0)
			{
				for(int i = 0; i < 50; i ++)
				{
				int dust = Dust.NewDust(new Vector2(npc.position.X, npc.position.Y), npc.width, npc.height, mod.DustType("BigPermafrostDust"));
				Main.dust[dust].velocity *= 5f;
				}
				if(phase == 1)
				{
					phase = 2;
					npc.lifeMax = (int)(InitiateHealth * (Main.expertMode ? ExpertHealthMult : 1));
					npc.life = (int)(InitiateHealth * (Main.expertMode ? ExpertHealthMult : 1));
				}
			}
		}
		public override void PostDraw(SpriteBatch spriteBatch, Color drawColor)
		{
			Texture2D texture = Main.npcTexture[npc.type];
			Color color = new Color(100, 100, 100, 0);
			Vector2 drawOrigin = new Vector2(Main.npcTexture[npc.type].Width * 0.5f, npc.height * 0.5f);
			for (int k = 0; k < 7; k++)
			{
				float x = Main.rand.Next(-10, 11) * 0.45f;
				float y = Main.rand.Next(-10, 11) * 0.45f;
				Main.spriteBatch.Draw(texture,
				new Vector2((float)(npc.Center.X - (int)Main.screenPosition.X) + x, (float)(npc.Center.Y - (int)Main.screenPosition.Y) + y),
				null, color, 0f, drawOrigin, 1f, SpriteEffects.None, 0f);
			}
			base.PostDraw(spriteBatch, drawColor);
		}
		public override void NPCLoot()
		{
			Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height,  mod.ItemType("DissolvingAurora"), 1);	
		}	
	}
}
