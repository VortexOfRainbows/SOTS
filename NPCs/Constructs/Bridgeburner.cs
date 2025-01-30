using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.NPCs.Constructs
{
	public class Bridgeburner : ModNPC
	{
        public override void SetStaticDefaults()
		{
			Main.npcFrameCount[NPC.type] = 1;
            NPCID.Sets.NoMultiplayerSmoothingByType[NPC.type] = true;
            NPCID.Sets.MPAllowedEnemies[Type] = true;
		}
        public override void SetDefaults()
		{
			NPC.aiStyle = NPCAIStyleID.Unicorn;
			NPC.lifeMax = 500;  
			NPC.damage = 50; 
			NPC.defense = 50;  
			NPC.knockBackResist = 0.0f;
			NPC.width = 76;
			NPC.height = 98;
			NPC.value = 3330;
			NPC.npcSlots = 3f;
			NPC.boss = false;
			NPC.lavaImmune = false;
			NPC.noGravity = false;
			NPC.noTileCollide = false;
			NPC.netAlways = true;
			NPC.HitSound = SoundID.NPCHit4;
			NPC.DeathSound = SoundID.NPCDeath14;
			NPC.rarity = 5;
		}
        public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
			Player player = Main.player[NPC.target];
			Texture2D t = Terraria.GameContent.TextureAssets.Npc[Type].Value;
            Texture2D tHeadGlow = ModContent.Request<Texture2D>("SOTS/NPCs/Constructs/BridgeburnerHeadGlow").Value;
            Texture2D tArmFrontGlow = ModContent.Request<Texture2D>("SOTS/NPCs/Constructs/BridgeburnerArmFrontGlow").Value;
            Texture2D tHead = ModContent.Request<Texture2D>("SOTS/NPCs/Constructs/BridgeburnerHead").Value;
            Texture2D tArmFront = ModContent.Request<Texture2D>("SOTS/NPCs/Constructs/BridgeburnerArmFront").Value;
            Texture2D tLegFront = ModContent.Request<Texture2D>("SOTS/NPCs/Constructs/BridgeburnerLegFront").Value;
            Texture2D tArmBack = ModContent.Request<Texture2D>("SOTS/NPCs/Constructs/BridgeburnerArmBack").Value;
            Texture2D tLegBack = ModContent.Request<Texture2D>("SOTS/NPCs/Constructs/BridgeburnerLegBack").Value;
            Vector2 origin = t.Size() / 2;
			bool flip = NPC.spriteDirection == 1;
			Vector2 HeadOrigin = new Vector2(!flip ? 14 : tHead.Width - 14, 22);
			Vector2 ArmOrigin = new Vector2(!flip ? 36 : tArmFront.Width - 36, 10);
            Vector2 LegOrigin = new Vector2(!flip ? 14 : tLegFront.Width - 14, 8);
			Vector2 drawPos = NPC.Center - screenPos;
            float anim = MathHelper.ToRadians(NPC.localAI[0]);
			Vector2 leg1 = legPosition(anim);
			Vector2 leg2 = legPosition(anim + MathF.PI);
            Vector2 bobbing = Vector2.Zero;
            if (leg1.Y > 0)
			{
				bobbing.Y -= leg1.Y;
                leg1.Y = 0;
            }
            if (leg2.Y > 0)
			{
                bobbing.Y -= leg2.Y;
                leg2.Y = 0;
            }
			//leg1 = leg2 = bobbing= Vector2.Zero;
			float dir = NPC.spriteDirection;
			float armRot = NPC.ai[0] + (!flip ? MathF.PI + MathHelper.PiOver4 : -MathHelper.PiOver4);
            bobbing.Y *= 0.5f;
            spriteBatch.Draw(tArmBack, bobbing + drawPos + new Vector2(22 * dir, NPC.gfxOffY - 7), null, drawColor, armRot, ArmOrigin, NPC.scale, flip ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);
            spriteBatch.Draw(tLegBack, leg1 + drawPos + new Vector2(16 * dir, NPC.gfxOffY + 23), null, drawColor, 0, LegOrigin, NPC.scale, flip ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);
            spriteBatch.Draw(t, bobbing + drawPos + new Vector2(0, NPC.gfxOffY), null, drawColor, 0, origin, NPC.scale, flip ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);
            spriteBatch.Draw(tHead, bobbing + drawPos + new Vector2(2 * dir, NPC.gfxOffY - 29), null, drawColor, 0, HeadOrigin, NPC.scale, flip ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);
            spriteBatch.Draw(tHeadGlow, bobbing + drawPos + new Vector2(2 * dir, NPC.gfxOffY - 29), null, Color.White, 0, HeadOrigin, NPC.scale, flip ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);
            spriteBatch.Draw(tLegFront, leg2 + drawPos + new Vector2(-22 * dir, NPC.gfxOffY + 23), null, drawColor, 0, LegOrigin, NPC.scale, flip ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);
            spriteBatch.Draw(tArmFront, bobbing + drawPos + new Vector2(-26 * dir, NPC.gfxOffY - 7), null, drawColor, armRot, ArmOrigin, NPC.scale, flip ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);
            spriteBatch.Draw(tArmFrontGlow, bobbing + drawPos + new Vector2(-26 * dir, NPC.gfxOffY - 7), null, Color.White, armRot, ArmOrigin, NPC.scale, flip ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);
        }
        private Vector2 legPosition(float anim)
        {
            Vector2 legOffset = new Vector2(0, 4).RotatedBy(anim);
            legOffset.X *= 0.25f * NPC.spriteDirection;
			return legOffset;
        }

        private float walkCounter = 0; //Counter for animation progress
        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            return false;
        }
        public override void HitEffect(NPC.HitInfo hit)
		{
			if (Main.netMode == NetmodeID.Server)
				return;
			if (NPC.life <= 0)
			{
				for (int k = 0; k < 20; k++)
				{
					Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Lead, 2.5f * (float)hit.HitDirection, -2.5f, 0, default(Color), 0.7f);
				}
				//for(int i = 1; i < 8; i++)
				//	Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, ModGores.GoreType("Gores/NatureConstructGore" + i), 1f);
				//for(int i = 0; i < 9; i++)
				//	Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Main.rand.Next(61,64), 1f);	
			}
		}
		public override void FindFrame(int frameHeight) 
		{
			float speed = Math.Abs(NPC.velocity.X * 0.7f);
			if (speed > 1.67f)
				speed = 1.67f;
			else if (speed <= 0.1f)
            {
				speed = 0;
				NPC.frame.Y = 0;
            }
			NPC.frameCounter += speed;
			if (NPC.frameCounter > 10f) 
			{
				NPC.frame.Y = (NPC.frame.Y + frameHeight);
				if(NPC.frame.Y >= frameHeight * 3)
				{
					NPC.frame.Y = 0;
				}
				NPC.frameCounter = 0;
			}
		}
		public override void AI()
		{
			Player player = Main.player[NPC.target];
			Vector2 toPlayer = player.Center - NPC.Center;
			NPC.spriteDirection = NPC.direction;
			NPC.velocity.X *= 0.825f;
			if(NPC.velocity.Y < 0)
				NPC.velocity.Y *= 0.99f;
			NPC.localAI[0] += MathF.Sqrt(MathF.Abs(NPC.velocity.X)) * 7 + 1;
			NPC.ai[0] = SOTSUtils.AngularLerp(NPC.ai[0], toPlayer.ToRotation(), 0.04f);
        }
        public override void OnKill()
		{
			//int n = NPC.NewNPC(NPC.GetSource_Death(), (int)NPC.Center.X, (int)NPC.Center.Y, ModContent.NPCType<NatureSpirit>());
			//Main.npc[n].velocity.Y = -10f;
			//Main.npc[n].netUpdate = true;
		}
		public override void ModifyNPCLoot(NPCLoot npcLoot)
		{
			//npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<FragmentOfNature>(), 1, 4, 7));
		}
	}
}