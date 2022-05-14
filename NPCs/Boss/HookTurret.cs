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
		private float owner
		{
			get => npc.localAI[0];
			set => npc.localAI[0] = value;
		}
		private float rotationAmtStorage
		{
			get => npc.localAI[1];
			set => npc.localAI[1] = value;
		}
		private float eyeReset = 0;
		private float fireRate = 0;
		int initiate = -1;
		public override bool CheckActive()
		{
			return false;
		}
		public override void SendExtraAI(BinaryWriter writer) 
		{
			writer.Write(fireRate);
			writer.Write(eyeReset);
			writer.Write(owner);
		}
		public override void ReceiveExtraAI(BinaryReader reader)
		{	
			fireRate = reader.ReadSingle();
			eyeReset = reader.ReadSingle();
			owner = reader.ReadSingle();
		}
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Putrid Turret");
			NPCID.Sets.TrailCacheLength[npc.type] = 4;  
			NPCID.Sets.TrailingMode[npc.type] = 0;  
		}
		public override bool PreDraw(ref Color lightColor)
		{
			Texture2D texture = Terraria.GameContent.TextureAssets.Npc[npc.type].Value;
			Vector2 drawOrigin = new Vector2(Terraria.GameContent.TextureAssets.Npc[npc.type].Value.Width * 0.5f, texture.Height * 0.5f);
			for (int k = 0; k < npc.oldPos.Length; k++) {
				Vector2 drawPos = npc.oldPos[k] - Main.screenPosition + drawOrigin;
				Color color = npc.GetAlpha(lightColor) * ((float)(npc.oldPos.Length - k) / (float)npc.oldPos.Length);
				spriteBatch.Draw(texture, drawPos, null, color * 0.35f, npc.rotation, drawOrigin, npc.scale, SpriteEffects.None, 0f);
			}
			Draw(spriteBatch, lightColor);
			return false;
		}	
		public override void SetDefaults()
		{
            NPC.aiStyle =-1; 
            NPC.lifeMax = 250;   
            NPC.damage = 40; 
            NPC.defense = 0;  
            NPC.knockBackResist = 0f;
            NPC.width = 34;
            NPC.height = 34;
            npc.value = 0;
            npc.npcSlots = 1f;
            npc.lavaImmune = true;
            npc.noGravity = true;
            npc.noTileCollide = true;
            npc.HitSound = SoundID.NPCHit1;
            npc.DeathSound = SoundID.NPCDeath5;
            npc.netAlways = true;
            npc.buffImmune[20] = true;
			npc.alpha = 100;
		}
		public void Draw(SpriteBatch spriteBatch, Color drawColor)
		{
			Player player = Main.player[npc.target];
			Texture2D texture = (Texture2D)ModContent.Request<Texture2D>("SOTS/NPCs/Boss/PutridHookEye");
			Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
			Vector2 drawPos = npc.Center - Main.screenPosition;

			texture = Terraria.GameContent.TextureAssets.Npc[npc.type].Value;
			drawOrigin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
			spriteBatch.Draw(texture, drawPos, null, drawColor, npc.rotation, drawOrigin, npc.scale, SpriteEffects.None, 0f);
			float shootToX = aimToX - npc.Center.X;
			float shootToY = aimToY - npc.Center.Y;
			float distance = (float)System.Math.Sqrt((double)(shootToX * shootToX + shootToY * shootToY));

			distance = eyeReset * 1f / distance;
				  
			shootToX *= distance * 5;
			shootToY *= distance * 5;
			
			drawPos.X += shootToX;
            drawPos.Y += -1 + shootToY;
			drawColor = npc.GetAlpha(drawColor); 
			texture = (Texture2D)ModContent.Request<Texture2D>("SOTS/NPCs/Boss/PutridHookEye");
			drawOrigin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
			spriteBatch.Draw(texture, drawPos, null, drawColor, npc.rotation, drawOrigin, npc.scale, SpriteEffects.None, 0f);

		}
        public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
        {
            NPC.damage = (int)(npc.damage * 0.8f);  
        }
		public override void AI()
		{
			npc.dontTakeDamage = true;
			if(initiate == -1 && Main.netMode != 1)
			{
				rotationAmtStorage = rotationAmt;
				npc.velocity = new Vector2(12, 0).RotatedBy(MathHelper.ToRadians(rotationAmt + hookID));
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
				fireRate += Main.expertMode ? 6.5f : 5.5f;
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
			int pIndex = -1;
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





















