using System;
using System.Linq.Expressions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
namespace SOTS.NPCs.Constructs
{
	public class Collector : ModNPC
	{
		int timer = 0;
		int ai1 = 0;
		float dir = 0f;
		public override void SetStaticDefaults()
		{
			
			DisplayName.SetDefault("Collector");
		}
		public override void SetDefaults()
		{
			npc.aiStyle = 0; 
			npc.lifeMax = 420;  
			npc.damage = 0; 
			npc.defense = 0;  
			npc.knockBackResist = 0.1f;
			npc.width = 102;
			npc.height = 58;
			Main.npcFrameCount[npc.type] = 1;  
			npc.value = 7075;
			npc.npcSlots = 3f;
			npc.noGravity = true;
			npc.noTileCollide = true;
			npc.netAlways = true;
			npc.alpha = 255;
			npc.HitSound = SoundID.NPCHit4;
			npc.DeathSound = SoundID.NPCDeath14;
		}
		public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
		{
			Texture2D texture = Main.npcTexture[npc.type];
			Texture2D texture4 = mod.GetTexture("NPCs/Constructs/CollectorDrill");
			Vector2 drawOrigin = new Vector2(texture4.Width * 0.5f, texture4.Height * 0.5f);

			for (int i = 0; i < 2; i++)
			{
				int direction = i * 2 - 1;
				float overrideRotation = MathHelper.ToRadians(35 * -direction);
				Vector2 fromBody = npc.Center + new Vector2(direction * 24, 10 + npc.ai[1] * 0.35f - npc.ai[2] * 0.35f).RotatedBy(npc.rotation);
				Vector2 drawPos = fromBody - Main.screenPosition + new Vector2(0f, npc.gfxOffY);
				spriteBatch.Draw(texture4, drawPos, null, drawColor, npc.rotation + overrideRotation, drawOrigin, (npc.ai[1] - npc.ai[2]) * 0.009f, direction == 1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);
			}
			return true;
		}
        public override void PostDraw(SpriteBatch spriteBatch, Color drawColor)
		{
			Texture2D texture2 = mod.GetTexture("NPCs/Constructs/CollectorBooster");
			Vector2 drawOrigin = new Vector2(texture2.Width * 0.5f, texture2.Height * 0.5f);
			Texture2D texture3 = mod.GetTexture("NPCs/Constructs/CollectorBoosterFill");
			Color color = new Color(100, 100, 100, 0);
			for (int i = 0; i < 2; i++)
			{
				int direction = i * 2 - 1;
				Vector2 rotationOrigin = new Vector2(-2.75f * -direction, 6f) - npc.velocity * 10f;
				float overrideRotation = rotationOrigin.ToRotation() - MathHelper.ToRadians(90);
				Vector2 fromBody = npc.Center + new Vector2(direction * (npc.width/2 - 4), -6).RotatedBy(npc.rotation);
				Vector2 drawPos = fromBody - Main.screenPosition + new Vector2(0f, npc.gfxOffY);
				for (int k = 0; k < 7; k++)
				{
					float x = Main.rand.Next(-10, 11) * 0.25f;
					float y = Main.rand.Next(-10, 11) * 0.25f;
					Main.spriteBatch.Draw(texture3, new Vector2( drawPos.X + x, drawPos.Y + y ), null, color * (1f - (npc.alpha / 255f)), npc.rotation + overrideRotation, drawOrigin, npc.scale, SpriteEffects.None, 0f);
				}
				spriteBatch.Draw(texture2, drawPos, null, drawColor, npc.rotation + overrideRotation, drawOrigin, 1f, SpriteEffects.None, 0f);
			}
			base.PostDraw(spriteBatch, drawColor);
        }
        public override void HitEffect(int hitDirection, double damage)
		{
			if (npc.life <= 0)
			{
				for (int k = 0; k < 20; k++)
				{
					Dust.NewDust(npc.position, npc.width, npc.height, mod.DustType("AvaritianDust"), 2.5f * (float)hitDirection, -2.5f, 0, default(Color), 0.7f);
				}
			}
		}
		public override void PostAI()
		{
			npc.rotation = npc.velocity.X * 0.3f;
			npc.alpha = 0;
			for (int i = 0; i < 2; i++)
			{
				int direction = i * 2 - 1;
				Vector2 rotationOrigin = new Vector2(-2.75f * -direction, 6f) - npc.velocity * 10f;
				float overrideRotation = rotationOrigin.ToRotation();
				Vector2 dustVelo = new Vector2(7.2f, 0).RotatedBy(overrideRotation);
				Vector2 fromBody = npc.Center + new Vector2(direction * (npc.width / 2 - 4), -6).RotatedBy(npc.rotation);
				int index = Dust.NewDust(fromBody + dustVelo * 1.5f * npc.scale + new Vector2(-4, -4), 0, 0, mod.DustType("CopyDust2"), 0, 0, 0, Color.White);
				Dust dust = Main.dust[index];
				dust.noGravity = true;
				dust.fadeIn = 0.1f;
				dust.velocity = dustVelo;
				dust.scale += 0.3f;
				dust.scale *= npc.scale;
				dust.alpha = npc.alpha;
			}
			base.PostAI();
		}
        public override bool CheckActive()
        {
			return false;
        }
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
			Lighting.AddLight(npc.Center, (255 - npc.alpha) * 0.15f / 155f, (255 - npc.alpha) * 0.25f / 155f, (255 - npc.alpha) * 0.65f / 155f);
			if(runOnce)
            {
				runOnce = false;
				toPos = npc.Center;
				npc.position += new Vector2(0, -600);
			}
			if(npc.ai[0] < 180)
			{
				npc.ai[0]++;
				Vector2 betweenPos = toPos + npc.Center + new Vector2(0, -600);
				betweenPos /= 2f;
				Vector2 circularLocation = new Vector2(0, -300).RotatedBy(MathHelper.ToRadians(-npc.ai[0]));
				circularLocation.X /= 2f;
				npc.position = circularLocation + betweenPos - new Vector2(npc.width / 2, npc.height / 2);
				npc.velocity = new Vector2(0, 1).RotatedBy(MathHelper.ToRadians(-npc.ai[0]));
			}
			else
            {
				npc.velocity *= 0f;
				if(npc.ai[1] < 100)
				{
					npc.ai[1] += 2;
				}
				else if(npc.ai[3] < 100)
                {
					npc.ai[3]++;
				}
				else if(npc.ai[2] < 100)
				{
					npc.ai[2]++;
                }
				else
                {
					npc.ai[3]++;
					float ai3 = npc.ai[3] - 100;
                }
            }
			return true;
		}
	}
}