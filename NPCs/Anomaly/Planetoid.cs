using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Dusts;
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
		}
        public override void SetDefaults()
		{
			NPC.lifeMax = 600;  
			NPC.damage = 35; 
			NPC.defense = 20;  
			NPC.knockBackResist = 0.0f;
			NPC.width = 72;
			NPC.height = 72;
			NPC.value = Item.buyPrice(0, 0, 20, 0);
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
		}
        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
			DrawLines(spriteBatch, screenPos);
			Texture2D texture = Terraria.GameContent.TextureAssets.Npc[NPC.type].Value;
			Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, texture.Height / Main.npcFrameCount[NPC.type] / 2);
			spriteBatch.Draw(texture, NPC.Center - screenPos, NPC.frame, Color.White, NPC.rotation, drawOrigin, 1f, SpriteEffects.FlipHorizontally, 0f);
			return false;
		}
		public void DrawLines(SpriteBatch spriteBatch, Vector2 screenPos)
		{
			return;
			Texture2D texture = ModContent.Request<Texture2D>("SOTS/NPCs/Anomaly/PlanetoidGridLine").Value;
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
			}
        }
		public override void HitEffect(int hitDirection, double damage)
		{
			if (Main.netMode == NetmodeID.Server)
				return;
			if (NPC.life <= 0)
			{ 

			}
		}
		public override void PostAI()
		{
			base.PostAI();
		}
		public bool drawNewLines = false;
		public const float DurationFindLocation = 120f;
		public const float FindTarget = 10f;
		public const float PrepareSlingDuration = 25f;
		public const float SlingDuration = 120f;
		public const float PropellDuration = 50;
		public PlanetoidTrail primTrail;
		public bool runOnce = true;
		public override bool PreAI()
		{
			if(runOnce)
            {
				primTrail = new PlanetoidTrail(NPC, 14);
				SOTS.primitives.CreateTrail(primTrail);
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
				Vector2 goToPlayer = target.Center - NPC.Center;
				goToPlayer = goToPlayer.SafeNormalize(Vector2.Zero) * 400;
				NPC.ai[1] = NPC.Center.X + goToPlayer.X;
				NPC.ai[2] = NPC.Center.Y + goToPlayer.Y;
				if (Main.netMode == NetmodeID.Server)
					NPC.netUpdate = true;
			}
			toPlayer = new Vector2(NPC.ai[1], NPC.ai[2]) - NPC.Center;
			if (NPC.ai[0] < DurationFindLocation)
			{
                if (NPC.ai[0] > FindTarget)
				{
					NPC.velocity += toPlayer.SafeNormalize(Vector2.Zero).RotatedBy(MathHelper.ToRadians(75));
					NPC.velocity *= 0.8f;
					drawNewLines = true;
				}
			}
			else
			{
				if (NPC.ai[0] < DurationFindLocation + PrepareSlingDuration)
				{
					NPC.velocity -= toPlayer.SafeNormalize(Vector2.Zero).RotatedBy(MathHelper.ToRadians(90));
					NPC.velocity *= 0.825f;
					drawNewLines = false;
				}
                else
				{
					float windUp = (NPC.ai[0] - DurationFindLocation - PrepareSlingDuration) / SlingDuration;
					float sinusoid = (float)Math.Sin(MathHelper.ToRadians((NPC.ai[0] - DurationFindLocation - PrepareSlingDuration) * 2f)) * (1 - windUp) * 0.9f;
					NPC.velocity -= toPlayer.SafeNormalize(Vector2.Zero) * sinusoid;
					NPC.velocity *= 0.8f;
				}
                if(NPC.ai[0] > DurationFindLocation + PrepareSlingDuration + SlingDuration)
                {
					NPC.ai[0] = -PropellDuration;
					NPC.velocity += toPlayer.SafeNormalize(Vector2.Zero) * 12;
                }
			}
			if (NPC.velocity.X > 0)
				NPC.spriteDirection = 1;
			else
				NPC.spriteDirection = -1;
			NPC.position -= NPC.velocity;
			RegisterLines();
			NPC.velocity.X /= 0.93f;
			return true;
		}
        public override void FindFrame(int frameHeight)
        {
			float frameSpeed = 0.5f + (float)Math.Sqrt(NPC.velocity.Length());
			NPC.frameCounter += frameSpeed;
			if(NPC.frameCounter >= 5)
            {
				NPC.frameCounter -= 5;
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
			npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<SkipSoul>(), 1, 2, 2));
		}
		public List<GravityWellLine> GravityWell = new List<GravityWellLine>();
		public void RegisterLines()
		{
			Player target = Main.player[NPC.target];
			Vector2 toPlayer = target.Center - NPC.Center;
			for(int a = 0; a < 2; a++)
			{
				Vector2 position = NPC.Center - new Vector2(42, 0).RotatedBy(toPlayer.ToRotation());
				if (drawNewLines)
				{
					GravityWell.Add(new GravityWellLine(position));
					while (GravityWell.Count > 100)
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
						bool inRange = NPC.Distance(vector) < 160;
						if (inRange && NPC.ai[0] > DurationFindLocation + PrepareSlingDuration)
						{
							float stretch = (float)Math.Pow(1 - NPC.Distance(vector) / 160f, 4);
							Vector2 awayFromNPC = vector - NPC.Center;
							float stretchDist = 3f * stretch * (1 - windUp);
							if (NPC.Distance(vector) < 40)
							{
								stretchDist += 40 - NPC.Distance(vector);
								if (NPC.ai[0] < DurationFindLocation + PrepareSlingDuration + SlingDuration - 15 && NPC.ai[0] > DurationFindLocation + PrepareSlingDuration && i < GravityWell.Count - 1)
								{
									if (Vector2.Distance(GravityWell[i].Position, GravityWell[i + 1].Position) > 8)
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
								GravityWell[i].Position = Vector2.Lerp(vector, GravityWell[i].OriginalPosition, 0.004f);
							else if (NPC.ai[0] < 0 && inRange)
							{
								float stretch = (float)Math.Pow(1 - NPC.Distance(vector) / 160f, 4);
								GravityWell[i].Position += NPC.velocity * 0.6f * stretch;
								GravityWell[i].Position = Vector2.Lerp(GravityWell[i].Position, GravityWell[i].OriginalPosition, 0.0675f);
							}
							else
								GravityWell[i].Position = Vector2.Lerp(vector, GravityWell[i].OriginalPosition, 0.05f);
						}
					}
				}
				NPC.position += NPC.velocity * 0.5f;
			}
			primTrail.ConvertListToPoints(GravityWell);
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