using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Dusts;
using SOTS.Helpers;
using SOTS.Items.Banners;
using SOTS.Items.Conduit;
using SOTS.Items.Fragments;
using SOTS.Prim.Trails;
using SOTS.Projectiles.Permafrost;
using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.NPCs.Anomaly
{
	public class Planetoid : ModNPC
	{
        public override void SetStaticDefaults()
		{
			NPCID.Sets.MPAllowedEnemies[Type] = true;
			Main.npcFrameCount[NPC.type] = 17;
			NPCID.Sets.TrailCacheLength[NPC.type] = 10;
			NPCID.Sets.TrailingMode[NPC.type] = 0;
            NPCID.Sets.NoMultiplayerSmoothingByType[NPC.type] = true;
        }
        public override void SetDefaults()
        {
            NPC.aiStyle = 0;
            NPC.lifeMax = 250;  
			NPC.damage = 60; 
			NPC.defense = 7;  
			NPC.knockBackResist = 0.0f;
			NPC.width = 72;
			NPC.height = 72;
			NPC.value = Item.buyPrice(0, 0, 40, 0);
			NPC.npcSlots = 3f;
			NPC.boss = false;
			NPC.lavaImmune = false;
			NPC.noGravity = true;
			NPC.noTileCollide = true;
			NPC.netAlways = true;
			NPC.alpha = 0;
			NPC.HitSound = SoundID.NPCHit54;
			NPC.DeathSound = SoundID.NPCDeath6;
			NPC.rarity = 5;
			Banner = NPC.type;
			BannerItem = ModContent.ItemType<PlanetoidBanner>();
		}
        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
			DrawLines(spriteBatch, screenPos);
			Texture2D texture = Terraria.GameContent.TextureAssets.Npc[NPC.type].Value;
			Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, texture.Height / Main.npcFrameCount[NPC.type] / 2f);
			Color color = new Color(210, 50, 230, 0) * 0.6f;
			color.A = 0;
			for(int i = 0; i < NPCID.Sets.TrailCacheLength[Type]; i++)
            {
				float percent = 1 - i / (float)NPCID.Sets.TrailCacheLength[Type];
				Vector2 position = NPC.oldPos[i] + drawOrigin;
				spriteBatch.Draw(texture, position - screenPos, NPC.frame, NPC.GetAlpha(color) * 0.45f * percent, NPC.rotation + i / 12f * MathHelper.TwoPi, drawOrigin, 1.0f, SpriteEffects.None, 0f);
			}
			for (int i = 0; i < 12; i++)
			{
				Vector2 offset = new Vector2(5f, 0).RotatedBy(MathHelper.ToRadians(i * 30 + SOTSWorld.GlobalCounter));
				spriteBatch.Draw(texture, NPC.Center - screenPos + offset, NPC.frame, NPC.GetAlpha(color) * 0.5f, NPC.rotation + i / 12f * MathHelper.TwoPi, drawOrigin, 1.0f, SpriteEffects.None, 0f);
			}
			spriteBatch.Draw(texture, NPC.Center - screenPos, NPC.frame, Color.White, NPC.rotation, drawOrigin, 1.0f, SpriteEffects.FlipHorizontally, 0f);
			return false;
		}
		public void DrawLines(SpriteBatch spriteBatch, Vector2 screenPos)
		{
			return;
			/*Texture2D texture = ModContent.Request<Texture2D>("SOTS/NPCs/Anomaly/PlanetoidGridLine").Value;
			Vector2 drawOrigin = new Vector2(0, 1);
			if(GravityWell.Count > 1)
			{
				Vector2 firstPosition = GravityWell[0].Position;
				for(int j = 0; j < 2; j++)
				{
					Color color = Color.Black;
					if(j == 1)
					{
						color = ColorHelpers.VoidAnomaly;
						color.A = 0;
					}
					for (int i = 1; i < GravityWell.Count; i++)
					{
						float percent = (float)i / GravityWell.Count;
						float sinusoid = (float)Math.Sin(percent * MathHelper.Pi);
						Vector2 toNextPosition = GravityWell[i].Position - firstPosition;
						float length = toNextPosition.Length();
						float xScale = length / texture.Width;
						float rotation = toNextPosition.ToRotation();
						spriteBatch.Draw(texture, firstPosition - screenPos, null, color * sinusoid, rotation, drawOrigin, new Vector2(xScale, 1f), SpriteEffects.None, 0f);
						firstPosition = GravityWell[i].Position;
					}
				}
			}*/
        }
		public override void HitEffect(NPC.HitInfo hit)
		{
			if (Main.netMode == NetmodeID.Server)
				return;
			if (NPC.life <= 0)
			{
				for (int k = 0; k < 30; k++)
				{
					Dust d = Dust.NewDustDirect(NPC.position, NPC.width, NPC.height, ModContent.DustType<CopyDust4>(), (float)(2 * hit.HitDirection), -2f);
					d.velocity *= 1.0f;
					d.fadeIn = 0.2f;
					d.noGravity = true;
					d.scale *= 1.5f;
					d.color = ColorHelper.VoidAnomaly;
					Vector2 circular = new Vector2(32, 0).RotatedBy(k / 30f * MathHelper.TwoPi);
					d = Dust.NewDustDirect(NPC.Center + circular - new Vector2(5, 5), 0, 0, ModContent.DustType<CopyDust4>(), (float)(2 * hit.HitDirection), -2f);
					d.velocity *= 0.4f;
					d.velocity += circular.SafeNormalize(Vector2.Zero) * 4;
					d.fadeIn = 0.2f;
					d.noGravity = true;
					d.scale *= 1.75f;
					d.color = ColorHelper.VoidAnomaly;
				}
			}
		}
		public override void PostAI()
		{
			base.PostAI();
		}
		public bool drawNewLines = false;
		public const float DurationFindLocation = 60f;
		public const float FindTarget = 10f;
		public const float PrepareSlingDuration = 20f;
		public const float SlingDuration = 60f;
		public const float PropellDuration = 50; 
		public PlanetoidTrail primTrail;
		public bool runOnce = true;
		public override bool PreAI()
		{
			if(runOnce)
			{
				if (Main.netMode != NetmodeID.Server)
				{
					primTrail = new PlanetoidTrail(NPC, 14);
					SOTS.primitives.CreateTrail(primTrail);
				}
				runOnce = false;
            }
			NPC.TargetClosest(false);
			Player target = Main.player[NPC.target];
			Vector2 toPlayer;
			if(!NPC.velocity.Equals(Vector2.Zero))
				NPC.rotation = NPC.velocity.ToRotation();
			NPC.ai[0]++;
			if (NPC.ai[0] > 0 && NPC.ai[0] < FindTarget)
			{
				NPC.ai[1] = target.Center.X;
				NPC.ai[2] = target.Center.Y;
				if (Main.netMode == NetmodeID.Server)
					NPC.netUpdate = true;
			}
			else if (NPC.ai[0] < 0)
			{
				for(int i = 0; i < 2; i++)
				{
					if (i == 0)
					{
						Dust d = Dust.NewDustDirect(NPC.position + new Vector2(5, 5), NPC.width - 10, NPC.height - 10, ModContent.DustType<CopyDust4>(), 0, 0);
						d.velocity *= 0.75f;
						d.velocity += NPC.velocity * 0.335f;
						d.fadeIn = 0.2f;
						d.noGravity = true;
						d.scale *= 1.5f;
						d.color = ColorHelper.VoidAnomaly;
					}
					else
					{
						Dust d = Dust.NewDustDirect(NPC.Center - new Vector2(5, 5), 0, 0, ModContent.DustType<PixelDust>(), 0, 0);
						d.velocity *= 0.25f;
						d.velocity += NPC.velocity * 0.25f;
						d.fadeIn = 8f;
						d.noGravity = true;
						d.scale *= 2f;
						d.color = ColorHelper.VoidAnomaly;
						d.color.A = 0;
					}
				}
			}
			toPlayer = new Vector2(NPC.ai[1], NPC.ai[2]) - NPC.Center;
			if (NPC.ai[0] < DurationFindLocation)
			{
                if (NPC.ai[0] > FindTarget)
				{
					NPC.velocity += toPlayer.SafeNormalize(Vector2.Zero).RotatedBy(MathHelper.ToRadians(75 * direction)) * (2.5f + (NPC.whoAmI % 9) * 0.025f);
					NPC.velocity *= 0.8f;
                    if (NPC.ai[0] > FindTarget + 10)
					drawNewLines = true;
                    if (NPC.ai[3] <= 0 && NPC.ai[0] < 50)
						NPC.ai[3] = 1;
				}
				else if(NPC.ai[0] > 0)
                {
					if(toPlayer.Length() < 240)
                    {
						NPC.velocity -= toPlayer.SafeNormalize(Vector2.Zero).RotatedBy(MathHelper.ToRadians(-30 * direction)) * 5.25f;
						NPC.velocity *= 0.8775f;
                    }
                }
			}
			else
			{
				if (NPC.ai[0] < DurationFindLocation + PrepareSlingDuration)
				{
					NPC.velocity -= toPlayer.SafeNormalize(Vector2.Zero).RotatedBy(MathHelper.ToRadians(90 * direction)) * 2.5f;
					NPC.velocity *= 0.825f;
					drawNewLines = false;
				}
                else
				{
                    if (NPC.ai[0] == DurationFindLocation + PrepareSlingDuration || NPC.ai[0] == DurationFindLocation + PrepareSlingDuration + 10 || NPC.ai[0] == DurationFindLocation + PrepareSlingDuration + 20)
						SOTSUtils.PlaySound(SoundID.Item15, NPC.Center, 1.1f, -0.15f);
					float windUp = (NPC.ai[0] - DurationFindLocation - PrepareSlingDuration) / SlingDuration;
					float sinusoid = (float)Math.Sin(MathHelper.ToRadians((NPC.ai[0] - DurationFindLocation - PrepareSlingDuration) / SlingDuration * 245f)) * (1 - windUp) * 2.25f;
					NPC.velocity -= toPlayer.SafeNormalize(Vector2.Zero) * sinusoid;
					NPC.velocity *= 0.77f;
				}
                if(NPC.ai[0] > DurationFindLocation + PrepareSlingDuration + SlingDuration)
				{
					SOTSUtils.PlaySound(SoundID.Item96, NPC.Center, 1.35f, -0.15f);
					NPC.ai[0] = -PropellDuration;
					NPC.velocity += toPlayer.SafeNormalize(Vector2.Zero) * 15f;
					NPC.ai[3] = -1f;
					if (Main.netMode == NetmodeID.Server)
						NPC.netUpdate = true;
					else
					{
						ResetGravityWell(true);
						for (int k = 0; k < 30; k++)
						{
							Dust d = Dust.NewDustDirect(NPC.position, NPC.width, NPC.height, ModContent.DustType<CopyDust4>(), 0, 0);
							d.velocity *= 0.85f;
							d.velocity += NPC.velocity * 0.85f;
							d.fadeIn = 0.2f;
							d.noGravity = true;
							d.scale *= 1.5f;
							d.color = ColorHelper.VoidAnomaly;
							Vector2 circular = new Vector2(32, 0).RotatedBy(k / 30f * MathHelper.TwoPi);
							d = Dust.NewDustDirect(NPC.Center + circular - new Vector2(5, 5), 0, 0, ModContent.DustType<CopyDust4>());
							d.velocity *= 0.35f;
							d.velocity += circular.SafeNormalize(Vector2.Zero) * 4 + NPC.velocity * 0.75f;
							d.fadeIn = 0.2f;
							d.noGravity = true;
							d.scale *= 1.75f;
							d.color = ColorHelper.VoidAnomaly;
						}
					}
				}
			}
			if (NPC.velocity.X > 0)
				NPC.spriteDirection = 1;
			else
				NPC.spriteDirection = -1;
			NPC.position -= NPC.velocity;
			RegisterLines();
			NPC.velocity.X /= 0.93f;
			if (Main.netMode == NetmodeID.Server)
				NPC.netUpdate = true;
			return true;
		}
        public override void FindFrame(int frameHeight)
        {
			float frameSpeed = 0.5f + (float)Math.Sqrt(NPC.velocity.Length());
			NPC.frameCounter += frameSpeed;
			if(NPC.frameCounter >= 6)
            {
				NPC.frameCounter -= 6;
				NPC.frame.Y += frameHeight * Math.Sign(NPC.velocity.X);
				if(NPC.frame.Y >= 17 * frameHeight)
                {
					NPC.frame.Y = 0;
                }
				if(NPC.frame.Y < 0)
                {
					NPC.frame.Y = frameHeight * 16;
                }
			}
        }
        public override void ModifyNPCLoot(NPCLoot npcLoot)
		{
			npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<SkipSoul>(), 1, 2, 3));
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<RiftCookie>(), 5, 1, 1));
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<TinyPlanetoid>(), 20, 1, 1));
        }
		public List<GravityWellLine> GravityWell = new List<GravityWellLine>();
		public float AlphaMultGravityWell = 1f;
		public int direction => NPC.whoAmI % 2 * 2 - 1;
		public void RegisterLines()
		{
			if (Main.netMode != NetmodeID.Server)
			{
				if (AlphaMultGravityWell <= 0)
				{
					ResetGravityWell();
				}
			}
            if (NPC.ai[3] < 0)
            {
				AlphaMultGravityWell = (float)(1 + NPC.ai[3] / 30f);
				if(AlphaMultGravityWell <= 0)
                {
					AlphaMultGravityWell = 0;
					NPC.ai[3] = 0;
					if (Main.netMode == NetmodeID.Server)
						NPC.netUpdate = true;
				}
				else
					NPC.ai[3]--;
			}
			else if (NPC.ai[3] > 0)
			{
				AlphaMultGravityWell = (float)(NPC.ai[3] / 60f);
				if (AlphaMultGravityWell >= 1)
				{
					AlphaMultGravityWell = 1;
					NPC.ai[3] = 0;
					if (Main.netMode == NetmodeID.Server)
						NPC.netUpdate = true;
				}
				else
					NPC.ai[3]++;
			}
			if (Main.netMode != NetmodeID.Server)
			{
				for (int a = 0; a < 4; a++)
				{
					Vector2 position = NPC.Center - new Vector2(0, -42 * direction).RotatedBy(NPC.velocity.ToRotation());
					if (drawNewLines)
					{
						GravityWell.Add(new GravityWellLine(position));
						while (GravityWell.Count > 500)
						{
							GravityWell.RemoveAt(0);
						}
					}
					else
					{
						float windUp = (NPC.ai[0] - DurationFindLocation - PrepareSlingDuration) / SlingDuration;
						for (int i = 0; i < GravityWell.Count; i++)
						{
							Vector2 vector = GravityWell[i].Position;
							bool inRange = NPC.Distance(vector) < 200f;
							if (inRange && NPC.ai[0] > DurationFindLocation + PrepareSlingDuration)
							{
								float stretch = (float)Math.Pow(1 - NPC.Distance(vector) / 200f, 4);
								Vector2 awayFromNPC = vector - NPC.Center;
								float stretchDist = 3.6f * stretch * (1 - windUp);
								if (NPC.Distance(vector) < 40)
								{
									stretchDist += 40 - NPC.Distance(vector);
									if (NPC.ai[0] < DurationFindLocation + PrepareSlingDuration + SlingDuration - 15 && NPC.ai[0] > DurationFindLocation + PrepareSlingDuration && i < GravityWell.Count - 1)
									{
										if (Vector2.Distance(GravityWell[i].Position, GravityWell[i + 1].Position) > 4)
										{
											GravityWell.Insert(i + 1, new GravityWellLine(
													Vector2.Lerp(GravityWell[i].Position, GravityWell[i + 1].Position, 0.5f),
													Vector2.Lerp(GravityWell[i].OriginalPosition, GravityWell[i + 1].OriginalPosition,
													0.5f)));
										}
									}

								}
								GravityWell[i].Position += awayFromNPC.SafeNormalize(Vector2.Zero) * stretchDist;
							}
							else
							{
								if (NPC.ai[0] < DurationFindLocation + PrepareSlingDuration + SlingDuration - 15 && NPC.ai[0] > DurationFindLocation + PrepareSlingDuration)
									GravityWell[i].Position = Vector2.Lerp(vector, GravityWell[i].OriginalPosition, 0.003f);
								else if (NPC.ai[0] < 0 && inRange)
								{
									float stretch = (float)Math.Pow(1 - NPC.Distance(vector) / 160f, 4);
									GravityWell[i].Position += NPC.velocity * 0.3f * stretch;
									GravityWell[i].Position = Vector2.Lerp(GravityWell[i].Position, GravityWell[i].OriginalPosition, 0.05f);
								}
								else
									GravityWell[i].Position = Vector2.Lerp(vector, GravityWell[i].OriginalPosition, 0.04f);
							}
						}
					}
					NPC.position += NPC.velocity * 0.25f;
				}
				primTrail.ConvertListToPoints(GravityWell, AlphaMultGravityWell);
			}
			else
				NPC.position += NPC.velocity;
        }
		public void ResetGravityWell(bool fakeReset = false)
        {
			for(int i = 0; i < GravityWell.Count; i++)
            {
				GravityWellLine gwl = GravityWell[i];
				if(Main.rand.NextBool(3))
				{
					Dust d = Dust.NewDustDirect(gwl.Position - new Vector2(17, 17), 24, 24, ModContent.DustType<CopyDust4>(), 0, 0);
					d.velocity *= 1.0f;
					if(fakeReset)
					{
						d.velocity += NPC.velocity * 0.6f;
					}
					d.fadeIn = 0.2f;
					d.noGravity = true;
					d.scale *= 1.6f;
					d.color = Color.Lerp(new Color(213, 66, 232), new Color(191, 190, 238), Main.rand.NextFloat());
					if (!fakeReset)
						d.alpha = 190;
				}
            }
			if(!fakeReset)
				GravityWell = new List<GravityWellLine>();
        }
		public class GravityWellLine
        {
			public Vector2 Position;
			public Vector2 OriginalPosition;
			public GravityWellLine(Vector2 pos)
            {
				Position = pos;
				OriginalPosition = pos;
			}
			public GravityWellLine(Vector2 pos, Vector2 ogPos)
			{
				Position = pos;
				OriginalPosition = ogPos;
			}
		}
	}
}