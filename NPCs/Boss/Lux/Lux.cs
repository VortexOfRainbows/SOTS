using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Common.GlobalNPCs;
using SOTS.Dusts;
using SOTS.Items.Banners;
using SOTS.Items.Chaos;
using SOTS.Items.Fragments;
using SOTS.NPCs.Constructs;
using SOTS.Projectiles.Chaos;
using SOTS.Void;
using SOTS.WorldgenHelpers;
using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace SOTS.NPCs.Boss.Lux
{
	[AutoloadBossHead]
	public class Lux : ModNPC
	{
		public override void SetStaticDefaults()
		{
			Main.npcFrameCount[NPC.type] = 1;
			DisplayName.SetDefault("Lux");
			NPCID.Sets.TrailCacheLength[NPC.type] = 10;
			NPCID.Sets.TrailingMode[NPC.type] = 0;
			NPCID.Sets.NPCBestiaryDrawModifiers drawModifiers = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
			{
				CustomTexturePath = "SOTS/BossCL/LuxBossLog",
				PortraitScale = 0.9f, // Portrait refers to the full picture when clicking on the icon in the bestiary
				PortraitPositionYOverride = -20f,
			};
			NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, drawModifiers);

			NPCID.Sets.DebuffImmunitySets.Add(Type, new Terraria.DataStructures.NPCDebuffImmunityData
			{
				SpecificallyImmuneTo = new int[]
				{
					BuffID.Poisoned,
					BuffID.Frostburn,
					BuffID.Ichor,
					BuffID.Venom,
					BuffID.OnFire,
					BuffID.BetsysCurse
				}
			});
		}
		public override void SetDefaults()
		{
			NPC.aiStyle = -1;
			NPC.lifeMax = 60000;
			NPC.damage = 100;
			NPC.defense = 54;
			NPC.knockBackResist = 0f;
			NPC.width = 70;
			NPC.height = 70;
			NPC.value = Item.buyPrice(0, 20, 0, 0);
			NPC.npcSlots = 10f;
			NPC.boss = true;
			NPC.lavaImmune = true;
			NPC.noGravity = true;
			NPC.noTileCollide = true;
			NPC.HitSound = SoundID.NPCHit54;
			NPC.DeathSound = SoundID.NPCDeath6;
			NPC.netAlways = false;
			Music = MusicLoader.GetMusicSlot(Mod, "Sounds/Music/Advisor");
			SceneEffectPriority = SceneEffectPriority.BossHigh;
		}
		public override void SendExtraAI(BinaryWriter writer)
		{
			writer.Write(NPC.dontTakeDamage);
			writer.Write(despawnTimer);
			writer.Write(NPC.alpha);
			writer.Write(attackTimer3);
			writer.Write(attackTimer4);
			writer.Write(SecondPhase);
			writer.Write(desperation);
			writer.Write(compressWings);
		}
        public override void ReceiveExtraAI(BinaryReader reader)
		{
			NPC.dontTakeDamage = reader.ReadBoolean();
			despawnTimer = reader.ReadSingle();
			NPC.alpha = reader.ReadInt32();
			attackTimer3 = reader.ReadSingle();
			attackTimer4 = reader.ReadSingle();
			SecondPhase = reader.ReadBoolean();
			desperation = reader.ReadBoolean();
			compressWings = reader.ReadSingle();
		}
		public float despawnTimer = 0;
        List<RingManager> rings = new List<RingManager>();
		private float wingCounter
		{
			get => NPC.ai[0];
			set => NPC.ai[0] = value;
		}
		private float attackPhase
		{
			get => NPC.ai[1];
			set => NPC.ai[1] = value;
		}
		private float attackTimer1
		{
			get => NPC.ai[2];
			set => NPC.ai[2] = value;
		}
		private float attackTimer2
		{
			get => NPC.ai[3];
			set => NPC.ai[3] = value;
		}
		private float attackTimer3 = 0;
		private float attackTimer4 = 0;
		float IdleTimer = 0;
		float wingSpeedMult = 1f;
		float wingHeight; //wings offset in degrees
		float drawNewWingsCounter = 0;
		public float wingHeightLerp = 0f;
		public float forcedWingHeight = 0;
		public bool allWingsForced = false;
		public float wingOutwardOffset = 0;
		public bool SecondPhase = false;
		public bool desperation = false;
		public float compressWings = 0;
        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
			Texture2D texture = Terraria.GameContent.TextureAssets.Npc[NPC.type].Value;
			Vector2 drawOrigin = new Vector2(Terraria.GameContent.TextureAssets.Npc[NPC.type].Value.Width * 0.5f, NPC.height * 0.5f);
			for (int k = 0; k < NPC.oldPos.Length; k++)
			{
				Vector2 drawPos = NPC.oldPos[k] - screenPos + drawOrigin + new Vector2(0f, NPC.gfxOffY);
				Color color = NPC.GetAlpha(VoidPlayer.pastelRainbow) * ((float)(NPC.oldPos.Length - k) / (float)NPC.oldPos.Length);
				color.A = 0;
				spriteBatch.Draw(texture, drawPos, null, color * 0.5f, NPC.rotation, drawOrigin, NPC.scale * 1.1f, SpriteEffects.None, 0f);
			}
			float bonusScale = drawNewWingsCounter * 0.1f;
			float bonusWidth = drawNewWingsCounter * 16f + wingOutwardOffset;
			float bonusDegree = drawNewWingsCounter * -13f;
			if(!desperation || attackTimer1 < 100)
				DrawRings(screenPos, spriteBatch, false);
			if (compressWings >= 2)
			{
				return false;
			}
			float overrideGenPercent = 1;
			if(desperation)
            {
				overrideGenPercent = 1 - attackTimer1 / 140f;
				if (overrideGenPercent < -0.05f)
					overrideGenPercent = -0.05f;
            }
			DrawWings(screenPos, 1, 1f + bonusScale, bonusWidth, bonusDegree, overrideGenPercent);
			if(drawNewWingsCounter > 0)
            {
				DrawWings(screenPos, 2, (1f + bonusScale) * 1.25f, bonusWidth + MathHelper.Lerp(12, 8, compressWings), bonusDegree + 36 * (1 - compressWings), drawNewWingsCounter * overrideGenPercent);
				DrawWings(screenPos, 0, (1f + bonusScale) * 0.75f, bonusWidth - MathHelper.Lerp(18, -12, compressWings), bonusDegree - 36 * (1 - compressWings), drawNewWingsCounter* overrideGenPercent);
			}
			return false;
		}
        public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
			Texture2D texture = Terraria.GameContent.TextureAssets.Npc[NPC.type].Value;
			Vector2 drawOrigin = new Vector2(Terraria.GameContent.TextureAssets.Npc[NPC.type].Value.Width * 0.5f, NPC.height * 0.5f);
			for (int k = 0; k < 7; k++)
			{
				Color color = new Color(100, 100, 100, 0);
				Vector2 circular = new Vector2(4, 0).RotatedBy(MathHelper.ToRadians(k * 60 + Main.GameUpdateCount));
				if (k != 0)
				{
					color = VoidPlayer.pastelAttempt(MathHelper.ToRadians(k * 60));
					color.A = 0;
				}
				else
					circular *= 0f;
				spriteBatch.Draw(texture, NPC.Center + circular - screenPos, null, NPC.GetAlpha(color), 0f, drawOrigin, NPC.scale * 1.1f, SpriteEffects.None, 0f);
			}
			if (!desperation || attackTimer1 < 100)
				DrawRings(screenPos, spriteBatch, true);
		}
		public void WingStuff()
		{
			wingCounter += 7.5f * wingSpeedMult;
			float dipAndRise = (float)Math.Sin(MathHelper.ToRadians(wingCounter));
			//dipAndRise *= (float)Math.sqrt(dipAndRise);
			wingHeight = 19 + dipAndRise * 27 * (1 - compressWings * 0.9f);
		}
		public void DrawWings(Vector2 screenPos, int ID, float sizeMult = 1f, float widthOffset = 0, float degreeOffset = 0, float genPercent = 1f)
		{
			Texture2D texture = (Texture2D)ModContent.Request<Texture2D>("SOTS/NPCs/Constructs/ChaosParticle");
			Vector2 origin = new Vector2(texture.Width / 2, texture.Height / 2);
			float dipAndRise;
			float wingHeight = this.wingHeight;
			int width = (int)(130 * sizeMult);
			int height = (int)(90 * sizeMult);
			int amtOfParticles = 120;
			if (ID == 2 || allWingsForced)
            {
				wingHeight = MathHelper.Lerp(this.wingHeight, forcedWingHeight, wingHeightLerp);
            }
			float supposedWingHeight = wingHeight - 19;
			dipAndRise = supposedWingHeight / 27f;
			if(ID == 2 || allWingsForced)
            {
				dipAndRise *= 1 - 2 * wingHeightLerp;
            }
			dipAndRise = MathHelper.Clamp(dipAndRise, -1, 1);
			for (int j = -1; j <= 1; j += 2)
			{
				float positionalRotation = NPC.rotation + MathHelper.ToRadians((wingHeight + degreeOffset) * -j);
				Vector2 position = NPC.Center + new Vector2(-28 * j * (1 - drawNewWingsCounter), 16).RotatedBy(NPC.rotation) + new Vector2(((width + NPC.width) / 2 + 54 + widthOffset) * j, 0).RotatedBy(positionalRotation);
				float degreesCount = 0f;
				float flapMult = 0.0f;
				float baseGrowth = 0.35f;
				float scaleGrowth = 0.15f;
				float totalSin = 450f;
				if (dipAndRise < 0)
				{
					baseGrowth = MathHelper.Lerp(baseGrowth, 0.2f, -dipAndRise);
					scaleGrowth = MathHelper.Lerp(scaleGrowth, 0.3f, -dipAndRise);
					totalSin = MathHelper.Lerp(totalSin, 405, (float)Math.Pow((-dipAndRise), 1.2f));
				}
				else
				{
					baseGrowth = MathHelper.Lerp(baseGrowth, 0.1f, dipAndRise);
					scaleGrowth = MathHelper.Lerp(scaleGrowth, 0.05f, dipAndRise);
					flapMult = MathHelper.Lerp(scaleGrowth, 0.3f, (float)Math.Pow(dipAndRise, 1.2f));
				}
				for (float i = 0; i < amtOfParticles;)
				{
					float half = amtOfParticles / 2f;
					if((i <= half * genPercent && i < half) || (i > half && i > amtOfParticles - half * genPercent))
					{
						float sinusoid = (float)Math.Sin(MathHelper.ToRadians(degreesCount));
						if (degreesCount < 0)
							sinusoid = 0;
						float radians = MathHelper.ToRadians(i * 360f / amtOfParticles);
						Color c = NPC.GetAlpha(VoidPlayer.pastelAttempt(radians + MathHelper.ToRadians(Main.GameUpdateCount)));
						Vector2 circular = new Vector2(-1, 0).RotatedBy(radians);
						float increaseAmount = 1f;
						if (i < amtOfParticles / 2)
						{
							degreesCount = MathHelper.Lerp(0, totalSin, (float)Math.Pow(i / amtOfParticles * 2f, 1.2f));
							circular.Y *= flapMult;
							circular.Y -= sinusoid * (baseGrowth + scaleGrowth * i / amtOfParticles);
						}
						else
						{
							float mult = (1 - (i - 60f) / amtOfParticles * 7f);
							if (mult < 0)
								mult = 0;
							circular.Y -= (float)Math.Sin(MathHelper.ToRadians(totalSin)) * (scaleGrowth + baseGrowth) * mult;
							if (circular.Y > 0)
							{
								sinusoid = (float)Math.Sin(MathHelper.ToRadians(Main.GameUpdateCount * 20f + i * 36));
								if (sinusoid < 0.0f)
								{
									if (sinusoid > -0.2f)
										increaseAmount = 0.35f;
									else if (sinusoid > -0.4f)
										increaseAmount = 0.5f;
									else if (sinusoid > -0.6f)
										increaseAmount = 0.75f;
									sinusoid = 0.0f;
								}
								else
								{
									increaseAmount = 0.25f;
								}
								circular.Y *= 1f + sinusoid * 0.4f;
							}
						}
						i += increaseAmount;
						circular.X *= width / 2 * j;
						circular.Y *= height / 2;
						circular = circular.RotatedBy(positionalRotation);
						Main.spriteBatch.Draw(texture, position - screenPos + circular, null, new Color(c.R, c.G, c.B, 0), radians * j, origin, NPC.scale * 0.8f * (0.5f + 0.5f * (float)Math.Sqrt(increaseAmount)), SpriteEffects.None, 0f);
					}
					else
                    {
						i++;
                    }
				}
			}
		}
		public void DrawRings(Vector2 screenPos, SpriteBatch spriteBatch, bool front = false)
        {
			if (runOnce)
				return;
			for(int i = 0; i < rings.Count; i++)
			{
				rings[i].Draw(screenPos, spriteBatch, 4 - i, (1 - NPC.alpha / 255f), drawNewWingsCounter, 1f, NPC.rotation, front);
				if (compressWings >= 2)
					break;
			}
        }
        public override void ModifyHitByProjectile(Projectile projectile, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
		{
			if (projectile.CountsAsClass(DamageClass.Melee))
			{
				damage = (int)(damage * 1.08f);
			}
			if (projectile.CountsAsClass(DamageClass.Magic))
			{
				if(projectile.type == ProjectileID.Blizzard)
				{
					damage = (int)(damage * 0.8f);
				}
				else
					damage = (int)(damage * 0.95f);
			}
			else if (projectile.CountsAsClass(DamageClass.Ranged))
			{
				damage = (int)(damage * 0.88f);
			}
			if(projectile.type == ProjectileID.UFOLaser)
			{
				damage = (int)(damage * 0.9f);
			}
			float damageMult = 1;
			switch(attackPhase)
            {
				case LaserOrbPhase:
					damageMult = 0.7f;
					break;
				case RGBTransition:
					damageMult = 0.35f;
					break;
				case RGBPhase:
					damageMult = 0.5f;
					break;
			}
			if (damageMult != 1)
				damage = (int)(damage * damageMult);
		}
        public void SetupDebuffImmunities()
        {
			NPC.buffImmune[BuffID.OnFire] = true;
			NPC.buffImmune[BuffID.Poisoned] = true;
			NPC.buffImmune[BuffID.Venom] = true;
			NPC.buffImmune[BuffID.Frostburn] = true;
			NPC.buffImmune[BuffID.Ichor] = true;
			NPC.buffImmune[BuffID.BetsysCurse] = true;
		}
        public override void ModifyNPCLoot(NPCLoot npcLoot)
		{
			npcLoot.Add(ItemDropRule.BossBag(ModContent.ItemType<LuxBag>()));
			LeadingConditionRule notExpertRule = new LeadingConditionRule(new Conditions.NotExpert());
			notExpertRule.OnSuccess(ItemDropRule.Common(ModContent.ItemType<PhaseOre>(), 1, 90, 150));
			notExpertRule.OnSuccess(ItemDropRule.Common(ItemID.SoulofLight, 1, 10, 20));
			npcLoot.Add(notExpertRule);
			npcLoot.Add(ItemDropRule.MasterModeCommonDrop(ModContent.ItemType<LuxRelic>()));
		}
        public override void OnKill()
		{
			//if (Main.netMode == NetmodeID.Server)
				//Terraria.Chat.ChatHelper.BroadcastChatMessage(NetworkText.FromLiteral("Gen Start -1"), VoidPlayer.ChaosPink);
			if (!SOTSWorld.downedLux && SOTSWorld.GlobalCounter > 120) //have to be in world for more than 2 seconds. Objective is to hopefully prevent recipe browser from crashing the game.
			{
				//if (Main.netMode == NetmodeID.Server)
					//Terraria.Chat.ChatHelper.BroadcastChatMessage(NetworkText.FromLiteral("Gen Start 0"), VoidPlayer.ChaosPink);
				PhaseWorldgenHelper.Generate();
				//if (Main.netMode == NetmodeID.Server)
					//Terraria.Chat.ChatHelper.BroadcastChatMessage(NetworkText.FromLiteral("Gen End 2"), VoidPlayer.ChaosPink);
				SOTSWorld.downedLux = true;
			}
		}
        public override void BossLoot(ref string name, ref int potionType)
        {
			potionType = ItemID.GreaterHealingPotion;
		}
        public override bool CanHitPlayer(Player target, ref int cooldownSlot)
        {
            return attackPhase != -1 && !NPC.dontTakeDamage;
        }
        public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
		{
			NPC.lifeMax = (int)(NPC.lifeMax * bossLifeScale * 0.75f); //90000 hp
			NPC.damage = (int)(NPC.damage * 0.75f); //150 damage
		}
		bool runOnce = true;
		public bool TargettingUnit()
		{
			NPC.velocity.X *= 0.93f; //consequence of being programmed with an aiStyle = 0
			if ((double)NPC.velocity.X > -0.1 && (double)NPC.velocity.X < 0.1) //consequence of being programmed with an aiStyle = 0
			{
				NPC.velocity.X = 0f;
			}
			if (!NPC.HasValidTarget)
				NPC.TargetClosest(false);
			else
			{
				if (NPC.Center.X < Main.player[NPC.target].Center.X)
					NPC.direction = -1;
				else
					NPC.direction = 1;
			}
			Player player = Main.player[NPC.target];
			if (despawnTimer > 300)
			{
				despawnTimer++;
				if (despawnTimer > 900)
					NPC.active = false;
				return true;
			}
			else
			{
				if (player.dead || !player.active)
				{
					despawnTimer++;
				}
				else if (despawnTimer > 0)
					despawnTimer--;
			}
			return false;
		}
		public override bool PreAI()
		{
			bool despawn = TargettingUnit(); //target only one player at a time before switching
			if(despawn && !desperation && attackPhase != DesperationPhase)
			{
				allWingsForced = false;
				modifyRotation(false, true);
				attackPhase = -2;
				NPC.dontTakeDamage = true;
				NPC.velocity.X *= 0.9f;
				if (despawnTimer > 390)
					NPC.velocity.Y -= 0.10f;
				else
				{
					for (int i = 0; i < 4; i++)
					{
						rings[i].MoveTo(NPC.Center, true);
						rings[i].ResetVariables();
					}
				}
			}
			int damage = SOTSNPCs.GetBaseDamage(NPC) / 2;
			Player player = Main.player[NPC.target];
			Vector2 toPlayer = player.Center - NPC.Center;
			NPC.TargetClosest(false);
			for (int i = 0; i < 4; i++)
			{
				if (runOnce)
					rings.Add(new RingManager(MathHelper.PiOver2 * i, 0.6f, 4 - (i / 2), 60 + (i / 2) * 24));
				else
					rings[i].CalculationStuff(NPC.Center);
			}
			if (runOnce)
			{
				NPC.dontTakeDamage = true;
				attackTimer1 = -90;
				attackPhase = -1;
				runOnce = false;
			}
			if(desperation)
			{
				modifyRotation(false, true);
				if (attackTimer1 < 90)
					for (int i = 0; i < 4; i++)
					{
						rings[i].ResetVariables();
						rings[i].MoveTo(NPC.Center, true);
					}
				if (attackPhase != DesperationPhase)
				{
					for(int i = 0; i < Main.projectile.Length; i++)
                    {
						Projectile proj = Main.projectile[i];
						if(proj.active && proj.type == ModContent.ProjectileType<DogmaSphere>())
						{
							proj.Kill();
						}
                    }
					SwapPhase(DesperationPhase);
                }
				NPC.dontTakeDamage = true;
				NPC.life = 1;
				NPC.lifeMax = 1;
				attackTimer1++;
				float lerp = attackTimer1 / 150f;
				for (int i = 0; i < 4; i++)
				{
					rings[i].radius = MathHelper.Lerp(rings[i].originalRadius, 0, lerp);
				}
				if (attackTimer1 > 180)
				{
					if (attackTimer2 == 0 && attackTimer3 == 0)
					{
						attackTimer2 = NPC.Center.X;
						attackTimer3 = NPC.Center.Y;
					}
					NPC.Center = new Vector2(attackTimer2, attackTimer3);
					if(attackTimer1 == 181 && Main.netMode != NetmodeID.MultiplayerClient)
					{
						NPC.velocity *= 0f;
						if (Main.netMode != NetmodeID.MultiplayerClient)
						{
							int n = NPC.NewNPC(NPC.GetSource_FromAI(), (int)NPC.Center.X + 10, (int)NPC.position.Y + 12, ModContent.NPCType<Collector2>());
							Main.npc[n].netUpdate = true;
							attackTimer4 = n;
							NPC.netUpdate = true;
						}
					}
					if (attackTimer4 != -1)
					{
						NPC.velocity *= 0f;
						NPC collector = Main.npc[(int)(attackTimer4 + 0.5f)];
						if (collector.type != ModContent.NPCType<Collector2>())
						{
							if (Main.netMode != NetmodeID.MultiplayerClient)
							{
								NPC npc2 = Main.npc[NPC.NewNPC(NPC.GetSource_FromAI(), (int)NPC.Center.X + 10, (int)NPC.position.Y + 12, ModContent.NPCType<Collector2>())];
								npc2.netUpdate = true;
								attackTimer4 = npc2.whoAmI;
								NPC.netUpdate = true;
							}
						}
						else
						{
							float ai3 = collector.ai[3];
							if (ai3 < 80 && ai3 > 0)
							{
								NPC.scale -= 0.012f;
								int dust3 = Dust.NewDust(new Vector2(NPC.position.X, NPC.position.Y), NPC.width, NPC.height, ModContent.DustType<CopyDust4>(), 0, 0, 0, VoidPlayer.pastelAttempt(Main.rand.NextFloat(6.28f), true));
								Dust dust4 = Main.dust[dust3];
								dust4.velocity *= 2.5f;
								dust4.noGravity = true;
								dust4.fadeIn = 0.1f;
								dust4.scale *= 2.5f;
								if (ai3 % 25 == 0)
								{
									if (Main.netMode != NetmodeID.MultiplayerClient)
									{
										int item = Item.NewItem(NPC.GetSource_Loot("SOTS:CollectorDissolvingBrilliance"), (int)NPC.position.X, (int)NPC.position.Y, NPC.width, NPC.height, ModContent.ItemType<DissolvingBrilliance>(), 1);
										Main.item[item].velocity = new Vector2(0, 5).RotatedBy(MathHelper.ToRadians(50 * ((ai3 - 50) / 25)));
										NetMessage.SendData(MessageID.SyncItem, -1, -1, null, item, 1f, 0.0f, 0.0f, 0, 0, 0);
									}
								}
							}
							if (ai3 >= 90)
							{
								NPC.StrikeNPC(10000, 0, 0);
								if (Main.netMode == NetmodeID.Server)
									NetMessage.SendData(MessageID.DamageNPC, -1, -1, null, NPC.whoAmI, 10000, 0, 0, 0, 0, 0);
							}
						}
					}
				}
				else
				{
					IdleTimer += 0.1f + 1.4f * (float)Math.Sin(MathHelper.Pi * attackTimer1 / 180f);
					NPC.Center += new Vector2(0, 3f * attackTimer1 / 180f * (float)Math.Sin(MathHelper.ToRadians(IdleTimer * 6)) * (float)Math.Sin(MathHelper.Pi * attackTimer1 / 180f));
				}
			}
			else
			{
				if (attackPhase == SetupPhase)
				{
					float lightMult = attackTimer1 / 120f;
					lightMult = MathHelper.Clamp(lightMult, 0, 1);
					SOTSWorld.LuxLightingFadeIn = lightMult;
					NPC.velocity *= 0.95f;
					attackTimer1++;
					if (attackTimer1 % 30 == 0 && attackTimer1 < 100 && attackTimer1 > 20)
					{
						SOTSUtils.PlaySound(SoundID.Item15, (int)NPC.Center.X, (int)NPC.Center.Y, 1.25f, 0.1f);
					}
					if (attackTimer1 == 120)
					{
						SOTSUtils.PlaySound(SoundID.Roar, (int)NPC.Center.X, (int)NPC.Center.Y, 1.3f, 0.1f);
						if(Main.netMode != NetmodeID.Server)
							Main.NewText("Lux has awoken!", 175, 75, byte.MaxValue);
					}
					if (attackTimer1 > 60)
					{
						NPC.dontTakeDamage = true;
						float mult = (attackTimer1 - 60f) / 60f;
						mult = MathHelper.Clamp(mult, 0, 1f);
						wingSpeedMult = MathHelper.Lerp(2f, 1.0f, mult);
						NPC.Center += new Vector2(0, (float)Math.Sin(MathHelper.ToRadians(attackTimer1 * 12)) * 6f * (1 - mult));
						if (attackTimer1 > 120)
						{
							SwapPhase(LaserOrbPhase);
						}
					}
					else if (attackTimer1 > 0)
					{
						float mult = attackTimer1 / 60f;
						mult = MathHelper.Clamp(mult, 0, 1f);
						wingSpeedMult = MathHelper.Lerp(0f, 2f, mult);
						drawNewWingsCounter = mult;
						if (attackTimer1 > 0)
							NPC.Center += new Vector2(0, (float)Math.Sin(MathHelper.ToRadians(attackTimer1 * 12)) * 6f * mult);
					}
					else
					{
						wingSpeedMult = MathHelper.Lerp(wingSpeedMult, 0f, 0.02f);
					}
					modifyRotation(false);
				}
				else
				{
					SOTSWorld.LuxLightingFadeIn = 1f;
					IdleTimer++;
					NPC.Center += new Vector2(0, (float)Math.Sin(MathHelper.ToRadians(IdleTimer * 6)));
				}
				if (attackPhase == LaserOrbPhase)
				{
					drawNewWingsCounter = 1;
					allWingsForced = false;
					modifyRotation(false, false);
					NPC.velocity *= 0.95f;
					NPC.dontTakeDamage = false;
					if (attackTimer2 == 0)
					{
						Vector2 toLocation = player.Center + new Vector2(0, -240);
						teleport(toLocation, player.Center);
						attackTimer2 = 1;
					}
					forcedWingHeight = 46;
					attackTimer1++;
					if (attackTimer1 <= 120)
					{
						wingHeightLerp = attackTimer1 / 120f * 0.9f;
						wingSpeedMult = 1 - attackTimer1 / 120f * 0.2f;
					}
					Vector2 laserPos = NPC.Center + new Vector2(0, -196);
					Vector2 otherLaserPosition = new Vector2(attackTimer3, attackTimer4);
					if (SecondPhase && attackTimer1 > 70)
					{
						if (attackTimer3 == 0 && attackTimer4 == 0)
						{
							otherLaserPosition = NPC.Center + toPlayer.SafeNormalize(Vector2.Zero) * 320 + Main.rand.NextVector2Circular(128, 128);
							attackTimer3 = otherLaserPosition.X;
							attackTimer4 = otherLaserPosition.Y;
							if (Main.netMode == NetmodeID.Server)
								NPC.netUpdate = true;
						}
					}
					if (attackTimer1 > 90 && attackTimer1 < 120)
					{
						for (int i = 2; i < 4; i++)
						{
							rings[i].MoveTo(laserPos);
							rings[i].targetRadius = 72;
						}
						if (SecondPhase)
						{
							for (int i = 0; i < 2; i++)
							{
								rings[i].MoveTo(otherLaserPosition);
								rings[i].targetRadius = 72;
							}
						}
					}
					if (attackTimer1 == 120)
					{
						for (int i = 2; i < 4; i++)
							rings[i].MoveTo(laserPos);
						if (Main.netMode != NetmodeID.MultiplayerClient)
						{
							if (SecondPhase)
							{
								Projectile.NewProjectile(NPC.GetSource_FromAI(), otherLaserPosition, Vector2.Zero, ModContent.ProjectileType<DogmaSphere>(), damage, 0, Main.myPlayer, NPC.target, -1); //Projectile.ai[1] value of -1 to carry on weaker dogma sphere projectile
								Projectile.NewProjectile(NPC.GetSource_FromAI(), laserPos, Vector2.Zero, ModContent.ProjectileType<DogmaSphere>(), damage, 0, Main.myPlayer, NPC.target, -2);
							}
							else
							{
								Projectile.NewProjectile(NPC.GetSource_FromAI(), laserPos, Vector2.Zero, ModContent.ProjectileType<DogmaSphere>(), damage, 0, Main.myPlayer, NPC.target);
							}
						}
					}
					if (attackTimer1 > 550)
					{
						for (int i = 0; i < 4; i++)
						{
							rings[i].ResetVariables();
							rings[i].MoveTo(NPC.Center, true);
						}
						float resetForceHeight = (1 - (attackTimer1 - 550) / 60f);
						if (resetForceHeight < 0)
						{
							forcedWingHeight = 0;
							resetForceHeight = 0;
						}
						wingHeightLerp = resetForceHeight * 0.9f;
						if (attackTimer1 >= 610)
						{
							SwapPhase(PickRandom((int)attackPhase));
						}
					}
				}
				if (attackPhase == ShotgunPhase)
				{
					NPC.velocity *= 0.93f;
					attackTimer1++;
					bool end = attackTimer3 > 5;
					if (end)
					{
						modifyRotation(false);
						for (int i = 0; i < 2; i++)
						{
							rings[i].ResetVariables();
						}
						if (attackTimer2 > 0)
							attackTimer2--;
						else
						{
							SwapPhase(PickRandom((int)attackPhase));
						}
					}
					else if (attackTimer1 > 0)
					{
						if (attackTimer1 < 100)
							modifyRotation(true);
						for (int i = 0; i < 2; i++)
						{
							rings[i].aiming = true;
							rings[i].targetRadius = 60;
						}
						if (attackTimer2 < 60)
							attackTimer2++;
						if (attackTimer1 == 90)
						{
							if (Main.netMode != NetmodeID.MultiplayerClient)
							{
								int rand = Main.rand.Next(4);
								if (attackTimer4 == rand)
									rand = Main.rand.Next(4); //allow it to hit the same angle again, but less likely
								float degrees = rand * 90f;
								Vector2 circular = new Vector2(640, 0).RotatedBy(MathHelper.ToRadians(degrees + Main.rand.NextFloat(35, 55f)));
								circular.Y *= 0.75f;
								teleport(player.Center + circular, player.Center, true);
								attackTimer4 = rand;
							}
						}
						else if (attackTimer1 > 100 && attackTimer1 <= 140)
						{
							float localCounter = attackTimer1 - 100;
							if (localCounter % 6 == 0)
							{
								Vector2 outward = new Vector2(0, 1).RotatedBy(NPC.rotation);
								SOTSUtils.PlaySound(SoundID.Item91, (int)NPC.Center.X, (int)NPC.Center.Y, 1.1f, 0.2f);
								if (Main.netMode != NetmodeID.MultiplayerClient)
								{
									int amt = 4 - (int)attackTimer3 % 2;
									for (int i = 0; i <= amt; i++)
									{
										float radians = MathHelper.ToRadians(28f * (i - amt / 2f));
										Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center + outward * 72, outward.RotatedBy(radians) * 5f, ModContent.ProjectileType<ChaosWave>(), damage, 0, Main.myPlayer, 0);
									}
								}
								NPC.velocity -= outward * 1.5f;
							}
						}
						if (attackTimer1 >= 150)
						{
							attackTimer1 = 89;
							attackTimer3++;
						}
						float speedMult = attackTimer1 / 60f;
						if (speedMult > 1)
							speedMult = 1;
						if (attackTimer1 < 90)
							NPC.velocity += toPlayer.SafeNormalize(Vector2.Zero) * 0.3f * speedMult;
					}
				}
				if (attackPhase == SubspaceCrossPhase)
				{
					modifyRotation(true);
					rings[0].aiming = true;
					rings[0].targetRadius = 60;
					NPC.velocity *= 0.6f;
					float distance = toPlayer.Length();
					int ring = 3 - ((int)attackTimer3 % 3);
					distance = (float)Math.Pow(distance, 1.2) * 0.01f - 12;
					NPC.velocity += toPlayer.SafeNormalize(Vector2.Zero) * distance * 0.25f;
					attackTimer1++;
					if (attackTimer3 > 12)
					{
						modifyRotation(false);
						if (attackTimer1 > 150)
						{
							for (int i = 1; i < 4; i++)
								rings[i].MoveTo(NPC.Center, true);
							rings[0].ResetVariables();
							if(attackTimer1 > 180)
								SwapPhase(PickRandom((int)attackPhase));
						}
					}
					else
					{
						if (attackTimer1 == 60)
						{
							rings[ring].MoveTo(player.Center);
						}
						if (attackTimer1 == 80)
						{
							if (Main.netMode != NetmodeID.MultiplayerClient)
							{
								Projectile.NewProjectile(NPC.GetSource_FromAI(), rings[ring].toLocation, Vector2.Zero, ModContent.ProjectileType<ChaosStar>(), damage, 0, Main.myPlayer, attackTimer3 % 2 * 45);
							}
							attackTimer3++;
							attackTimer1 = 50;
						}
						if (attackTimer3 > 2)
						{
							attackTimer2++;
							if (attackTimer2 > 70)
							{
								SOTSUtils.PlaySound(SoundID.Item91, (int)NPC.Center.X, (int)NPC.Center.Y, 1.1f, 0.2f);
								if (Main.netMode != NetmodeID.MultiplayerClient)
								{
									Vector2 outward = new Vector2(0, 1).RotatedBy(NPC.rotation);
									Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center + outward * 72, outward * 3f, ModContent.ProjectileType<ChaosDart>(), damage, 0, Main.myPlayer, NPC.target);
								}
								attackTimer2 = 0;
							}
						}
					}
				}
				if (attackPhase == ShatterLaserPhase)
				{
					allWingsForced = false;
					modifyRotation(false);
					attackTimer1++;
					int maxCharges = 8;
					int timeBetweenShots = 45;
					float speed2 = 40f;
					if (attackTimer4 >= maxCharges)
					{
						for (int i = 0; i < 4; i++)
						{
							rings[i].ResetVariables();
						}
						float timeToExplosion = timeBetweenShots + 120;
						float explodeMult = attackTimer1 / timeToExplosion;
						float slowDownMult = attackTimer1 / timeToExplosion * 2f;
						if (explodeMult > 1)
							explodeMult = 1;
						if (slowDownMult > 1)
							slowDownMult = 1;
						NPC.alpha = (int)(255 * (1 - explodeMult));
						speed2 *= 0.5f * (1 - slowDownMult) * (1 - slowDownMult);
						NPC.velocity *= (1 - slowDownMult);
						if (attackTimer1 > timeToExplosion + 60f)
						{
							forcedWingHeight = 0;
							wingHeightLerp = 0;
							SwapPhase(PickRandom((int)attackPhase));
						}
						else
						{
							if (attackTimer1 > timeToExplosion)
							{
								explodeMult = 1 - (attackTimer1 - timeToExplosion) / 50f;
								if (explodeMult < 0)
									explodeMult = 0;
							}
							forcedWingHeight = 54;
							wingHeightLerp = explodeMult * 0.95f;
							wingSpeedMult = 1 - explodeMult * 0.6f;
							wingOutwardOffset = -32 * explodeMult;
						}
					}
					else
					{
						wingSpeedMult = 1;
						NPC.alpha += 3 + ((int)attackTimer1 % 3 / 2);
						float compressMult = attackTimer1 / (30f + timeBetweenShots);
						if (compressMult > 1)
							compressMult = 1;
						speed2 *= compressMult;
						for (int i = 0; i < 4; i++)
						{
							rings[i].targetRadius = MathHelper.Lerp(rings[i].originalRadius, 30, compressMult);
						}
						if (attackTimer1 >= 30 + timeBetweenShots)
						{
							if (Main.netMode != NetmodeID.MultiplayerClient)
							{
								Vector2 spawnPosition = new Vector2(attackTimer2, attackTimer3);
								if (attackTimer2 <= 0 || attackTimer3 <= 0)
									spawnPosition = NPC.Center;
								int timeUntilEnd = 90 + timeBetweenShots * (int)(maxCharges - attackTimer4);
								Vector2 rotate = spawnPosition - player.Center;
								float lengthX = player.velocity.X * 1.2f;
								float lengthY = player.velocity.Y * 0.6f;
								float length = (lengthX * lengthX + lengthY * lengthY) * 0.525f;
								if (length > 70)
									length = 70;
								Vector2 spawnAt = player.Center + Main.rand.NextVector2Circular(96, 96) + player.velocity * length;
								Projectile.NewProjectile(NPC.GetSource_FromAI(), spawnAt, spawnPosition, ModContent.ProjectileType<ChaosDiamond>(), (int)(damage * 1.5f), 1, Main.myPlayer, timeUntilEnd, NPC.target);
								attackTimer2 = spawnAt.X;
								attackTimer3 = spawnAt.Y;
								NPC.netUpdate = true;
							}
							attackTimer1 = 30;
							attackTimer4++;
						}
					}
					if (speed2 > toPlayer.Length())
						speed2 = toPlayer.Length();
					NPC.velocity = toPlayer.SafeNormalize(Vector2.Zero) * speed2 * 0.25f;
				}
				if (attackPhase == ScatterBulletsPhase)
				{
					modifyRotation(false);
					NPC.velocity *= 0.93f;
					attackTimer1++;
					bool end = attackTimer3 > 7;
					if (SecondPhase)
						end = attackTimer3 > 12;
					if (end)
					{
						if (attackTimer2 > 0)
						{
							attackTimer2--;
							wingOutwardOffset = MathHelper.Lerp(0, -12, attackTimer2 / 30f);
						}
						else
						{
							SwapPhase(PickRandom((int)attackPhase));
						}
					}
					else if (attackTimer1 > 0)
					{
						if (attackTimer2 < 30)
						{
							wingOutwardOffset = MathHelper.Lerp(0, -12, attackTimer2 / 30f);
							attackTimer2++;

						}
						if (attackTimer1 == 90)
						{
							if (Main.netMode != NetmodeID.MultiplayerClient)
							{
								int rand = Main.rand.Next(8);
								if (attackTimer4 == rand)
									rand = Main.rand.Next(8); //allow it to hit the same angle again, but less likely
								float degrees = rand * 45f;
								Vector2 circular = new Vector2(Main.rand.NextFloat(640, 720), 0).RotatedBy(MathHelper.ToRadians(degrees + Main.rand.NextFloat(-5f, 5f)));
								circular.Y *= 0.85f;
								teleport(player.Center + circular, player.Center, true);
								attackTimer4 = rand;
							}
						}
						else if (attackTimer1 == 95)
						{
							SOTSUtils.PlaySound(SoundID.Item91, (int)NPC.Center.X, (int)NPC.Center.Y, 1.1f, 0.2f);
							if (Main.netMode != NetmodeID.MultiplayerClient)
							{
								if (SecondPhase)
								{
									int amt = 1;
									if (Main.expertMode)
										amt = 2;
									float startRadians = toPlayer.ToRotation() + MathHelper.ToRadians(Main.rand.NextFloat(-2.5f, 2.5f));
									for (int i = -amt; i <= amt; i++)
									{
										float radians = startRadians + MathHelper.ToRadians(i * 30f);
										Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center + new Vector2(1f, 0).RotatedBy(radians) * 40, new Vector2(1, 0).RotatedBy(radians) * 2.5f, ModContent.ProjectileType<ChaosDart2>(), damage, 0, Main.myPlayer, 0);
									}
								}
								else
								{
									int amt = 10;
									if (Main.expertMode)
										amt = 12;
									float startRadians = toPlayer.ToRotation() + MathHelper.ToRadians(Main.rand.NextFloat(-7f, 7f));
									for (int i = 0; i <= amt; i++)
									{
										float radians = startRadians + MathHelper.ToRadians(i * 360f / amt);
										Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center + new Vector2(1f, 0).RotatedBy(radians) * 40, new Vector2(1, 0).RotatedBy(radians) * 6f, ModContent.ProjectileType<ChaosBall>(), damage, 0, Main.myPlayer, 0);
									}
								}
							}
							wingOutwardOffset = 24;
						}
						else if (attackTimer1 > 95)
						{
							wingOutwardOffset = MathHelper.Lerp(24f, -12, 1 - ((attackTimer1 - 95) / 15f));
						}
						if (attackTimer1 > 110 || (SecondPhase && attackTimer1 > 100))
						{
							attackTimer1 = 89;
							attackTimer3++;
						}
						float speedMult = attackTimer1 / 60f;
						if (speedMult > 1)
							speedMult = 1;
						if (attackTimer1 < 90)
							NPC.velocity += toPlayer.SafeNormalize(Vector2.Zero) * 0.3f * speedMult;
					}
				}
				if (attackPhase == BigLaserPhase)
				{
					NPC.velocity *= 0.964f;
					attackTimer1++;
					int cycles = 3;
					if (SecondPhase)
						cycles = 4;
					if (attackTimer3 >= cycles)
					{
						for (int i = 0; i < 4; i++)
						{
							rings[i].ResetVariables();
						}
						if (attackTimer1 > 120)
						{
							SwapPhase(PickRandom((int)attackPhase));
						}
					}
					else
					{
						int numberOfShots = 30;
						if (SecondPhase)
							numberOfShots = 20;
						if (attackTimer1 == 60)
						{
							Vector2 toLocation = player.Center + new Vector2(480, 0).RotatedBy(toPlayer.ToRotation()); //teleports behind the player
							teleport(toLocation, player.Center);
						}
						if (attackTimer1 < 120 && attackTimer1 >= 60)
						{
							modifyRotation(true);
							for (int i = 0; i < 4; i++)
							{
								rings[i].aiming = true;
								rings[i].targetRadius = 72;
							}
							if (attackTimer1 % 20 == 0)
							{
								SOTSUtils.PlaySound(SoundID.Item15, (int)NPC.Center.X, (int)NPC.Center.Y, 1.6f, 0.1f - 0.1f * attackTimer1 / 20f);
							}
						}
						if (attackTimer1 >= 120)
						{
							if (attackTimer1 % 5 == 0)
							{
								Vector2 outward = new Vector2(0, 1).RotatedBy(NPC.rotation);
								if (Main.netMode != NetmodeID.MultiplayerClient)
								{
									Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, outward * 4, ModContent.ProjectileType<ChaosEraser>(), (int)(damage * 1.4f), 0, Main.myPlayer, NPC.target, 0.023f * attackTimer2 * 30f / numberOfShots);
								}
								attackTimer2++;
								NPC.velocity -= outward * 1f;
							}
						}
						else if (attackTimer1 % 2 == 0 && SecondPhase && attackTimer1 > 60)
							attackTimer1++;
						if (attackTimer2 > numberOfShots)
						{
							attackTimer1 = 50;
							attackTimer2 = 0;
							attackTimer3++;
						}
					}
				}
				if (attackPhase == RGBTransition || attackPhase == RGBPhase)
				{
					for (int i = 0; i < 4; i++)
					{
						rings[i].ResetVariables();
					}
					allWingsForced = false;
					modifyRotation(false, true);
					NPC.velocity *= 0.9f;
					attackTimer1++;
					compressWings = attackTimer1 / 180f;
					if (compressWings > 1)
					{
						compressWings = 1;
					}
					if (attackTimer1 >= 180)
					{
						float end = 1200;
						if (attackPhase == RGBPhase)
							end = 900;
						compressWings = 2;
						if (attackTimer1 == 180)
						{
							teleport(player.Center + new Vector2(0, -240), player.Center);
							attackTimer2 = player.Center.X;
							attackTimer3 = player.Center.Y - 240;
							SOTSUtils.PlaySound(SoundID.Item62, (int)NPC.Center.X, (int)NPC.Center.Y, 1.2f, 0.4f);
							for (int i = 0; i < 120; i++)
							{
								Dust dust = Dust.NewDustDirect(new Vector2(NPC.position.X, NPC.position.Y), NPC.width, NPC.height, DustID.RainbowMk2);
								dust.color = VoidPlayer.pastelAttempt(Main.rand.NextFloat(6.28f), true);
								dust.noGravity = true;
								dust.fadeIn = 0.1f;
								dust.scale *= 2.4f;
								dust.velocity *= 4f;
							}
							if (Main.netMode != NetmodeID.MultiplayerClient)
								for (int i = 0; i < 3; i++)
								{
									int npc1 = NPC.NewNPC(NPC.GetSource_FromAI(), (int)NPC.Center.X, (int)NPC.Center.Y, ModContent.NPCType<FakeLux>(), 0, NPC.whoAmI, 120 * i, 0, attackPhase == RGBPhase ? 1 : 0); //summons 120, 240, and 360
									Main.npc[npc1].netUpdate = true;
								}
						}
						else
						{
							float rnTimer = attackTimer1 - 180; //will start at 1
							attackTimer4++;
							Vector2 nextDestination = new Vector2(attackTimer2, attackTimer3);
							float multiplier = rnTimer / (150f); //will equal 1 when rnTimer > 180
							if (multiplier > 1)
								multiplier = 1;
							if (attackTimer4 > end)
							{
								if (attackTimer4 > end + 120)
								{
									compressWings = 0;
									NPC.alpha = 0;
									SwapPhase(PickRandom(RGBPhase));
									teleport(new Vector2(player.Center.X, player.Center.Y - 240), player.Center);
									SOTSUtils.PlaySound(SoundID.Item62, (int)NPC.Center.X, (int)NPC.Center.Y, 1.2f, 0.4f);
									for (int i = 0; i < 120; i++)
									{
										Dust dust = Dust.NewDustDirect(new Vector2(NPC.position.X, NPC.position.Y), NPC.width, NPC.height, DustID.RainbowMk2);
										dust.color = VoidPlayer.pastelAttempt(Main.rand.NextFloat(6.28f), true);
										dust.noGravity = true;
										dust.fadeIn = 0.1f;
										dust.scale *= 2.4f;
										dust.velocity *= 4f;
									}
								}
								NPC.alpha += 4;
							}
							else if (rnTimer >= 200)
							{
								SOTSUtils.PlaySound(SoundID.Item91, (int)NPC.Center.X, (int)NPC.Center.Y, 1.1f, 0.2f);
								if (Main.netMode != NetmodeID.MultiplayerClient)
								{
									int amt = 8;
									if (Main.expertMode)
										amt = 12;
									for (int i = 0; i <= amt; i++)
									{
										float radians = MathHelper.ToRadians(i * 360f / amt);
										Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center + new Vector2(1f, 0).RotatedBy(radians) * 40, new Vector2(1, 0).RotatedBy(radians) * 6f, ModContent.ProjectileType<ChaosBall>(), damage, 0, Main.myPlayer, 0);
									}
								}
								attackTimer1 -= 200;
							}
							else if (rnTimer % 50 == 0)
							{
								nextDestination = NPC.Center + toPlayer * (0.4f + 0.7f * multiplier) + Main.rand.NextVector2CircularEdge(80, 80) * (1.0f - 0.7f * multiplier);
								attackTimer2 = nextDestination.X;
								attackTimer3 = nextDestination.Y;
								NPC.netUpdate = true;
							}
							else
							{
								float lerpSpeed = 0.065f;
								if (attackPhase == RGBPhase)
									lerpSpeed = 0.03f;
								NPC.Center = Vector2.Lerp(NPC.Center, nextDestination, lerpSpeed);
							}
						}
						if (attackTimer4 < end)
							NPC.alpha = 0;
						SecondPhase = true;
					}
					else
					{
						if (attackTimer1 > 90)
						{
							float rnMult = (attackTimer1 - 90f) / 60f;
							if (rnMult > 1)
								rnMult = 1;
							NPC.alpha = (int)(rnMult * 255);
						}
					}
				}
				if (attackPhase == HelixColumnPhase)
				{
					NPC.velocity *= 0.93f;
					attackTimer1++;
					bool end = attackTimer3 > 6;
					if (end)
					{
						modifyRotation(false);
						for (int i = 0; i < 4; i++)
						{
							rings[i].ResetVariables();
						}
						if (attackTimer2 > 0)
							attackTimer2--;
						else
						{
							SwapPhase(PickRandom((int)attackPhase));
						}
					}
					else if (attackTimer1 > 0)
					{
						if (attackTimer1 < 100)
							modifyRotation(true);
						rings[0].aiming = true;
						rings[0].targetRadius = 60;
						if (attackTimer2 < 60)
							attackTimer2++;
						if (attackTimer1 == 90)
						{
							if (Main.netMode != NetmodeID.MultiplayerClient)
							{
								int rand = Main.rand.Next(4);
								if (attackTimer4 == rand)
									rand = Main.rand.Next(4); //allow it to hit the same angle again, but less likely
								float degrees = rand * 90f;
								Vector2 circular = new Vector2(640, 0).RotatedBy(MathHelper.ToRadians(degrees + Main.rand.NextFloat(-14, 14f)));
								circular.Y *= 0.75f; //480
								teleport(player.Center + circular, player.Center, true);
								attackTimer4 = rand;
							}
						}
						else if (attackTimer1 >= 100 && attackTimer1 <= 170) //will launch 14 shots total
						{
							float localCounter = attackTimer1 - 100;
							if (localCounter % 5 == 0)
							{
								Vector2 outward = new Vector2(0, 1).RotatedBy(NPC.rotation);
								SOTSUtils.PlaySound(SoundID.Item91, (int)NPC.Center.X, (int)NPC.Center.Y, 1.1f, 0.2f);
								if (Main.netMode != NetmodeID.MultiplayerClient)
								{
									float degrees = attackTimer4 * 90f;
									Vector2 direction = new Vector2(-1, 0).RotatedBy(MathHelper.ToRadians(degrees));
									direction.Y *= 0.75f;
									Vector2 spreadOut = new Vector2(0, 1200).RotatedBy(MathHelper.ToRadians(degrees));
									spreadOut.X *= 0.825f;
									Vector2 destinationTop = NPC.Center + direction * 3000 + spreadOut;
									Vector2 destinationBot = NPC.Center + direction * 3000 - spreadOut;
									float num = localCounter / 70f;
									if (attackTimer3 % 2 == 0)
										num = 1 - num;
									Vector2 destination = Vector2.Lerp(destinationTop, destinationBot, num);
									Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center + outward * 96, direction, ModContent.ProjectileType<ThunderSpawnBeam>(), damage, NPC.target, Main.myPlayer, destination.X, destination.Y);
								}
								NPC.velocity -= outward * 2f;
							}
						}
						if (attackTimer1 >= 171)
						{
							attackTimer1 = 89;
							attackTimer3++;
						}
						float speedMult = attackTimer1 / 60f;
						if (speedMult > 1)
							speedMult = 1;
						if (attackTimer1 < 90)
							NPC.velocity += toPlayer.SafeNormalize(Vector2.Zero) * 0.3f * speedMult;
					}
				}
			}
			WingStuff();
			NPC.alpha = (int)MathHelper.Clamp(NPC.alpha, 0, 255);
			if(attackPhase != DesperationPhase && !despawn)
			{
				if (NPC.alpha > 150)
					NPC.dontTakeDamage = true;
				else if (attackPhase != SetupPhase)
					NPC.dontTakeDamage = false;
			}
			return true;
		}
		public const float timeToAimAtPlayer = 40;
		public float aimToPlayer = 0;
		public void modifyRotation(bool aimAtPlayer, bool modifyWings = true)
		{
			Player player = Main.player[NPC.target];
			Vector2 toPlayer = player.Center - NPC.Center;
			NPC.rotation = NPC.velocity.X * 0.06f;
			if (aimAtPlayer)
			{
				if (modifyWings)
				{
					forcedWingHeight = 40;
					allWingsForced = true;
				}
				aimToPlayer++;
			}
			else
            {
				aimToPlayer--;
			}
			aimToPlayer = MathHelper.Clamp(aimToPlayer, 0, timeToAimAtPlayer);
			if (modifyWings)
				wingHeightLerp = aimToPlayer / timeToAimAtPlayer * 0.85f;
			float r = toPlayer.ToRotation() - MathHelper.PiOver2;
			float x = NPC.rotation - r;
			x = MathHelper.WrapAngle(x);
			float lerpedAngle = MathHelper.Lerp(x, 0, aimToPlayer / timeToAimAtPlayer);
			lerpedAngle += r;
			NPC.rotation = lerpedAngle;

		}
		public override void PostAI()
		{
			int dust2 = Dust.NewDust(new Vector2(NPC.position.X, NPC.position.Y), NPC.width, NPC.height, DustID.RainbowMk2);
			Dust dust = Main.dust[dust2];
			dust.color = NPC.GetAlpha(VoidPlayer.pastelAttempt(Main.rand.NextFloat(6.28f), true));
			dust.noGravity = true;
			dust.fadeIn = 0.1f;
			dust.scale *= 2f;
		}
		public override void HitEffect(int hitDirection, double damage)
		{
			if (NPC.life <= 0)
			{
				if(attackPhase != DesperationPhase)
                {
					desperation = true;
					NPC.dontTakeDamage = true;
					NPC.life = 1;
					NPC.lifeMax = 1;
				}
				if (Main.netMode != NetmodeID.Server)
				{
					for (int i = 0; i < 50; i++)
					{
						Dust dust = Dust.NewDustDirect(new Vector2(NPC.position.X, NPC.position.Y), NPC.width, NPC.height, DustID.RainbowMk2);
						dust.color = VoidPlayer.pastelAttempt(Main.rand.NextFloat(6.28f), true);
						dust.noGravity = true;
						dust.fadeIn = 0.1f;
						dust.scale *= 2.2f;
						dust.velocity *= 4f;
					}
				}
			}
		}
        public override bool CheckDead()
        {
			if(attackPhase == DesperationPhase && desperation && attackTimer1 > 180)
            {
				return true;
			}
			desperation = true;
			NPC.lifeMax = 1;
			NPC.life = 1;
			NPC.dontTakeDamage = true;
			return false;
        }
        public void teleport(Vector2 destination, Vector2 playerDestination, bool serverOnly = false)
		{
			if (Main.netMode != NetmodeID.MultiplayerClient)
			{
				Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, playerDestination, ModContent.ProjectileType<LuxRelocatorBeam>(), 0, 0, Main.myPlayer, destination.X, destination.Y);
			}
			if(!serverOnly || Main.netMode != NetmodeID.MultiplayerClient)
				NPC.Center = destination;
			if(Main.netMode == NetmodeID.Server)
				NPC.netUpdate = true;
		}
		public const int SetupPhase = -1;
		public const int LaserOrbPhase = 0;
		public const int ShotgunPhase = 1;
		public const int SubspaceCrossPhase = 2;
		public const int ShatterLaserPhase = 3;
		public const int ScatterBulletsPhase = 4;
		public const int BigLaserPhase = 5;
		public const int RGBTransition = 6;
		public const int HelixColumnPhase = 7;
		public const int RGBPhase = 8;
		public const int DesperationPhase = 9;
		public int[] previousAttacks = new int[] { -2, -2, -2 };
		public int PickRandom(int exclude = -1)
        {
			if(!SecondPhase)
			{
				int max = 6;
				int rand = Main.rand.Next(max);
				while (rand == exclude || previousAttacks.Contains(rand))
					rand = Main.rand.Next(max);
				return rand;
			}
			else
            {
				int[] phase2Attacks = new int[] { HelixColumnPhase, LaserOrbPhase, ScatterBulletsPhase, BigLaserPhase, RGBPhase };
				int rand = Main.rand.Next(phase2Attacks.Count());
				while(phase2Attacks[rand] == exclude || previousAttacks[0] == phase2Attacks[rand] || previousAttacks[1] == phase2Attacks[rand])
					rand = Main.rand.Next(phase2Attacks.Count());
				return phase2Attacks[rand];
			}
		}
		public bool canEnterSecondPhase()
        {
			float healthRange = 0.45f;
			if (Main.expertMode)
				healthRange = 0.55f;
			return !SecondPhase && NPC.life <= (NPC.lifeMax  * healthRange);
		}
		public void SwapPhase(int phase)
		{
			if (canEnterSecondPhase())
			{
				phase = RGBTransition;
			}
			int setAttack = (int)attackPhase; //ban previous attack phase
			for (int i = 0; i < 3; i++)
			{
				int temp = previousAttacks[i];
				previousAttacks[i] = setAttack;
				setAttack = temp;
			}
			//Main.NewText("Banned: " + previousAttacks[0] + " : " + previousAttacks[1] + " : " + previousAttacks[2]);
			attackPhase = phase;
			attackTimer1 = 0;
			attackTimer2 = 0;
			attackTimer3 = 0;
			attackTimer4 = 0;
			if (phase == ShotgunPhase)
			{
				attackTimer4 = -1;
				attackTimer1 = -40;
			}
			if (phase == SubspaceCrossPhase)
			{
				attackTimer1 = 0;
			}
			if (Main.netMode == NetmodeID.Server)
				NPC.netUpdate = true;
        }
	}
	public class RingManager
	{
		public const int AimTimerMax = 60;
		public Vector2 location = Vector2.Zero;
		public Vector2 toLocation = Vector2.Zero;
		public bool backToNPC = false;
		public float nextRotation = 0f;
		public float nextCompression = 0f;
		public float prevRotation = 0f;
		public float prevCompression = 0f;
		public float rotation = 0f;
		public float compression = 0f;
		public float originalRadius = 32f;
		public float radius = 32f;
		public float counter = -1;
		public float countingSpeed = 3f;
		public float targetRadius = 32f;
		public bool aiming = false;
		public float aimTimer = 0;
		public void ResetVariables()
        {
			aiming = false;
			targetRadius = originalRadius;
        }
		public RingManager(float rotation, float compression, float countingSpeed = 3f, float radius = 32f)
		{
			this.rotation = rotation;
			this.compression = compression;
			this.radius = radius;
			targetRadius = radius;
			originalRadius = radius;
			this.countingSpeed = countingSpeed;
		}
		public void MoveTo(Vector2 destination, bool backToNPC = false)
        {
			toLocation = destination;
			this.backToNPC = backToNPC;
		}
		public void CalculationStuff(Vector2 npcCenter)
		{
			if (counter <= 0)
			{
				counter = 0;
				nextRotation = Main.rand.NextFloat(-1 * (float)Math.PI, (float)Math.PI);
				nextCompression = Main.rand.NextFloat(0, 1);
				prevRotation = rotation;
				prevCompression = compression;
			}
			if (counter < 180)
				counter += countingSpeed;
			if(aiming)
            {
				aimTimer++;
            }				
			else
            {
				aimTimer--;
            }
			aimTimer = MathHelper.Clamp(aimTimer, 0, 60);
			spinCounter += 1 + 4 * aimTimer / 60f;
			if (location == Vector2.Zero || toLocation == Vector2.Zero)
            {
				location = npcCenter;
			}
			else
            {
				float speed = 0.08f;
				if (backToNPC)
					speed = 0.11f;
				location = Vector2.Lerp(location, toLocation, speed);
				if(Vector2.Distance(location, toLocation) < 5f)
                {
					location = toLocation;
					if(backToNPC)
                    {
						location = npcCenter;
						toLocation = Vector2.Zero;
						backToNPC = false;
					}
                }
				else
                {
					location += (toLocation - location).SafeNormalize(Vector2.Zero) * 4;
				}
            }
			float scale = 0.5f - 0.5f * (float)Math.Cos(MathHelper.ToRadians(counter));
			if (counter >= 180)
			{
				counter -= 180;
			}
			radius = MathHelper.Lerp(radius, targetRadius, 0.06f);
			if (Math.Abs(radius - targetRadius) < 1f)
				radius = targetRadius;
			rotation = MathHelper.Lerp(prevRotation, nextRotation, scale);
			compression = MathHelper.Lerp(prevCompression, nextCompression, scale);
		}
		float spinCounter = 0;
		public void Draw(Vector2 screenPos, SpriteBatch spriteBatch, int ID, float alphaMult = 1f, float radiusMult = 1f, float sizeMult = 1f, float baseRotation = 0f, bool front = false)
        {
			Draw(screenPos, spriteBatch, Color.White, ID, alphaMult, radiusMult, sizeMult, baseRotation, front);
        }
		public void Draw(Vector2 screenPos, SpriteBatch spriteBatch, Color overrideColor, int ID, float alphaMult = 1f, float radiusMult = 1f, float sizeMult = 1f, float baseRotation = 0f, bool front = false)
		{
			baseRotation += MathHelper.PiOver2;
			Texture2D texture = Terraria.GameContent.TextureAssets.Projectile[ModContent.ProjectileType<ChaosSphere>()].Value;
			Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
			int start = 0;
			int end = 180;
			float radiusOverride = 1f;
			if (front)
			{
				start += 180;
				end += 180;
			}
			float offset = 96;
			if (ID == 3)
			{
				offset = 80;
				radiusOverride = 1.0f;
			}
			else if (ID == 4)
			{
				offset = 140;
				radiusOverride = 0.8f;
			}
			else if (ID == 1)
			{
				offset = 190;
				radiusOverride = 0.7f;
			}
			else if (ID == 2)
			{
				offset = 240;
				radiusOverride = 0.6f;
			}
			float aimMult = aimTimer / (float)AimTimerMax;
			float overrideCompression = MathHelper.Lerp(compression, 0.5f, aimMult);
			float overrideRotation = MathHelper.Lerp(MathHelper.WrapAngle(rotation), 0f, aimMult);
			radiusMult = MathHelper.Lerp(radiusMult, radiusOverride, aimMult);
			Vector2 aimOffset = new Vector2(offset * aimMult, 0).RotatedBy(baseRotation);
			Vector2 center = location + aimOffset;
			for (int i = start; i < end; i += 4)
			{
				Color color = VoidPlayer.pastelAttempt(MathHelper.ToRadians(i), overrideColor);
				float radians = MathHelper.ToRadians(i + spinCounter);
				Vector2 rotationV = new Vector2(radius * radiusMult, 0).RotatedBy(radians);
				rotationV.X *= overrideCompression;
				rotationV = rotationV.RotatedBy(overrideRotation + baseRotation);
				spriteBatch.Draw(texture, center - screenPos + rotationV, null, new Color(color.R, color.G, color.B, 0) * alphaMult, baseRotation, drawOrigin, 0.8f * sizeMult, SpriteEffects.None, 0f);
			}
		}
	}
}
