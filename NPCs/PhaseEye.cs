using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Common.GlobalNPCs;
using SOTS.Items.Otherworld.Blocks;
using SOTS.Projectiles.Otherworld;
using System;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.NPCs
{
	public class PhaseEye : ModNPC
	{
		private float tracerPosX
		{
			get => NPC.ai[2];
			set => NPC.ai[2] = value;
		}
		private float tracerPosY
		{
			get => NPC.ai[3];
			set => NPC.ai[3] = value;
		}
		public override void SendExtraAI(BinaryWriter writer)
		{
			writer.Write(lookAtPos.X);
			writer.Write(lookAtPos.Y);
		}
		public override void ReceiveExtraAI(BinaryReader reader)
		{
			lookAtPos.X = reader.ReadSingle();
			lookAtPos.Y = reader.ReadSingle();
		}
		private Vector2 lookAtPos = new Vector2(-1, -1);
		public override void SetStaticDefaults()
		{
			
			DisplayName.SetDefault("Phase Eye");
		}
		public override void SetDefaults()
		{
            NPC.aiStyle =0; 
            NPC.lifeMax = 50;   
            NPC.damage = 50; 
            NPC.defense = 0;  
            NPC.knockBackResist = 0f;
            NPC.width = 36;
            NPC.height = 36;
			Main.npcFrameCount[NPC.type] = 1;  
            NPC.value = 0;
            NPC.npcSlots = 2.5f;
            NPC.HitSound = SoundID.NPCHit53;
            NPC.DeathSound = SoundID.NPCDeath14;
			NPC.lavaImmune = true;
			NPC.netAlways = true;
			NPC.noGravity = true;
			NPC.buffImmune[BuffID.OnFire] = true;
			NPC.buffImmune[BuffID.Frostburn] = true;
			//Banner = NPC.type;
			//BannerItem = ItemType<SittingMushroomBanner>();
		}
        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            return false;
        }
        public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
			Texture2D texture = Mod.Assets.Request<Texture2D>("NPCs/PhaseEyeOutline").Value;
			Texture2D texture3 = Mod.Assets.Request<Texture2D>("NPCs/PhaseEyePupil").Value;
			Texture2D texture4 = Mod.Assets.Request<Texture2D>("NPCs/PhaseEyeFill").Value;
			Color color = new Color(110, 110, 110, 0);
			Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
			Vector2 drawOrigin3 = new Vector2(texture3.Width * 0.5f, texture3.Height * 0.5f);
			for (int k = 0; k < 5; k++)
			{
				float x = Main.rand.Next(-10, 11) * 0.1f;
				float y = Main.rand.Next(-10, 11) * 0.1f;
				Vector2 between = new Vector2(lookAtPos.X, lookAtPos.Y) - NPC.Center;
				if(between.Length() > 1.1f)
				{
					between.Normalize();
				}
				else
				{
					between = Vector2.Zero;
				}

				if (k == 0)
					Main.spriteBatch.Draw(texture4, new Vector2((float)(NPC.Center.X - (int)Main.screenPosition.X), (float)(NPC.Center.Y - (int)Main.screenPosition.Y) - 4), null, color * 0.5f, 0f, drawOrigin, NPC.scale, SpriteEffects.None, 0f);

				Main.spriteBatch.Draw(texture, new Vector2((float)(NPC.Center.X - (int)Main.screenPosition.X) + x, (float)(NPC.Center.Y - (int)Main.screenPosition.Y) + y - 4), null, color * ((255 - NPC.alpha) / 255f), 0f, drawOrigin, NPC.scale, SpriteEffects.None, 0f);

				float scaleFactor = counter / 180f;
				scaleFactor = scaleFactor > 1 ? 1 : scaleFactor;

				if (lookAtPos.X != -1 && lookAtPos.Y != -1)
				{
					Main.spriteBatch.Draw(texture3, new Vector2((float)(NPC.Center.X - (int)Main.screenPosition.X) + x, (float)(NPC.Center.Y - (int)Main.screenPosition.Y) + y - 4) + between * NPC.scale * (6 + -14 * NPC.ai[1]), null, color * ((255 - NPC.alpha) / 255f), 0f, drawOrigin3, 1.5f * NPC.scale - NPC.ai[1], SpriteEffects.None, 0f);
				}
			}
		}
		public void MoveCursorToPlayer()
		{
			float scaleFactor = counter / 120f;
			scaleFactor = scaleFactor > 1 ? 1 : scaleFactor;
			Player player = Main.player[NPC.target];
			Vector2 between = new Vector2(tracerPosX, tracerPosY) - player.Center;
			float length = between.Length() + 0.1f;
			float speed = 6f * scaleFactor;
			if(length > 1.1f)
			{
				between.Normalize();
				if (length > speed)
				{
					length = speed;
				}
				between *= -length;
				tracerPosX += between.X;
				tracerPosY += between.Y;
			}
		}
		public void MoveToPlayer()
		{
			float scaleFactor = counter / 120f;
			scaleFactor = scaleFactor > 1 ? 1 : scaleFactor;
			Player player = Main.player[NPC.target];
			Vector2 between = NPC.Center - player.Center;
			float length = between.Length() + 0.1f;
			float speed = 0.7f * scaleFactor;
			if (length > 1.1f)
			{
				between = between.SafeNormalize(Vector2.Zero);
				Vector2 dynamicAddition = new Vector2(0.3f + (NPC.whoAmI % 4) * 0.25f, 0).RotatedBy(MathHelper.ToRadians(counter));
				if (length > speed)
				{
					length = speed;
				}
				between *= -length;
				NPC.velocity = between + dynamicAddition;
			}
		}
		public void MoveEyesToCursor()
		{
			float scaleFactor = counter / 120f;
			scaleFactor = scaleFactor > 1 ? 1 : scaleFactor;
			Player player = Main.player[NPC.target];
			Vector2 between = new Vector2(tracerPosX, tracerPosY) - lookAtPos;
			float length = between.Length() + 0.1f;
			float speed = 6f * scaleFactor;
			if (length > 1.1f)
			{
				between.Normalize();
				if (length > speed)
				{
					length = speed;
				}
				between *= length;
				lookAtPos.X += between.X;
				lookAtPos.Y += between.Y;
			}
		}
		int counter = 0;
		bool hasSeenPlayer = false;
		float startup = -1;
		public override bool PreAI()
		{
			if(startup <= 30)
			{
				if (startup == -1)
				{
					startup++;
					tracerPosX = NPC.Center.X;
					tracerPosY = NPC.position.Y;
					lookAtPos = new Vector2(tracerPosX, tracerPosY);
					NPC.scale = 0.1f;
                }
				NPC.scale *= 1.01f;
				Vector2 circular = new Vector2(2 * (1 - NPC.scale), 0).RotatedBy(MathHelper.ToRadians(360 * NPC.scale));
				NPC.velocity.Y = -circular.Y;
				for (int k = 0; k < 360; k += 15)
				{
					Vector2 circularLocation = new Vector2(-28 * NPC.scale, 0).RotatedBy(MathHelper.ToRadians(k));
					circularLocation += 0.5f * new Vector2(Main.rand.Next(-1, 2), Main.rand.Next(-1, 2));
					int type = 242;
					if (Main.rand.NextBool(30))
					{
						int num1 = Dust.NewDust(new Vector2(NPC.Center.X + circularLocation.X - 4, NPC.Center.Y + circularLocation.Y - 4), 4, 4, type);
						Main.dust[num1].noGravity = true;
						Main.dust[num1].scale *= 0.5f + 1.8f * NPC.scale;
						Main.dust[num1].velocity = -circularLocation * 0.07f;
					}
				}
				lookAtPos = (lookAtPos - NPC.Center).RotatedBy(MathHelper.ToRadians(NPC.scale * 15)) + NPC.Center;
				if(NPC.scale > 1)
                {
					NPC.scale = 1;
					startup = 31;
                }
				return false;
			}
			Player player = Main.player[NPC.target];
			NPC.TargetClosest(true);
			float distanceX = player.Center.X - NPC.Center.X;
			float distanceY = player.Center.Y - NPC.Center.Y;
			distanceX = Math.Abs(distanceX);
			distanceY = Math.Abs(distanceY);
			if (distanceX < 1200 && distanceY < 800)
			{
				hasSeenPlayer = true;
			}
			if(!hasSeenPlayer)
			{
				if (NPC.ai[0] == 0 && tracerPosX == 0 && tracerPosY == 0)
				{
					tracerPosX = NPC.Center.X;
					tracerPosY = NPC.Center.Y;
					lookAtPos = new Vector2(tracerPosX, tracerPosY);
				}
			}
			return hasSeenPlayer;
		}
		public override void AI()
		{
			if(Main.rand.NextBool(5))
            {
				int dust = Dust.NewDust(NPC.position, NPC.width, NPC.height, 242);
				Main.dust[dust].velocity *= 0.1f;
				Main.dust[dust].scale *= 2f;
				Main.dust[dust].noGravity = true;
			}
			Player player = Main.player[NPC.target];
			counter++;
			MoveToPlayer();
			MoveCursorToPlayer();
			MoveEyesToCursor();
			NPC.ai[0]++;
			if (NPC.ai[1] > 0)
			{
				NPC.ai[1] -= 0.05f;
			}
			if (NPC.ai[1] < 0)
			{
				NPC.ai[1] = 0;
			}
			if (NPC.ai[0] >= 240 && NPC.ai[0] <= 270)
			{
				int damage2 = SOTSNPCs.GetBaseDamage(NPC) / 2;
				if (NPC.ai[0] % 20 == 0)
				{
					Vector2 between = lookAtPos - NPC.Center;
					between.Normalize();

					if (Main.netMode != NetmodeID.MultiplayerClient)
						Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center.X + between.X * 24, NPC.Center.Y + between.Y * 24, between.X * 5, between.Y * 5, ModContent.ProjectileType<OtherworldlyBolt>(), damage2, 1f, Main.myPlayer, 0, 0);

					SOTSUtils.PlaySound(SoundID.Item92, (int)NPC.Center.X, (int)NPC.Center.Y, 0.5f);
					NPC.ai[1] = 1;
				}
			}
			if (NPC.ai[0] >= 440)
			{
				NPC.ai[0] = Main.rand.Next(-150, 100);
				NPC.netUpdate = true;
			}
		}
        public override void HitEffect(int hitDirection, double damage)
		{
			if (NPC.life > 0)
			{
				int num = 0;
				while ((double)num < damage / (double)NPC.lifeMax * 40.0)
				{
					int dust = Dust.NewDust(NPC.position, NPC.width, NPC.height, 242, (float)(2 * hitDirection), -2f, 0, default);
					Main.dust[dust].velocity *= 2f;
					Main.dust[dust].scale *= 4f;
					Main.dust[dust].noGravity = true;
					num++;
				}
			}
			else
			{
				for (int k = 0; k < 30; k++)
				{
					int dust = Dust.NewDust(NPC.position, NPC.width, NPC.height, 242, (float)(2 * hitDirection), -2f, 0, default);
					Main.dust[dust].velocity *= 2f;
					Main.dust[dust].scale *= 5f;
					Main.dust[dust].noGravity = true;
				}
			}
		}
        public override bool PreKill()
        {
            return false;
        }
    }
}