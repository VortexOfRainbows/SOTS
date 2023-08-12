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
using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace SOTS.NPCs
{
	public class TwilightScouter : ModNPC
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
		public override void SetStaticDefaults()
		{
			Main.npcFrameCount[NPC.type] = 3;
			NPCID.Sets.DebuffImmunitySets.Add(Type, new Terraria.DataStructures.NPCDebuffImmunityData
			{
				SpecificallyImmuneTo = new int[]
				{
					BuffID.Frostburn,
					BuffID.OnFire,
					BuffID.Poisoned,
					BuffID.Venom
				}
			});
		}
		public override void SetDefaults()
		{
            NPC.aiStyle =0; 
            NPC.lifeMax = 75;   
            NPC.damage = 34; 
            NPC.defense = 14;  
            NPC.knockBackResist = 0.66f;
            NPC.width = 56;
            NPC.height = 36;
            NPC.value = Item.buyPrice(0, 0, 12, 50);
            NPC.npcSlots = 1f;
			NPC.HitSound = SoundID.NPCHit4;
			NPC.DeathSound = SoundID.NPCDeath14;
			NPC.lavaImmune = true;
			NPC.netAlways = true;
			NPC.noGravity = true;
			NPC.noTileCollide = true;
			Banner = NPC.type;
			BannerItem = ItemType<TwilightScouterBanner>();
		}
        public override void FindFrame(int frameHeight)
        {
			NPC.frameCounter++;
			if(NPC.frameCounter >= 4)
            {
				NPC.frame.Y = (NPC.frame.Y + frameHeight) % (3 * frameHeight);
				NPC.frameCounter = 0;
				Vector2 from = NPC.Center + new Vector2(-30, 0).RotatedBy(NPC.rotation);
				Dust dust = Dust.NewDustDirect(from - new Vector2(5), 0, 0, DustID.Electric, 0, 0, NPC.alpha);
				dust.velocity *= 0.3f;
				dust.velocity += new Vector2(-2, 0).RotatedBy(NPC.rotation);
				dust.noGravity = true;
				dust.alpha = NPC.alpha;
			}
            base.FindFrame(frameHeight);
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
			Texture2D texture = Terraria.GameContent.TextureAssets.Npc[NPC.type].Value;
			Texture2D texture2 = (Texture2D)Request<Texture2D>("SOTS/NPCs/TwilightScouterGlow");
			Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, texture.Height / 6);
			float dir = NPC.rotation;
			bool flip = false;
			if (Math.Abs(MathHelper.WrapAngle(dir)) <= MathHelper.ToRadians(90))
			{
				flip = true;
			}
			float bonusDir = !flip ? MathHelper.ToRadians(180) : 0;
			Main.spriteBatch.Draw(texture, NPC.Center - screenPos + new Vector2(0, NPC.gfxOffY), NPC.frame, drawColor * ((255 - NPC.alpha) / 255f), dir - bonusDir, drawOrigin, NPC.scale, !flip ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);
			Main.spriteBatch.Draw(texture2, NPC.Center - screenPos + new Vector2(0, NPC.gfxOffY), NPC.frame, new Color(100, 100, 120, 0), dir - bonusDir, drawOrigin, NPC.scale, !flip ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);
			return false;
        }
		public void MoveCursorToPlayer()
		{
			float scaleFactor = counter / 120f;
			if (scaleFactor > 1)
				scaleFactor = 1;
			Player player = Main.player[NPC.target];
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
			Player player = Main.player[NPC.target];
			Vector2 between = player.Center - NPC.Center;
			float length = between.Length();
			float speed = (float)Math.Pow(between.Length(), 1.1f) * 0.001f - 0.5f;
			if (speed > length)
			{
				speed = length;
			}
			between = between.SafeNormalize(Vector2.Zero) * speed * 0.4f;
			NPC.velocity *= 0.93f;
			NPC.velocity += between;
		}
		int counter = 0;
		bool hasSeenPlayer = false;
		public bool runOnce = true;
		public override bool PreAI()
		{
			counter++;
			NPC.TargetClosest(false);
			Player player = Main.player[NPC.target];
			if (runOnce)
			{
				if (player.Center.X > NPC.Center.X)
					NPC.ai[1] = 1;
				else
					NPC.ai[1] = -1;
				if (Main.netMode == NetmodeID.Server)
					NPC.netUpdate = true;
				runOnce = false;
			}
			if ((Vector2.Distance(player.Center, NPC.Center) < 96 || NPC.life < NPC.lifeMax) && counter > 60)
			{
				hasSeenPlayer = true;
			}
			if(!hasSeenPlayer)
			{
				NPC.velocity *= 0.98f;
				NPC.velocity.X += NPC.ai[1] * 0.07f;
				tracerPosX = NPC.Center.X + NPC.velocity.X * 0.2f;
				tracerPosY = NPC.Center.Y;
			}
			return hasSeenPlayer;
		}
		public override void AI()
		{
			MoveCursorToPlayer();
			NPC.ai[0]++;
			if(NPC.ai[0] > 180 && NPC.ai[0] < 280)
            {
				NPC.velocity *= 0.975f;
            }
			else
				MoveToPlayer();
			if (NPC.ai[0] >= 240 && NPC.ai[0] <= 270)
			{
				int damage = SOTSNPCs.GetBaseDamage(NPC) / 2;
				if(NPC.ai[0] % 20 == 0)
				{
					Vector2 between = new Vector2(tracerPosX, tracerPosY) - NPC.Center;
					between = between.SafeNormalize(Vector2.Zero);
					NPC.velocity -= between * 3.4f;
					if (Main.netMode != NetmodeID.MultiplayerClient)
						Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center + between * 24, between * 2.5f, ProjectileType<ThunderColumnBlue>(), damage, 2f, Main.myPlayer, 2);
					SOTSUtils.PlaySound(SoundID.Item92, (int)NPC.Center.X, (int)NPC.Center.Y, 0.5f, 0.1f);
					for (int k = 0; k < 16; k++)
					{
						Dust dust = Dust.NewDustDirect(NPC.position, NPC.width, NPC.height, DustID.Electric, 0, 0, 0, default, 1f);
						dust.velocity *= 0.5f;
						dust.velocity += between * 4;
						dust.scale = dust.scale * 0.5f + 0.6f;
						dust.noGravity = true;
					}
				}
			}
			if (NPC.ai[0] >= 270)
			{
				NPC.ai[0] = Main.rand.Next(-60, 20);
				if(Main.netMode == NetmodeID.Server)
					NPC.netUpdate = true;
			}
		}
        public override void PostAI()
		{
			NPC.rotation = (new Vector2(tracerPosX, tracerPosY) - NPC.Center).ToRotation();
		}
        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
			npcLoot.Add(ItemDropRule.Common(ItemType<TwilightGel>(), 1, 1, 2));
			npcLoot.Add(ItemDropRule.Common(ItemType<FragmentOfOtherworld>()));
		}
        public override void HitEffect(NPC.HitInfo hit)
		{
			if (Main.netMode == NetmodeID.Server)
				return;
			if (NPC.life > 0)
			{
				int num = 0;
				while ((double)num < damage / (double)NPC.lifeMax * 40.0)
				{
					Dust.NewDust(NPC.position, NPC.width, NPC.height, DustType<AvaritianDust>(), (float)(2 * hitDirection), -2f, 0, default, 0.5f);
					num++;
				}
			}
			else
			{
				for (int k = 0; k < 50; k++)
				{
					if (k % 5 == 0)
						Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Electric, (float)(2 * hitDirection), -2f, 0, default, 1f);
					Dust.NewDust(NPC.position, NPC.width, NPC.height, DustType<AvaritianDust>(), (float)(2 * hitDirection), -2f, 0, new Color(100, 100, 100, 250), 1f);
				}
				for (int i = 1; i <= 3; i++)
					Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, ModGores.GoreType("Gores/TwilightScouter/TwilightScouterGore" + i), 1f);
			}
		}
	}
}