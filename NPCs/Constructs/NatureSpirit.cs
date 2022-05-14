using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;


namespace SOTS.NPCs.Constructs
{
	public class NatureSpirit : ModNPC
	{	
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Nature Spirit");
			NPCID.Sets.TrailCacheLength[npc.type] = 5;  
			NPCID.Sets.TrailingMode[npc.type] = 0;   
		}
		public override void SetDefaults()
		{
			NPC.aiStyle =10;
            NPC.lifeMax = 275; 
            NPC.damage = 30; 
            NPC.defense = 0;   
            NPC.knockBackResist = 0f;
            NPC.width = 58;
            NPC.height = 58;
			Main.npcFrameCount[NPC.type] = 1;   
            npc.value = 30000;
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
			NPC.damage = 44;
			NPC.lifeMax = 400;
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
		private int InitiateHealth = 1000;
		private float ExpertHealthMult = 1.5f;
		
		int phase = 1;
		int counter = 0;
		public void SpellLaunch()
		{
			if (Main.netMode != 1)
			{
				int damage = npc.damage / 2;
				if (Main.expertMode)
				{
					damage = (int)(damage / Main.expertDamage);
				}
				Projectile.NewProjectile(npc.Center.X, npc.Center.Y, Main.rand.NextFloat(-5f, 5f), Main.rand.NextFloat(-5f, 5f), mod.ProjectileType("NatureBolt"), damage, 0, Main.myPlayer, Main.rand.NextFloat(15f, 25f), npc.target);
			}
			SoundEngine.PlaySound(SoundID.Item, (int)npc.Center.X, (int)npc.Center.Y, 92, 0.55f, 0.4f);
		}
		public override void AI()
		{	
			Player player = Main.player[npc.target];
			if(phase == 3)
			{
				if (Main.netMode != 1)
				{
					npc.netUpdate = true;
				}
				npc.dontTakeDamage = false;
				npc.ai[0]++;
				float speed = 6f;
				Vector2 rotatePos = new Vector2(npc.ai[1], 0).RotatedBy(MathHelper.ToRadians(npc.ai[0] * 0.6f));
				rotatePos = player.Center + rotatePos;
				Vector2 vectorTo = rotatePos - npc.Center;
				float distance = vectorTo.Length();
				if(npc.ai[1] < 100)
				{
					speed = 1f;
				}
				Vector2 goTo = new Vector2((speed < distance ? speed : distance), 0).RotatedBy(Math.Atan2(vectorTo.Y, vectorTo.X));
				npc.velocity = goTo;
				if((int)npc.ai[0] % 20 == 0)
				{
					SpellLaunch();
				}
				if(npc.ai[0] >= 600)
				{
					npc.ai[1]--;
					if(npc.ai[0] >= 1100)
					{
						if(npc.ai[1] < -400)
						{
							npc.ai[1] = 340;
						}
						npc.ai[0] = 0;
					}
				}
			}
			if(phase == 2)
			{
				if (Main.netMode != 1)
				{
					npc.netUpdate = true;
				}
				npc.dontTakeDamage = false;
				NPC.aiStyle =-1;
				npc.ai[0] = 0;
				npc.ai[1] = 300;
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
			if(counter >= 1020)
			{
				if (Main.netMode != 1)
				{
					npc.netUpdate = true;
				}
				phase = 1;
				NPC.aiStyle =-1;
				npc.velocity.Y -= 0.014f;
				npc.dontTakeDamage = true;
			}
			int dust2 = Dust.NewDust(new Vector2(npc.position.X, npc.position.Y), npc.width, npc.height, 267);
			Dust dust = Main.dust[dust2];
			dust.color = new Color(64, 178, 77);
			dust.noGravity = true;
			dust.fadeIn = 0.1f;
			dust.scale *= 2f;
		}
		public override bool PreDraw(ref Color lightColor)
		{
			Texture2D texture = Terraria.GameContent.TextureAssets.Npc[npc.type].Value;
			Vector2 drawOrigin = new Vector2(Terraria.GameContent.TextureAssets.Npc[npc.type].Value.Width * 0.5f, npc.height * 0.5f);
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
				for (int i = 0; i < 50; i++)
				{
					int dust3 = Dust.NewDust(new Vector2(npc.position.X, npc.position.Y), npc.width, npc.height, 267);
					Dust dust4 = Main.dust[dust3];
					dust4.velocity *= 2.5f;
					dust4.color = new Color(64, 178, 77);
					dust4.noGravity = true;
					dust4.fadeIn = 0.1f;
					dust4.scale *= 2.5f;
				}
				if (phase == 1)
				{
					phase = 2;
					NPC.lifeMax = (int)(InitiateHealth * (Main.expertMode ? ExpertHealthMult : 1));
					npc.life = (int)(InitiateHealth * (Main.expertMode ? ExpertHealthMult : 1));
				}
			}
		}
		public override void PostDraw(SpriteBatch spriteBatch, Color drawColor)
		{
			Texture2D texture = Terraria.GameContent.TextureAssets.Npc[npc.type].Value;
			Color color = new Color(100, 100, 100, 0);
			Vector2 drawOrigin = new Vector2(Terraria.GameContent.TextureAssets.Npc[npc.type].Value.Width * 0.5f, npc.height * 0.5f);
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
			Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height,  mod.ItemType("DissolvingNature"), 1);	
		}	
	}
}
