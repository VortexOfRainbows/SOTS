using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Common.GlobalNPCs;
using SOTS.Dusts;
using SOTS.Items.Banners;
using SOTS.Items.Earth.Glowmoth;
using SOTS.Items.Pyramid;
using SOTS.Projectiles.Earth.Glowmoth;
using SOTS.Projectiles.Pyramid;
using SOTS.WorldgenHelpers;
using System;
using System.IO;
using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace SOTS.NPCs.Boss.Glowmoth
{
	[AutoloadBossHead]
	public class Glowmoth : ModNPC
	{
		public Color glowColor = Color.White;
		public const int InitializationPhase = -1;
		public const int WanderPhase = 0;
		public const int ScatterShotPhase = 1;
		public const int MothAttackPhase = 2;
		public const int GlowBombPhase = 3;
		public const int SparklePhase = 4;
		public float SinusoidalCounter = 0;
		int despawn = 0;
        public override void SendExtraAI(BinaryWriter writer)
        {
			writer.Write(NPC.dontTakeDamage);
        }
        public override void ReceiveExtraAI(BinaryReader reader)
        {
			NPC.dontTakeDamage = reader.ReadBoolean();
        }
        private float AI0
		{
			get => NPC.ai[0];
			set => NPC.ai[0] = value;
		}
		private float AI1
		{
			get => NPC.ai[1];
			set => NPC.ai[1] = value;
		}
		private float AI2
		{
			get => NPC.ai[2];
			set => NPC.ai[2] = value;
		}
		private float AI3
		{
			get => NPC.ai[3];
			set => NPC.ai[3] = value;
		}
		public override bool CanHitPlayer(Player target, ref int cooldownSlot)
		{
			return !NPC.dontTakeDamage;
		}
		public override void SetStaticDefaults()
		{
			Main.npcFrameCount[Type] = 10;
			NPCID.Sets.TrailCacheLength[Type] = 10;
			NPCID.Sets.TrailingMode[Type] = 0;
			NPCID.Sets.MPAllowedEnemies[Type] = true;
            NPCID.Sets.NoMultiplayerSmoothingByType[NPC.type] = true;
        }
		public override void SetDefaults()
		{
			NPC.lifeMax = 2400;
			NPC.aiStyle = -1;
			NPC.damage = 28;
			NPC.defense = 10;
			NPC.knockBackResist = 0f;
			NPC.width = 62;
			NPC.height = 94;
			NPC.value = Item.buyPrice(0, 1, 0, 0);
			NPC.npcSlots = 5f;
			NPC.lavaImmune = true;
			NPC.noGravity = true;
			NPC.noTileCollide = true;
			NPC.netUpdate = true;
			NPC.HitSound = SoundID.NPCHit32;
			NPC.DeathSound = SoundID.NPCDeath5;
			NPC.netAlways = true;
			NPC.boss = true;
			NPC.dontTakeDamage = true;
			NPC.alpha = 255;
			Music = MusicID.Boss1; //MusicLoader.GetMusicSlot(Mod, "Sounds/Music/PutridPinky");
			SceneEffectPriority = SceneEffectPriority.BossLow;
		}
		public override void ApplyDifficultyAndPlayerScaling(int numPlayers, float balance, float bossAdjustment)/* tModPorter Note: bossLifeScale -> balance (bossAdjustment is different, see the docs for details) */
		{
			NPC.lifeMax = (int)(NPC.lifeMax * balance * bossAdjustment * 14 / 20); //140%, 210%
			NPC.damage = (int)(NPC.damage * 0.75f); //150%, 225%
		}
		public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
		{
			if (NPC.IsABestiaryIconDummy)
				NPC.alpha = 0;
			Texture2D texture = Terraria.GameContent.TextureAssets.Npc[NPC.type].Value;
			Texture2D textureG = (Texture2D)Request<Texture2D>("SOTS/NPCs/Boss/Glowmoth/GlowmothGlow");
			Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, texture.Height / Main.npcFrameCount[NPC.type] / 2);
			int frameHeight = texture.Height / Main.npcFrameCount[NPC.type];
			Vector2 drawPos = NPC.Center - screenPos;
			Color glow2 = glowColor * 0.5f;
			glow2.A = 0;
			int CurrentFrameNumber = (int)(NPC.frame.Y / frameHeight + 0.5f);
			for (int j = 0; j < NPCID.Sets.TrailCacheLength[Type]; j++)
			{
				int thisFrameNumber = (CurrentFrameNumber + j) % 10;
				Vector2 position = NPC.oldPos[j] + new Vector2(NPC.width, NPC.height) / 2;
				float alphaMult = 1 - (j / (float)NPCID.Sets.TrailCacheLength[Type]);
				Color trailColor = ColorHelpers.VibrantColorAttempt(j * 12);
				if (AI0 != SparklePhase)
				{
					trailColor = NPC.GetAlpha(trailColor) * 0.4f;
				}
				spriteBatch.Draw(textureG, position - Main.screenPosition, new Rectangle(0, thisFrameNumber * frameHeight, texture.Width, NPC.height), trailColor * alphaMult, NPC.rotation, drawOrigin, alphaMult, SpriteEffects.None, 0f);
			}
			spriteBatch.Draw(texture, drawPos, new Rectangle(0, NPC.frame.Y, texture.Width, NPC.height), NPC.GetAlpha(drawColor), NPC.rotation, drawOrigin, 1f, SpriteEffects.None, 0f);
			for (int i = 0; i < 4; i++)
			{
				Vector2 circular = new Vector2(2, 0).RotatedBy(MathHelper.PiOver2 * i + MathHelper.TwoPi / 270f * SinusoidalCounter);
				spriteBatch.Draw(textureG, drawPos + circular, new Rectangle(0, NPC.frame.Y, texture.Width, NPC.height), NPC.GetAlpha(glow2) * 0.75f, NPC.rotation, drawOrigin, 1f, SpriteEffects.None, 0f);
			}
			spriteBatch.Draw(textureG, drawPos, new Rectangle(0, NPC.frame.Y, texture.Width, NPC.height), NPC.GetAlpha(glowColor), NPC.rotation, drawOrigin, 1f, SpriteEffects.None, 0f);
			return false;
		}
		public bool RunOnce = true;
		public override bool PreAI()
		{
			NPC.TargetClosest(true);
			Player player = Main.player[NPC.target];
			if (RunOnce)
			{
				if(NPC.Distance(player.Center) > 640) //Basically teleport above the player when summoned with the candle
                {
					NPC.Center = player.Center + new Vector2(0, -120);
					if(Main.netMode == NetmodeID.Server)
                    {
						NPC.netUpdate = true;
                    }
                }
				SwapPhase(InitializationPhase);
				RunOnce = false;
			}
			if (despawn >= 600)
			{
				despawn++;
				NPC.velocity.Y -= 0.2f;
				if (despawn >= 720)
					NPC.active = false;
				return false;
			}
			else if (player.dead)
			{
				despawn++;
			}
			glowColor = ColorHelpers.VibrantColor;
			return true;
		}
        public override void AI()
		{
			NPC.TargetClosest(true);
			Player player = Main.player[NPC.target];
			Vector2 toPlayer = player.Center - NPC.Center;
			float distToPlayer = toPlayer.Length();
			if (AI0 == InitializationPhase)
			{
				NPC.alpha -= 3;
				if (NPC.alpha <= 0)
				{
					NPC.alpha = 0;
					if (AI1 >= 60)
						SwapPhase(WanderPhase);
					AI1++;
					return;
				}
				if (NPC.alpha <= 20)
				{
					if (AI2 != -1)
					{
						if (Main.netMode != NetmodeID.MultiplayerClient)
						{
							CircularBallBurst(24);
						}
						if (Main.netMode != NetmodeID.Server)
							Main.NewText(Language.GetTextValue("Mods.SOTS.BossAwoken.Glowmoth"), 175, 75, byte.MaxValue);
						SOTSUtils.PlaySound(SoundID.Roar, NPC.Center, 1.0f, 0.3f);
					}
					AI2 = -1;
				}
			}
			if (AI0 == WanderPhase)
			{
				toPlayer = toPlayer.SafeNormalize(Vector2.Zero) * (6.5f + distToPlayer / 80f);
				if (distToPlayer < 240)
				{
					toPlayer *= 0.25f;
				}
				NPC.velocity = Vector2.Lerp(NPC.velocity, toPlayer, 0.0425f + distToPlayer / 24000f);
				if (AI1 >= 240)
				{
					SwapPhase(MothAttackPhase);
					return;
				}
				AI1++;
			}
			if (AI0 == ScatterShotPhase)
			{
				if (AI1 < 60)
				{
					NPC.velocity = -toPlayer.SafeNormalize(Vector2.Zero) * 12f * (1 - AI1 / 60f);
				}
				NPC.velocity *= 0.5f;
				if (AI1 >= 60)
				{
					int shotcount = 12;
					int betweenShots = 100;
					if (Main.expertMode)
					{
						betweenShots = 80;
						shotcount = 18; //This will also be active in master mode
					}
					if (Main.masterMode)
						betweenShots = 60;
					if (AI1 % betweenShots == 0)
					{
						if (AI2 == 4)
						{
							SwapPhase(WanderPhase);
						}
						else
						{
							float staggerAmount = 120;
							if (AI2 == 1)
							{
								staggerAmount = 90;
							}
							if (AI2 == 2)
								staggerAmount = 60;
							if (AI2 == 3)
								staggerAmount = 45;
							CircularBallBurst(shotcount, staggerAmount + Main.rand.Next(-10, 11), AI2 + 2);
							AI2++;
						}
					}
				}
				AI1++;
			}
			if (AI0 == MothAttackPhase)
			{
				if (AI1 < 120)
				{
					NPC.velocity *= 0.8f;
				}
				else if (AI1 < 60)
				{
					NPC.velocity = toPlayer.SafeNormalize(Vector2.Zero) * 9f * (1.1f - AI1 / 60f);
				}
				if (AI1 == 120)
				{
					SummonMoths();
				}
				if (AI1 >= 120)
				{
					AI2++;
					int totalCycles = 6;
					float dashAI = AI2 % 140;
					if (dashAI == 139)
					{
						SOTSUtils.PlaySound(SoundID.Item60, NPC.Center, 1.1f, -0.4f);
						toPlayer = toPlayer.SafeNormalize(Vector2.Zero) * 12f;
						if (player.Center.Y > NPC.Center.Y)
							toPlayer += new Vector2(0, 2);
						else
							toPlayer -= new Vector2(0, 2);
						NPC.velocity *= 0.04f;
						NPC.velocity += toPlayer;
					}
					else if (dashAI > 40 && dashAI <= 50 && AI1 - 120 > dashAI)
					{
						if (dashAI == 50)
						{
							if (Main.netMode != NetmodeID.MultiplayerClient)
							{
								int damage = NPC.GetBaseDamage() / 2;
								int count = 8;
								for (int i = 0; i < count; i++)
								{
									float radians = i / (float)count * MathHelper.TwoPi;
									Vector2 circular = new Vector2(4f, 0).RotatedBy(radians + NPC.velocity.ToRotation());
									Projectile.NewProjectileDirect(NPC.GetSource_FromAI(), NPC.Center + circular.SafeNormalize(Vector2.Zero) * 32, circular, ModContent.ProjectileType<WaveBall>(), damage, 1f, Main.myPlayer, -1.25f, 0);
								}
							}
						}
						NPC.velocity *= 0.94f;
					}
					else if (dashAI > 50 && dashAI < 65)
					{
						toPlayer = toPlayer.SafeNormalize(Vector2.Zero) * (4.0f + distToPlayer / 80f);
						if (distToPlayer < 270)
						{
							toPlayer *= 0.2f;
						}
						NPC.velocity = Vector2.Lerp(NPC.velocity, toPlayer, 0.04f + distToPlayer / 27000f);
					}
					else if (dashAI > 90)
					{
						if (dashAI == 100)
						{
							SOTSUtils.PlaySound(SoundID.Item15, NPC.Center, 1.0f, -0.3f, 0.01f);
						}
						NPC.velocity *= 0.875f;
						float sinusoidal = (float)Math.Sin((dashAI - 100) / 50f * MathHelper.TwoPi);
						NPC.velocity -= toPlayer.SafeNormalize(Vector2.Zero) * 1.6f * sinusoidal;
					}
					if (AI1 - 120 >= 140 * totalCycles && dashAI == 60)
					{
						SwapPhase(GlowBombPhase);
						return;
					}
				}
				AI1++;
			}
			if (AI0 == GlowBombPhase)
			{
				AI1++;
				int cycleLength = 240;
				float aiCycle = AI1 % cycleLength;
				toPlayer += new Vector2(0, -96);
				if (aiCycle < 60)
				{
					toPlayer = toPlayer.SafeNormalize(Vector2.Zero) * (7.0f + distToPlayer / 160f);
					if (distToPlayer < 360)
					{
						toPlayer *= 0.5f;
					}
					NPC.velocity = Vector2.Lerp(NPC.velocity, toPlayer, 0.05f + distToPlayer / 18000f);
				}
				if (aiCycle > 60 && aiCycle < 120)
				{
					float sinusoid = (float)Math.Sin(MathHelper.ToRadians((aiCycle - 60) * 6f)); //this should complete a full sinusoid
					if (aiCycle == 65)
					{
						SOTSUtils.PlaySound(SoundID.Item15, NPC.Center, 1.0f, -0.2f);
					}
					if (aiCycle >= 65 && aiCycle % 8 == 0)
					{
						Vector2 center = NPC.Center + new Vector2(0, -64 + sinusoid * 48);
						float offsetSize = (aiCycle - 65) / 55f;
						for (int i = 0; i <= 360; i += 15)
						{
							Vector2 circularLocation = new Vector2(0, 48 * offsetSize).RotatedBy(MathHelper.ToRadians(i));
							circularLocation.Y *= 1 - offsetSize * 0.5f;
							int dust2 = Dust.NewDust(new Vector2(center.X + circularLocation.X - 5, center.Y + circularLocation.Y - 5), 0, 0, ModContent.DustType<Dusts.CopyDust4>(), Scale: 1 + offsetSize * 0.5f);
							Dust dust = Main.dust[dust2];
							dust.velocity = -circularLocation * 0.1f;
							dust.velocity.Y += NPC.velocity.Y - 2;
							dust.color = ColorHelpers.VibrantColorAttempt(Main.rand.NextFloat(180) + 180, true);
							dust.noGravity = true;
							dust.alpha = 100;
						}
					}
					NPC.velocity *= 0.925f;
					NPC.velocity.Y += sinusoid * (float)Math.Sqrt((aiCycle - 60) / 60f);
				}
				else if (aiCycle > 120)
				{
					toPlayer = toPlayer.SafeNormalize(Vector2.Zero) * (1.0f + distToPlayer / 480f);
					if (distToPlayer < 240)
					{
						toPlayer *= 0.25f;
					}
					NPC.velocity = Vector2.Lerp(NPC.velocity, toPlayer, 0.06f);
				}
				if (aiCycle == 120)
				{
					SOTSUtils.PlaySound(SoundID.Item94, NPC.Center, 0.75f, 0.1f);
					if (Main.netMode != NetmodeID.MultiplayerClient)
					{
						int damage = NPC.GetBaseDamage() / 2;
						Projectile.NewProjectileDirect(NPC.GetSource_FromAI(), NPC.Center + new Vector2(0, -48), new Vector2(0, -8), ProjectileType<GlowBombOrb>(), damage, 1f, Main.myPlayer, player.Center.Y - 40, 4);
					}
				}
				if (AI1 > cycleLength * 4)
				{
					SwapPhase(ScatterShotPhase);
				}
			}
			if (AI0 == SparklePhase)
			{
				AI1++;
				if (AI1 < 80)
				{
					NPC.alpha += 4;
					if (NPC.alpha > 255)
					{
						NPC.dontTakeDamage = true;
						NPC.alpha = 255;
					}
					toPlayer = toPlayer.SafeNormalize(Vector2.Zero) * (3.0f + distToPlayer / 240f);
					if (distToPlayer < 240)
					{
						toPlayer *= 0.1f;
					}
					NPC.velocity = Vector2.Lerp(NPC.velocity, toPlayer, 0.075f);
				}
				if (AI1 >= 80)
				{
					int totalCycles = (int)(AI1 - 80) / 30;
					if(totalCycles < 8)
					{
						float warpAI = (AI1 - 80) % 30;
						if (warpAI == 0)
						{
							if (Main.netMode != NetmodeID.MultiplayerClient)
							{
								Vector2 circular = Main.rand.NextVector2CircularEdge(600, 300);
								AI2 = circular.X + player.Center.X;
								AI3 = circular.Y + player.Center.Y;
							}
							if(totalCycles % 3 == 0)
								CircularBallBurst(12, 0, 1, 1.2f);
							NPC.netUpdate = true;
						}
						if (warpAI > 2 && warpAI < 24)
						{
							Vector2 travelTo = new Vector2(AI2, AI3);
							NPC.velocity = (travelTo - NPC.Center).SafeNormalize(Vector2.Zero) * 4;
							NPC.Center = Vector2.Lerp(NPC.Center, travelTo, (warpAI - 2f) / 22f);
						}
						if (AI1 % 20 == 5)
						{
							SOTSUtils.PlaySound(SoundID.Item42, NPC.Center, 0.75f, -0.4f);
							if (Main.netMode != NetmodeID.MultiplayerClient)
							{
								int damage = NPC.GetBaseDamage() / 2;
								Vector2 circular = Main.rand.NextVector2Circular(96, 96);
								Projectile.NewProjectileDirect(NPC.GetSource_FromAI(), NPC.Center, Main.rand.NextVector2Circular(12, 12), ProjectileType<GlowSparkle>(), damage, 1f, Main.myPlayer, player.Center.X + circular.X, player.Center.Y + circular.Y);
							}
						}
					}
					else
					{
						float warpAI = (AI1 - 80) % 30;
						if (totalCycles == 8 && warpAI == 0)
						{
							AI2 = player.Center.X;
							AI3 = player.Center.Y - 196;
						}
						else if(totalCycles < 10)
                        {
							Vector2 travelTo = new Vector2(AI2, AI3);
							NPC.velocity = (travelTo - NPC.Center).SafeNormalize(Vector2.Zero) * 4;
							NPC.Center = Vector2.Lerp(NPC.Center, travelTo, warpAI / 30f);
							if (totalCycles == 9)
							{
								NPC.velocity = Vector2.Zero;
								NPC.Center = travelTo;
							}
						}
						if(totalCycles == 10 && warpAI == 0)
                        {
							NPC.alpha = 0;
							NPC.velocity *= 0f;
							SOTSUtils.PlaySound(SoundID.Item62, NPC.Center, 1f, -0.5f);
							CircularBallBurst(24, 45, 4, 1.2f);
						}
						if (totalCycles == 12)
						{
							SwapPhase(WanderPhase);
						}
					}
				}
			}
		}
		public override void PostAI()
		{
			float sinusoid = (float)Math.Sin(MathHelper.ToRadians(SinusoidalCounter * 5));
			NPC.position += new Vector2(0, -sinusoid * 0.825f);
			NPC.alpha = Math.Clamp(NPC.alpha, 0, 255);
			float scalingFactor = 1 - NPC.alpha / 255f;
			SinusoidalCounter += scalingFactor * scalingFactor;
			NPC.rotation = NPC.velocity.X * 0.07f;
			bool tileCollide = true;
			if (AI0 == MothAttackPhase || AI0 == GlowBombPhase || AI0 == SparklePhase)
            {
				tileCollide = false;
            }
			if(tileCollide)
				NPC.velocity = Collision.TileCollision(NPC.position + new Vector2(20, 20), NPC.velocity, NPC.width - 40, NPC.height - 40, true, true);
		}
		public void SwapPhase(int Phase)
        {
			AI1 = 0;
			AI2 = 0;
			if(Phase == InitializationPhase)
			{
				NPC.dontTakeDamage = true;
			}
			else
				NPC.dontTakeDamage = false;
			if(Phase == ScatterShotPhase && NPC.life < NPC.lifeMax * 0.4f)
            {
				Phase = SparklePhase;
            }
			AI0 = Phase;
			NPC.netUpdate = true;
		}
		public void CircularBallBurst(int count = 0, float staggerAmount = 0, float staggerInterval = 1, float speed = 2.2f)
		{
			if (Main.netMode == NetmodeID.MultiplayerClient)
				return;
			int damage = NPC.GetBaseDamage() / 2;
			int previousIdentity = 0;
			Projectile firstProjectile = null;
			for (int i = 0; i < count; i++)
            {
				float radians = i / (float)count * MathHelper.TwoPi;
				Vector2 circular = new Vector2(speed, 0).RotatedBy(radians);
				float stagger = i % staggerInterval * staggerAmount;
				Projectile proj = Projectile.NewProjectileDirect(NPC.GetSource_FromAI(), NPC.Center + circular.SafeNormalize(Vector2.Zero) * 32, circular, ModContent.ProjectileType<WaveBall>(), damage, 1f, Main.myPlayer, previousIdentity, stagger);
				previousIdentity = proj.identity;
				if (firstProjectile == null)
					firstProjectile = proj;
            }
			firstProjectile.ai[0] = previousIdentity;
			firstProjectile.netUpdate = true;
		}
		public void SummonMoths()
		{
			SOTSUtils.PlaySound(SoundID.Item44, (int)NPC.Center.X, (int)NPC.Center.Y, 1f, -0.3f);
			if (Main.netMode == NetmodeID.MultiplayerClient)
				return;
			for (int j = 0; j < 2; j++)
            {
				float orbitRing = 180 / 2 * j;
				for (int i = 0; i < 8; i++)
				{
					float degreesRing = 360 / 8 * i + j * 30;
					NPC moth = NPC.NewNPCDirect(NPC.GetSource_FromAI(), NPC.Center + Main.rand.NextVector2CircularEdge(120, 120), ModContent.NPCType<GlowmothMinion>(), 0, NPC.whoAmI, degreesRing, orbitRing, Main.rand.NextBool(3) ? -1 : 0, 255);
					moth.netUpdate = true;
				}
			}
        }
        public override void FindFrame(int frameHeight) 
		{
			NPC.frameCounter++;
			if (NPC.frameCounter >= 3.5f) 
			{
				NPC.frameCounter -= 3.5f;
				NPC.frame.Y += frameHeight;
				if(NPC.frame.Y >= Main.npcFrameCount[NPC.type] * frameHeight)
				{
					NPC.frame.Y = 0;
				}
			}
		}
		public override void ModifyNPCLoot(NPCLoot npcLoot)
		{
			npcLoot.Add(ItemDropRule.BossBag(ItemType<GlowmothBag>()));

			LeadingConditionRule notExpertRule = new LeadingConditionRule(new Conditions.NotExpert());
			notExpertRule.OnSuccess(ItemDropRule.Common(ModContent.ItemType<Items.Earth.Glowmoth.TorchBomb>(), 1, 8, 16));
			notExpertRule.OnSuccess(ItemDropRule.FewFromOptions(1, 1, new int[] {
				ModContent.ItemType<IlluminantAxe>(),
				ModContent.ItemType<GuideToIllumination>(),
				ModContent.ItemType<IlluminantBow>(),
				ModContent.ItemType<IlluminantStaff>(),
				ModContent.ItemType<Items.Earth.Glowmoth.NightIlluminator>() }));
			npcLoot.Add(notExpertRule);
			npcLoot.Add(ItemDropRule.MasterModeCommonDrop(ModContent.ItemType<GlowmothRelic>()));
		}
		public override void HitEffect(NPC.HitInfo hit)
		{
			if (Main.netMode == NetmodeID.Server)
				return;
			if (NPC.life > 0)
			{
				int num = 0;
				while (num < hit.Damage / NPC.lifeMax * 50.0)
				{
					Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Silk, (float)(2 * hit.HitDirection), -2f);
					num++;
				}
			}
            else
			{
				for (int k = 0; k < 30; k++)
				{
					Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Silk, (float)(2 * hit.HitDirection), -2f);
				}
			}
		}
		public override void OnKill()
		{
			SOTSWorld.downedGlowmoth = true;
		}
	}
}





















