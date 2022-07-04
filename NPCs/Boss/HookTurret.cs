using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Projectiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.NPCs.Boss
{
	public class HookTurret : ModNPC
	{	
		private float aimToX {
			get => NPC.ai[0];
			set => NPC.ai[0] = value;
		}

		private float aimToY {
			get => NPC.ai[1];
			set => NPC.ai[1] = value;
		}
		
		private float hookID {
			get => NPC.ai[2];
			set => NPC.ai[2] = value;
		}
		
		private float rotationAmt {
			get => NPC.ai[3];
			set => NPC.ai[3] = value;
		}
		private float owner
		{
			get => NPC.localAI[0];
			set => NPC.localAI[0] = value;
		}
		private float rotationAmtStorage
		{
			get => NPC.localAI[1];
			set => NPC.localAI[1] = value;
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
			NPCID.Sets.TrailCacheLength[NPC.type] = 4;  
			NPCID.Sets.TrailingMode[NPC.type] = 0;
			NPCID.Sets.NPCBestiaryDrawModifiers drawModifiers = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
			{
				Hide = true
			};
			NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, drawModifiers);
		}
        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
			Texture2D texture = Terraria.GameContent.TextureAssets.Npc[NPC.type].Value;
			Vector2 drawOrigin = new Vector2(Terraria.GameContent.TextureAssets.Npc[NPC.type].Value.Width * 0.5f, texture.Height * 0.5f);
			for (int k = 0; k < NPC.oldPos.Length; k++) {
				Vector2 drawPos = NPC.oldPos[k] - screenPos + drawOrigin;
				Color color = NPC.GetAlpha(drawColor) * ((float)(NPC.oldPos.Length - k) / (float)NPC.oldPos.Length);
				spriteBatch.Draw(texture, drawPos, null, color * 0.35f, NPC.rotation, drawOrigin, NPC.scale, SpriteEffects.None, 0f);
			}
			Draw(spriteBatch, screenPos, drawColor);
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
            NPC.value = 0;
            NPC.npcSlots = 1f;
            NPC.lavaImmune = true;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath5;
            NPC.netAlways = true;
            NPC.buffImmune[20] = true;
			NPC.alpha = 100;
		}
		public void Draw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
		{
			Player player = Main.player[NPC.target];
			Texture2D texture = (Texture2D)ModContent.Request<Texture2D>("SOTS/NPCs/Boss/PutridHookEye");
			Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
			Vector2 drawPos = NPC.Center - screenPos;

			texture = Terraria.GameContent.TextureAssets.Npc[NPC.type].Value;
			drawOrigin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
			spriteBatch.Draw(texture, drawPos, null, drawColor, NPC.rotation, drawOrigin, NPC.scale, SpriteEffects.None, 0f);
			float shootToX = aimToX - NPC.Center.X;
			float shootToY = aimToY - NPC.Center.Y;
			float distance = (float)System.Math.Sqrt((double)(shootToX * shootToX + shootToY * shootToY));

			distance = eyeReset * 1f / distance;
				  
			shootToX *= distance * 5;
			shootToY *= distance * 5;
			
			drawPos.X += shootToX;
            drawPos.Y += -1 + shootToY;
			drawColor = NPC.GetAlpha(drawColor); 
			texture = (Texture2D)ModContent.Request<Texture2D>("SOTS/NPCs/Boss/PutridHookEye");
			drawOrigin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
			spriteBatch.Draw(texture, drawPos, null, drawColor, NPC.rotation, drawOrigin, NPC.scale, SpriteEffects.None, 0f);

		}
        public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
        {
            NPC.damage = (int)(NPC.damage * 0.8f);  
        }
		public override void AI()
		{
			NPC.dontTakeDamage = true;
			if(initiate == -1 && Main.netMode != 1)
			{
				rotationAmtStorage = rotationAmt;
				NPC.velocity = new Vector2(12, 0).RotatedBy(MathHelper.ToRadians(rotationAmt + hookID));
				rotationAmt = 124;
				hookID = -1;
				NPC.netUpdate = true;
				initiate = 1;
			}
			if(rotationAmt > 8 && hookID == -1)
			{
				rotationAmt--;
				Vector2 aimTo = new Vector2(aimToX - NPC.Center.X, aimToY - NPC.Center.Y).RotatedBy(MathHelper.ToRadians(rotationAmt));
				aimToX = aimTo.X + NPC.Center.X;
				aimToY = aimTo.Y + NPC.Center.Y;
				NPC.netUpdate = true;
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
					NPC.netUpdate = true;
					float shootToX = aimToX - NPC.Center.X;
					float shootToY = aimToY - NPC.Center.Y;
					float distance = (float)Math.Sqrt((double)(shootToX * shootToX + shootToY * shootToY));

					distance = (Main.expertMode ? 5.25f : 5f) / distance;
						  
					shootToX *= distance;
					shootToY *= distance;

					int damage = NPC.GetBaseDamage() / 2;
					if(Main.netMode != 1)
						Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center.X, NPC.Center.Y, shootToX, shootToY, ModContent.ProjectileType<PinkBullet>(), damage, 0, Main.myPlayer);
				}
			}
			
			NPC.velocity *= 0.97f;
			Lighting.AddLight(NPC.Center, (255 - NPC.alpha) * 1f / 155f, (255 - NPC.alpha) * 1f / 155f, (255 - NPC.alpha) * 1f / 155f);
			int pIndex = -1;
			for(int i = 0; i < 200; i++)
			{
				NPC npc1 = Main.npc[i];
				if(npc1.type == ModContent.NPCType<PutridPinkyPhase2>() && npc1.active)
				{
					NPC.active = true;
					NPC.timeLeft = 1000;
					pIndex = i;
					break;
				}
			}
			if(pIndex == -1)
			{
				NPC.life--;
				NPC.scale *= 0.98f;
				if(NPC.life < 50 || NPC.scale < 0.4f)
				{
					NPC.active = false;
				}
				return;
			}
			NPC putridPinky = Main.npc[pIndex];
			Dust.NewDust(new Vector2(NPC.position.X, NPC.position.Y), NPC.width, NPC.height, Mod.Find<ModDust>("BigPinkDust").Type);
		}
	}
}





















