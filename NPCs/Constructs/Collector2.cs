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
			npc.aiStyle = 0; 
			npc.lifeMax = 800;  
			npc.damage = 0; 
			npc.defense = 0;  
			npc.knockBackResist = 0.1f;
			npc.width = 86;
			npc.height = 64;
			Main.npcFrameCount[npc.type] = 1;  
			npc.value = 0;
			npc.npcSlots = 3f;
			npc.noGravity = true;
			npc.noTileCollide = true;
			npc.HitSound = SoundID.NPCHit4;
			npc.DeathSound = SoundID.NPCDeath14;
			npc.dontTakeDamage = true;
			npc.alpha = 255;
		}
		public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
		{
			if (runOnce)
				return false;
			DrawLightning(spriteBatch, drawColor, 0);
			DrawLightning(spriteBatch, drawColor, 1);
			DrawLightning(spriteBatch, drawColor, 2);
			DrawLightning(spriteBatch, drawColor, 3);
			Texture2D texture = Main.npcTexture[npc.type];
			Texture2D textureDrill = Mod.Assets.Request<Texture2D>("NPCs/Constructs/Collector2Drill").Value;
			Texture2D textureSpirit = Mod.Assets.Request<Texture2D>("NPCs/Constructs/Collector2Spirit").Value;
			Vector2 drawOrigin = new Vector2(textureDrill.Width * 0.5f, textureDrill.Height * 0.5f);
			Vector2 drawOrigin2 = new Vector2(textureSpirit.Width * 0.5f, textureSpirit.Height * 0.5f);
			for (int i = 0; i < 2; i++)
			{
				int direction = i * 2 - 1;
				float overrideRotation = MathHelper.ToRadians(55 * -direction);
				Vector2 fromBody = npc.Center + new Vector2(direction * 28, 10 + npc.ai[1] * 0.5f - npc.ai[2] * 0.5f).RotatedBy(npc.rotation);
				Vector2 drawPos = fromBody - Main.screenPosition + new Vector2(0f, npc.gfxOffY);
				spriteBatch.Draw(textureDrill, drawPos, null, drawColor * (1f - (npc.alpha / 255f)), npc.rotation + overrideRotation, drawOrigin, (npc.ai[1] - npc.ai[2]) * 0.015f, direction == 1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);
			}
			float spiritScale = npc.ai[3] / timeToChargeOrb;
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
				Vector2 drawPos = npc.Center - Main.screenPosition + new Vector2(0f, npc.gfxOffY + 4 * (1 - spiritScale) + 2);
				Main.spriteBatch.Draw(textureSpirit, drawPos + circular, null, color * (1f - (npc.alpha / 255f)), npc.rotation, drawOrigin2, spiritScale, SpriteEffects.None, 0f);
			}
			return true;
		}
        public override void PostDraw(SpriteBatch spriteBatch, Color drawColor)
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
					Vector2 fromBody = npc.Center + new Vector2(i * (72 - j * 24), -18 - j * 30).RotatedBy(npc.rotation) * engineExtendMult(j);
					Vector2 drawPos = fromBody - Main.screenPosition;
					spriteBatch.Draw(texture2, drawPos, null, drawColor * (1f - (npc.alpha / 255f)), npc.rotation + overrideRotation + MathHelper.ToRadians(i == -1 ? 180 : 0), drawOrigin, 1f, i == 1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);
					for (int k = 0; k < 7; k++)
					{
						Main.spriteBatch.Draw(texture3, drawPos + Main.rand.NextVector2Circular(1, 1), null, color * (1f - (npc.alpha / 255f)), npc.rotation + overrideRotation + MathHelper.ToRadians(i == -1 ? 180 : 0), drawOrigin, npc.scale, i == 1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);
					}
				}
			}
			base.PostDraw(spriteBatch, drawColor);
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
		public void DrawLightning(SpriteBatch spriteBatch, Color lightColor, int id)
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
				float scale = 0.4f + 0.4f * npc.scale * (trailPos.Length - k) / (float)trailPos.Length;
				scale *= 0.8f;
				if (trailPos[k] == Vector2.Zero)
				{
					return;
				}
				Vector2 drawPos = trailPos[k] - Main.screenPosition;
				Vector2 currentPos = trailPos[k];
				Vector2 betweenPositions = previousPosition - currentPos;
				Color color = new Color(100, 100, 100, 0) * (0.2f + ((trailPos.Length - k) / (float)trailPos.Length) * 0.4f);
				float max = betweenPositions.Length() / (4 * scale);
				for (int i = 0; i < max; i++)
				{
					drawPos = previousPosition + -betweenPositions * (i / max) - Main.screenPosition;
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
					Vector2 fromBody = new Vector2(i * (72 - j * 24), -18 - j * 30).RotatedBy(npc.rotation) * engineExtendMult(j);
					Vector2 center = npc.Center + new Vector2(36 * i, -10).RotatedBy(npc.rotation);
					runOnce = false;
					for (int a = 0; a < 10; a++)
					{
						float radius = (float)Math.Sin(MathHelper.ToRadians(a * 18)) * 3f;
						Vector2 pos = Vector2.Lerp(npc.Center + fromBody, center, a / 10f);
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
					Vector2 fromBody = center + new Vector2(i * (74 - j * 24), -18 - j * 30).RotatedBy(npc.rotation) * engineExtendMult(j);
					Vector2 dustVelo = new Vector2(7.2f, 0).RotatedBy(overrideRotation);
					int index = Dust.NewDust(fromBody + dustVelo * npc.scale + new Vector2(-4, -4), 0, 0, ModContent.DustType<CopyDust4>(), 0, 0, 0, new Color(167, 45, 225));
					Dust dust = Main.dust[index];
					dust.noGravity = true;
					dust.fadeIn = 0.1f;
					dust.velocity = dustVelo;
					dust.scale += 0.3f;
					dust.scale *= npc.scale;
					dust.alpha = npc.alpha;
				}
			}
		}
		public override void PostAI()
		{
			lerpVelo = Vector2.Lerp(lerpVelo, npc.velocity, 0.09f);
			npc.rotation = npc.velocity.X * 0.3f;
			GenDust(npc.Center);
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
			npc.TargetClosest(false);
			npc.spriteDirection = 1;
		}
        public override bool PreAI()
		{
			npc.TargetClosest(false);
			if(runOnce)
            {
				if(Main.netMode != NetmodeID.MultiplayerClient)
				{
					toPos = npc.Center;
					npc.netUpdate = true;
				}
				runOnce = false;
				for (int i = 0; i < 4;i++)
                {
					trailPos.Add(new Vector2[10]);
                }
			}
			npc.alpha = 0;
			npc.ai[0]++;
			if (npc.ai[0] <= 60)
			{
				npc.alpha = 0;
				Vector2 betweenPos = toPos + npc.Center + new Vector2(0, -600);
				betweenPos *= 0.5f;
				Vector2 circularLocation = new Vector2(0, -300).RotatedBy(MathHelper.ToRadians(-npc.ai[0] * 3));
				circularLocation.X *= 0.33f;
				Vector2 old = npc.Center;
				if(npc.ai[0] > 3)
					GenDust(npc.Center);
				npc.Center = circularLocation + betweenPos;
				if (npc.ai[0] > 3)
					GenDust(Vector2.Lerp(old, npc.Center, 0.5f));
				npc.velocity = new Vector2(0, 1).RotatedBy(MathHelper.ToRadians(-npc.ai[0] * 3));
			}
			else
            {
				npc.velocity *= 0f;
				if(npc.ai[1] < 60)
				{
					npc.ai[1] += 1f;
				}
				else if(npc.ai[3] < timeToChargeOrb)
                {
					npc.ai[3] += 1f;
					if(npc.ai[3] % 10 == 0)
					{
						SoundEngine.PlaySound(SoundID.Item, (int)npc.Center.X, (int)npc.Center.Y, 13, 1.2f);
					}
					float spiritScale = npc.ai[3] / timeToChargeOrb;
					if (spiritScale < 1f)
					{
						for (int k = 0; k < 180; k += 10)
						{
							Vector2 circularLocation = new Vector2(-40 * npc.scale, 0).RotatedBy(MathHelper.ToRadians(k));
							circularLocation += 0.5f * new Vector2(Main.rand.Next(-2, 3), Main.rand.Next(-2, 3));
							if (Main.rand.NextBool(30))
							{
								int index = Dust.NewDust(new Vector2(npc.Center.X + circularLocation.X - 4, npc.Center.Y + circularLocation.Y - 6) + new Vector2(-5), 0, 0, ModContent.DustType<CopyDust4>(), 0, 0, 0, new Color(167, 45, 225));
								Dust dust = Main.dust[index];
								dust.noGravity = true;
								dust.fadeIn = 0.1f;
								dust.velocity = -circularLocation * 0.07f;
								dust.scale += 0.1f;
								dust.scale *= 1f + 0.5f * spiritScale;
								dust.alpha = npc.alpha;
							}
						}
					}
				}
				else
				{
					if (npc.ai[2] < 60)
					{
						npc.ai[2] += 2f;
					}
					npc.ai[3]++;
					float ai3 = npc.ai[3] - timeToChargeOrb;
					float sinusoid = 1.5f * (float)Math.Sin(MathHelper.ToRadians(ai3 * 225f / 150f + 0.5f));
					npc.velocity = new Vector2(-sinusoid, 0).RotatedBy(MathHelper.ToRadians(-70));
					if(ai3 % 30 == 0 && ai3 < 100)
						SoundEngine.PlaySound(SoundID.Item, (int)npc.Center.X, (int)npc.Center.Y, 15, 0.3f + ai3 * 0.05f);
					if(ai3 == 95)
						SoundEngine.PlaySound(SoundID.Item, (int)npc.Center.X, (int)npc.Center.Y, 121, 1.3f, 0);
					if (ai3 > 150 && startRunning)
                    {
						for (int k = 0; k < 300; k++)
                        {
							npc.position += npc.velocity * 4f;
							GenDust(npc.Center);
						}
						startRunning = false; 
					}
					if(ai3 > 150)
                    {
						npc.active = false;
                    }
				}
			}
			SetUpTrails();
			Lighting.AddLight(npc.Center, (255 - npc.alpha) * 0.15f / 155f, (255 - npc.alpha) * 0.25f / 155f, (255 - npc.alpha) * 0.65f / 155f);
			return true;
		}
	}
}