using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Void;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.NPCs.Constructs
{
	public class ChaosRubble : ModNPC
	{
        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        { 
			Vector2 origin = new Vector2(NPC.width / 2, NPC.height / 2);
			Texture2D texture = Terraria.GameContent.TextureAssets.Npc[NPC.type].Value;
			for(int i = 0; i < 8; i++)
			{
				Vector2 circular = new Vector2(6, 0).RotatedBy(MathHelper.ToRadians(i * 45 - SOTSWorld.GlobalCounter));
				Color color = VoidPlayer.pastelAttempt(MathHelper.ToRadians(i * 45 + SOTSWorld.GlobalCounter));
				color.A = 0;
				spriteBatch.Draw((Texture2D)ModContent.Request<Texture2D>("SOTS/NPCs/Constructs/ChaosRubbleGlow"), NPC.Center - screenPos + new Vector2(0, NPC.gfxOffY) + circular, null, color * 0.4f, NPC.rotation, origin, NPC.scale, SpriteEffects.None, 0f);
			}
			spriteBatch.Draw(texture, NPC.Center - screenPos + new Vector2(0, NPC.gfxOffY), null, drawColor, NPC.rotation, origin, NPC.scale, SpriteEffects.None, 0f);
			spriteBatch.Draw((Texture2D)ModContent.Request<Texture2D>("SOTS/NPCs/Constructs/ChaosRubbleGlow"), NPC.Center - screenPos + new Vector2(0, NPC.gfxOffY), null, Color.White, NPC.rotation, origin, NPC.scale, SpriteEffects.None, 0f);
			return false;
		}
		public override void SetDefaults()
		{
            NPC.aiStyle =-1;  
            NPC.lifeMax = 1000;
            NPC.damage = 0;
            NPC.defense = 30;
            NPC.knockBackResist = 0.95f;
            NPC.width = 70;
            NPC.height = 70;
            NPC.value = 0;
            NPC.npcSlots = 1f;
            NPC.boss = false;
            NPC.lavaImmune = true;
            NPC.noGravity = false;
            NPC.noTileCollide = false;
            NPC.HitSound = SoundID.NPCHit4;
            NPC.DeathSound = SoundID.NPCDeath14;
            NPC.netAlways = true;
			NPC.dontTakeDamage = true;
		}
        public override bool PreKill()
        {
            return false;
        }
        public override void HitEffect(int hitDirection, double damage)
		{
			if (Main.netMode == NetmodeID.Server)
				return;
			if (NPC.life <= 0)
			{
				if (Main.netMode != NetmodeID.Server)
				{
					for (int k = 0; k < 30; k++)
					{
						Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Platinum, 2.5f * (float)hitDirection, -2.5f, 0, default(Color), 1.3f);
					}
					for (int i = 0; i < 10; i++)
						Gore.NewGore(NPC.GetSource_Death(), NPC.position + new Vector2(Main.rand.NextFloat(50), Main.rand.NextFloat(50)), NPC.velocity, Main.rand.Next(61, 64), 1f);
				}
			}
		}
		public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
        {
			NPC.lifeMax = (int)(NPC.lifeMax * 3 / 5);
			NPC.damage = 0;
        }
        public override bool PreAI()
		{
			NPC.dontTakeDamage = true;
			return base.PreAI();
		}
		Vector2 lastVelocity = Vector2.Zero;
        public override void AI()
        {
			NPC.ai[0]++;
			if(NPC.ai[0] > 120)
            {
				NPC.dontTakeDamage = false;
			}
			if(NPC.velocity.Y == 0)
			{
				if(NPC.ai[1] == -1)
				{
					SOTSUtils.PlaySound(SoundID.Item53, (int)NPC.Center.X, (int)NPC.Center.Y, 1.5f, 0.1f);
					float dampen = (Math.Abs(lastVelocity.Y) - 0.5f) * 0.8f;
					if (dampen < 0)
						dampen = 0;
					NPC.velocity.Y = -dampen;
					for (int k = 0; k < dampen * 3; k++)
					{
						Dust.NewDust(NPC.position + new Vector2(0, 60), NPC.width, 60 - NPC.height, DustID.Platinum, NPC.velocity.X * 0.5f, NPC.velocity.Y, 0, default(Color), 1.0f);
					}
				}
				NPC.ai[1] = 1;
			}
			else
            {
				NPC.ai[1] = -1;
            }
			NPC.TargetClosest(false);
			NPC.direction = 1;
			NPC.spriteDirection = 1;
			NPC.velocity.X *= 0.986f;
			if (Math.Abs(NPC.velocity.X) > 15)
				NPC.velocity.X *= 0.9f;
			NPC.rotation += NPC.velocity.X * 0.02f;
			lastVelocity = NPC.velocity;
        }
	}
}





















