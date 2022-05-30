using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System.IO;
using SOTS.Dusts;
using SOTS.Void;
using System;
using System.Collections.Generic;

namespace SOTS.NPCs.Constructs
{
	public class Collector2 : ModNPC
	{
		public override void SendExtraAI(BinaryWriter writer)
		{
			writer.Write(toPos.X);
			writer.Write(toPos.Y);
		}
		public override void ReceiveExtraAI(BinaryReader reader)
		{
			toPos.X = reader.ReadSingle();
			toPos.Y = reader.ReadSingle();
		}
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Collector"); //mr steal yo kill is back!
		}
		public override void SetDefaults()
		{
			NPC.aiStyle =0; 
			NPC.lifeMax = 800;  
			NPC.damage = 0; 
			NPC.defense = 0;  
			NPC.knockBackResist = 0.1f;
			NPC.width = 86;
			NPC.height = 64;
			Main.npcFrameCount[NPC.type] = 1;  
			NPC.value = 0;
			NPC.npcSlots = 3f;
			NPC.noGravity = true;
			NPC.noTileCollide = true;
			NPC.HitSound = SoundID.NPCHit4;
			NPC.DeathSound = SoundID.NPCDeath14;
			NPC.dontTakeDamage = true;
			NPC.alpha = 255;
		}
        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
			if (runOnce)
				return false;
			DrawLightning(spriteBatch, screenPos, drawColor, 0);
			DrawLightning(spriteBatch, screenPos, drawColor, 1);
			DrawLightning(spriteBatch, screenPos, drawColor, 2);
			DrawLightning(spriteBatch, screenPos, drawColor, 3);
			Texture2D texture = Terraria.GameContent.TextureAssets.Npc[NPC.type].Value;
			Texture2D textureDrill = Mod.Assets.Request<Texture2D>("NPCs/Constructs/Collector2Drill").Value;
			Texture2D textureSpirit = Mod.Assets.Request<Texture2D>("NPCs/Constructs/Collector2Spirit").Value;
			Vector2 drawOrigin = new Vector2(textureDrill.Width * 0.5f, textureDrill.Height * 0.5f);
			Vector2 drawOrigin2 = new Vector2(textureSpirit.Width * 0.5f, textureSpirit.Height * 0.5f);
			for (int i = 0; i < 2; i++)
			{
				int direction = i * 2 - 1;
				float overrideRotation = MathHelper.ToRadians(55 * -direction);
				Vector2 fromBody = NPC.Center + new Vector2(direction * 28, 10 + NPC.ai[1] * 0.5f - NPC.ai[2] * 0.5f).RotatedBy(NPC.rotation);
				Vector2 drawPos = fromBody - screenPos + new Vector2(0f, NPC.gfxOffY);
				spriteBatch.Draw(textureDrill, drawPos, null, drawColor * (1f - (NPC.alpha / 255f)), NPC.rotation + overrideRotation, drawOrigin, (NPC.ai[1] - NPC.ai[2]) * 0.015f, direction == 1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);
			}
			float spiritScale = NPC.ai[3] / timeToChargeOrb;
			if(spiritScale > 1)
            {
				spiritScale = 1;
            }
			spiritScale *= 0.75f;
			Color color = new Color(100, 100, 100, 0);
			for (int k = 0; k < 7; k++)
			{
				Vector2 circular = new Vector2(2, 0).RotatedBy(MathHelper.ToRadians(k * 60 + Main.GameUpdateCount));
				if (k != 0)
				{
					color = VoidPlayer.pastelAttempt(MathHelper.ToRadians(k * 60));
					color.A = 0;
				}
				else
					circular *= 0f;
				Vector2 drawPos = NPC.Center - screenPos + new Vector2(0f, NPC.gfxOffY + 4 * (1 - spiritScale) + 2);
				Main.spriteBatch.Draw(textureSpirit, drawPos + circular, null, color * (1f - (NPC.alpha / 255f)), NPC.rotation, drawOrigin2, spiritScale, SpriteEffects.None, 0f);
			}
			return true;
		}
        public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
			if (runOnce)
				return;
			Texture2D texture2 = Mod.Assets.Request<Texture2D>("NPCs/Constructs/Collector2Booster").Value;
			Texture2D texture3 = Mod.Assets.Request<Texture2D>("NPCs/Constructs/Collector2BoosterEffect").Value;
			Vector2 drawOrigin = new Vector2(texture2.Width * 0.5f, texture2.Height / 2);
			Color color = new Color(100, 100, 100, 0);
			for (int i = -1; i <= 1; i += 2)
			{
				for(int j = 0; j < 2; j ++)
				{
					float overrideRotation = engineRotation(i, j).ToRotation();
					Vector2 fromBody = NPC.Center + new Vector2(i * (72 - j * 24), -18 - j * 30).RotatedBy(NPC.rotation) * engineExtendMult(j);
					Vector2 drawPos = fromBody - screenPos;
					spriteBatch.Draw(texture2, drawPos, null, drawColor * (1f - (NPC.alpha / 255f)), NPC.rotation + overrideRotation + MathHelper.ToRadians(i == -1 ? 180 : 0), drawOrigin, 1f, i == 1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);
					for (int k = 0; k < 7; k++)
					{
						Main.spriteBatch.Draw(texture3, drawPos + Main.rand.NextVector2Circular(1, 1), null, color * (1f - (NPC.alpha / 255f)), NPC.rotation + overrideRotation + MathHelper.ToRadians(i == -1 ? 180 : 0), drawOrigin, NPC.scale, i == 1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);
					}
				}
			}
		}
		Vector2 lerpVelo = Vector2.Zero;
		public Vector2 engineRotation(int i, int j)
		{
			return new Vector2((1f + 1f * j) * i, 6f) - lerpVelo * 7;
		}
		public float engineExtendMult(int j)
        {
			return 1.5f;
        }
		public void DrawLightning(SpriteBatch spriteBatch, Vector2 screenPos, Color lightColor, int id)
		{
			if (runOnce)
				return;
			Texture2D texture = Mod.Assets.Request<Texture2D>("Projectiles/Otherworld/ThunderColumn").Value;
			Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
			Vector2[] trailPos = this.trailPos[id];
			Vector2 previousPosition = trailPos[0];
			if (previousPosition == Vector2.Zero)
			{
				return;
			}
			for (int k = 0; k < trailPos.Length; k++)
			{
				float scale = 0.4f + 0.4f * NPC.scale * (trailPos.Length - k) / (float)trailPos.Length;
				scale *= 0.8f;
				if (trailPos[k] == Vector2.Zero)
				{
					return;
				}
				Vector2 drawPos = trailPos[k] - screenPos;
				Vector2 currentPos = trailPos[k];
				Vector2 betweenPositions = previousPosition - currentPos;
				Color color = new Color(100, 100, 100, 0) * (0.2f + ((trailPos.Length - k) / (float)trailPos.Length) * 0.4f);
				float max = betweenPositions.Length() / (4 * scale);
				for (int i = 0; i < max; i++)
				{
					drawPos = previousPosition + -betweenPositions * (i / max) - screenPos;
					for (int j = 0; j < 4; j++)
					{
						spriteBatch.Draw(texture, drawPos + Main.rand.NextVector2Circular(j, j), null, color, betweenPositions.ToRotation() + MathHelper.ToRadians(90), drawOrigin, scale, SpriteEffects.None, 0f);
					}
				}
				previousPosition = currentPos;
			}
			return;
		}
		List<Vector2[]> trailPos = new List<Vector2[]>();
		public void SetUpTrails()
		{
			int num = 0;
			for (int i = -1; i <= 1; i += 2)
			{
				for (int j = 0; j < 2; j++)
				{
					Vector2 fromBody = new Vector2(i * (72 - j * 24), -18 - j * 30).RotatedBy(NPC.rotation) * engineExtendMult(j);
					Vector2 center = NPC.Center + new Vector2(36 * i, -10).RotatedBy(NPC.rotation);
					runOnce = false;
					for (int a = 0; a < 10; a++)
					{
						float radius = (float)Math.Sin(MathHelper.ToRadians(a * 18)) * 3f;
						Vector2 pos = Vector2.Lerp(NPC.Center + fromBody, center, a / 10f);
						trailPos[num][a] = pos + Main.rand.NextVector2CircularEdge(radius, radius);
					}
					num++;
				}
			}
		}
		public void GenDust(Vector2 center)
		{
			for (int i = -1; i <= 1; i += 2)
			{
				for (int j = 0; j < 2; j++)
				{
					float overrideRotation = engineRotation(i, j).ToRotation();
					Vector2 fromBody = center + new Vector2(i * (74 - j * 24), -18 - j * 30).RotatedBy(NPC.rotation) * engineExtendMult(j);
					Vector2 dustVelo = new Vector2(7.2f, 0).RotatedBy(overrideRotation);
					int index = Dust.NewDust(fromBody + dustVelo * NPC.scale + new Vector2(-4, -4), 0, 0, ModContent.DustType<CopyDust4>(), 0, 0, 0, new Color(167, 45, 225));
					Dust dust = Main.dust[index];
					dust.noGravity = true;
					dust.fadeIn = 0.1f;
					dust.velocity = dustVelo;
					dust.scale += 0.3f;
					dust.scale *= NPC.scale;
					dust.alpha = NPC.alpha;
				}
			}
		}
		public override void PostAI()
		{
			lerpVelo = Vector2.Lerp(lerpVelo, NPC.velocity, 0.09f);
			NPC.rotation = NPC.velocity.X * 0.3f;
			GenDust(NPC.Center);
			base.PostAI();
		}
        public override bool CheckActive()
        {
			return false;
        }
		public float timeToChargeOrb = 90;
		public bool startRunning = true;
        bool runOnce = true;
		Vector2 toPos = Vector2.Zero;
        public override void AI()
		{
			NPC.TargetClosest(false);
			NPC.spriteDirection = 1;
		}
        public override bool PreAI()
		{
			NPC.TargetClosest(false);
			if(runOnce)
            {
				if(Main.netMode != NetmodeID.MultiplayerClient)
				{
					toPos = NPC.Center;
					NPC.netUpdate = true;
				}
				runOnce = false;
				for (int i = 0; i < 4;i++)
                {
					trailPos.Add(new Vector2[10]);
                }
			}
			NPC.alpha = 0;
			NPC.ai[0]++;
			if (NPC.ai[0] <= 60)
			{
				NPC.alpha = 0;
				Vector2 betweenPos = toPos + NPC.Center + new Vector2(0, -600);
				betweenPos *= 0.5f;
				Vector2 circularLocation = new Vector2(0, -300).RotatedBy(MathHelper.ToRadians(-NPC.ai[0] * 3));
				circularLocation.X *= 0.33f;
				Vector2 old = NPC.Center;
				if(NPC.ai[0] > 3)
					GenDust(NPC.Center);
				NPC.Center = circularLocation + betweenPos;
				if (NPC.ai[0] > 3)
					GenDust(Vector2.Lerp(old, NPC.Center, 0.5f));
				NPC.velocity = new Vector2(0, 1).RotatedBy(MathHelper.ToRadians(-NPC.ai[0] * 3));
			}
			else
            {
				NPC.velocity *= 0f;
				if(NPC.ai[1] < 60)
				{
					NPC.ai[1] += 1f;
				}
				else if(NPC.ai[3] < timeToChargeOrb)
                {
					NPC.ai[3] += 1f;
					if(NPC.ai[3] % 10 == 0)
					{
						SOTSUtils.PlaySound(SoundID.Item13, (int)NPC.Center.X, (int)NPC.Center.Y, 1.2f);
					}
					float spiritScale = NPC.ai[3] / timeToChargeOrb;
					if (spiritScale < 1f)
					{
						for (int k = 0; k < 180; k += 10)
						{
							Vector2 circularLocation = new Vector2(-40 * NPC.scale, 0).RotatedBy(MathHelper.ToRadians(k));
							circularLocation += 0.5f * new Vector2(Main.rand.Next(-2, 3), Main.rand.Next(-2, 3));
							if (Main.rand.NextBool(30))
							{
								int index = Dust.NewDust(new Vector2(NPC.Center.X + circularLocation.X - 4, NPC.Center.Y + circularLocation.Y - 6) + new Vector2(-5), 0, 0, ModContent.DustType<CopyDust4>(), 0, 0, 0, new Color(167, 45, 225));
								Dust dust = Main.dust[index];
								dust.noGravity = true;
								dust.fadeIn = 0.1f;
								dust.velocity = -circularLocation * 0.07f;
								dust.scale += 0.1f;
								dust.scale *= 1f + 0.5f * spiritScale;
								dust.alpha = NPC.alpha;
							}
						}
					}
				}
				else
				{
					if (NPC.ai[2] < 60)
					{
						NPC.ai[2] += 2f;
					}
					NPC.ai[3]++;
					float ai3 = NPC.ai[3] - timeToChargeOrb;
					float sinusoid = 1.5f * (float)Math.Sin(MathHelper.ToRadians(ai3 * 225f / 150f + 0.5f));
					NPC.velocity = new Vector2(-sinusoid, 0).RotatedBy(MathHelper.ToRadians(-70));
					if(ai3 % 30 == 0 && ai3 < 100)
						SOTSUtils.PlaySound(SoundID.Item15, (int)NPC.Center.X, (int)NPC.Center.Y, 0.3f + ai3 * 0.05f);
					if(ai3 == 95)
						SOTSUtils.PlaySound(SoundID.Item121, (int)NPC.Center.X, (int)NPC.Center.Y, 1.3f, 0);
					if (ai3 > 150 && startRunning)
                    {
						for (int k = 0; k < 300; k++)
                        {
							NPC.position += NPC.velocity * 4f;
							GenDust(NPC.Center);
						}
						startRunning = false; 
					}
					if(ai3 > 150)
                    {
						NPC.active = false;
                    }
				}
			}
			SetUpTrails();
			Lighting.AddLight(NPC.Center, (255 - NPC.alpha) * 0.15f / 155f, (255 - NPC.alpha) * 0.25f / 155f, (255 - NPC.alpha) * 0.65f / 155f);
			return true;
		}
	}
}