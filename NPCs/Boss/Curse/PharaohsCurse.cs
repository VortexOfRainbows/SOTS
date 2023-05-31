using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Utilities;
using SOTS.Dusts;
using SOTS.Items.Banners;
using SOTS.Items.Pyramid;
using SOTS.Projectiles.Pyramid;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;

namespace SOTS.NPCs.Boss.Curse
{
	[AutoloadBossHead]
	public class PharaohsCurse : ModNPC
	{
		public override void SendExtraAI(BinaryWriter writer)
		{
			writer.Write(direction);
			writer.Write(enteredSecondPhase);
			writer.Write(smaller);
			writer.Write(NPC.dontTakeDamage);
		}
		public override void ReceiveExtraAI(BinaryReader reader)
		{
			direction = reader.ReadInt32();
			enteredSecondPhase = reader.ReadBoolean();
			smaller = reader.ReadBoolean();
			NPC.dontTakeDamage = reader.ReadBoolean();
		}
		int despawn = 0;
		private float ai1
		{
			get => NPC.ai[0];
			set => NPC.ai[0] = value;
		}
		private float ai2
		{
			get => NPC.ai[1];
			set => NPC.ai[1] = value;
		}
		private float ai3
		{
			get => NPC.ai[2];
			set => NPC.ai[2] = value;
		}
		private float aiPhase
		{
			get => NPC.ai[3];
			set => NPC.ai[3] = value;
		}
		public override void SetStaticDefaults()
		{
			Main.npcFrameCount[NPC.type] = 1;
			NPCID.Sets.NPCBestiaryDrawModifiers drawModifiers = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
			{
				CustomTexturePath = "SOTS/BossCL/PharaohPortrait",
				PortraitScale = 0.5f, // Portrait refers to the full picture when clicking on the icon in the bestiary
				Scale = 0.5f,
				PortraitPositionYOverride = 0f,
			};
			NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, drawModifiers);

