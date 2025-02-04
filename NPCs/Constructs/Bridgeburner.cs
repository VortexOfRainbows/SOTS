using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using SOTS.Items.Fragments;
using SOTS.Projectiles.AbandonedVillage;
using Terraria;
using Terraria.GameContent.ItemDropRules;
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
			NPC.aiStyle = NPCAIStyleID.Fighter;
            NPC.lifeMax = 3250;
            NPC.damage = 60;
            NPC.defense = 40;
            NPC.knockBackResist = 0.0f;
			NPC.width = 76;
			NPC.height = 98;
			NPC.value = Item.buyPrice(0, 6, 0, 0);
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
		private Vector2 armPosLeft => new Vector2(22 * NPC.spriteDirection, NPC.gfxOffY - 7);
		private Vector2 armPosRight => new Vector2(-26 * NPC.spriteDirection, NPC.gfxOffY - 7);
        public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
			Player player = Main.player[NPC.target];
			Vector2 toPlayer = (player.Center - NPC.Center).SNormalize();
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
            float anim = NPC.localAI[0];
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
            if (screenPos != Main.screenPosition && Main.netMode != NetmodeID.Server)
            {
				NPC.localAI[3] = 3 * MathHelper.PiOver4;
				NPC.localAI[2] = 3 * MathHelper.PiOver4;
            }
            //leg1 = leg2 = bobbing= Vector2.Zero;
            float dir = NPC.spriteDirection;
			float armRot = NPC.localAI[3] + (!flip ? MathF.PI + MathHelper.PiOver4 : -MathHelper.PiOver4);
			float armRot2 = NPC.localAI[2] + (!flip ? MathF.PI + MathHelper.PiOver4 : -MathHelper.PiOver4);
			Vector2 armLeftRecoil = -toPlayer * fireRecoilLeft;
			Vector2 armRightRecoil = -toPlayer * fireRecoilRight;
            bobbing.Y *= 0.5f;
            spriteBatch.Draw(tArmBack, armLeftRecoil + bobbing + drawPos + armPosLeft, null, drawColor, armRot2, ArmOrigin, NPC.scale, flip ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);
            spriteBatch.Draw(tLegBack, leg1 + drawPos + new Vector2(16 * dir, NPC.gfxOffY + 23), null, drawColor, 0, LegOrigin, NPC.scale, flip ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);
            spriteBatch.Draw(t, bobbing + drawPos + new Vector2(0, NPC.gfxOffY), null, drawColor, 0, origin, NPC.scale, flip ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);
            spriteBatch.Draw(tHead, bobbing + drawPos + new Vector2(4 * dir, NPC.gfxOffY - 29), null, drawColor, 0, HeadOrigin, NPC.scale, flip ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);
            spriteBatch.Draw(tHeadGlow, bobbing + drawPos + new Vector2(2 * dir, NPC.gfxOffY - 29), null, Color.White, 0, HeadOrigin, NPC.scale, flip ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);
            spriteBatch.Draw(tLegFront, leg2 + drawPos + new Vector2(-22 * dir, NPC.gfxOffY + 23), null, drawColor, 0, LegOrigin, NPC.scale, flip ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);
            spriteBatch.Draw(tArmFront, armRightRecoil + bobbing + drawPos + armPosRight, null, drawColor, armRot, ArmOrigin, NPC.scale, flip ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);
            spriteBatch.Draw(tArmFrontGlow, armRightRecoil + bobbing + drawPos + armPosRight, null, Color.White, armRot, ArmOrigin, NPC.scale, flip ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);
        }
        private Vector2 legPosition(float anim)
        {
            Vector2 legOffset = new Vector2(0, 4).RotatedBy(anim);
            legOffset.X *= 0.25f * NPC.spriteDirection;
			return legOffset;
        }
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
				for(int i = 1; i <= 12; i++)
				{
					Vector2 offset = Vector2.Zero;
					if (i == 1)
						offset += new Vector2(NPC.width * 0.5f, 0);
                    if (i == 2)
                        offset += new Vector2(0, NPC.height * 0.4f);
                    if (i == 3)
                        offset += new Vector2(NPC.width * 0.5f, NPC.height * 0.4f);
                    if (i == 4)
                        offset += new Vector2(NPC.width * 0.65f, NPC.height * 0.6f);
                    if (i == 5)
                        offset += new Vector2(NPC.width * 0.65f, NPC.height * 0.75f);
                    if (i == 6)
                        offset += new Vector2(NPC.width * 0.25f, NPC.height * 0.6f);
                    if (i == 7)
                        offset += new Vector2(NPC.width * 0.25f, NPC.height * 0.75f);
                    if (i == 8)
                        offset += new Vector2(NPC.width * 0.25f, NPC.height * 0.25f);
                    if (i == 9)
                        offset += new Vector2(NPC.width * 0.8f, NPC.height * 0.1f);
                    if (i == 10)
                        offset += new Vector2(NPC.width * 0.7f, NPC.height * 0.5f);
                    if (i == 12)
                        offset += new Vector2(NPC.width * 0.1f, NPC.height * 0.5f);
                    Gore.NewGore(NPC.GetSource_Death(), NPC.position + offset, NPC.velocity, ModGores.GoreType($"Gores/BridgeburnerGore{i}"), 1f);
                }
                for (int i = 0; i < 9; i++)
					Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Main.rand.Next(61,64), 1f);	
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
		private float fireRecoilLeft = 0;
		private float fireRecoilRight = 0;
		public void FireLaserAtPlayer(int type = 0)
        {
			ref float i = ref (type == 0 ? ref NPC.localAI[3] : ref NPC.localAI[2]);
			Vector2 pos = NPC.Center + (type == 0 ? armPosRight : armPosLeft);
			Vector2 dir = new Vector2(1, 0).RotatedBy(i);
            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                Projectile.NewProjectile(NPC.GetSource_FromAI(), pos + new Vector2(52, 0).RotatedBy(i), dir, ModContent.ProjectileType<BridgeburnerLaser>(), NPC.GetBaseDamage() / 2, 0, Main.myPlayer, NPC.Center.X + dir.X * 40, NPC.Center.Y + dir.Y * 40);
            }
			float rad = MathHelper.ToRadians(5);
            i -= rad * NPC.spriteDirection;
			if (type == 0)
                fireRecoilRight += 10;
			else
                fireRecoilLeft += 10;
			NPC.velocity.X -= dir.X;
        }
		public override void AI()
        {
            NPC.TargetClosest(true);
			Player player = Main.player[NPC.target];
            Vector2 toPlayer = player.Center - NPC.Center;
			if(toPlayer.Length() < 2400)
				NPC.DiscourageDespawn(600);
			NPC.spriteDirection = NPC.direction;
            NPC.velocity.X *= 0.825f;
			if(NPC.velocity.Y < 0)
				NPC.velocity.Y *= 0.9f;
			NPC.localAI[0] += MathHelper.ToRadians(MathF.Sqrt(MathF.Abs(NPC.velocity.X)) * 7.5f);
			NPC.localAI[0] = MathHelper.WrapAngle(NPC.localAI[0]);
            NPC.localAI[1]++;
			bool canSeePlayer = Collision.CanHitLine(player.position, player.width, player.height, NPC.position, NPC.width, NPC.height);
			if(!canSeePlayer)
			{
				if (NPC.localAI[1] > 0)
					NPC.localAI[1]--;
			}
			if (NPC.localAI[1] > 240)
            {
                NPC.localAI[0] = SOTSUtils.AngularLerp(NPC.localAI[0], MathHelper.ToRadians(90), 0.04f);
                NPC.aiStyle = -1;
                NPC.velocity.X *= 0.6f;
				if (NPC.localAI[1] % 30 == 0 && NPC.localAI[1] > 300)
                {
					FireLaserAtPlayer((int)NPC.localAI[1] / 30 % 2);
                }
				if (NPC.localAI[1] > 550)
                    NPC.localAI[1] = -60;
				Vector2 toPlayerLeft = player.Center - NPC.Center - armPosLeft;
				Vector2 toPlayerRight = player.Center - NPC.Center - armPosRight;
                NPC.localAI[3] = SOTSUtils.AngularLerp(NPC.localAI[3], toPlayerRight.ToRotation(), 0.035f);
                NPC.localAI[2] = SOTSUtils.AngularLerp(NPC.localAI[2], toPlayerLeft.ToRotation(), 0.035f);
            }
			else
			{
				NPC.aiStyle = NPCAIStyleID.Unicorn;

                NPC.localAI[3] = SOTSUtils.AngularLerp(NPC.localAI[3], MathHelper.ToRadians(90 - 10 * NPC.spriteDirection + MathF.Sin(NPC.localAI[0]) * 20), 0.04f);
                NPC.localAI[2] = SOTSUtils.AngularLerp(NPC.localAI[2], MathHelper.ToRadians(90 - 10 * NPC.spriteDirection - MathF.Sin(NPC.localAI[0]) * 20), 0.04f);
            }
			fireRecoilLeft *= 0.925f;
			fireRecoilRight *= 0.925f;
        }
        public override void OnKill()
		{
			int n = NPC.NewNPC(NPC.GetSource_Death(), (int)NPC.Center.X, (int)NPC.Center.Y, ModContent.NPCType<EvilSpirit>());
			Main.npc[n].velocity.Y = -10f;
			Main.npc[n].netUpdate = true;
		}
		public override void ModifyNPCLoot(NPCLoot npcLoot)
		{
			npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<FragmentOfEvil>(), 1, 4, 7));
		}
        public override bool CheckActive()
        {
            return true;
        }
    }
}