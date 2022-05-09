using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.NPCs.Boss
{
	public class PutridHook : ModNPC
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
		private float owner {
			get => npc.localAI[0];
			set => npc.localAI[0] = value;
		}
		private float bonusAmt
		{
			get => npc.localAI[1];
			set => npc.localAI[1] = value;
		}
		public override void SendExtraAI(BinaryWriter writer)
		{
			writer.Write(owner);
			writer.Write(randMult);
		}
		public override void ReceiveExtraAI(BinaryReader reader)
		{
			owner = reader.ReadSingle();
			randMult = reader.ReadSingle();
		}
		public override bool CheckActive()
		{
			return false;
		}
		private int storeDamage = -1;
		private int counter = 0;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Putrid Hook");
		}
		public override void SetDefaults()
		{
            npc.aiStyle = -1; 
            npc.lifeMax = 225;   
            npc.damage = 40; 
            npc.defense = 8;  
            npc.knockBackResist = 0f;
            npc.width = 34;
            npc.height = 34;
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
		float counter2 = 0;
		float randMult = 1f;
		bool runOnce = true;
		float[] counterArr = new float[12];
		float[] randSeed1 = new float[12];
        public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
		{
			Texture2D texture;
			Vector2 drawOrigin;
			Vector2 drawPos = npc.Center - Main.screenPosition;
			drawColor = npc.GetAlpha(drawColor);
			if (!runOnce)
			{
				for (int i = 0; i < 12; i++)
				{
					counterArr[i] += randSeed1[i];
					texture = (Texture2D)ModContent.Request<Texture2D>("SOTS/NPCs/Boss/PinkyGrappleSpike");
					Vector2 circular = new Vector2(0, (npc.width / 2) * npc.scale).RotatedBy(MathHelper.ToRadians(i * 30 + counter2 * 0.3f * randMult));
					int frame = 0;
					if(counterArr[i] >= 10)
                    {
						frame = 1;
					}
					if (counterArr[i] >= 20)
					{
						frame = 2;
					}
					if (counterArr[i] >= 40)
					{
						frame = 3;
					}
					if (counterArr[i] >= 50)
					{
						frame = 0;
						counterArr[i] = 0;
						randSeed1[i] = Main.rand.NextFloat(0.8f, 1.2f);
					}
					Rectangle FrameSize = new Rectangle(0, texture.Height / 4 * frame, texture.Width, texture.Height / 4);
					spriteBatch.Draw(texture, drawPos + circular, FrameSize, drawColor, npc.rotation + circular.ToRotation() - MathHelper.ToRadians(90), new Vector2(texture.Width/2, 3.5f), npc.scale, SpriteEffects.None, 0f);
				}
			}
			texture = Main.npcTexture[npc.type];
			drawOrigin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
			spriteBatch.Draw(texture, drawPos, null, drawColor, npc.rotation, drawOrigin, npc.scale, SpriteEffects.None, 0f);
			return false;
        }
        public override void PostDraw(SpriteBatch spriteBatch, Color drawColor)
		{
			Player player = Main.player[npc.target];
			Texture2D texture = (Texture2D)ModContent.Request<Texture2D>("SOTS/NPCs/Boss/PutridHookEye");
			Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
			Vector2 drawPos = npc.Center - Main.screenPosition;
			
			float shootToX = aimToX - npc.Center.X;
			float shootToY = aimToY - npc.Center.Y;
			float distance = (float)System.Math.Sqrt((double)(shootToX * shootToX + shootToY * shootToY));

			distance = 1f/ distance;
				  
			shootToX *= distance * 5;
			shootToY *= distance * 5;
			
			drawColor = npc.GetAlpha(drawColor);
			drawPos.X += shootToX;
			drawPos.Y += shootToY;
			if(npc.scale == 1)
				spriteBatch.Draw(texture, drawPos, null, drawColor, npc.rotation, drawOrigin, npc.scale, SpriteEffects.None, 0f);
		}
		public override void ModifyHitByItem(Player player, Item item, ref int damage, ref float knockback, ref bool crit)
		{
			base.ModifyHitByItem(player, item, ref damage, ref knockback, ref crit);
		}
		public override void ModifyHitByProjectile(Projectile projectile, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
		{
			if (Projectile.active && (Projectile.modProjectile == null || Projectile.modProjectile.ShouldUpdatePosition()))
			{
				Projectile.velocity.X *= -0.9f;
				Projectile.velocity.Y *= -0.9f;
				Projectile.netUpdate = true;
			}
		}
        public override bool StrikeNPC(ref double damage, int defense, ref float knockback, int hitDirection, ref bool crit)
		{
			if (npc.defense > 1000)
			{
				damage = 0;
				crit = false;
				return false;
			}
			return true;
        }
        public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
        {
			npc.lifeMax = (int)(600 * bossLifeScale * 0.75f);
			npc.damage = 60;  
        }
		public override void NPCLoot()
		{
			if(Main.netMode != 1)
			{
				int num1 = NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, mod.NPCType("HookTurret"), 0, npc.ai[0], npc.ai[1], npc.ai[2], npc.ai[3]);	
				NPC newNpc = Main.npc[num1];
				newNpc.localAI[0] = (int)owner;
				newNpc.netUpdate = true;
			}
		}
		public override void AI()
		{
			if (hookID > 360)
				hookID -= 360;
			if (hookID < 0)
				hookID += 360;
			counter2++;
			if (runOnce)
            {
				randMult = Main.rand.NextFloat(0.8f, 1.2f) * (Main.rand.Next(2) * 2 - 1);
				if(Main.netMode != 1)
					npc.netUpdate = true;
				for (int i = 0; i < 12; i++)
                {
					counterArr[i] = 0;
					randSeed1[i] = Main.rand.NextFloat(0.8f, 1.2f);
                }
				runOnce = false;
            }
			Lighting.AddLight(npc.Center, (255 - npc.alpha) * 1f / 155f, (255 - npc.alpha) * 1f / 155f, (255 - npc.alpha) * 1f / 155f);
			int pIndex = -1;
			int totalHook = 0;
			if(storeDamage == -1)
			{
				storeDamage = npc.damage;
			}
			for(int i = 0; i < 200; i++)
			{
				NPC npc1 = Main.npc[i];
				if(npc1.type == mod.NPCType("PutridPinkyPhase2") && npc1.active && pIndex == -1 && npc1.whoAmI == (int)owner)
				{
					pIndex = i;
				}
				if(npc1.type == npc.type && npc1.active && (int)npc1.localAI[0] == (int)owner)
				{
					totalHook++;
					float hookID2 = npc1.ai[2];
					float diff = hookID - hookID2;
					if (diff < 30 && diff > 0)
						hookID++;
					if (diff > -30 && diff < 0)
						hookID--;
					if (diff > 330 && diff <= 360)
                    {
						hookID--;
					}
					if (diff < -330 && diff >= -360)
					{
						hookID++;
					}
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
			float rotationDistance = putridPinky.ai[3];
			float rotationSpeed = putridPinky.ai[2];
			hookID += rotationSpeed;
			Vector2 rotationArea = new Vector2(rotationDistance + new Vector2(8, 0).RotatedBy(MathHelper.ToRadians(randMult * counter2 * 2)).X, 0).RotatedBy(MathHelper.ToRadians(hookID));
			rotationArea += putridPinky.Center + new Vector2(0, 3.5f);
			npc.Center = rotationArea;
			counter++;
			if(Main.netMode != 1 && counter % 15 == 0)
			{
				npc.netUpdate = true;
			}
				
			npc.alpha = putridPinky.alpha;
			npc.dontTakeDamage = false;
			if(totalHook <= 4)
			{
				npc.defense = 9999;
				if(counter % (totalHook * totalHook) == 0)
                {
					npc.life++;
                }
			}
			else
            {
				npc.defense = 8;
            }
			if(npc.alpha > 70)
			{
				npc.dontTakeDamage = true;
				npc.damage = 0;
			}
			else
			{
				npc.damage = storeDamage;
			}
		}
        public override void HitEffect(int hitDirection, double damage)
		{
			if (npc.life > 0)
			{
				for (int i = 0; i < 4; i++)
				{
					Dust dust = Dust.NewDustDirect(npc.position, npc.width, npc.height, DustID.PinkSlime, hitDirection * 2, 0, 120);
					dust.scale *= 1.5f;
				}
				return;
			}
			base.HitEffect(hitDirection, damage);
        }
    }
}





















