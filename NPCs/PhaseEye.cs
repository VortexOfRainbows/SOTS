using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
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
			get => npc.ai[2];
			set => npc.ai[2] = value;
		}
		private float tracerPosY
		{
			get => npc.ai[3];
			set => npc.ai[3] = value;
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
            npc.aiStyle = 0; 
            npc.lifeMax = 50;   
            npc.damage = 50; 
            npc.defense = 0;  
            npc.knockBackResist = 0f;
            npc.width = 36;
            npc.height = 36;
			Main.npcFrameCount[npc.type] = 1;  
            npc.value = 0;
            npc.npcSlots = 2.5f;
            npc.HitSound = SoundID.NPCHit53;
            npc.DeathSound = SoundID.NPCDeath14;
			npc.lavaImmune = true;
			npc.netAlways = true;
			npc.noGravity = true;
			npc.buffImmune[BuffID.OnFire] = true;
			npc.buffImmune[BuffID.Frostburn] = true;
			//banner = npc.type;
			//bannerItem = ItemType<SittingMushroomBanner>();
		}
        public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            return false;
        }
        public override void PostDraw(SpriteBatch spriteBatch, Color drawColor)
		{
			Texture2D texture = mod.GetTexture("NPCs/PhaseEyeOutline");
			Texture2D texture3 = mod.GetTexture("NPCs/PhaseEyePupil");
			Texture2D texture4 = mod.GetTexture("NPCs/PhaseEyeFill");
			Color color = new Color(110, 110, 110, 0);
			Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
			Vector2 drawOrigin3 = new Vector2(texture3.Width * 0.5f, texture3.Height * 0.5f);
			for (int k = 0; k < 5; k++)
			{
				float x = Main.rand.Next(-10, 11) * 0.1f;
				float y = Main.rand.Next(-10, 11) * 0.1f;
				Vector2 between = new Vector2(lookAtPos.X, lookAtPos.Y) - npc.Center;
				if(between.Length() > 1.1f)
				{
					between.Normalize();
				}
				else
				{
					between = Vector2.Zero;
				}

				if (k == 0)
					Main.spriteBatch.Draw(texture4, new Vector2((float)(npc.Center.X - (int)Main.screenPosition.X), (float)(npc.Center.Y - (int)Main.screenPosition.Y) - 4), null, color * 0.5f, 0f, drawOrigin, npc.scale, SpriteEffects.None, 0f);

				Main.spriteBatch.Draw(texture, new Vector2((float)(npc.Center.X - (int)Main.screenPosition.X) + x, (float)(npc.Center.Y - (int)Main.screenPosition.Y) + y - 4), null, color * ((255 - npc.alpha) / 255f), 0f, drawOrigin, npc.scale, SpriteEffects.None, 0f);

				float scaleFactor = counter / 180f;
				scaleFactor = scaleFactor > 1 ? 1 : scaleFactor;

				if (lookAtPos.X != -1 && lookAtPos.Y != -1)
				{
					Main.spriteBatch.Draw(texture3, new Vector2((float)(npc.Center.X - (int)Main.screenPosition.X) + x, (float)(npc.Center.Y - (int)Main.screenPosition.Y) + y - 4) + between * npc.scale * (6 + -14 * npc.ai[1]), null, color * ((255 - npc.alpha) / 255f), 0f, drawOrigin3, 1.5f * npc.scale - npc.ai[1], SpriteEffects.None, 0f);
				}
			}
			base.PostDraw(spriteBatch, drawColor);
		}
		public void MoveCursorToPlayer()
		{
			float scaleFactor = counter / 120f;
			scaleFactor = scaleFactor > 1 ? 1 : scaleFactor;
			Player player = Main.player[npc.target];
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
			Player player = Main.player[npc.target];
			Vector2 between = npc.Center - player.Center;
			float length = between.Length() + 0.1f;
			float speed = 0.7f * scaleFactor;
			if (length > 1.1f)
			{
				between = between.SafeNormalize(Vector2.Zero);
				Vector2 dynamicAddition = new Vector2(0.3f + (npc.whoAmI % 4) * 0.25f, 0).RotatedBy(MathHelper.ToRadians(counter));
				if (length > speed)
				{
					length = speed;
				}
				between *= -length;
				npc.velocity = between + dynamicAddition;
			}
		}
		public void MoveEyesToCursor()
		{
			float scaleFactor = counter / 120f;
			scaleFactor = scaleFactor > 1 ? 1 : scaleFactor;
			Player player = Main.player[npc.target];
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
					tracerPosX = npc.Center.X;
					tracerPosY = npc.position.Y;
					lookAtPos = new Vector2(tracerPosX, tracerPosY);
					npc.scale = 0.1f;
                }
				npc.scale *= 1.01f;
				Vector2 circular = new Vector2(2 * (1 - npc.scale), 0).RotatedBy(MathHelper.ToRadians(360 * npc.scale));
				npc.velocity.Y = -circular.Y;
				for (int k = 0; k < 360; k += 15)
				{
					Vector2 circularLocation = new Vector2(-28 * npc.scale, 0).RotatedBy(MathHelper.ToRadians(k));
					circularLocation += 0.5f * new Vector2(Main.rand.Next(-1, 2), Main.rand.Next(-1, 2));
					int type = 242;
					if (Main.rand.NextBool(30))
					{
						int num1 = Dust.NewDust(new Vector2(npc.Center.X + circularLocation.X - 4, npc.Center.Y + circularLocation.Y - 4), 4, 4, type);
						Main.dust[num1].noGravity = true;
						Main.dust[num1].scale *= 0.5f + 1.8f * npc.scale;
						Main.dust[num1].velocity = -circularLocation * 0.07f;
					}
				}
				lookAtPos = (lookAtPos - npc.Center).RotatedBy(MathHelper.ToRadians(npc.scale * 15)) + npc.Center;
				if(npc.scale > 1)
                {
					npc.scale = 1;
					startup = 31;
                }
				return false;
			}
			Player player = Main.player[npc.target];
			npc.TargetClosest(true);
			float distanceX = player.Center.X - npc.Center.X;
			float distanceY = player.Center.Y - npc.Center.Y;
			distanceX = Math.Abs(distanceX);
			distanceY = Math.Abs(distanceY);
			if (distanceX < 1200 && distanceY < 800)
			{
				hasSeenPlayer = true;
			}
			if(!hasSeenPlayer)
			{
				if (npc.ai[0] == 0 && tracerPosX == 0 && tracerPosY == 0)
				{
					tracerPosX = npc.Center.X;
					tracerPosY = npc.Center.Y;
					lookAtPos = new Vector2(tracerPosX, tracerPosY);
				}
			}
			return hasSeenPlayer;
		}
		public override void AI()
		{
			if(Main.rand.NextBool(5))
            {
				int dust = Dust.NewDust(npc.position, npc.width, npc.height, 242);
				Main.dust[dust].velocity *= 0.1f;
				Main.dust[dust].scale *= 2f;
				Main.dust[dust].noGravity = true;
			}
			Player player = Main.player[npc.target];
			counter++;
			MoveToPlayer();
			MoveCursorToPlayer();
			MoveEyesToCursor();
			npc.ai[0]++;
			if (npc.ai[1] > 0)
			{
				npc.ai[1] -= 0.05f;
			}
			if (npc.ai[1] < 0)
			{
				npc.ai[1] = 0;
			}
			if (npc.ai[0] >= 240 && npc.ai[0] <= 270)
			{
				int damage2 = npc.damage / 2;
				if (Main.expertMode)
				{
					damage2 = (int)(damage2 / Main.expertDamage);
				}
					
				if(npc.ai[0] % 20 == 0)
				{
					Vector2 between = lookAtPos - npc.Center;
					between.Normalize();

					if (Main.netMode != NetmodeID.MultiplayerClient)
						Projectile.NewProjectile(npc.Center.X + between.X * 24, npc.Center.Y + between.Y * 24, between.X * 5, between.Y * 5, mod.ProjectileType("OtherworldlyBolt"), damage2, 1f, Main.myPlayer, 0, 0);

					Main.PlaySound(2, (int)npc.Center.X, (int)npc.Center.Y, 92, 0.5f);
					npc.ai[1] = 1;
				}
			}
			if (npc.ai[0] >= 440)
			{
				npc.ai[0] = Main.rand.Next(-150, 100);
				npc.netUpdate = true;
			}
		}
		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			return 0;
			//spawnrates manually added in SOTSNPCs.EditSpawnPool in order to avoid conflicts in hardmode
			Player player = spawnInfo.player;
			SOTSPlayer modPlayer = player.GetModPlayer<SOTSPlayer>();
			bool correctBlock = spawnInfo.spawnTileType == mod.TileType("DullPlatingTile") || spawnInfo.spawnTileType == mod.TileType("PortalPlatingTile") || spawnInfo.spawnTileType == mod.TileType("AvaritianPlatingTile");
			//correctBlock = true;
			if (modPlayer.PlanetariumBiome && correctBlock)
			{
				return 0.1f;
			}
		}
        public override bool PreNPCLoot()
        {
            return false;
        }
        public override void HitEffect(int hitDirection, double damage)
		{
			if (npc.life > 0)
			{
				int num = 0;
				while ((double)num < damage / (double)npc.lifeMax * 40.0)
				{
					int dust = Dust.NewDust(npc.position, npc.width, npc.height, 242, (float)(2 * hitDirection), -2f, 0, default);
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
					int dust = Dust.NewDust(npc.position, npc.width, npc.height, 242, (float)(2 * hitDirection), -2f, 0, default);
					Main.dust[dust].velocity *= 2f;
					Main.dust[dust].scale *= 5f;
					Main.dust[dust].noGravity = true;
				}
			}
		}
	}
}