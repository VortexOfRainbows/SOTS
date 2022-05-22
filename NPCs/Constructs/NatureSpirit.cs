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
			NPCID.Sets.TrailCacheLength[NPC.type] = 5;  
			NPCID.Sets.TrailingMode[NPC.type] = 0;   
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
            NPC.value = 30000;
            NPC.npcSlots = 4f;
            NPC.boss = false;
            NPC.lavaImmune = true;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.HitSound = SoundID.NPCHit54;
            NPC.DeathSound = SoundID.NPCDeath6;
            NPC.netAlways = false;
			NPC.rarity = 2;
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
				int damage = NPC.damage / 2;
				if (Main.expertMode)
				{
					damage = (int)(damage / Main.expertDamage);
				}
				Projectile.NewProjectile(NPC.Center.X, NPC.Center.Y, Main.rand.NextFloat(-5f, 5f), Main.rand.NextFloat(-5f, 5f), Mod.Find<ModProjectile>("NatureBolt").Type, damage, 0, Main.myPlayer, Main.rand.NextFloat(15f, 25f), NPC.target);
			}
			Terraria.Audio.SoundEngine.PlaySound(SoundID.Item, (int)NPC.Center.X, (int)NPC.Center.Y, 92, 0.55f, 0.4f);
		}
		public override void AI()
		{	
			Player player = Main.player[NPC.target];
			if(phase == 3)
			{
				if (Main.netMode != 1)
				{
					NPC.netUpdate = true;
				}
				NPC.dontTakeDamage = false;
				NPC.ai[0]++;
				float speed = 6f;
				Vector2 rotatePos = new Vector2(NPC.ai[1], 0).RotatedBy(MathHelper.ToRadians(NPC.ai[0] * 0.6f));
				rotatePos = player.Center + rotatePos;
				Vector2 vectorTo = rotatePos - NPC.Center;
				float distance = vectorTo.Length();
				if(NPC.ai[1] < 100)
				{
					speed = 1f;
				}
				Vector2 goTo = new Vector2((speed < distance ? speed : distance), 0).RotatedBy(Math.Atan2(vectorTo.Y, vectorTo.X));
				NPC.velocity = goTo;
				if((int)NPC.ai[0] % 20 == 0)
				{
					SpellLaunch();
				}
				if(NPC.ai[0] >= 600)
				{
					NPC.ai[1]--;
					if(NPC.ai[0] >= 1100)
					{
						if(NPC.ai[1] < -400)
						{
							NPC.ai[1] = 340;
						}
						NPC.ai[0] = 0;
					}
				}
			}
			if(phase == 2)
			{
				if (Main.netMode != 1)
				{
					NPC.netUpdate = true;
				}
				NPC.dontTakeDamage = false;
				NPC.aiStyle =-1;
				NPC.ai[0] = 0;
				NPC.ai[1] = 300;
				phase = 3;
			}
			else if(phase == 1)
			{
				counter++;
			}
			if(Main.player[NPC.target].dead)
			{
				counter++;
			}
			if(counter >= 1020)
			{
				if (Main.netMode != 1)
				{
					NPC.netUpdate = true;
				}
				phase = 1;
				NPC.aiStyle =-1;
				NPC.velocity.Y -= 0.014f;
				NPC.dontTakeDamage = true;
			}
			int dust2 = Dust.NewDust(new Vector2(NPC.position.X, NPC.position.Y), NPC.width, NPC.height, 267);
			Dust dust = Main.dust[dust2];
			dust.color = new Color(64, 178, 77);
			dust.noGravity = true;
			dust.fadeIn = 0.1f;
			dust.scale *= 2f;
		}
		public override bool PreDraw(ref Color lightColor)
		{
			Texture2D texture = Terraria.GameContent.TextureAssets.Npc[NPC.type].Value;
			Vector2 drawOrigin = new Vector2(Terraria.GameContent.TextureAssets.Npc[NPC.type].Value.Width * 0.5f, NPC.height * 0.5f);
			for (int k = 0; k < NPC.oldPos.Length; k++) {
				Vector2 drawPos = NPC.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(0f, NPC.gfxOffY);
				Color color = NPC.GetAlpha(lightColor) * ((float)(NPC.oldPos.Length - k) / (float)NPC.oldPos.Length);
				spriteBatch.Draw(texture, drawPos, null, color * 0.5f, NPC.rotation, drawOrigin, NPC.scale, SpriteEffects.None, 0f);
			}
			return false;
		}	
		public override void HitEffect(int hitDirection, double damage)
		{
			if (NPC.life <= 0)
			{
				for (int i = 0; i < 50; i++)
				{
					int dust3 = Dust.NewDust(new Vector2(NPC.position.X, NPC.position.Y), NPC.width, NPC.height, 267);
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
					NPC.life = (int)(InitiateHealth * (Main.expertMode ? ExpertHealthMult : 1));
				}
			}
		}
		public override void PostDraw(SpriteBatch spriteBatch, Color drawColor)
		{
			Texture2D texture = Terraria.GameContent.TextureAssets.Npc[NPC.type].Value;
			Color color = new Color(100, 100, 100, 0);
			Vector2 drawOrigin = new Vector2(Terraria.GameContent.TextureAssets.Npc[NPC.type].Value.Width * 0.5f, NPC.height * 0.5f);
			for (int k = 0; k < 7; k++)
			{
				float x = Main.rand.Next(-10, 11) * 0.45f;
				float y = Main.rand.Next(-10, 11) * 0.45f;
				Main.spriteBatch.Draw(texture,
				new Vector2((float)(NPC.Center.X - (int)Main.screenPosition.X) + x, (float)(NPC.Center.Y - (int)Main.screenPosition.Y) + y),
				null, color, 0f, drawOrigin, 1f, SpriteEffects.None, 0f);
			}
			base.PostDraw(spriteBatch, drawColor);
		}
		public override void NPCLoot()
		{
			Item.NewItem((int)NPC.position.X, (int)NPC.position.Y, NPC.width, NPC.height,  Mod.Find<ModItem>("DissolvingNature").Type, 1);	
		}	
	}
}
