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
			npc.aiStyle = 10;
            npc.lifeMax = 300; 
            npc.damage = 85; 
            npc.defense = 0;   
            npc.knockBackResist = 0f;
            npc.width = 58;
            npc.height = 58;
			Main.npcFrameCount[npc.type] = 1;   
            npc.value = 30000;
            npc.npcSlots = 4f;
            npc.boss = false;
            npc.lavaImmune = true;
            npc.noGravity = true;
            npc.noTileCollide = true;
            npc.HitSound = SoundID.NPCHit54;
            npc.DeathSound = SoundID.NPCDeath6;
            npc.netAlways = false;
			music = MusicID.Boss3;
		}
		int phase = 1;
		int counter = 0;
		public void SpellLaunch()
		{
			if(npc.damage > 0)
			{
				Projectile.NewProjectile(npc.Center.X, npc.Center.Y, npc.velocity.Y * -1, npc.velocity.X * -1,  mod.ProjectileType("NatureBolt"), (int)(npc.damage * 0.3f * (Main.expertMode ? 0.7f : 1)), 0, 0, 20, npc.target);
			}
			else
			{
				Projectile.NewProjectile(npc.Center.X, npc.Center.Y, npc.velocity.Y * -1, npc.velocity.X * -1,  mod.ProjectileType("NatureBolt"), (int)(npc.ai[2] * 0.3f * (Main.expertMode ? 0.7f : 1)), 0, 0, 20, npc.target);
			}
			//Main.PlaySound(SoundID.Item92, (int)(npc.Center.X), (int)(npc.Center.Y));
		}
		public override void AI()
		{	
			Player player = Main.player[npc.target];
			if(phase == 3)
			{
				npc.ai[0]++;
				Vector2 rotatePos = new Vector2(npc.ai[1], 0).RotatedBy(MathHelper.ToRadians(npc.ai[0] * 0.6f));
				rotatePos = player.Center + rotatePos;
				Vector2 vectorTo = rotatePos - npc.Center;
				float distance = vectorTo.Length();
				Vector2 goTo = new Vector2((6 < distance ? 6 : distance), 0).RotatedBy(Math.Atan2(vectorTo.Y, vectorTo.X));
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
				npc.dontTakeDamage = false;
				npc.aiStyle = -1;
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
			if(counter >= 420)
			{
				phase = 1;
				npc.aiStyle = -1;
				npc.velocity.Y -= 0.014f;
				npc.dontTakeDamage = true;
			}
			Dust.NewDust(new Vector2(npc.position.X, npc.position.Y), npc.width, npc.height, mod.DustType("BigNatureDust"));
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
			return true;
		}	
		public override void HitEffect(int hitDirection, double damage)
		{
			if (npc.life <= 0)
			{
				for(int i = 0; i < 50; i ++)
				{
				int dust = Dust.NewDust(new Vector2(npc.position.X, npc.position.Y), npc.width, npc.height, mod.DustType("BigNatureDust"));
				Main.dust[dust].velocity *= 5f;
				}
				if(phase == 1)
				{
					phase = 2;
					npc.lifeMax = (int)(2000 * (Main.expertMode ? 1.5 : 1));
					npc.life = (int)(2000 * (Main.expertMode ? 1.5 : 1));
				}
			}
		}
		public override void NPCLoot()
		{
			
		}	
	}
}
