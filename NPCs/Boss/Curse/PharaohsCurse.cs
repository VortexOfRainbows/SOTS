using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.NPCs.Boss.Curse
{
	[AutoloadBossHead]
	public class PharaohsCurse : ModNPC
	{
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

		private float ai4
		{
			get => npc.ai[3];
			set => npc.ai[3] = value;
		}
		public override void SendExtraAI(BinaryWriter writer)
		{
			writer.Write(ai1);
		}
		public override void ReceiveExtraAI(BinaryReader reader)
		{
			ai1 = reader.ReadSingle();
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
			npc.damage = 40;
			npc.defense = 12;
			npc.knockBackResist = 0f;
			npc.width = 38;
			npc.height = 44;
			npc.npcSlots = 20f;
			npc.boss = true;
			npc.lavaImmune = true;
			npc.noGravity = true;
			npc.noTileCollide = false;
			npc.HitSound = SoundID.NPCHit54;
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
			npc.damage = (int)(npc.damage * 0.8f);
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
					Color color = npc.GetAlpha(new Color(shade + dustList[i].dustColorVariation, shade - dustList[i].dustColorVariation, shade - dustList[i].dustColorVariation));
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
						Color color = npc.GetAlpha(new Color(shade + dustList[i].dustColorVariation, shade - dustList[i].dustColorVariation, shade - dustList[i].dustColorVariation));
						if (j != 2)
							color = Lighting.GetColor((int)dustList[i].position.X / 16, (int)dustList[i].position.Y / 16, color);
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
		public void DrawLimbs(List<CurseFoam> dustList)
		{
			for (int i = 0; i < limbs.Count; i++)
			{
				ArmEnd Limb = limbs[i];
				Vector2 distanceToOwner = Limb.position - npc.Center;
				int max = 24;
				for(int k = 0; k < max; k++)
                {
					float percent = (float)k / max;
					Vector2 toARM = Limb.position - npc.Center;
					toARM *= percent;
					Vector2 finalPosition = npc.Center + toARM;
					float scale = 1.0f - 0.8f * percent;
					Vector2 rotational = new Vector2(0, 1.7f).RotatedBy(MathHelper.ToRadians(Main.rand.NextFloat(360)));
					rotational += Limb.velocity * 0.1f;
					rotational.Y -= 0.25f;
					dustList.Add(new CurseFoam(finalPosition - rotational.SafeNormalize(Vector2.Zero) * 4, rotational, Main.rand.NextFloat(0.9f, 1.1f) * scale));
				}
				/*for(int k = (!dust ? 1 : 1); k >= 0; k--)
				{
					Vector2 last = npc.Center;
					Vector2 lastR = npc.Center;
					for (int j = max; j > 0; j--)
					{
						float interval = -180f / (max + 1);
						Vector2 rotationPos = new Vector2(radius, 0).RotatedBy(MathHelper.ToRadians(interval * j));
						rotationPos.Y /= 3.5f;
						rotationPos = rotationPos.RotatedBy(startingRadians);
						Vector2 pos = rotationPos + centerOfCircle;
						Vector2 storeForLater = pos;
						Vector2 fromLast = pos - last;
						last = pos;
						float zDepth = 0;
						if (!dust)
						{
							Vector2 spiralAddition = new Vector2(1, 0).RotatedBy(MathHelper.ToRadians(22.5f * j + ai1 * 0.2f));
							rotationPos += new Vector2(0, spiralAddition.X * 12).RotatedBy(fromLast.ToRotation());
							pos = rotationPos + centerOfCircle;
							zDepth = spiralAddition.Y;
						}
						Vector2 fromLastR = pos - lastR;
						lastR = pos;
						Vector2 circular = new Vector2(1, 0).RotatedBy(MathHelper.ToRadians(interval * i));
						Vector2 dynamicAddition = new Vector2(0.5f + 2.0f * circular.Y, 0).RotatedBy(MathHelper.ToRadians(j * interval * 2 + ai1 * 1));
						if (!dust && (zDepth > 0 && depth == 1) || (zDepth < 0 && depth == -1))
						{
							Vector2 drawPos = pos - Main.screenPosition;
							Texture2D texture = ModContent.GetTexture("SOTS/NPCs/Boss/Curse/Bandage");
							Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, texture.Height / 2);
							Rectangle frame = new Rectangle(0, 0, texture.Width, texture.Width);
							spriteBatch.Draw(texture, drawPos + dynamicAddition, frame, Color.White, fromLastR.ToRotation(), drawOrigin, npc.scale * 0.85f, j % 2 == 0 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);
						}
						else if(dust)
						{
						}
					}
				}*/
			}
		}
		public void SpawnPassiveDust(Texture2D texture, Vector2 spawnLocation, float scale, List<CurseFoam> dustList, float velocityScale = 1f, int style = 0, int rate = 45)
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
					position = spawnLocation + scale * (-new Vector2(texture.Width / 2, texture.Height / 2) + new Vector2(localX, localY)).RotatedBy(npc.rotation);
					Vector2 rotational = new Vector2(0, 1.00f).RotatedBy(MathHelper.ToRadians(Main.rand.NextFloat(360)));
					float scale2 = 0.9f;
					if(style == 1 || style == 2)
                    {
						if (style == 2)
							scale2 = 0.75f;
						else
							scale2 = 0.55f;
						Vector2 toCenter = npc.Center - position;
						float speedMult = 1.5f;
						if (style == 2)
							speedMult = -1.5f;
						rotational = toCenter.SafeNormalize(Vector2.Zero) * speedMult;
						dustList.Add(new CurseFoam(position - rotational.SafeNormalize(Vector2.Zero) * 2, rotational * velocityScale, Main.rand.NextFloat(0.9f, 1.1f) * scale2, style == 2));
					}
					else
						dustList.Add(new CurseFoam(position + rotational.SafeNormalize(Vector2.Zero) * 2, rotational * velocityScale, Main.rand.NextFloat(0.9f, 1.1f) * scale2));
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
		public List<ArmEnd> limbs;
		float varianceCounter = 0f;
		public override bool PreAI()
		{
			npc.rotation = npc.velocity.X * 0.05f;
			Lighting.AddLight(npc.Center, new Vector3(110 / 255f, 36 / 255f, 20 / 255f));
			int num = -1;
			if (npc.velocity.X > 0)
				num = 1;
			varianceCounter += num + npc.velocity.X * 0.5f;
			Texture2D texture = ModContent.GetTexture("SOTS/NPCs/Boss/Curse/FartGas");
			SpawnPassiveDust(texture, npc.Center + new Vector2(0, 10), 0.9f, foamParticleList1, 1, 0, 42);
			SpawnPassiveDust(ModContent.GetTexture("SOTS/NPCs/Boss/Curse/FartGasBorder"), npc.Center + new Vector2(0, 10), 1.2f, foamParticleList4, 0.2f, 2, 3600);
			texture = ModContent.GetTexture("SOTS/NPCs/Boss/Curse/PharaohsCurseOutline");
			SpawnPassiveDust(texture, npc.Center, 1.0f, foamParticleList2, 0.1f, 1, 27);
			SpawnPassiveDust(texture, npc.Center, 1.0f, foamParticleList3, 0.125f, 1, 56);
			cataloguePos();
			Player player = Main.player[npc.target];
			ai1 += 2;
			if (runOnce)
			{
				limbs = new List<ArmEnd>();
				for (int i = -1; i < 2; i += 2)
				{
					Vector2 outWards = new Vector2(-12 * i, 0);
					Vector2 outWards2 = new Vector2(-4 * i, 16);
					limbs.Add(new ArmEnd(npc.Center, outWards));
					limbs.Add(new ArmEnd(npc.Center, outWards2));
				}
				runOnce = false;
			}
			else
			{
				DrawLimbs(foamParticleList1);
			}
			for (int i = 0; i < limbs.Count; i++)
			{
				ArmEnd limb = limbs[i];
				limb.AI();
			}
			for (int i = 0; i < 4; i ++)
			{
				float variance = (float)Math.Sin(MathHelper.ToRadians(ai1 + i * 45f));
				Vector2 outWards = new Vector2(2 + 16 * variance, 0).RotatedBy(MathHelper.ToRadians(i * 90 + varianceCounter));
				outWards += npc.Center;
				limbs[i].TrackDestination(outWards);
			}
			npc.netUpdate = true;
			float variant = (float)Math.Sin(MathHelper.ToRadians(ai1));
			Vector2 goTo = player.Center + new Vector2(0, -128 + variant * 24);
			MoveTo(goTo);
			return true;
		}
		public void cataloguePos()
		{
			for (int i = 0; i < foamParticleList1.Count; i++)
			{
				CurseFoam particle = foamParticleList1[i];
				particle.Update();
				particle.Update();
				particle.position += npc.velocity * 0.825f;
				if (!particle.active)
				{
					foamParticleList1.RemoveAt(i);
					i--;
				}
			}
			for (int i = 0; i < foamParticleList2.Count; i++)
			{
				CurseFoam particle = foamParticleList2[i];
				particle.Update();
				particle.position += npc.velocity * 0.9f;
				if (!particle.active)
				{
					foamParticleList2.RemoveAt(i);
					i--;
				}
			}
			for (int i = 0; i < foamParticleList3.Count; i++)
			{
				CurseFoam particle = foamParticleList3[i];
				particle.Update();
				particle.position += npc.velocity;
				if (!particle.active)
				{
					foamParticleList3.RemoveAt(i);
					i--;
				}
			}
			for (int i = 0; i < foamParticleList4.Count; i++)
			{
				CurseFoam particle = foamParticleList4[i];
				particle.Update();
				particle.velocity.Y += 0.11f;
				if (!particle.active)
				{
					foamParticleList4.RemoveAt(i);
					i--;
				}
			}
		}
		public void MoveTo(Vector2 goTo)
		{
			Vector2 toDestination = goTo - npc.Center;
			float speed = (7 + toDestination.Length() * 0.00045f);
			if (speed > toDestination.Length())
			{
				speed = toDestination.Length();
			}
			npc.velocity *= 0.2f;
			npc.velocity += toDestination.SafeNormalize(Vector2.Zero) * speed * 0.75f;
		}
		public override void AI()
		{

		}
		public override void HitEffect(int hitDirection, double damage)
		{
			if (npc.life > 0)
			{
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
	}
	public class ArmEnd
	{
		public Vector2 position;
		public Vector2 velocity;
		public Vector2 drawVelo;
		public void AI()
		{
			position += velocity;
			velocity *= 0.955f;
			drawVelo += velocity.SafeNormalize(Vector2.Zero) * (float)Math.Sqrt(velocity.Length());
			drawVelo *= 0.955f;
		}
		public void TrackDestination(Vector2 destination)
		{
			Vector2 toDestination = destination - position;
			float speed = 8 + toDestination.Length() * 0.0005f;
			if (speed > toDestination.Length())
			{
				speed = toDestination.Length();
			}
			velocity *= 0.2f;
			velocity += toDestination.SafeNormalize(Vector2.Zero) * speed * 0.8f;
		}
		public ArmEnd(Vector2 position, Vector2 velo)
		{
			this.position = position;
			this.velocity = velo;
		}
	}
	public class CurseFoam
	{
		public Vector2 position;
		public Vector2 velocity;
		public float mult;
		public int dustColorVariation = 0;
		bool longerLife = false;
		public float rotation = 0;
		public CurseFoam()
		{
			position = Vector2.Zero;
			velocity = Vector2.Zero;
			scale = 1;
			mult = Main.rand.NextFloat(0.9f, 1.1f);
		}
		public CurseFoam(Vector2 position, Vector2 velocity, float scale, bool longerLife = false)
		{
			this.position = position;
			this.velocity = velocity;
			this.scale = scale;
			mult = Main.rand.NextFloat(0.9f, 1.4f);
			this.longerLife = longerLife;
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
				if (longerLife)
				{
					rotation += velocity.X * 0.5f;
					velocity.X *= 0.975f;
					scale *= 0.99f;
				}
				else
				{
					velocity *= 0.925f;
					scale *= 0.955f;
				}
			}
			if (!longerLife)
				counter++;
			if (scale <= 0.05f)
				active = false;
		}
	}
}