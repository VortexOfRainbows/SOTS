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
		private float owner {
			get => NPC.localAI[0];
			set => NPC.localAI[0] = value;
		}
		private float bonusAmt
		{
			get => NPC.localAI[1];
			set => NPC.localAI[1] = value;
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
            NPC.aiStyle =-1; 
            NPC.lifeMax = 225;   
            NPC.damage = 40; 
            NPC.defense = 8;  
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
			Vector2 drawPos = NPC.Center - Main.screenPosition;
			drawColor = NPC.GetAlpha(drawColor);
			if (!runOnce)
			{
				for (int i = 0; i < 12; i++)
				{
					counterArr[i] += randSeed1[i];
					texture = (Texture2D)ModContent.Request<Texture2D>("SOTS/NPCs/Boss/PinkyGrappleSpike");
					Vector2 circular = new Vector2(0, (NPC.width / 2) * NPC.scale).RotatedBy(MathHelper.ToRadians(i * 30 + counter2 * 0.3f * randMult));
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
					spriteBatch.Draw(texture, drawPos + circular, FrameSize, drawColor, NPC.rotation + circular.ToRotation() - MathHelper.ToRadians(90), new Vector2(texture.Width/2, 3.5f), NPC.scale, SpriteEffects.None, 0f);
				}
			}
			texture = Terraria.GameContent.TextureAssets.Npc[NPC.type].Value;
			drawOrigin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
			spriteBatch.Draw(texture, drawPos, null, drawColor, NPC.rotation, drawOrigin, NPC.scale, SpriteEffects.None, 0f);
			return false;
        }
        public override void PostDraw(SpriteBatch spriteBatch, Color drawColor)
		{
			Player player = Main.player[NPC.target];
			Texture2D texture = (Texture2D)ModContent.Request<Texture2D>("SOTS/NPCs/Boss/PutridHookEye");
			Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
			Vector2 drawPos = NPC.Center - Main.screenPosition;
			
			float shootToX = aimToX - NPC.Center.X;
			float shootToY = aimToY - NPC.Center.Y;
			float distance = (float)System.Math.Sqrt((double)(shootToX * shootToX + shootToY * shootToY));

			distance = 1f/ distance;
				  
			shootToX *= distance * 5;
			shootToY *= distance * 5;
			
			drawColor = NPC.GetAlpha(drawColor);
			drawPos.X += shootToX;
			drawPos.Y += shootToY;
			if(NPC.scale == 1)
				spriteBatch.Draw(texture, drawPos, null, drawColor, NPC.rotation, drawOrigin, NPC.scale, SpriteEffects.None, 0f);
		}
		public override void ModifyHitByItem(Player player, Item item, ref int damage, ref float knockback, ref bool crit)
		{
			base.ModifyHitByItem(player, item, ref damage, ref knockback, ref crit);
		}
		public override void ModifyHitByProjectile(Projectile projectile, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
		{
			if (Projectile.active && (Projectile.ModProjectile == null || Projectile.ModProjectile.ShouldUpdatePosition()))
			{
				Projectile.velocity.X *= -0.9f;
				Projectile.velocity.Y *= -0.9f;
				Projectile.netUpdate = true;
			}
		}
        public override bool StrikeNPC(ref double damage, int defense, ref float knockback, int hitDirection, ref bool crit)
		{
			if (NPC.defense > 1000)
			{
				damage = 0;
				crit = false;
				return false;
			}
			return true;
        }
        public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
        {
			NPC.lifeMax = (int)(600 * bossLifeScale * 0.75f);
			NPC.damage = 60;  
        }
		public override void NPCLoot()
		{
			if(Main.netMode != 1)
			{
				int num1 = NPC.NewNPC((int)NPC.Center.X, (int)NPC.Center.Y, Mod.Find<ModNPC>("HookTurret").Type, 0, NPC.ai[0], NPC.ai[1], NPC.ai[2], NPC.ai[3]);	
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
					NPC.netUpdate = true;
				for (int i = 0; i < 12; i++)
                {
					counterArr[i] = 0;
					randSeed1[i] = Main.rand.NextFloat(0.8f, 1.2f);
                }
				runOnce = false;
            }
			Lighting.AddLight(NPC.Center, (255 - NPC.alpha) * 1f / 155f, (255 - NPC.alpha) * 1f / 155f, (255 - NPC.alpha) * 1f / 155f);
			int pIndex = -1;
			int totalHook = 0;
			if(storeDamage == -1)
			{
				storeDamage = NPC.damage;
			}
			for(int i = 0; i < 200; i++)
			{
				NPC npc1 = Main.npc[i];
				if(npc1.type == Mod.Find<ModNPC>("PutridPinkyPhase2") .Type&& npc1.active && pIndex == -1 && npc1.whoAmI == (int)owner)
				{
					pIndex = i;
				}
				if(npc1.type == NPC.type && npc1.active && (int)npc1.localAI[0] == (int)owner)
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
				NPC.life--;
				NPC.scale *= 0.98f;
				if(NPC.life < 50 || NPC.scale < 0.4f)
				{
					NPC.active = false;
				}
				return;
			}
			NPC putridPinky = Main.npc[pIndex];
			float rotationDistance = putridPinky.ai[3];
			float rotationSpeed = putridPinky.ai[2];
			hookID += rotationSpeed;
			Vector2 rotationArea = new Vector2(rotationDistance + new Vector2(8, 0).RotatedBy(MathHelper.ToRadians(randMult * counter2 * 2)).X, 0).RotatedBy(MathHelper.ToRadians(hookID));
			rotationArea += putridPinky.Center + new Vector2(0, 3.5f);
			NPC.Center = rotationArea;
			counter++;
			if(Main.netMode != 1 && counter % 15 == 0)
			{
				NPC.netUpdate = true;
			}
				
			NPC.alpha = putridPinky.alpha;
			NPC.dontTakeDamage = false;
			if(totalHook <= 4)
			{
				NPC.defense = 9999;
				if(counter % (totalHook * totalHook) == 0)
                {
					NPC.life++;
                }
			}
			else
            {
				NPC.defense = 8;
            }
			if(NPC.alpha > 70)
			{
				NPC.dontTakeDamage = true;
				NPC.damage = 0;
			}
			else
			{
				NPC.damage = storeDamage;
			}
		}
        public override void HitEffect(int hitDirection, double damage)
		{
			if (NPC.life > 0)
			{
				for (int i = 0; i < 4; i++)
				{
					Dust dust = Dust.NewDustDirect(NPC.position, NPC.width, NPC.height, DustID.PinkSlime, hitDirection * 2, 0, 120);
					dust.scale *= 1.5f;
				}
				return;
			}
			base.HitEffect(hitDirection, damage);
        }
    }
}





















