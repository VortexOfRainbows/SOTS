using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Dusts;
using SOTS.Items.Banners;
using SOTS.Items.Fragments;
using SOTS.Items.Otherworld;
using SOTS.Items.Otherworld.FromChests;
using SOTS.Items.Slime.Furniture;
using SOTS.Projectiles.Otherworld;
using System;
using System.ComponentModel;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace SOTS.NPCs
{
	public class TwilightScouter : ModNPC
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
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Twilight Scouter");
			Main.npcFrameCount[npc.type] = 3;
		}
		public override void SetDefaults()
		{
            npc.aiStyle = 0; 
            npc.lifeMax = 75;   
            npc.damage = 34; 
            npc.defense = 14;  
            npc.knockBackResist = 0.66f;
            npc.width = 56;
            npc.height = 36;
            npc.value = Item.buyPrice(0, 0, 12, 50);
            npc.npcSlots = 1f;
			npc.HitSound = SoundID.NPCHit4;
			npc.DeathSound = SoundID.NPCDeath14;
			npc.lavaImmune = true;
			npc.netAlways = true;
			npc.noGravity = true;
			npc.noTileCollide = false;
			npc.buffImmune[BuffID.OnFire] = true;
			npc.buffImmune[BuffID.Frostburn] = true;
			banner = npc.type;
			bannerItem = ItemType<TwilightScouterBanner>();
		}
        public override void FindFrame(int frameHeight)
        {
			npc.frameCounter++;
			if(npc.frameCounter >= 4)
            {
				npc.frame.Y = (npc.frame.Y + frameHeight) % (3 * frameHeight);
				npc.frameCounter = 0;
				Vector2 from = npc.Center + new Vector2(-30, 0).RotatedBy(npc.rotation);
				Dust dust = Dust.NewDustDirect(from - new Vector2(5), 0, 0, DustID.Electric, 0, 0, npc.alpha);
				dust.velocity *= 0.3f;
				dust.velocity += new Vector2(-2, 0).RotatedBy(npc.rotation);
				dust.noGravity = true;
				dust.alpha = npc.alpha;
			}
            base.FindFrame(frameHeight);
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
        {
			Texture2D texture = Main.npcTexture[npc.type];
			Texture2D texture2 = GetTexture("SOTS/NPCs/TwilightScouterGlow");
			Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, texture.Height / 6);
			float dir = npc.rotation;
			bool flip = false;
			if (Math.Abs(MathHelper.WrapAngle(dir)) <= MathHelper.ToRadians(90))
			{
				flip = true;
			}
			float bonusDir = !flip ? MathHelper.ToRadians(180) : 0;
			Main.spriteBatch.Draw(texture, npc.Center - Main.screenPosition + new Vector2(0, npc.gfxOffY), npc.frame, drawColor * ((255 - npc.alpha) / 255f), dir - bonusDir, drawOrigin, npc.scale, !flip ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);
			Main.spriteBatch.Draw(texture2, npc.Center - Main.screenPosition + new Vector2(0, npc.gfxOffY), npc.frame, new Color(100, 100, 120, 0), dir - bonusDir, drawOrigin, npc.scale, !flip ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);
			return false;
        }
		public void MoveCursorToPlayer()
		{
			float scaleFactor = counter / 120f;
			if (scaleFactor > 1)
				scaleFactor = 1;
			Player player = Main.player[npc.target];
			Vector2 between = player.Center - new Vector2(tracerPosX, tracerPosY);
			float length = between.Length();
			float speed = 12f * scaleFactor;
			if (speed > length)
			{
				speed = length;
			}
			between = between.SafeNormalize(Vector2.Zero) * speed;
			tracerPosX += between.X;
			tracerPosY += between.Y;
		}
		public void MoveToPlayer()
		{
			Player player = Main.player[npc.target];
			Vector2 between = player.Center - npc.Center;
			float length = between.Length();
			float speed = (float)Math.Pow(between.Length(), 1.1f) * 0.001f - 0.5f;
			if (speed > length)
			{
				speed = length;
			}
			between = between.SafeNormalize(Vector2.Zero) * speed * 0.4f;
			npc.velocity *= 0.93f;
			npc.velocity += between;
		}
		int counter = 0;
		bool hasSeenPlayer = false;
		public bool runOnce = true;
		public override bool PreAI()
		{
			counter++;
			npc.TargetClosest(false);
			Player player = Main.player[npc.target];
			if (runOnce)
			{
				if (player.Center.X > npc.Center.X)
					npc.ai[1] = 1;
				else
					npc.ai[1] = -1;
				if (Main.netMode == NetmodeID.Server)
					npc.netUpdate = true;
				runOnce = false;
			}
			if ((Vector2.Distance(player.Center, npc.Center) < 96 || npc.life < npc.lifeMax) && counter > 60)
			{
				hasSeenPlayer = true;
			}
			if(!hasSeenPlayer)
			{
				npc.velocity *= 0.98f;
				npc.velocity.X += npc.ai[1] * 0.07f;
				tracerPosX = npc.Center.X + npc.velocity.X * 0.2f;
				tracerPosY = npc.Center.Y;
			}
			return hasSeenPlayer;
		}
		public override void AI()
		{
			MoveCursorToPlayer();
			npc.ai[0]++;
			if(npc.ai[0] > 180 && npc.ai[0] < 280)
            {
				npc.velocity *= 0.975f;
            }
			else
				MoveToPlayer();
			if (npc.ai[0] >= 240 && npc.ai[0] <= 270)
			{
				int damage2 = npc.damage / 2;
				if (Main.expertMode)
				{
					damage2 = (int)(damage2 / Main.expertDamage);
				}
				if(npc.ai[0] % 20 == 0)
				{
					Vector2 between = new Vector2(tracerPosX, tracerPosY) - npc.Center;
					between = between.SafeNormalize(Vector2.Zero);
					npc.velocity -= between * 3.4f;
					if (Main.netMode != NetmodeID.MultiplayerClient)
						Projectile.NewProjectile(npc.Center + between * 24, between * 2.5f, ProjectileType<ThunderColumnBlue>(), damage2, 2f, Main.myPlayer, 2);
					Main.PlaySound(SoundID.Item, (int)npc.Center.X, (int)npc.Center.Y, 92, 0.5f, 0.1f);
					for (int k = 0; k < 16; k++)
					{
						Dust dust = Dust.NewDustDirect(npc.position, npc.width, npc.height, DustID.Electric, 0, 0, 0, default, 1f);
						dust.velocity *= 0.5f;
						dust.velocity += between * 4;
						dust.scale = dust.scale * 0.5f + 0.6f;
						dust.noGravity = true;
					}
				}
			}
			if (npc.ai[0] >= 270)
			{
				npc.ai[0] = Main.rand.Next(-60, 20);
				if(Main.netMode == NetmodeID.Server)
					npc.netUpdate = true;
			}
		}
        public override void PostAI()
		{
			npc.rotation = (new Vector2(tracerPosX, tracerPosY) - npc.Center).ToRotation();
		}
        public override void NPCLoot()
		{
			Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemType<TwilightGel>(), Main.rand.Next(2) + 1);
			Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemType<FragmentOfOtherworld>(), 1);
			if (!Main.rand.NextBool(3) && SOTSWorld.downedAdvisor)
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemType<TwilightShard>(), 1);
			if(Main.rand.NextBool(70))
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemType<Items.Otherworld.ThundershockShortbow>(), 1);
		}
        public override void HitEffect(int hitDirection, double damage)
		{
			if (npc.life > 0)
			{
				int num = 0;
				while ((double)num < damage / (double)npc.lifeMax * 40.0)
				{
					Dust.NewDust(npc.position, npc.width, npc.height, DustType<AvaritianDust>(), (float)(2 * hitDirection), -2f, 0, default, 0.5f);
					num++;
				}
			}
			else
			{
				for (int k = 0; k < 50; k++)
				{
					if (k % 5 == 0)
						Dust.NewDust(npc.position, npc.width, npc.height, DustID.Electric, (float)(2 * hitDirection), -2f, 0, default, 1f);
					Dust.NewDust(npc.position, npc.width, npc.height, DustType<AvaritianDust>(), (float)(2 * hitDirection), -2f, 0, new Color(100, 100, 100, 250), 1f);
				}
				for (int i = 1; i <= 3; i++)
					Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/TwilightScouter/TwilightScouterGore" + i), 1f);
			}
		}
	}
}