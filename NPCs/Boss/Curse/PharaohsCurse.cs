using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Items.Pyramid;
using SOTS.Projectiles.Pyramid;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.NPCs.Boss.Curse
{
	[AutoloadBossHead]
	public class PharaohsCurse : ModNPC
	{
		public override void SendExtraAI(BinaryWriter writer)
		{
			writer.Write(direction);
		}
		public override void ReceiveExtraAI(BinaryReader reader)
		{
			direction = reader.ReadInt32();
		}
		int despawn = 0;
		private float ai1
		{
			get => npc.ai[0];
			set => npc.ai[0] = value;
		}
		private float ai2
		{
			get => npc.ai[1];
			set => npc.ai[1] = value;
		}
		private float ai3
		{
			get => npc.ai[2];
			set => npc.ai[2] = value;
		}
		private float aiPhase
		{
			get => npc.ai[3];
			set => npc.ai[3] = value;
		}
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Pharaoh's Curse");
			Main.npcFrameCount[npc.type] = 1;
		}
		public override void SetDefaults()
		{
			npc.aiStyle = 0;
			npc.lifeMax = 6000;
			npc.damage = 50;
			npc.defense = 12;
			npc.knockBackResist = 0f;
			npc.width = 38;
			npc.height = 44;
			npc.npcSlots = 20f;
			npc.boss = true;
			npc.lavaImmune = true;
			npc.noGravity = true;
			npc.noTileCollide = true;
			npc.HitSound = null;
			npc.DeathSound = SoundID.NPCDeath6;
			music = MusicID.Sandstorm;
			musicPriority = MusicPriority.BossMedium;
			npc.buffImmune[24] = true;
			npc.buffImmune[39] = true;
			npc.buffImmune[44] = true;
			npc.buffImmune[69] = true;
			npc.buffImmune[70] = true;
			npc.buffImmune[153] = true;
			bossBag = mod.ItemType("CurseBag");
			npc.netAlways = true;
			npc.alpha = 255;
		}
        public override bool CanHitPlayer(Player target, ref int cooldownSlot)
        {
            return npc.alpha == 0;
        }
        public override void HitEffect(int hitDirection, double damage)
		{
			if (npc.life > 0)
			{
				Main.PlaySound(3, (int)npc.Center.X, (int)npc.Center.Y, 54, 1.2f, -0.25f);
				int num = 0;
				while ((double)num < damage / (double)npc.lifeMax * 60.0)
				{
					Dust.NewDust(npc.position, npc.width, npc.height, mod.DustType("CurseDust"), (float)(2 * hitDirection), -2f);
					num++;
				}
			}
			else
			{
				if (Main.netMode != 1)
				{
					for (int k = 0; k < 80; k++)
					{
						Dust.NewDust(npc.position, npc.width, npc.height, mod.DustType("CurseDust"), (float)(2 * hitDirection), -2f);
					}
				}
			}
		}
		public override void BossLoot(ref string name, ref int potionType)
		{
			if (!SOTSWorld.downedCurse)
			{
				Main.NewText("The pyramid's curse weakens once more", 155, 115, 0);
			}
			SOTSWorld.downedCurse = true;
			potionType = ItemID.HealingPotion;

			if (Main.expertMode)
			{
				npc.DropBossBags();
			}
			else
			{
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("CursedMatter"), Main.rand.Next(12, 25));
			}
		}
		public override void FindFrame(int frameHeight)
		{

		}
		public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
		{
			npc.lifeMax = (int)(npc.lifeMax * bossLifeScale * 0.75f);
			npc.damage = (int)(npc.damage * 0.75f);
		}
		public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
		{
			if(!runOnce)
			{
				//DrawLimbs(spriteBatch, false, -1);
				DrawFoam(foamParticleList4, 3);
				DrawFoam(foamParticleList1, 2);
				DrawFoam(foamParticleList2, 1);
				//DrawLimbs(spriteBatch, false, 1);
			}
			Vector2 drawPos3 = npc.Center - Main.screenPosition;
			Texture2D texture = Main.npcTexture[npc.type];
			Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
			spriteBatch.Draw(texture, drawPos3 + new Vector2(0, 0), null, npc.GetAlpha(drawColor), npc.rotation, drawOrigin, npc.scale, SpriteEffects.None, 0f);
			if(!runOnce)
			{
				DrawFoam(foamParticleList3, 0);
			}
			return false;
		}
		public void DrawFoam(List<CurseFoam> dustList, int startPoint = 2)
		{
			Texture2D texture = ModContent.GetTexture("SOTS/NPCs/Boss/Curse/CurseFoam");
			Vector2 drawOrigin = new Vector2(texture.Width / 2, texture.Height/6);
			if (startPoint == 3)
			{
				for (int i = 0; i < dustList.Count; i++)
				{
					texture = ModContent.GetTexture("SOTS/NPCs/Boss/Curse/TumorBall");
					drawOrigin = new Vector2(texture.Width / 2, texture.Height / 2);
					int shade = 255 - (int)(dustList[i].counter * 4f);
					Color color = new Color(shade + dustList[i].dustColorVariation, shade - dustList[i].dustColorVariation, shade - dustList[i].dustColorVariation);
					color = Lighting.GetColor((int)dustList[i].position.X / 16, (int)dustList[i].position.Y / 16, color);
					Vector2 drawPos = dustList[i].position - Main.screenPosition;
					Rectangle frame = new Rectangle(0, 0, texture.Width, texture.Width);
					Main.spriteBatch.Draw(texture, drawPos + new Vector2(0, 0), frame, color, dustList[i].rotation, drawOrigin, dustList[i].scale * 1, SpriteEffects.None, 0f);
				}
			}
			else
			{
				if(startPoint != 2)
					texture = ModContent.GetTexture("SOTS/NPCs/Boss/Curse/CurseFoamAlt");
				for (int j = startPoint; j >= 0; j--)
				{
					for (int i = 0; i < dustList.Count; i++)
					{
						int shade = 255 - (int)(dustList[i].counter * 4f);
						Color color = new Color(shade + dustList[i].dustColorVariation, shade - dustList[i].dustColorVariation, shade - dustList[i].dustColorVariation);
						if (j != 2)
							color = Lighting.GetColor((int)dustList[i].position.X / 16, (int)dustList[i].position.Y / 16, color);
						else
                        {
							float reduction = shade / 255f;
							Color first = new Color((int)(111 * reduction), (int)(80 * reduction), (int)(154 * reduction));
							Color second = new Color((int)(76 * reduction), (int)(58 * reduction), (int)(101 * reduction));
							color = Color.Lerp(first, second, 0.5f + 0.5f * (float)Math.Sin(MathHelper.ToRadians(Void.VoidPlayer.soulColorCounter * 2)));
						}
						Vector2 drawPos = dustList[i].position - Main.screenPosition;
						Rectangle frame = new Rectangle(0, texture.Height / 3 * j, texture.Width, texture.Width);
						float scale = j == 0 ? 1.5f : 2.0f;
						Main.spriteBatch.Draw(texture, drawPos + new Vector2(0, 0), frame, color, dustList[i].rotation, drawOrigin, dustList[i].scale * scale, SpriteEffects.None, 0f);
					}
					/*texture = ModContent.GetTexture("SOTS/NPCs/Boss/Curse/CurseFoamAlt");
					for (int i = 0; i < dustList.Count; i++)
					{
						if (dustList[i].alt)
						{
							int shade = 255 - (int)(dustList[i].counter * 4f);
							Color color = npc.GetAlpha(new Color(shade + dustList[i].dustColorVariation, shade - dustList[i].dustColorVariation, shade - dustList[i].dustColorVariation));
							if (j != 2)
								color = Lighting.GetColor((int)dustList[i].position.X / 16, (int)dustList[i].position.Y / 16, color);
							Vector2 drawPos = dustList[i].position - Main.screenPosition;
							Rectangle frame = new Rectangle(0, texture.Height / 3 * j, texture.Width, texture.Width);
							float scale = j == 0 ? 1.5f : 2.0f;
							Main.spriteBatch.Draw(texture, drawPos + new Vector2(0, 0), frame, color, dustList[i].rotation, drawOrigin, dustList[i].scale * scale, SpriteEffects.None, 0f);
						}
					}*/
				}
			}
		}
		public static void SpawnPassiveDust(Texture2D texture, Vector2 spawnLocation, float scale, List<CurseFoam> dustList, float velocityScale = 1f, int style = 0, int rate = 45, float rotation = 0)
        {
			int width = texture.Width;
			int height = texture.Height;
			Color[] data = new Color[width * height];
			texture.GetData(data);
			Vector2 position;
			int localX = 0;
			int localY = 0;
			for (int i = 0; i < width * height; i++)
			{
				if (data[i].A >= 255 && Main.rand.NextBool(rate))
                {
					position = spawnLocation + scale * (-new Vector2(texture.Width / 2, texture.Height / 2) + new Vector2(localX, localY)).RotatedBy(rotation);
					Vector2 rotational = new Vector2(0, 1.00f).RotatedBy(MathHelper.ToRadians(Main.rand.NextFloat(360)));
					float scale2 = 0.9f;
					bool noMovement = false;
					if (style == 3)
                    {
						noMovement = true;
						scale2 = 0.33f;
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
						dustList.Add(new CurseFoam(position - rotational.SafeNormalize(Vector2.Zero) * 2 * scale2, rotational * velocityScale, Main.rand.NextFloat(0.9f, 1.1f) * scale2, style == 2));
					}
					else
						dustList.Add(new CurseFoam(position + rotational.SafeNormalize(Vector2.Zero) * 2 * scale2, rotational * velocityScale, Main.rand.NextFloat(0.9f, 1.1f) * scale2, noMovement));
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
		public override bool PreAI()
		{
			if (runOnce)
			{
				startParticles = 0.0f;
				aiPhase = -1;
				runOnce = false;
				direction = Main.rand.Next(2) * 2 - 1;
				CenterPosition = npc.Center;
			}
			npc.TargetClosest();
			Player player = Main.player[npc.target];
			if(aiPhase != -1)
				npc.rotation = npc.velocity.X * 0.05f;
			Lighting.AddLight(npc.Center, new Vector3(110 / 255f, 36 / 255f, 20 / 255f));
			if(Main.netMode != NetmodeID.Server)
			{
				Texture2D texture = ModContent.GetTexture("SOTS/NPCs/Boss/Curse/FartGas");
				if(startParticles != 0.0f)
				{
					SpawnPassiveDust(texture, npc.Center + new Vector2(0, 10), 0.9f * startParticles, foamParticleList1, 1, 0, 50, npc.rotation);
					SpawnPassiveDust(ModContent.GetTexture("SOTS/NPCs/Boss/Curse/FartGasInline"), npc.Center + new Vector2(0, 10), 0.9f * startParticles, foamParticleList1, 1, 0, 200, npc.rotation);
					SpawnPassiveDust(ModContent.GetTexture("SOTS/NPCs/Boss/Curse/FartGasBorder"), npc.Center + new Vector2(0, 10), 1.2f * startParticles, foamParticleList4, 0.2f, 2, 3600, npc.rotation);
				}
				texture = ModContent.GetTexture("SOTS/NPCs/Boss/Curse/PharaohsCurseOutline");
				SpawnPassiveDust(texture, npc.Center, 1.0f, foamParticleList2, 0.1f, 1, (int)(30 * (1f + Math.Pow(npc.alpha, 0.5f))), npc.rotation);
				SpawnPassiveDust(texture, npc.Center, 1.0f, foamParticleList3, 0.125f, 1, (int)(60 * (1f + Math.Pow(npc.alpha, 0.5f))), npc.rotation);
			}
			cataloguePos();
			ai1 += 2;
			bool inRange = Vector2.Distance(player.Center, npc.Center) <= 1600f;
			if (Main.netMode != NetmodeID.MultiplayerClient)
				npc.netUpdate = true;
			if (player.dead || !SOTSPlayer.ModPlayer(player).PyramidBiome || !inRange)
			{
				despawn++;
			}
			if (despawn >= 600)
			{
				npc.active = false;
			}
			return inRange;
		}
		public void ResetLists()
		{
			foamParticleList1 = new List<CurseFoam>();
			foamParticleList2 = new List<CurseFoam>();
			foamParticleList3 = new List<CurseFoam>();
			foamParticleList4 = new List<CurseFoam>();
			return;
			/*List<CurseFoam> temp = new List<CurseFoam>();
			for(int i = 0; i < foamParticleList1.Count; i++)
            {
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
				temp.Add(foamParticleList4[i]);
			}
			foamParticleList4 = new List<CurseFoam>();
			for (int i = 0; i < temp.Count; i++)
			{
				foamParticleList4.Add(temp[i]);
			}*/
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
						particle.position += npc.velocity * 0.825f;
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
					particle.position += npc.velocity * 0.9f;
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
					particle.position += npc.velocity;
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
		public void MoveTo(Vector2 goTo, float slowDownMult, float flatSpeed)
		{
			Vector2 toDestination = goTo - npc.Center;
			float speed = (flatSpeed + toDestination.Length() * 0.00045f);
			if (speed > toDestination.Length())
			{
				speed = toDestination.Length();
			}
			npc.velocity *= slowDownMult;
			npc.velocity += toDestination.SafeNormalize(Vector2.Zero) * speed * 0.75f;
		}
		public void MimicPolarisMovement(float speed)
		{
			Player player = Main.player[npc.target];
			npc.velocity *= 0.5f;
			Vector2 vectorToPlayer = player.Center - npc.Center;
			float yDist = vectorToPlayer.Y * 1.15f;
			float xDist = vectorToPlayer.X;
			float length = (float)Math.Sqrt(xDist * xDist + yDist * yDist);
			float speedMult = -4f + (float)Math.Pow(length, 1.055) * 0.015f;
			if (speedMult < 0)
			{
				speedMult *= 0.5f;
			}
			npc.velocity += vectorToPlayer.SafeNormalize(Vector2.Zero) * speedMult * speed;
		}
		public Vector2 CenterPosition;
		float dustAcceleration = 0f;
		int direction = 1;
		int resetListTimer = 0;
		public void BurstRings()
		{
			Main.PlaySound(2, (int)npc.Center.X, (int)npc.Center.Y, 14, 0.85f, 0.2f);
			if (Main.netMode == 1)
				return;
			for (int i = 0; i < 6; i++)
			{
				Vector2 outWards = new Vector2(-2f * Main.rand.NextFloat(0.9f, 1.1f), 0).RotatedBy(MathHelper.ToRadians(i * 60 + 30));
				int damage = npc.damage / 2;
				if (Main.expertMode)
				{
					damage = (int)(damage / Main.expertDamage);
				}
				Projectile.NewProjectile(npc.Center, outWards, ModContent.ProjectileType<CurseRing>(), damage, 0f, Main.myPlayer, npc.whoAmI);
			}
		}
		public override void AI()
		{
			resetListTimer--;
			if(resetListTimer < 0)
            {
				resetListTimer = 1800;
				ResetLists();
            }
			Player player = Main.player[npc.target];
			ai2++;
			if(aiPhase == -1)
            {
				float timeToStart = 360f;
				if (ai2 >= timeToStart)
				{
					if(ai2 == timeToStart)
					{
						Main.NewText("Pharaoh's Curse has awoken!", 175, 75, byte.MaxValue);
						Main.PlaySound(SoundID.Item14, (int)npc.Center.X, (int)npc.Center.Y);
						for (int j = 0; j < 240; j++)
						{
							float scale = Main.rand.NextFloat(0.5f, 1.5f);
							Vector2 rotational = new Vector2(0, -Main.rand.NextFloat(2.75f, 8.5f) / scale).RotatedBy(MathHelper.ToRadians(Main.rand.NextFloat(360f)));
							foamParticleList1.Add(new CurseFoam(npc.Center, rotational, 1.75f * scale, true));
						}
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
					npc.alpha = 255 - (int)ai2;
					if (npc.alpha < 0)
						npc.alpha = 0;
					npc.velocity.Y = -1 * (0.25f + 0.5f * ai2 / timeToStart);
					float rotationLimit = 1500f;
					npc.rotation = MathHelper.ToRadians(rotationLimit - rotationLimit * (float)Math.Pow(ai2 / timeToStart, 0.5f));
				}
				if (ai2 >= timeToStart + 30)
				{
					startParticles = 1;
					aiPhase = 0;
					ai2 = -30;
					npc.alpha = 0;
				}
            }
			if (aiPhase == 0)
			{
				if(Main.expertMode)
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
								float rand = Main.rand.NextFloat(-15, 15);
								Vector2 outWards = new Vector2(12, 0).RotatedBy(MathHelper.ToRadians(90 * i + rand));
								int damage = npc.damage / 2;
								if (Main.expertMode)
								{
									damage = (int)(damage / Main.expertDamage);
								}
								Projectile.NewProjectile(npc.Center, outWards, ModContent.ProjectileType<CurseArm>(), damage, 0f, Main.myPlayer, npc.whoAmI);
							}
						}
					}
					if(ai2 >= 270)
					{
						MimicPolarisMovement(0.2f);
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
					float speed = 3f;
					if (ai2 < 0)
					{
						speed = 0.3f;
					}
					MoveTo(CenterPosition, 0.2f, speed);
				}
				if (ai2 >= 730)
                {
					aiPhase = 0;
					ai2 = 0;
				}
			}
		}
		public void DashAttacks(float distance, float speedMult, int amt = 4)
		{
			Player player = Main.player[npc.target];
			if (ai2 <= 40)
			{
				Vector2 dashArea = player.Center + new Vector2(distance * direction, 0);
				MoveTo(dashArea, 0.2f, 12f * speedMult);
			}
			else if (ai2 < 60)
			{
				float current = ai2 - 60;
				float sin = (float)Math.Sin(MathHelper.ToRadians(current * 180 / 20f));
				npc.velocity *= 0.1f;
				npc.velocity.X += sin * -5 * direction;
			}
			if (ai2 == 60)
			{
				Main.PlaySound(2, (int)npc.Center.X, (int)npc.Center.Y, 73, 1.75f, 0.2f);
				npc.velocity += new Vector2(-24 * direction * speedMult, 0);
			}
			if (ai2 > 60)
			{
				npc.velocity += new Vector2(-1f * speedMult * direction, 0);
			}
			if (ai2 > 95)
			{
				ai2 = 0;
				ai3++;
				direction *= -1;
				BurstRings();
			}
			if (ai3 > 4)
			{
				ai2 = -10;
				ai3 = 0;
				aiPhase = 1;
			}
		}
		bool[] ignore;
		public override void PostAI()
		{
			Player player = Main.player[npc.target];
			if(ignore == null)
			{
				ignore = new bool[Main.tileSolid.Length];
				for (int i = 0; i < ignore.Length; i++)
				{
					ignore[i] = true;
				}
				ignore[ModContent.TileType<TrueSandstoneTile>()] = false;
			}
			npc.velocity = Collision.AdvancedTileCollision(ignore, npc.position, npc.velocity, npc.width, npc.height, true, true);
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
		public float counter = 0;
		public float scale;
		public bool active = true;
		public void Update()
		{
			position += velocity;
			for (int i = 0; i < 1 + (int)(Main.rand.NextFloat(1f) * mult); i++)
			{
				if(noMovement)
				{
					scale *= 0.945f;
					velocity *= 0.9f;
				}
                else
				{
					velocity *= 0.925f;
					scale *= 0.955f;
				}
			}
			if (scale <= 0.05f)
				active = false;
		}
	}
}