using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.NPCs.Boss
{
	public class HookTurret : ModNPC
	{	
		private float aimToX {
			get => npc.ai[0];
			set => npc.ai[0] = value;
		}

		private float aimToY {
			get => npc.ai[1];
			set => npc.ai[1] = value;
		}
		
		private float hookID {
			get => npc.ai[2];
			set => npc.ai[2] = value;
		}
		
		private float rotationAmt {
			get => npc.ai[3];
			set => npc.ai[3] = value;
		}
		private float eyeReset = 0;
		private float fireRate = 0;
		int initiate = -1;
		
		public override void SendExtraAI(BinaryWriter writer) 
		{
			writer.Write(fireRate);
			writer.Write(eyeReset);
		}
		public override void ReceiveExtraAI(BinaryReader reader)
		{	
			fireRate = reader.ReadSingle();
			eyeReset = reader.ReadSingle();
		}
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Putrid Turret");
			NPCID.Sets.TrailCacheLength[npc.type] = 4;  
			NPCID.Sets.TrailingMode[npc.type] = 0;  
		}
		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Texture2D texture = Main.npcTexture[npc.type];
			Vector2 drawOrigin = new Vector2(Main.npcTexture[npc.type].Width * 0.5f, texture.Height * 0.5f);
			for (int k = 0; k < npc.oldPos.Length; k++) {
				Vector2 drawPos = npc.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(0, 4);
				Color color = npc.GetAlpha(lightColor) * ((float)(npc.oldPos.Length - k) / (float)npc.oldPos.Length);
				spriteBatch.Draw(texture, drawPos, null, color * 0.35f, npc.rotation, drawOrigin, npc.scale, SpriteEffects.None, 0f);
			}
			return true;
		}	
		public override void SetDefaults()
		{
			
            npc.aiStyle = -1; 
            npc.lifeMax = 250;   
            npc.damage = 40; 
            npc.defense = 0;  
            npc.knockBackResist = 0f;
            npc.width = 36;
            npc.height = 36;
            npc.value = 0;
            npc.npcSlots = 1f;
            npc.lavaImmune = true;
            npc.noGravity = true;
            npc.noTileCollide = true;
            npc.HitSound = SoundID.NPCHit1;
            npc.DeathSound = SoundID.NPCDeath5;
            npc.netAlways = true;
            npc.buffImmune[20] = true;
		}
		public override void PostDraw(SpriteBatch spriteBatch, Color drawColor)
		{
			Player player = Main.player[npc.target];
			Texture2D texture = ModContent.GetTexture("SOTS/NPCs/Boss/PutridHookEye");
			Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
			Vector2 drawPos = npc.Center - Main.screenPosition;
			
			float shootToX = aimToX - npc.Center.X;
			float shootToY = aimToY - npc.Center.Y;
			float distance = (float)System.Math.Sqrt((double)(shootToX * shootToX + shootToY * shootToY));

			distance = eyeReset * 2f/ distance;
				  
			shootToX *= distance * 5;
			shootToY *= distance * 5;
			
			drawPos.X += shootToX;
			drawPos.Y += -1 + shootToY + (texture.Height * 0.5f);
			if(npc.scale == 1)
			spriteBatch.Draw(texture, drawPos, null, drawColor, npc.rotation, drawOrigin, npc.scale, SpriteEffects.None, 0f);
		}
        public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
        {
            npc.damage = (int)(npc.damage * 0.8f);  
        }
		public override void AI()
		{	
			if(initiate == -1 && Main.netMode != 1)
			{
				npc.velocity = new Vector2(12, 0).RotatedBy(MathHelper.ToRadians(rotationAmt + (hookID * 30)));
				rotationAmt = 124;
				hookID = -1;
				npc.netUpdate = true;
				initiate = 1;
			}
			if(rotationAmt > 8 && hookID == -1)
			{
				rotationAmt--;
				Vector2 aimTo = new Vector2(aimToX - npc.Center.X, aimToY - npc.Center.Y).RotatedBy(MathHelper.ToRadians(rotationAmt));
				aimToX = aimTo.X + npc.Center.X;
				aimToY = aimTo.Y + npc.Center.Y;
				npc.netUpdate = true;
				fireRate += Main.expertMode ? 10.5f : 6.5f;
			}
			fireRate++;
			eyeReset = eyeReset < 1 ? eyeReset + 0.05f : 1;
			if(fireRate >= 180)
			{
				eyeReset = 0.3f;
				fireRate = 0;
				if(Main.netMode != 1)
				{
					npc.netUpdate = true;
					float shootToX = aimToX - npc.Center.X;
					float shootToY = aimToY - npc.Center.Y;
					float distance = (float)System.Math.Sqrt((double)(shootToX * shootToX + shootToY * shootToY));

					distance = (Main.expertMode ? 5.25f : 5f) / distance;
						  
					shootToX *= distance;
					shootToY *= distance;
					
					int damage = npc.damage / 2;
					if (Main.expertMode) 
					{
						damage = (int)(damage / Main.expertDamage);
					}
					
					if(Main.netMode != 1)
					Projectile.NewProjectile(npc.Center.X, npc.Center.Y, shootToX, shootToY, mod.ProjectileType("PinkBullet"), damage, 0, Main.myPlayer);
				}
			}
			
			npc.velocity *= 0.97f;
			Lighting.AddLight(npc.Center, (255 - npc.alpha) * 1f / 155f, (255 - npc.alpha) * 1f / 155f, (255 - npc.alpha) * 1f / 155f);
			npc.rotation += 0.07f;
			int pIndex = -1;
			if(Main.expertMode)
			{
				npc.dontTakeDamage = true;
			}
			for(int i = 0; i < 200; i++)
			{
				NPC npc1 = Main.npc[i];
				if(npc1.type == mod.NPCType("PutridPinkyPhase2") && npc1.active)
				{
					npc.active = true;
					npc.timeLeft = 1000;
					pIndex = i;
					break;
				}
			}
			if(pIndex == -1)
			{
				npc.life--;
				npc.scale *= 0.98f;
				if(npc.life < 50 || npc.scale < 0.4f)
				{
					npc.active = false;
				}
				return;
			}
			NPC putridPinky = Main.npc[pIndex];
			Dust.NewDust(new Vector2(npc.position.X, npc.position.Y), npc.width, npc.height, mod.DustType("BigPinkDust"));
		}
	}
}





















