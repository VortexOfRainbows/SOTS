using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Common.GlobalNPCs;
using SOTS.Dusts;
using SOTS.Items.Banners;
using SOTS.Items.Fragments;
using SOTS.Items.Otherworld;
using SOTS.Items.Otherworld.FromChests;
using SOTS.Projectiles.Otherworld;
using System;
using System.IO;
using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace SOTS.NPCs
{
	public class HoloEye : ModNPC
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
			writer.Write(rotateCursor);
			writer.Write(lookAtPos.X);
			writer.Write(lookAtPos.Y);
		}
		public override void ReceiveExtraAI(BinaryReader reader)
		{
			rotateCursor = reader.ReadSingle();
			lookAtPos.X = reader.ReadSingle();
			lookAtPos.Y = reader.ReadSingle();
		}
		private Vector2 lookAtPos = new Vector2(-1, -1);
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Holo Eye");
			NPCID.Sets.DebuffImmunitySets.Add(Type, new Terraria.DataStructures.NPCDebuffImmunityData
			{
				SpecificallyImmuneTo = new int[]
				{
					BuffID.Poisoned,
					BuffID.Frostburn,
					BuffID.Ichor,
					BuffID.Venom,
					BuffID.OnFire
				}
			});
		}
		public override void SetDefaults()
		{
            NPC.aiStyle =0; 
            NPC.lifeMax = 100;   
            NPC.damage = 42; 
            NPC.defense = 24;  
            NPC.knockBackResist = 0f;
            NPC.width = 36;
            NPC.height = 46;
			Main.npcFrameCount[NPC.type] = 1;  
            NPC.value = 275;
            NPC.npcSlots = 2.5f;
            NPC.HitSound = SoundID.NPCHit53;
            NPC.DeathSound = SoundID.NPCDeath14;
			NPC.lavaImmune = true;
			NPC.netAlways = true;
			Banner = NPC.type;
			BannerItem = ItemType<HoloEyeBanner>();
		}
        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
			Texture2D texture = Mod.Assets.Request<Texture2D>("NPCs/HoloEyeBase").Value;
			Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
			Vector2 drawPos = new Vector2((float)(NPC.Center.X - (int)screenPos.X), (float)(NPC.Center.Y - (int)screenPos.Y) + 18);
			Main.spriteBatch.Draw(texture, drawPos, null, drawColor * ((255 - NPC.alpha) / 255f), 0f, drawOrigin, NPC.scale, SpriteEffects.None, 0f);
			Color color = new Color(100, 100, 100, 0); 
			texture = Mod.Assets.Request<Texture2D>("NPCs/HoloEyeBaseGlow").Value;
			for (int k = 0; k < 5; k++)
			{
				Vector2 offset = new Vector2(Main.rand.NextFloat(-1, 1f), Main.rand.NextFloat(-1, 1f)) * 0.25f * k;
				spriteBatch.Draw(texture, drawPos + offset, null, color * 0.66f, 0f, drawOrigin, NPC.scale, SpriteEffects.None, 0f);
			}
			return false;
		}
        public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
			Texture2D texture = Mod.Assets.Request<Texture2D>("NPCs/HoloEyeOutline").Value;
			Texture2D texture2 = Mod.Assets.Request<Texture2D>("NPCs/HoloEyeReticle").Value;
			Texture2D texture3 = Mod.Assets.Request<Texture2D>("NPCs/HoloEyePupil").Value;
			Texture2D texture4 = Mod.Assets.Request<Texture2D>("NPCs/HoloEyeFill").Value;
			Color color = new Color(110, 110, 110, 0);
			Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
			Vector2 drawOrigin2 = new Vector2(texture2.Width * 0.5f, texture2.Height * 0.5f);
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
					spriteBatch.Draw(texture4, new Vector2((float)(NPC.Center.X - (int)screenPos.X), (float)(NPC.Center.Y - (int)screenPos.Y) - 4), null, color * 0.5f, 0f, drawOrigin, NPC.scale, SpriteEffects.None, 0f);

				spriteBatch.Draw(texture, new Vector2((float)(NPC.Center.X - (int)screenPos.X) + x, (float)(NPC.Center.Y - (int)screenPos.Y) + y - 4), null, color * ((255 - NPC.alpha) / 255f), 0f, drawOrigin, NPC.scale, SpriteEffects.None, 0f);

				float scaleFactor = counter / 180f;
				scaleFactor = scaleFactor > 1 ? 1 : scaleFactor;
				if (lookAtPos.X != -1 && lookAtPos.Y != -1)
				{
					Main.spriteBatch.Draw(texture3, new Vector2((float)(NPC.Center.X - (int)screenPos.X) + x, (float)(NPC.Center.Y - (int)screenPos.Y) + y - 4) + between * (6 + -14 * NPC.ai[1]), null, color * ((255 - NPC.alpha) / 255f), 0f, drawOrigin3, 0.5f + NPC.scale - NPC.ai[1], SpriteEffects.None, 0f);
					Main.spriteBatch.Draw(texture2, new Vector2((float)(tracerPosX - (int)screenPos.X) + x, (float)(tracerPosY - (int)screenPos.Y) + y), null, color * scaleFactor, tracerXVelo * 0.04f, drawOrigin2, scaleFactor, SpriteEffects.None, 0f);
				}
			}
		}
		float tracerXVelo = 0;
		public void MoveCursorToPlayer()
		{
			float scaleFactor = counter / 360f;
			scaleFactor = scaleFactor > 1 ? 1 : scaleFactor;
			Player player = Main.player[NPC.target];
			Vector2 between = new Vector2(tracerPosX, tracerPosY) - player.Center;
			float length = between.Length() + 0.1f;
			float speed = 6.5f * scaleFactor;
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
		public void MoveEyesToCursor()
		{
			float scaleFactor = counter / 360f;
			scaleFactor = scaleFactor > 1 ? 1 : scaleFactor;
			Player player = Main.player[NPC.target];
			Vector2 between = new Vector2(tracerPosX, tracerPosY) - lookAtPos;
			float length = between.Length() + 0.1f;
			float speed = 5f * scaleFactor;
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
		float rotateCursor = 0;
		bool hasSeenPlayer = false;
		public override bool PreAI()
		{
			Player player = Main.player[NPC.target];
			NPC.TargetClosest(true);
			float distanceX = player.Center.X - NPC.Center.X;
			float distanceY = player.Center.Y - NPC.Center.Y;
			distanceX = Math.Abs(distanceX);
			distanceY = Math.Abs(distanceY);
			if (distanceX < 800 && distanceY < 400)
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
			Player player = Main.player[NPC.target];
			if (rotateCursor < 0)
			{
				rotateCursor = MathHelper.ToRadians(360) + rotateCursor;
			}
			while (rotateCursor > MathHelper.ToRadians(360))
			{
				rotateCursor -= MathHelper.ToRadians(360);
			}
			tracerXVelo = tracerPosX;
			counter++;
			if (NPC.ai[0] <= 180)
			{
				if(NPC.ai[0] == 0 && tracerPosX == 0 && tracerPosY == 0)
				{
					tracerPosX = NPC.Center.X;
					tracerPosY = NPC.Center.Y;
					lookAtPos = new Vector2(tracerPosX, tracerPosY);
				}
				MoveCursorToPlayer();
				MoveEyesToCursor();
			}
			tracerXVelo = tracerPosX - tracerXVelo;
			if (NPC.ai[0] == 180)
			{
				Vector2 between = lookAtPos - NPC.Center;
				rotateCursor = between.ToRotation();
				if(Main.netMode != NetmodeID.MultiplayerClient)
				{
					NPC.netUpdate = true;
				}
			}
			if (NPC.ai[0] > 180 && NPC.ai[0] < 240)
			{
				lookAtPos = new Vector2(tracerPosX, tracerPosY);
				Vector2 between = lookAtPos - NPC.Center;
				between = new Vector2(120, 0).RotatedBy(rotateCursor);
				lookAtPos = NPC.Center + between;
				if (rotateCursor != MathHelper.ToRadians(270))
				{
					if (rotateCursor > MathHelper.ToRadians(272) || rotateCursor < MathHelper.ToRadians(90))
					{
						rotateCursor -= MathHelper.ToRadians(2.5f);
					}
					else if (rotateCursor < MathHelper.ToRadians(268))
					{
						rotateCursor += MathHelper.ToRadians(2.5f);
					}
					else //trigger effects her ig
					{
						rotateCursor = MathHelper.ToRadians(270);
						NPC.ai[0] = 240;
					}
				}
			}
			else
			{
				NPC.ai[0]++;
			}
			if (NPC.ai[1] > 0)
			{
				NPC.ai[1] -= 0.05f;
			}
			if (NPC.ai[1] < 0)
			{
				NPC.ai[1] = 0;
			}
			if (NPC.ai[0] >= 240 && NPC.ai[0] <= 360)
			{
				int damage2 = SOTSNPCs.GetBaseDamage(NPC) / 2;
				if(NPC.ai[0] % 11 == 0)
				{
					Vector2 between = lookAtPos - NPC.Center;
					between.Normalize();
					if (Main.netMode != NetmodeID.MultiplayerClient)
						Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center.X + between.X * 24, NPC.Center.Y + between.Y * 24, 0, -1.666f, ModContent.ProjectileType<FallingBolt>(), damage2, 1f, Main.myPlayer, tracerPosX + Main.rand.Next(-14, 15), tracerPosY + Main.rand.Next(-14, 15));
					SOTSUtils.PlaySound(SoundID.Item92, (int)NPC.Center.X, (int)NPC.Center.Y, 0.5f);
					NPC.ai[1] = 1;
				}
			}
			if (NPC.ai[0] >= 440)
			{
				NPC.ai[0] = 0;
			}
		}
		public override void ModifyNPCLoot(NPCLoot npcLoot)
		{
			LeadingConditionRule postAdvisor = new LeadingConditionRule(new Common.ItemDropConditions.DownedAdvisorDropCondition());
			postAdvisor.OnSuccess(ItemDropRule.Common(ItemType<TwilightShard>(), 5));
			npcLoot.Add(postAdvisor);
			npcLoot.Add(ItemDropRule.Common(ItemType<TwilightGel>(), 1, 1, 2));
			npcLoot.Add(ItemDropRule.Common(ItemType<FragmentOfOtherworld>(), 5));
		}
		public override void HitEffect(int hitDirection, double damage)
		{
			if (NPC.life > 0)
			{
				int num = 0;
				while ((double)num < damage / (double)NPC.lifeMax * 40.0)
				{
					float scale = 1f;
					int type = DustID.Electric;
					if (Main.rand.NextBool(3))
					{
						type = ModContent.DustType<Dusts.CodeDust2>();
						scale = 2f;
					}
					Dust.NewDust(NPC.position, NPC.width, NPC.height, type, (float)(2 * hitDirection), -2f, 0, default, 0.6f * scale);
					num++;
				}
			}
			else
			{
				for (int k = 0; k < 50; k++)
				{
					float scale = 1f;
					int type = DustID.Electric;
					if (Main.rand.NextBool(3))
					{
						type = ModContent.DustType<Dusts.CodeDust2>();
						scale = 2f;
					}
					Dust.NewDust(NPC.position, NPC.width, NPC.height, type, (float)(2 * hitDirection), -2f, 0, default, scale);
					if (k % 2 == 0)
						Dust.NewDust(NPC.position, NPC.width, NPC.height, DustType<AvaritianDust>(), (float)(2 * hitDirection), -2f, 0, new Color(100, 100, 100, 250), 1f);
				}
				Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, ModGores.GoreType("Gores/HoloEyeGore1"), 1f);
			}
		}
	}
}