			NPCID.Sets.DebuffImmunitySets.Add(Type, new NPCDebuffImmunityData
			{
				SpecificallyImmuneTo = new int[]
				{
					BuffID.Poisoned,
					BuffID.Frostburn,
					BuffID.Venom,
					BuffID.OnFire,
					BuffID.ShadowFlame,
					BuffID.CursedInferno
				}
			});
		}
		public override void SetDefaults()
		{
			NPC.aiStyle = 0;
			NPC.lifeMax = 4000;
			NPC.damage = 45;
			NPC.defense = 13;
			NPC.knockBackResist = 0f;
			NPC.width = 42;
			NPC.height = 54;
			NPC.npcSlots = 20f;
			NPC.value = Item.buyPrice(0, 7, 0, 0);
			NPC.boss = true;
			NPC.lavaImmune = true;
			NPC.noGravity = true;
			NPC.noTileCollide = true;
			NPC.HitSound = null;
			NPC.DeathSound = SoundID.NPCDeath6;
			Music = MusicLoader.GetMusicSlot(Mod, "Sounds/Music/PharaohsCurse");
			SceneEffectPriority = SceneEffectPriority.BossMedium;
			NPC.netAlways = true;
			NPC.alpha = 255;
			NPC.dontTakeDamage = true;
		}
		public override bool CanHitPlayer(Player target, ref int cooldownSlot)
		{
			return NPC.alpha == 0 && !NPC.dontTakeDamage;
		}
		public override void HitEffect(int hitDirection, double damage)
		{
			if (NPC.life > 0)
			{
				if (Main.netMode != NetmodeID.Server)
				{
					SOTSUtils.PlaySound(SoundID.NPCHit54, NPC.Center, 1.2f, -0.25f);
					int num = 0;
					while ((double)num < damage / (double)NPC.lifeMax * 60.0)
					{
						Dust.NewDust(NPC.position, NPC.width, NPC.height, ModContent.DustType<CurseDust>(), (float)(2 * hitDirection), -2f);
						num++;
					}
				}
			}
			else
			{
				if (!enteredSecondPhase) // enter second phase
				{
					enteredSecondPhase = true;
					int temp = NPC.lifeMax;
					NPC.lifeMax = (int)(temp * 1.0f);
					NPC.life = NPC.lifeMax;
					NPC.netUpdate = true;
				}
				else if(aiPhase == 7)
				{
					if (Main.netMode != NetmodeID.Server)
					{
						for (int k = 0; k < 240; k++)
						{
							Dust dust = Dust.NewDustDirect(NPC.position, NPC.width, NPC.height, ModContent.DustType<CurseDust>(), (float)(2 * hitDirection), -2f);
							dust.velocity = new Vector2(Main.rand.NextFloat(4), 0).RotatedBy(MathHelper.ToRadians(Main.rand.NextFloat(360))) + new Vector2(0, -2);
							dust.scale *= Main.rand.NextFloat(2) + 1;
							dust = Dust.NewDustDirect(NPC.position, NPC.width, NPC.height, ModContent.DustType<CurseDust>(), (float)(2 * hitDirection), -2f);
							dust.velocity = new Vector2(Main.rand.NextFloat(4), 0).RotatedBy(MathHelper.ToRadians(Main.rand.NextFloat(360)));
							dust.scale *= Main.rand.NextFloat(2) + 1;
							dust.noGravity = true;
						}
					}
				}
				else
                {
					TransitionPhase(7);
					NPC.lifeMax = 666;
					NPC.life = 666;
				}
			}
		}
        public override void OnKill()
		{
			SOTSWorld.downedCurse = true;
		}
        public override void ModifyNPCLoot(NPCLoot npcLoot)
		{
			npcLoot.Add(ItemDropRule.BossBag(ModContent.ItemType<CurseBag>()));
			LeadingConditionRule notExpertRule = new LeadingConditionRule(new Conditions.NotExpert());
			notExpertRule.OnSuccess(ItemDropRule.Common(ModContent.ItemType<CursedMatter>(), 1, 12, 24));
			npcLoot.Add(notExpertRule);
			npcLoot.Add(ItemDropRule.MasterModeCommonDrop(ModContent.ItemType<PharaohsCurseRelic>()));
		}
        public override void BossLoot(ref string name, ref int potionType)
		{
			potionType = ItemID.HealingPotion;
		}
		public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
		{
			NPC.lifeMax = (int)(NPC.lifeMax * bossLifeScale * 0.625f);
			NPC.damage = (int)(NPC.damage * 0.75f);
		}
        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
			if(ai1 <= 1)
            {
				TruePreDraw(spriteBatch, screenPos, drawColor, 0);
            }
			return false;
		}
		public void TruePreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor, int fadeIn = 0)
		{
			List<int> shadeSpearSlots = new List<int>();
			if (!runOnce)
			{
				//DrawLimbs(spriteBatch, false, -1);
				if(!enteredSecondPhase)
					DrawFoam(foamParticleList4, screenPos, 3);
				List<int> slots = new List<int>();
				List<int> npcSlots = new List<int>();
				for (int i = 0; i < Main.projectile.Length; i++)
				{
					Projectile proj = Main.projectile[i];
					if (proj.type == ModContent.ProjectileType<CurseWave>() && proj.active && proj.ai[0] == NPC.whoAmI)
					{
						CurseWave ring = proj.ModProjectile as CurseWave;
						ring.DrawTelegraph(spriteBatch, drawColor);
						slots.Add(i);
					}
					if (proj.type == ModContent.ProjectileType<ShadeSpear>() && proj.active && proj.ai[0] == NPC.whoAmI)
					{
						ShadeSpear spear = proj.ModProjectile as ShadeSpear;
						spear.setRand();
						shadeSpearSlots.Add(i);
					}
				}
				for(int i = 0; i < Main.npc.Length; i++)
				{
					NPC npc2 = Main.npc[i];
					if (npc2.type == ModContent.NPCType<SmallGas>() && npc2.active && npc2.ai[0] == NPC.whoAmI)
					{
						npcSlots.Add(i);
					}
				}
				int endPoint = 0;
				if((enteredSecondPhase && ai1 >= 200) || SOTS.Config.lowFidelityMode)
                {
					endPoint = 1;
                }
				for (int j = 2; j >= endPoint; j--)
				{
					for (int i = 0; i < slots.Count; i++)
					{
						Projectile proj = Main.projectile[slots[i]];
						if (proj.type == ModContent.ProjectileType<CurseWave>() && proj.active && proj.ai[0] == NPC.whoAmI)
						{
							CurseWave ring = proj.ModProjectile as CurseWave;
							List<CurseFoam> list = ring.foamParticleList1;
							DrawFoam(list, screenPos, 2, j, (byte)fadeIn);
						}
						if (j <= 1 && proj.type == ModContent.ProjectileType<ShadeSpear>() && proj.active && proj.ai[0] == NPC.whoAmI)
						{
							ShadeSpear spear = proj.ModProjectile as ShadeSpear;
							spear.TruePreDraw(spriteBatch, j);
						}
					}
					for(int i = 0; i < npcSlots.Count; i++)
					{
						NPC npc2 = Main.npc[npcSlots[i]];
						if (npc2.type == ModContent.NPCType<SmallGas>() && npc2.active && npc2.ai[0] == NPC.whoAmI)
						{
							SmallGas gas = npc2.ModNPC as SmallGas;
							List<CurseFoam> list = gas.foamParticleList1;
							DrawFoam(list, screenPos, 2, j, (byte)fadeIn);
							if(j == 1)
                            {
								DrawEye(spriteBatch, screenPos, npc2.Center, false);
                            }
						}
					}
				}
				if(shadeAlpha != 0f)
				{
					DrawFoam(foamParticleList1, screenPos, 2, -1, (byte)fadeIn);
					DrawFoam(foamParticleList2, screenPos, 1);
				}
				//DrawLimbs(spriteBatch, false, 1);
			}
			Vector2 drawPos3 = NPC.Center - screenPos;
			Texture2D texture = Terraria.GameContent.TextureAssets.Npc[NPC.type].Value;
			if (!enteredSecondPhase || (ai1 < 1 && ai2 < 90))
			{
				Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
				spriteBatch.Draw(texture, drawPos3, null, NPC.GetAlpha(drawColor), NPC.rotation, drawOrigin, NPC.scale, SpriteEffects.None, 0f);
			}
			else
			{
				DrawEye(spriteBatch, screenPos, NPC.Center, true);
			}
			if (!runOnce)
			{
				for (int j = 1; j >= 0; j--)
					for (int i = 0; i < shadeSpearSlots.Count; i++)
					{
						Projectile proj = Main.projectile[shadeSpearSlots[i]];
						if (j <= 1 && proj.type == ModContent.ProjectileType<ShadeSpear>() && proj.active && proj.ai[0] == NPC.whoAmI)
						{
							ShadeSpear spear = proj.ModProjectile as ShadeSpear;
							spear.TruePreDraw(spriteBatch, j);
						}
					}
				DrawFoam(foamParticleList3, screenPos, 0);
			}
		}
		public Vector2 EyeDirection;
		int eyeFrame = 0;
		float eyeOffsetMult = 1f;
		public float shadeAlpha = 1f;
		public int PharaohsShade = -1;
		public void DrawEye(SpriteBatch spriteBatch, Vector2 screenPos, Vector2 position, bool pupil = true)
		{
			float scale = smaller ? 0.85f : 1f;
			Texture2D texture = Mod.Assets.Request<Texture2D>("NPCs/Boss/Curse/PharaohsCurseEye").Value;
			Rectangle frame = new Rectangle(0, 44 * eyeFrame, 64, 44);
			Texture2D textureP = Mod.Assets.Request<Texture2D>("NPCs/Boss/Curse/PharaohsCurseEyePupil").Value;
			float alphaMult = ai1 / 200f * shadeAlpha;
			Vector2 drawOrigin = new Vector2(textureP.Width * 0.5f, textureP.Height * 0.5f);
			float randOffsetMult = 1 - startParticles;
			spriteBatch.Draw(texture, position - screenPos, frame, NPC.GetAlpha(Color.White) * alphaMult, NPC.rotation, drawOrigin, NPC.scale * scale - randOffsetMult * 0.45f, SpriteEffects.None, 0f);
			if (eyeFrame == 2 && pupil)
			{
				Player player = Main.player[NPC.target];
				Vector2 toLocation = EyeDirection.SafeNormalize(Vector2.Zero) * -2 * eyeOffsetMult;
				Vector2 randOffset = new Vector2(Main.rand.NextFloat(-1, 1), Main.rand.NextFloat(-1, 1)) * randOffsetMult * 6f;
				spriteBatch.Draw(textureP, position + toLocation + randOffset - screenPos + new Vector2(0, -2.6f) * randOffsetMult * 2f, null, NPC.GetAlpha(Color.White) * alphaMult, NPC.rotation, drawOrigin, NPC.scale * scale + randOffsetMult * 0.5f, SpriteEffects.None, 0f);
			}
		}
		public void DrawFoam(List<CurseFoam> dustList, Vector2 screenPos, int startPoint = 2, int overrideStart = -1, byte fadeIn = 0)
		{
			Texture2D texture = (Texture2D)ModContent.Request<Texture2D>("SOTS/NPCs/Boss/Curse/CurseFoam");
			Vector2 drawOrigin = new Vector2(texture.Width / 2, texture.Height / 6);
			if (startPoint == 3)
			{
				for (int i = 0; i < dustList.Count; i++)
				{
					texture = (Texture2D)ModContent.Request<Texture2D>("SOTS/NPCs/Boss/Curse/TumorBall");
					drawOrigin = new Vector2(texture.Width / 2, texture.Height / 2);
					int shade = 255 - (int)(dustList[i].counter * 4f);
					Color color = new Color(shade + dustList[i].dustColorVariation, shade - dustList[i].dustColorVariation, shade - dustList[i].dustColorVariation);
					color = Lighting.GetColor((int)dustList[i].position.X / 16, (int)dustList[i].position.Y / 16, color);
					Vector2 drawPos = dustList[i].position - screenPos;
					Rectangle frame = new Rectangle(0, 0, texture.Width, texture.Width);
					Main.spriteBatch.Draw(texture, drawPos + new Vector2(0, 0), frame, color, dustList[i].rotation, drawOrigin, dustList[i].scale * 1, SpriteEffects.None, 0f);
				}
			}
			else
			{
				if (overrideStart != -1)
				{
					for (int i = 0; i < dustList.Count; i++)
					{
						int shade = 255 - (int)(dustList[i].counter * 4f) - (overrideStart != 2 ? fadeIn : 0);
						if (shade < 0)
							shade = 0;
						Color color = new Color(shade + dustList[i].dustColorVariation, shade - dustList[i].dustColorVariation, shade - dustList[i].dustColorVariation);
						if (overrideStart != 2)
						{
							color = Lighting.GetColor((int)dustList[i].position.X / 16, (int)dustList[i].position.Y / 16, color);
						}
						else
						{
							float reduction = shade / 255f;
							Color first = new Color((int)(111 * reduction), (int)(80 * reduction), (int)(154 * reduction));
							Color second = new Color((int)(76 * reduction), (int)(58 * reduction), (int)(101 * reduction));
							color = Color.Lerp(first, second, 0.5f + 0.5f * (float)Math.Sin(MathHelper.ToRadians(ColorHelpers.soulColorCounter * 2)));
							color = new Color((byte)(color.R * shadeAlpha), (byte)(color.G * shadeAlpha), (byte)(color.B * shadeAlpha));
						}
						Vector2 drawPos = dustList[i].position - screenPos;
						Rectangle frame = new Rectangle(0, texture.Height / 3 * overrideStart, texture.Width, texture.Width);
						float scale = overrideStart == 0 ? 1.5f : 2.0f;
						Main.spriteBatch.Draw(texture, drawPos + new Vector2(0, 0), frame, color, dustList[i].rotation, drawOrigin, dustList[i].scale * scale, SpriteEffects.None, 0f);
					}
				}
				else
				{
					int endPoint = 0;
					if ((enteredSecondPhase && ai1 >= 200) || (SOTS.Config.lowFidelityMode && startPoint != 1))
					{
						endPoint = 1;
					}
					if (startPoint != 2)
						texture = (Texture2D)ModContent.Request<Texture2D>("SOTS/NPCs/Boss/Curse/CurseFoamAlt");
					for (int j = startPoint; j >= endPoint; j--)
					{
						for (int i = 0; i < dustList.Count; i++)
						{
							int shade = 255 - (int)(dustList[i].counter * 4f) - (j != 2 ? fadeIn : 0);
							if (shade < 0)
								shade = 0;
							Color color = new Color(shade + dustList[i].dustColorVariation, shade - dustList[i].dustColorVariation, shade - dustList[i].dustColorVariation);
							if (j != 2)
							{
								color = Lighting.GetColor((int)dustList[i].position.X / 16, (int)dustList[i].position.Y / 16, color);
							}
							else
							{
								float reduction = shade / 255f;
								Color first = new Color((int)(111 * reduction), (int)(80 * reduction), (int)(154 * reduction));
								Color second = new Color((int)(76 * reduction), (int)(58 * reduction), (int)(101 * reduction));
								color = Color.Lerp(first, second, 0.5f + 0.5f * (float)Math.Sin(MathHelper.ToRadians(ColorHelpers.soulColorCounter * 2)));
								color = new Color((byte)(color.R * shadeAlpha), (byte)(color.G * shadeAlpha), (byte)(color.B * shadeAlpha));
							}
							Vector2 drawPos = dustList[i].position - screenPos;
							Rectangle frame = new Rectangle(0, texture.Height / 3 * j, texture.Width, texture.Width);
							float scale = j == 0 ? 1.5f : 2.0f;
							Main.spriteBatch.Draw(texture, drawPos + new Vector2(0, 0), frame, color, dustList[i].rotation, drawOrigin, dustList[i].scale * scale, SpriteEffects.None, 0f);
						}
						/*texture = (Texture2D)ModContent.Request<Texture2D>("SOTS/NPCs/Boss/Curse/CurseFoamAlt");
						for (int i = 0; i < dustList.Count; i++)
						{
							if (dustList[i].alt)
							{
								int shade = 255 - (int)(dustList[i].counter * 4f);
								Color color = npc.GetAlpha(new Color(shade + dustList[i].dustColorVariation, shade - dustList[i].dustColorVariation, shade - dustList[i].dustColorVariation));
								if (j != 2)
									color = Lighting.GetColor((int)dustList[i].position.X / 16, (int)dustList[i].position.Y / 16, color);
								Vector2 drawPos = dustList[i].position - screenPos;
								Rectangle frame = new Rectangle(0, texture.Height / 3 * j, texture.Width, texture.Width);
								float scale = j == 0 ? 1.5f : 2.0f;
								Main.spriteBatch.Draw(texture, drawPos + new Vector2(0, 0), frame, color, dustList[i].rotation, drawOrigin, dustList[i].scale * scale, SpriteEffects.None, 0f);
							}
						}*/
					}
				}
			}
		}
		public static void SpawnPassiveDust(Texture2D texture, Vector2 spawnLocation, float scale, List<CurseFoam> dustList, float velocityScale = 1f, int style = 0, int rate = 45, float rotation = 0, float lifeMult = 1f)
		{
			if (dustList == null)
				return;
			int width = texture.Width;
			int height = texture.Height;
			Color[] data = new Color[width * height];
			texture.GetData(data);
			Vector2 position;
			int localX = 0;
			int localY = 0;
			for (int i = 0; i < width * height; i++)
			{
				if (rate <= 1)
					rate = 1;
				if (data[i].A >= 255 && Main.rand.NextBool(rate))
				{
					position = spawnLocation + scale * (-new Vector2(texture.Width / 2, texture.Height / 2) + new Vector2(localX, localY)).RotatedBy(rotation);
					Vector2 rotational = new Vector2(0, 1.00f).RotatedBy(MathHelper.ToRadians(Main.rand.NextFloat(360)));
					float scale2 = 0.9f;
					bool noMovement = false;
					if (style == 3 || style == 4)
					{
						noMovement = style == 3;
						scale2 = style == 3 ? 0.33f : 0.4f;
					}
					if (style == 1 || style == 2)
					{
						if (style == 2)
							scale2 = 1.25f;
						else
							scale2 = 0.55f;
						Vector2 toCenter = spawnLocation - position;
						float speedMult = 1.5f;
						if (style == 2)
							speedMult = -1.5f;
						rotational = toCenter.SafeNormalize(Vector2.Zero) * speedMult;
						dustList.Add(new CurseFoam(position - rotational.SafeNormalize(Vector2.Zero) * 2 * scale2, rotational * velocityScale, Main.rand.NextFloat(0.9f, 1.1f) * scale2, style == 2, lifeMult));
					}
					else
						dustList.Add(new CurseFoam(position + rotational.SafeNormalize(Vector2.Zero) * 2 * scale2, rotational * velocityScale, Main.rand.NextFloat(0.9f, 1.1f) * scale2, noMovement, lifeMult));
				}
				localX++;
				if (localX > width)
				{
					localX -= width;
					localY++;
				}
			}
		}
		bool runOnce = true;
		public List<CurseFoam> foamParticleList1 = new List<CurseFoam>();
		public List<CurseFoam> foamParticleList2 = new List<CurseFoam>();
		public List<CurseFoam> foamParticleList3 = new List<CurseFoam>();
		public List<CurseFoam> foamParticleList4 = new List<CurseFoam>();
		float startParticles = 1;
		int playerPosCounter = 0;
		bool playerValidPosition = true;
		public void SpawnDoors()
        {
			if(Main.netMode != NetmodeID.MultiplayerClient)
				for(int i = 0; i <= 1; i ++)
				{
					int xOff = -56;
					if (i == 1)
						xOff = 54;
					Vector2 offset = new Vector2(xOff, 6) * 16;
					Vector2 center = new Vector2((int)(NPC.Center.X / 16), (int)(NPC.Center.Y / 16)) * 16;
					center += offset;
					Projectile.NewProjectile(NPC.GetSource_FromAI(), center, Vector2.Zero, ModContent.ProjectileType<SpawnGoldGate>(), 0, 0, Main.myPlayer, i * 2 - 1, 0);
				}
        }
		public override bool PreAI()
		{
			Player player = Main.player[NPC.target];
			playerPosCounter++;
			if (runOnce)
			{
				playerValidPosition = true;
				SpawnDoors();
				startParticles = 0.0f;
				aiPhase = -1;
				runOnce = false;
				direction = Main.rand.Next(2) * 2 - 1;
				CenterPosition = NPC.Center;
			}
			if(playerValidPosition && playerPosCounter % 60 == 0)
            {
				playerValidPosition = PlayerValidPosition();
            }
			if(aiPhase != 7)
			{
				if (eyeOffsetMult < 1)
				{
					eyeOffsetMult += 0.125f;
				}
				else
				{
					eyeOffsetMult = 1;
				}
			}
			NPC.TargetClosest();
			if (aiPhase != -1)
				NPC.rotation = NPC.velocity.X * 0.05f;
			Lighting.AddLight(NPC.Center, new Vector3(110 / 255f, 36 / 255f, 20 / 255f));
			if (Main.netMode != NetmodeID.Server)
			{
				Texture2D texture = (Texture2D)ModContent.Request<Texture2D>("SOTS/NPCs/Boss/Curse/FartGas");
				Texture2D textureFill = (Texture2D)ModContent.Request<Texture2D>("SOTS/NPCs/Boss/Curse/FartGasInline");
				if (smaller)
				{
					texture = (Texture2D)ModContent.Request<Texture2D>("SOTS/NPCs/Boss/Curse/SmallGas");
					textureFill = (Texture2D)ModContent.Request<Texture2D>("SOTS/NPCs/Boss/Curse/SmallGasFill");
				}
				if (startParticles != 0.0f)
				{
					SpawnPassiveDust(texture, NPC.Center + new Vector2(0, smaller? 0 : 10), (smaller ? 1.0f : 0.9f) * startParticles, foamParticleList1, 1, 0, smaller ? 40 : 50, NPC.rotation);
					SpawnPassiveDust(textureFill, NPC.Center + new Vector2(0, smaller ? 0 : 10), (smaller ? 1.0f : 0.9f) * startParticles, foamParticleList1, 1, 0, smaller ? 100 : 200, NPC.rotation);
					if(!enteredSecondPhase)
						SpawnPassiveDust((Texture2D)ModContent.Request<Texture2D>("SOTS/NPCs/Boss/Curse/FartGasBorder"), NPC.Center + new Vector2(0, 10), 1.2f * startParticles, foamParticleList4, 0.2f, 2, 3600, NPC.rotation);
				}
				int alphaCounter = enteredSecondPhase ? (int)(255 * ai2 / 90f) : NPC.alpha;
				if(!enteredSecondPhase || (ai1 < 1 && ai2 < 90))
				{
					texture = (Texture2D)ModContent.Request<Texture2D>("SOTS/NPCs/Boss/Curse/PharaohsCurseOutline");
					SpawnPassiveDust(texture, NPC.Center, 1.0f, foamParticleList2, 0.1f, 1, (int)(30 * (1f + Math.Pow(alphaCounter, 0.5f))), NPC.rotation);
					SpawnPassiveDust(texture, NPC.Center, 1.0f, foamParticleList3, 0.125f, 1, (int)(60 * (1f + Math.Pow(alphaCounter, 0.5f))), NPC.rotation);
				}
			}
			cataloguePos();
			bool inRange = Vector2.Distance(player.Center, NPC.Center) <= 1600f;
			if (Main.netMode != NetmodeID.MultiplayerClient)
				NPC.netUpdate = true;
			if (player.dead || !SOTSPlayer.ModPlayer(player).PyramidBiome || !inRange || !playerValidPosition)
			{
				despawn++;
			}
			if (despawn >= 600)
			{
				NPC.active = false;
			}
			return inRange;
		}
		public void ResetLists()
		{	
			/*
			foamParticleList1 = new List<CurseFoam>();
			foamParticleList2 = new List<CurseFoam>();
			foamParticleList3 = new List<CurseFoam>();
			foamParticleList4 = new List<CurseFoam>();
			return;*/
			List<CurseFoam> temp = new List<CurseFoam>();
			for(int i = 0; i < foamParticleList1.Count; i++)
            {
				if(foamParticleList1[i].active && foamParticleList1[i] != null)
					temp.Add(foamParticleList1[i]);
			}
			foamParticleList1 = new List<CurseFoam>();
			for (int i = 0; i < temp.Count; i++)
			{
				foamParticleList1.Add(temp[i]);
			}

			temp = new List<CurseFoam>();
			for (int i = 0; i < foamParticleList2.Count; i++)
			{
				if (foamParticleList2[i].active && foamParticleList2[i] != null)
					temp.Add(foamParticleList2[i]);
			}
			foamParticleList2 = new List<CurseFoam>();
			for (int i = 0; i < temp.Count; i++)
			{
				foamParticleList2.Add(temp[i]);
			}

			temp = new List<CurseFoam>();
			for (int i = 0; i < foamParticleList3.Count; i++)
			{
				if (foamParticleList3[i].active && foamParticleList3[i] != null)
					temp.Add(foamParticleList3[i]);
			}
			foamParticleList3 = new List<CurseFoam>();
			for (int i = 0; i < temp.Count; i++)
			{
				foamParticleList3.Add(temp[i]);
			}

			temp = new List<CurseFoam>();
			for (int i = 0; i < foamParticleList4.Count; i++)
			{
				if (foamParticleList4[i].active && foamParticleList4[i] != null)
					temp.Add(foamParticleList4[i]);
			}
			foamParticleList4 = new List<CurseFoam>();
			for (int i = 0; i < temp.Count; i++)
			{
				foamParticleList4.Add(temp[i]);
			}
		}
		public void cataloguePos()
		{
			for (int i = 0; i < foamParticleList1.Count; i++)
			{
				CurseFoam particle = foamParticleList1[i];
				particle.Update();
				if (!particle.active)
				{
					particle = null;
					foamParticleList1.RemoveAt(i);
					i--;
				}
				else
				{
					particle.Update();
					if (!particle.active)
					{
						particle = null;
						foamParticleList1.RemoveAt(i);
						i--;
					}
					else if (!particle.noMovement)
						particle.position += NPC.velocity * 0.825f;
				}
			}
			for (int i = 0; i < foamParticleList2.Count; i++)
			{
				CurseFoam particle = foamParticleList2[i];
				particle.Update();
				if (!particle.active)
				{
					particle = null;
					foamParticleList2.RemoveAt(i);
					i--;
				}
				else if (!particle.noMovement)
					particle.position += NPC.velocity * 0.9f;
			}
			for (int i = 0; i < foamParticleList3.Count; i++)
			{
				CurseFoam particle = foamParticleList3[i];
				particle.Update();
				if (!particle.active)
				{
					particle = null;
					foamParticleList3.RemoveAt(i);
					i--;
				}
				else if (!particle.noMovement)
					particle.position += NPC.velocity;
			}
			for (int i = 0; i < foamParticleList4.Count; i++)
			{
				CurseFoam particle = foamParticleList4[i];
				particle.Update();
				if (!particle.active)
				{
					particle = null;
					foamParticleList4.RemoveAt(i);
					i--;
				}
				else particle.velocity.Y += 0.11f;
			}
		}
		public void MoveTo(Vector2 goTo, float slowDownMult, float flatSpeed, float distanceMult = 1f)
		{
			Vector2 toDestination = goTo - NPC.Center;
			float speed = (flatSpeed + toDestination.Length() * 0.00045f * distanceMult);
			if (speed > toDestination.Length())
			{
				speed = toDestination.Length();
			}
			NPC.velocity *= slowDownMult;
			NPC.velocity += toDestination.SafeNormalize(Vector2.Zero) * speed * 0.75f;
		}
		public void MimicPolarisMovement(float speed)
		{
			Player player = Main.player[NPC.target];
			NPC.velocity *= 0.5f;
			Vector2 vectorToPlayer = player.Center - NPC.Center;
			float yDist = vectorToPlayer.Y * 1.15f;
			float xDist = vectorToPlayer.X;
			float length = (float)Math.Sqrt(xDist * xDist + yDist * yDist);
			float speedMult = -2f + (float)Math.Pow(length, 1.045) * 0.0125f;
			if (speedMult < 0)
			{
				speedMult *= 0.5f;
			}
			NPC.velocity += vectorToPlayer.SafeNormalize(Vector2.Zero) * speedMult * speed;
		}
		public Vector2 CenterPosition;
		float dustAcceleration = 0f;
		int direction = 1;
		int resetListTimer = 0;
		public bool enteredSecondPhase = false;
		bool smaller = false;
		public void BurstRings(int style = 0)
		{
			if (style == 0)
			{
				SOTSUtils.PlaySound(SoundID.Item62, (int)NPC.Center.X, (int)NPC.Center.Y, 1f, 0.2f);
				ParticleExplosion(120, true);
				if (Main.netMode == NetmodeID.MultiplayerClient)
					return;
				for (int i = 0; i < 6; i++)
				{
					Vector2 outWards = new Vector2(-2f * Main.rand.NextFloat(0.9f, 1.1f), 0).RotatedBy(MathHelper.ToRadians(i * 60 + 30));
					int damage = Common.GlobalNPCs.SOTSNPCs.GetBaseDamage(NPC) / 2;
					Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, outWards, ModContent.ProjectileType<CurseRing>(), damage, 0f, Main.myPlayer, NPC.whoAmI);
				}
			}
			else
			{
				if (Main.netMode == 1)
					return;
				for (int i = 0; i < 12; i++)
				{
					Vector2 outWards = new Vector2(-2f, 0).RotatedBy(MathHelper.ToRadians(i / 2 * 60));
					int damage = Common.GlobalNPCs.SOTSNPCs.GetBaseDamage(NPC) / 2;
					Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, outWards, ModContent.ProjectileType<CurseWave>(), damage, 0f, Main.myPlayer, NPC.whoAmI, (i % 2 * 2 - 1));
				}
			}
		}
		public void ParticleExplosion(int amt = 240, bool quiet = false)
		{
			if(!quiet)
				SOTSUtils.PlaySound(SoundID.Item62, (int)NPC.Center.X, (int)NPC.Center.Y, 1f, 0.2f);
			for (int j = 0; j < amt; j++)
			{
				float scale = Main.rand.NextFloat(0.5f, 1.5f);
				Vector2 rotational = new Vector2(0, -Main.rand.NextFloat(2.75f, 8.5f) / scale).RotatedBy(MathHelper.ToRadians(Main.rand.NextFloat(360f)));
				foamParticleList1.Add(new CurseFoam(NPC.Center, rotational, 1.75f * scale, true));
			}
		}
		public override void AI()
		{
			resetListTimer--;
			if (resetListTimer < 0)
			{
				resetListTimer = 1800;
				ResetLists();
			}
			Player player = Main.player[NPC.target];
			if(enteredSecondPhase && ai1 < 1 && !NPC.dontTakeDamage)
            {
				NPC.dontTakeDamage = true;
				TransitionPhase(3);
			}
			ai2++;
			if (aiPhase == -1)
			{
				float timeToStart = 360f;
				if (ai2 >= timeToStart)
				{
					if (ai2 == timeToStart)
					{
						if (Main.netMode != NetmodeID.Server)
							Main.NewText(Language.GetTextValue("Mods.SOTS.BossAwoken.PharaohsCurse"), 175, 75, byte.MaxValue);
						ParticleExplosion();
					}
					if (startParticles < 1)
					{
						dustAcceleration += 0.05f;
						startParticles += dustAcceleration;
					}
					else if (startParticles > 1)
					{
						dustAcceleration -= 0.05f;
						startParticles += dustAcceleration;
					}
					else
					{
						ai2 = 1000;
					}
				}
				else
				{
					startParticles = 0.0f;
					NPC.alpha = 255 - (int)ai2;
					if (NPC.alpha < 0)
						NPC.alpha = 0;
					NPC.velocity.Y = -1 * (0.25f + 0.5f * ai2 / timeToStart);
					float rotationLimit = 1500f;
					NPC.rotation = MathHelper.ToRadians(rotationLimit - rotationLimit * (float)Math.Pow(ai2 / timeToStart, 0.5f));
				}
				if (ai2 >= timeToStart + 30)
				{
					startParticles = 1;
					TransitionPhase(0);
					ai2 = -30;
					NPC.alpha = 0;
					NPC.dontTakeDamage = false;
				}
			}
			if (aiPhase == 0)
			{
				if (Main.expertMode)
					DashAttacks(280, 0.9f, 4);
				else
					DashAttacks(280, 0.875f, 3);
			}
			if (aiPhase == 1)
			{
				if (ai2 >= 150 && ai2 <= 730)
				{
					float mult = (300 - ai2) / 100f;
					if (mult < 0.4f)
						mult = 0.4f;
					if (ai2 == 220)
					{
						if (Main.netMode != NetmodeID.MultiplayerClient)
						{
							for (int i = 0; i < 4; i++)
							{
								float rand = Main.rand.NextFloat(-10, 10);
								Vector2 outWards = new Vector2(12, 0).RotatedBy(MathHelper.ToRadians(90 * i + rand));
								int damage = Common.GlobalNPCs.SOTSNPCs.GetBaseDamage(NPC) / 2;
								Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, outWards, ModContent.ProjectileType<CurseArm>(), damage, 0f, Main.myPlayer, NPC.whoAmI);
							}
						}
					}
					if (ai2 >= 270)
					{
						MimicPolarisMovement(0.175f);
					}
					else
					{
						if (ai2 >= 210)
							mult *= 0.01f;
						MimicPolarisMovement(0.1f * mult);
					}
				}
				else
				{
					float speed = 6f;
					if (ai2 < 0)
					{
						speed = 0.6f;
					}
					MoveTo(CenterPosition, 0.2f, speed);
				}
				if (ai2 >= 730)
				{
					TransitionPhase(2);
				}
			}
			if (aiPhase == 2)
            {
				if(ai2 < 60)
                {
					float speed = 4f;
					if (ai2 < 0)
					{
						speed = 2f;
					}
					MoveTo(CenterPosition, 0.3f, speed);
				}
				else
				{
					NPC.velocity *= 0.1f;
				}
				if(ai2 % 240 == 0 && ai2 < 720)
                {
					ParticleExplosion();
					BurstRings(1);
                }
				else if(ai2 % 240 >= 120 && ai2 < 480)
                {
					float mult = 120f / (ai2 % 240);
					MimicPolarisMovement(1f * mult);
				}
				if(ai2 >= 720)
				{
					if(enteredSecondPhase)
						TransitionPhase(4);
					else
						TransitionPhase(0);
				}
            }
			if (aiPhase == 3) //This is the start of the second phase attacks
			{
				if (Main.netMode != NetmodeID.MultiplayerClient)
				{
					if (PharaohsShade == -1)
					{
						PharaohsShade = Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, Vector2.Zero, ModContent.ProjectileType<Projectiles.Pyramid.PharaohShade>(), 0, 0, Main.myPlayer, (NPC.whoAmI + 0.5f), 0);
					}
				}
				if (ai2 < 150)
				{
					NPC.velocity *= 0.975f;
					NPC.position.Y -= 0.1f;
					if (ai2 == 90 && Main.netMode != NetmodeID.Server)
					{
						for (int k = 0; k < 6; k++)
							Gore.NewGore(NPC.GetSource_Misc("SOTS:PharaohEnterPhaseTwo"), NPC.Center - new Vector2(16, 16), NPC.velocity * 0.1f, ModGores.GoreType("Gores/Curse/PharaohsMask" + (1 + k)), 1f);
						ParticleExplosion();
						for (int i = 0; i < 3; i++)
						{
							int goreIndex = Gore.NewGore(NPC.GetSource_Misc("SOTS:PharaohEnterPhaseTwo"), NPC.Center - new Vector2(16, 16), default(Vector2), Main.rand.Next(61, 64), 1f);
							Main.gore[goreIndex].scale = 0.55f;
						}
					}
				}
				else if (ai1 < 200)
				{
					NPC.velocity *= 0.95f;
					ai1++;
					if (ai1 < 170)
					{
						eyeFrame = 0;
					}
					else
                    {
						if(ai1 < 180)
						{
							eyeFrame = 1;
						}
						else
						{
							eyeFrame = 2;
						}
						if(ai1 == 182)
						{
							SOTSUtils.PlaySound(SoundID.Roar, (int)NPC.Center.X, (int)NPC.Center.Y, 1.25f);
						}
                    }
				}
				else
				{
					NPC.dontTakeDamage = false;
					TransitionPhase(4);
				}
			}
			Vector2 center = NPC.Center + new Vector2(0, 4);
			if (aiPhase == 4)
			{
				if (ai2 >= 120 && ai2 <= 1290)
				{
					int damage = Common.GlobalNPCs.SOTSNPCs.GetBaseDamage(NPC) / 2;
					if (ai2 == 190)
					{
						if (Main.netMode != NetmodeID.MultiplayerClient)
						{
							for (int i = 1; i < 3; i++)
							{
								Vector2 outWards = new Vector2(0, 12).RotatedBy(MathHelper.ToRadians(180 * i));
								Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, outWards, ModContent.ProjectileType<CurseArm>(), damage, 0f, Main.myPlayer, NPC.whoAmI, 180 * i);
							}
						}
					}
					if(ai2 >= 360 && ai2 <= 1290)
                    {
						int currentCounter = (int)(ai2 - 280) % 300;
						if (currentCounter == 150 || currentCounter == 230 || currentCounter == 10)
						{
							eyeOffsetMult = -1f;
							SOTSUtils.PlaySound(SoundID.Item96, (int)NPC.Center.X, (int)NPC.Center.Y, 0.875f, 0.2f);
							if (Main.netMode != NetmodeID.MultiplayerClient)
							{
								Vector2 toPlayer = NPC.Center - player.Center;
								toPlayer = toPlayer.SafeNormalize(Vector2.Zero) * -1.25f;
								for (int i = 0; i < 4; i++)
								{
									Vector2 random = new Vector2(Main.rand.NextFloat(-1, 1), Main.rand.NextFloat(-1, 1)) * 0.225f;
									Projectile.NewProjectile(NPC.GetSource_FromAI(), center + toPlayer * 8, toPlayer + random, ModContent.ProjectileType<ShadeSpear>(), (int)(damage * 1.25f), 0f, Main.myPlayer, NPC.whoAmI, 0);
								}
							}
						}
						if(currentCounter % 5 == 0 && currentCounter >= 40 && currentCounter <= 60)
						{
							SOTSUtils.PlaySound(SoundID.Item96, (int)NPC.Center.X, (int)NPC.Center.Y, 0.75f, 0.75f);
							eyeOffsetMult = -1f;
							if (Main.netMode != NetmodeID.MultiplayerClient)
							{
								float spread = 90 - (currentCounter - 45) * 4f;
								for (int i = 0; i < 2; i++)
								{
									int direction = i * 2 - 1;
									Vector2 toPlayer = NPC.Center - player.Center;
									toPlayer = toPlayer.SafeNormalize(Vector2.Zero) * -1.25f;
									toPlayer = toPlayer.RotatedBy(MathHelper.ToRadians(spread * direction));
									Projectile.NewProjectile(NPC.GetSource_FromAI(), center + toPlayer * 8, toPlayer, ModContent.ProjectileType<ShadeSpear>(), (int)(damage * 1.0f), 0f, Main.myPlayer, NPC.whoAmI, 0);
								}
							}
                        }
                    }
				}
				float speed = 3.0f;
				MoveTo(CenterPosition, 0.45f, speed, 15f);
				if (ai2 >= 1290)
				{
					TransitionPhase(5);
				}
			}
			if (aiPhase == 5)
			{
				if (ai2 < 0)
				{
					ChangeShade(0);
					MimicPolarisMovement(0.075f);
				}
				else
				{
					int total = 340;
					if(ai2 > total * 2)
                    {
						TransitionPhase(6);
						eyeFrame = 2;
					}
					else
					{
						int amt = Main.expertMode ? 3 : 4;
						int cycleAI = (int)ai2 % total;
						if (cycleAI < 280)
						{
							Vector2 toPlayer = NPC.Center - player.Center;
							EyeDirection = toPlayer;
							cycleAI %= 140;
						}
						else if (cycleAI % amt == 0)
						{
							eyeOffsetMult = -1f;
							SOTSUtils.PlaySound(SoundID.Item96, (int)NPC.Center.X, (int)NPC.Center.Y, 0.825f, 0.3f);
							if (Main.netMode != NetmodeID.MultiplayerClient)
							{
								int damage = Common.GlobalNPCs.SOTSNPCs.GetBaseDamage(NPC) / 2;
								Vector2 circular = new Vector2(-1.05f * direction).RotatedBy(MathHelper.ToRadians((cycleAI - 280) * 6 + ai3));
								EyeDirection = circular;
								Projectile.NewProjectile(NPC.GetSource_FromAI(), center + circular * 8, circular, ModContent.ProjectileType<ShadeSpear>(), (int)(damage * 1.1f), 0f, Main.myPlayer, NPC.whoAmI, 0);
							}
						}
						if (cycleAI == 139 || cycleAI == total - 1)
						{
							if (cycleAI == total - 1)
								direction *= -1;
							ai3 = Main.rand.Next(360);
						}
						if (cycleAI < 60)
						{
							ChangeShade(0, 0.05f);
							MimicPolarisMovement(0.075f);
							if (cycleAI < 10)
							{
								eyeFrame = 1;
							}
							else if (cycleAI < 20)
							{
								eyeFrame = 0;
							}
							if (cycleAI > 30)
							{
								Vector2 dashArea = player.Center + new Vector2(300, 0).RotatedBy(MathHelper.ToRadians(ai3));
								MoveTo(dashArea, 0.2f, 20f, 6f);
							}
						}
						else if (cycleAI <= 80)
						{
							if (cycleAI < 70)
							{
								Vector2 dashArea = player.Center + new Vector2(300, 0).RotatedBy(MathHelper.ToRadians(ai3));
								MoveTo(dashArea, 0.2f, 100f, 10f);
							}
							if (cycleAI == 80)
							{
								ai3 += 180;
								Vector2 dashArea = player.Center + new Vector2(300, 0).RotatedBy(MathHelper.ToRadians(ai3));
								storeDashArea = dashArea;
							}
							if (cycleAI > 65 && cycleAI < 75)
							{
								eyeFrame = 1;
							}
							else
							{
								eyeFrame = 2;
							}
							ChangeShade(0.9f, 0.05f);
							MimicPolarisMovement(0.5f);
						}
						else if (cycleAI <= 130)
						{
							if (cycleAI == 81)
							{
								SOTSUtils.PlaySound(SoundID.Item73, (int)NPC.Center.X, (int)NPC.Center.Y, 1.75f, 0.3f);
							}
							MoveTo(storeDashArea, 0.0f, 16f, 4f);
						}
					}
				}
            }
			else
            {
				Vector2 toPlayer = NPC.Center - player.Center;
				EyeDirection = toPlayer;
			}
			if (aiPhase == 6)
			{
				if (ai2 < 0)
				{
					MoveTo(CenterPosition, 0.45f, 9f, 15f);
				}
				else if(ai2 < 90)
				{
					MoveTo(CenterPosition, 0.45f, 1f, 1f);
					if (ai2 < 10)
					{
						eyeFrame = 1;
					}
					else if(ai2 < 20)
                    {
						eyeFrame = 0;
                    }
					NPC.position += new Vector2(Main.rand.NextFloat(-1.5f, 1.5f), Main.rand.NextFloat(-1.5f, 1.5f));
					ChangeShade(0.2f, 0.02f);
                }
				else
				{
					if(ai2 == 90)
                    {
						smaller = true;
						ParticleExplosion(200, false);
						if (Main.netMode != NetmodeID.MultiplayerClient)
   							for (int i = 2; i <= 4; i++)
							{
								int npc1 = NPC.NewNPC(NPC.GetSource_FromAI(), (int)NPC.Center.X, (int)NPC.Center.Y, ModContent.NPCType<SmallGas>(), 0, NPC.whoAmI, i * 90, 0, 0); //summons 180, 270, and 360
								Main.npc[npc1].netUpdate = true;
							}
                    }
					else if (ai2 > 90)
					{
						ChangeShade(1, 0.035f);
						if(ai2 < 100)
                        {
							eyeFrame = 1;
                        }
						else
                        {
							eyeFrame = 2;
							/*if (ai2 % 60 == 0 && ai2 > 120 && Main.expertMode)
							{
								Terraria.Audio.SoundEngine.PlaySound(2, (int)npc.Center.X, (int)npc.Center.Y, 96, 0.875f, 0.2f);
								eyeOffsetMult = -1;
								if (Main.netMode != NetmodeID.MultiplayerClient)
								{
									int damage = npc.damage / 2;
									if (Main.expertMode)
									{
										damage = (int)(damage / Main.expertDamage);
									}
									Vector2 toPlayer = npc.Center - player.Center;
									toPlayer = toPlayer.SafeNormalize(Vector2.Zero) * -0.8f;
									for (int i = -1; i <= 1; i++)
									{
										Vector2 velo = toPlayer.RotatedBy(MathHelper.ToRadians(i * 3.5f));
										Projectile.NewProjectile(center + velo * 12f, velo, ModContent.ProjectileType<ShadeSpear>(), (int)(damage * 1.1f), 0f, Main.myPlayer, npc.whoAmI, 0);
									}
								}
							}*/
						}
					}
					if (ai2 % 720 == 630 || ai3 > 0) //do slam attack
					{
						NPC.velocity.X *= 0.5f;
						ai3++;
						if (ai3 < 60)
						{
							float waveY = (float)Math.Sin(MathHelper.ToRadians(ai3 * 4f));
							NPC.velocity.Y *= 0.875f;
							NPC.velocity.Y -= 0.4f * waveY;
							if (ai3 == 40)
							{
								SOTSUtils.PlaySound(SoundID.Item15, (int)player.Center.X, (int)player.Center.Y, 1.33f, -0.05f);
							}
						}
						else
						{
							if (ai3 == 60)
							{
								SOTSUtils.PlaySound(SoundID.Item96, (int)player.Center.X, (int)player.Center.Y, 1f, 0f);
								NPC.velocity.Y += 4.5f;
							}
							NPC.velocity.Y += 0.8f;
							Vector2 temp = NPC.velocity;
							NPC.velocity = Collision.AdvancedTileCollision(ignore, NPC.position, NPC.velocity, NPC.width, NPC.height, true, true);
							if (NPC.velocity != temp)
							{
								ParticleExplosion();
								if (Main.netMode != 1)
								{
									int damage = Common.GlobalNPCs.SOTSNPCs.GetBaseDamage(NPC) / 2;
									for (int i = 0; i < 6; i++)
									{
										Vector2 outWards = new Vector2(-2f, 0).RotatedBy(MathHelper.ToRadians(30 + i / 2 * 40));
										Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, outWards, ModContent.ProjectileType<CurseWave>(), damage, 0f, Main.myPlayer, NPC.whoAmI, (i % 2 * 2 - 1) * 0.8f);
									}
								}
								TransitionPhase(4);
							}
						}
					}
					else
					{
						Vector2 rotatePos = new Vector2(160, 0).RotatedBy(MathHelper.ToRadians(ai2));
						Vector2 toPos = rotatePos + player.Center;
						Vector2 goToPos = NPC.Center - toPos;
						float length = goToPos.Length() + 0.1f;
						if (length > 12)
						{
							length = 12;
						}
						goToPos = goToPos.SafeNormalize(Vector2.Zero);
						NPC.velocity = goToPos * -length;
					}
				}
			}
			if(aiPhase == 7)
            {
				NPC.dontTakeDamage = true;
				eyeOffsetMult = 0;
				if(startParticles > 0.5f)
                {
					startParticles *= 0.9975f;
                }
				else
                {
					startParticles = 0.5f;
					if(ai2 > 180)
                    {
						NPC.StrikeNPC(667 + NPC.defense / 2, 0, 0);
                    }
				}
            }
		}
		Vector2 storeDashArea;
		public void ChangeShade(float target, float changingPrinciple = 0.04f)
        {
			if(shadeAlpha > target)
            {
				shadeAlpha -= changingPrinciple;
            }
			else if(shadeAlpha < target)
            {
				shadeAlpha += changingPrinciple;
            }
			if(Math.Abs(shadeAlpha - target) < changingPrinciple)
            {
				shadeAlpha = target;
            }
			if(shadeAlpha <= 0.65f)
            {
				NPC.dontTakeDamage = true;
            }
			else
			{
				NPC.dontTakeDamage = false;
			}
			shadeAlpha = MathHelper.Clamp(shadeAlpha, 0, 1);
        }
		public void DashAttacks(float distance, float speedMult, int amt = 4)
		{
			Player player = Main.player[NPC.target];
			if (ai2 <= 40)
			{
				Vector2 dashArea = player.Center + new Vector2(distance * direction, 0);
				MoveTo(dashArea, 0.2f, 12f * speedMult);
			}
			else if (ai2 < 60)
			{
				float current = ai2 - 60;
				float sin = (float)Math.Sin(MathHelper.ToRadians(current * 180 / 20f));
				NPC.velocity *= 0.1f;
				NPC.velocity.X += sin * -5 * direction;
			}
			if (ai2 == 60)
			{
				SOTSUtils.PlaySound(SoundID.Item73, (int)NPC.Center.X, (int)NPC.Center.Y, 1.75f, 0.2f);
				NPC.velocity += new Vector2(-24 * direction * speedMult, 0);
			}
			if (ai2 > 60)
			{
				NPC.velocity += new Vector2(-1f * speedMult * direction, 0);
			}
			if (ai2 > 95)
			{
				ai2 = 0;
				ai3++;
				direction *= -1;
				BurstRings();
			}
			if (ai3 > amt)
			{
				if (enteredSecondPhase)
					TransitionPhase(2);
				else
					TransitionPhase(1);
			}
		}
        public override bool? DrawHealthBar(byte hbPosition, ref float scale, ref Vector2 position)
        {
            return smaller ? false : (bool?)null;
		}
		bool[] ignore;
		public override void PostAI()
		{
			Player player = Main.player[NPC.target];
			if(ignore == null)
			{
				ignore = new bool[Main.tileSolid.Length];
				for (int i = 0; i < ignore.Length; i++)
				{
					ignore[i] = true;
				}
				ignore[ModContent.TileType<TrueSandstoneTile>()] = false;
				ignore[ModContent.TileType<AncientGoldGateTile>()] = false;
			}
			NPC.velocity = Collision.AdvancedTileCollision(ignore, NPC.position, NPC.velocity, NPC.width, NPC.height, true, true);
		}
		public void TransitionPhase(int nextPhase)
		{
			smaller = false;
			if(nextPhase == 0)
			{
				if (NPC.Center.X > Main.player[NPC.target].Center.X)
					direction = 1;
				else
					direction = -1;
				ai2 = -90;
				ai3 = 0;
			}
			if(nextPhase == 1)
            {
				ai2 = 60;
				ai3 = 0;
			}
			if (nextPhase == 2)
			{
				ai2 = -80;
				ai3 = 0;
			}
			if (nextPhase == 3)
			{
				ai2 = 0;
				ai3 = 0;
			}
			if (nextPhase == 4)
			{
				ai2 = 60;
				ai3 = 0;
			}
			if (nextPhase == 5)
			{
				ai3 = Main.rand.Next(60, 180);
				ai2 = -50;
			}
			if (nextPhase == 6)
			{
				ai3 = 0;
				ai2 = -90;
			}
			aiPhase = nextPhase;
		}
		public bool PlayerValidPosition()
        {
			Player player = Main.player[NPC.target];
			int foundTrueTiles = 0;
			for(int checkDir = 0; checkDir < 4; checkDir++)
            {
				int length = 110;
				int dirX = 0;
				int dirY = 0;
				if (checkDir == 0)
					dirX = 1;
				if (checkDir == 1)
				{
					dirY = 1;
					length = 66;
				}
				if (checkDir == 2)
					dirX = -1;
				if (checkDir == 3)
				{
					dirY = -1;
					length = 66;
				}
				for (int k = 2; k < length; k++)
                {
					int i = (int)player.Center.X / 16;
					int j = (int)player.Center.Y / 16;
					i += k * dirX;
					j += k * dirY;
					Tile tile = Framing.GetTileSafely(i, j);
					if(tile.TileType == ModContent.TileType<TrueSandstoneTile>() || (tile.WallType == ModContent.WallType<TrueSandstoneWallWall>() && dirX != 0))
                    {
						foundTrueTiles++;
						break;
                    }
				}
            }
			return foundTrueTiles >= 4;
        }
	}
	public class CurseFoam
	{
		public Vector2 position;
		public Vector2 velocity;
		public float mult;
		public int dustColorVariation = 0;
		public bool noMovement = false;
		public float rotation = 0;
		public CurseFoam()
		{
			position = Vector2.Zero;
			velocity = Vector2.Zero;
			scale = 1;
			mult = Main.rand.NextFloat(0.9f, 1.1f);
		}
		public CurseFoam(Vector2 position, Vector2 velocity, float scale, bool noMovement = false)
		{
			this.position = position;
			this.velocity = velocity;
			this.scale = scale;
			mult = Main.rand.NextFloat(0.9f, 1.4f);
			this.noMovement = noMovement;
			dustColorVariation = Main.rand.Next(30);
		}
		public CurseFoam(Vector2 position, Vector2 velocity, float scale, bool noMovement, float lifeTimeMult)
		{
			this.position = position;
			this.velocity = velocity;
			this.scale = scale;
			mult = Main.rand.NextFloat(0.9f, 1.4f) * lifeTimeMult;
			this.noMovement = noMovement;
			dustColorVariation = Main.rand.Next(30);
		}
		public float counter = 0;
		public float scale;
		public bool active = true;
		float currentExtra = 0;
		public void Update()
		{
			position += velocity;
			currentExtra += mult;
			for (; currentExtra >= 1; currentExtra--)
			{
				if(noMovement)
				{
					scale *= 0.945f;
					velocity *= 0.9f;
				}
                else
				{
					scale *= 0.955f;
					velocity *= 0.925f;
				}
			}
			if (scale <= 0.05f)
				active = false;
		}
	}
}