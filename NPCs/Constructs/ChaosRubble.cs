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
		public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
		{
			Vector2 origin = new Vector2(npc.width / 2, npc.height / 2);
			Texture2D texture = Main.npcTexture[npc.type];
			for(int i = 0; i < 8; i++)
			{
				Vector2 circular = new Vector2(6, 0).RotatedBy(MathHelper.ToRadians(i * 45 - SOTSWorld.GlobalCounter));
				Color color = VoidPlayer.pastelAttempt(MathHelper.ToRadians(i * 45 + SOTSWorld.GlobalCounter));
				color.A = 0;
				Main.spriteBatch.Draw(ModContent.GetTexture("SOTS/NPCs/Constructs/ChaosRubbleGlow"), npc.Center - Main.screenPosition + new Vector2(0, npc.gfxOffY) + circular, null, color * 0.4f, npc.rotation, origin, npc.scale, SpriteEffects.None, 0f);
			}
			Main.spriteBatch.Draw(texture, npc.Center - Main.screenPosition + new Vector2(0, npc.gfxOffY), null, drawColor, npc.rotation, origin, npc.scale, SpriteEffects.None, 0f);
			Main.spriteBatch.Draw(ModContent.GetTexture("SOTS/NPCs/Constructs/ChaosRubbleGlow"), npc.Center - Main.screenPosition + new Vector2(0, npc.gfxOffY), null, Color.White, npc.rotation, origin, npc.scale, SpriteEffects.None, 0f);
			return false;
		}
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Chaos Construct Chassis");
		}
		public override void SetDefaults()
		{
            npc.aiStyle = -1;  
            npc.lifeMax = 1000;
            npc.damage = 0;
            npc.defense = 30;
            npc.knockBackResist = 0.95f;
            npc.width = 70;
            npc.height = 70;
            npc.value = 0;
            npc.npcSlots = 1f;
            npc.boss = false;
            npc.lavaImmune = true;
            npc.noGravity = false;
            npc.noTileCollide = false;
            npc.HitSound = SoundID.NPCHit4;
            npc.DeathSound = SoundID.NPCDeath14;
            npc.netAlways = true;
			npc.dontTakeDamage = true;
		}
        public override bool PreNPCLoot()
        {
            return false;
        }
        public override void HitEffect(int hitDirection, double damage)
		{
			if (npc.life <= 0)
			{
				if (Main.netMode != NetmodeID.Server)
				{
					for (int k = 0; k < 30; k++)
					{
						Dust.NewDust(npc.position, npc.width, npc.height, DustID.Platinum, 2.5f * (float)hitDirection, -2.5f, 0, default(Color), 1.3f);
					}
					for (int i = 0; i < 10; i++)
						Gore.NewGore(npc.position + new Vector2(Main.rand.NextFloat(50), Main.rand.NextFloat(50)), npc.velocity, Main.rand.Next(61, 64), 1f);
				}
			}
		}
		public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
        {
			npc.lifeMax = 1200;
			npc.damage = 0;
        }
        public override bool PreAI()
		{
			npc.dontTakeDamage = true;
			return base.PreAI();
		}
		Vector2 lastVelocity = Vector2.Zero;
        public override void AI()
        {
			npc.ai[0]++;
			if(npc.ai[0] > 30)
            {
				npc.dontTakeDamage = false;
			}
			if(npc.velocity.Y == 0)
			{
				if(npc.ai[1] == -1)
				{
					Main.PlaySound(SoundID.Item, (int)npc.Center.X, (int)npc.Center.Y, 53, 1.5f, 0.1f);
					float dampen = (Math.Abs(lastVelocity.Y) - 0.5f) * 0.8f;
					if (dampen < 0)
						dampen = 0;
					npc.velocity.Y = -dampen;
					for (int k = 0; k < dampen * 3; k++)
					{
						Dust.NewDust(npc.position + new Vector2(0, 60), npc.width, 60 - npc.height, DustID.Platinum, npc.velocity.X * 0.5f, npc.velocity.Y, 0, default(Color), 1.0f);
					}
				}
				npc.ai[1] = 1;
			}
			else
            {
				npc.ai[1] = -1;
            }
			npc.TargetClosest(false);
			npc.direction = 1;
			npc.spriteDirection = 1;
			npc.velocity.X *= 0.986f;
			if (Math.Abs(npc.velocity.X) > 15)
				npc.velocity.X *= 0.9f;
			npc.rotation += npc.velocity.X * 0.02f;
			lastVelocity = npc.velocity;
        }
	}
}





















