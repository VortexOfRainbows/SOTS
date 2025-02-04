using System;
using System.IO;
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
        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(NPC.localAI[0]);
            writer.Write(NPC.localAI[1]);
            writer.Write(NPC.localAI[2]);
            writer.Write(NPC.localAI[3]);
        }
        public override void ReceiveExtraAI(BinaryReader reader)
        {
            NPC.localAI[0] = reader.ReadSingle();
            NPC.localAI[1] = reader.ReadSingle();
            NPC.localAI[2] = reader.ReadSingle();
            NPC.localAI[3] = reader.ReadSingle();
        }
        public override void ApplyDifficultyAndPlayerScaling(int numPlayers, float balance, float bossAdjustment)/* tModPorter Note: bossLifeScale -> balance (bossAdjustment is different, see the docs for details) */
        {
            NPC.damage = (int)(NPC.damage * 5 / 6);
            NPC.lifeMax = (int)(NPC.lifeMax * 6 / 7);
        }
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
            leg1.Y = MathF.Sqrt(MathF.Abs(leg1.Y)) * MathF.Sign(leg1.Y);
            leg2.Y = MathF.Sqrt(MathF.Abs(leg2.Y)) * MathF.Sign(leg2.Y);
            leg1 *= 4;
            leg2 *= 4;
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
            Vector2 legOffset = new Vector2(0, 1).RotatedBy(anim);
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
		public void FireLaserAtPlayer(int type = 0, int projType = 0)
        {
			ref float i = ref (type == 0 ? ref NPC.localAI[3] : ref NPC.localAI[2]);
			Vector2 pos = NPC.Center + (type == 0 ? armPosRight : armPosLeft);
			Vector2 dir = new Vector2(1, 0).RotatedBy(i);
            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
				float speed = projType == 0 ? 1 : 10;
                Projectile.NewProjectile(NPC.GetSource_FromAI(), pos + new Vector2(52, 0).RotatedBy(i), dir * speed, projType == 0 ? ModContent.ProjectileType<BridgeburnerLaser>() : ModContent.ProjectileType<BridgeburnerFlame>(), NPC.GetBaseDamage() / 2, 0, Main.myPlayer, NPC.Center.X + dir.X * 40, NPC.Center.Y + dir.Y * 40);
            }
			float recoilMult = projType == 0 ? 1 : 0.2f;
			float rad = MathHelper.ToRadians(5);
            i -= rad * NPC.spriteDirection * recoilMult;
			if (type == 0)
                fireRecoilRight += 10 * recoilMult;
			else
                fireRecoilLeft += 10 * recoilMult;
			NPC.velocity.X -= dir.X * recoilMult;
            NPC.netUpdate = true;
        }
        private float soundCooldown = 0;
		public override void AI()
        {
            NPC.TargetClosest(true);
			Player player = Main.player[NPC.target];
            Vector2 toPlayer = player.Center - NPC.Center;
            float toPlayerLength = toPlayer.Length();
            if (toPlayerLength < 2400)
				NPC.DiscourageDespawn(600);
			NPC.spriteDirection = NPC.direction;
            NPC.velocity.X *= 0.825f;
			if(NPC.velocity.Y < 0)
				NPC.velocity.Y *= 0.9f;
            float legMoveAmt = MathHelper.ToRadians(MathF.Sqrt(MathF.Abs(NPC.velocity.X)) * 7.5f);

            NPC.localAI[0] += legMoveAmt;
			NPC.localAI[0] = MathHelper.WrapAngle(NPC.localAI[0]);
            soundCooldown += MathF.Abs(legMoveAmt);
            if (soundCooldown > MathF.PI + MathHelper.PiOver2)
            {
                soundCooldown -= MathF.PI;
                if(NPC.velocity.Y == 0)
                    SOTSUtils.PlaySound(SoundID.Item53, NPC.Center, 0.9f, -0.4f);
            }
            NPC.localAI[1]++;
			bool canSeePlayer = Collision.CanHitLine(player.position, player.width, player.height, NPC.position, NPC.width, NPC.height);
			if(!canSeePlayer && NPC.localAI[1] < 240)
			{
				if (NPC.localAI[1] > 0)
					NPC.localAI[1]--;
			}
			if (NPC.localAI[1] > 240)
            {
                float old = NPC.localAI[0];
                NPC.localAI[0] = SOTSUtils.AngularLerp(NPC.localAI[0], MathHelper.ToRadians(90), 0.04f);
                float diff = old - NPC.localAI[0];
                soundCooldown -= diff;
                NPC.aiStyle = -1;
                NPC.velocity.X *= 0.6f;
				int type = (int)NPC.ai[3];
                if (type != 0 && type != 1)
                    type = 0;
                Vector2 toPlayerLeft = player.Center - NPC.Center - armPosLeft;
                Vector2 toPlayerRight = player.Center - NPC.Center - armPosRight;
                if (type == 1)
				{
					if(NPC.localAI[1] <= 480)
                    {
                        if (NPC.localAI[1] % 3 == 0 && NPC.localAI[1] > 300)
                        {
                            if (NPC.localAI[1] % 9 == 0)
                                SOTSUtils.PlaySound(SoundID.Item34, NPC.Center, 0.9f, 0.2f, 0);
                            FireLaserAtPlayer((int)NPC.localAI[1] / 3 % 2, type);
                        }
                        float sin = MathF.Sin((NPC.localAI[1] - 300) / 30f * MathF.PI);
                        NPC.localAI[3] = SOTSUtils.AngularLerp(NPC.localAI[3], toPlayerRight.ToRotation() + MathHelper.ToRadians(30 * sin), 0.035f);
                        NPC.localAI[2] = SOTSUtils.AngularLerp(NPC.localAI[2], toPlayerLeft.ToRotation() - MathHelper.ToRadians(30 * sin), 0.035f);
                    }
                    else
                    {
                        NPC.localAI[3] = SOTSUtils.AngularLerp(NPC.localAI[3], toPlayerRight.ToRotation(), 0.035f);
                        NPC.localAI[2] = SOTSUtils.AngularLerp(NPC.localAI[2], toPlayerLeft.ToRotation(), 0.035f);
                    }
                }
				else
                {
                    if (NPC.localAI[1] % 30 == 0 && NPC.localAI[1] > 300)
                    {
                        FireLaserAtPlayer((int)NPC.localAI[1] / 30 % 2, type);
                    }
                    NPC.localAI[3] = SOTSUtils.AngularLerp(NPC.localAI[3], toPlayerRight.ToRotation(), 0.035f);
                    NPC.localAI[2] = SOTSUtils.AngularLerp(NPC.localAI[2], toPlayerLeft.ToRotation(), 0.035f);
                }
                if (NPC.localAI[1] > 550)
                {
                    NPC.localAI[1] = -60;
                    NPC.netUpdate = true;
                }
            }
			else
			{
				NPC.aiStyle = NPCAIStyleID.Fighter;
                NPC.localAI[3] = SOTSUtils.AngularLerp(NPC.localAI[3], MathHelper.ToRadians(90 - 10 * NPC.spriteDirection + MathF.Sin(NPC.localAI[0]) * 20), 0.04f);
                NPC.localAI[2] = SOTSUtils.AngularLerp(NPC.localAI[2], MathHelper.ToRadians(90 - 10 * NPC.spriteDirection - MathF.Sin(NPC.localAI[0]) * 20), 0.04f);
                if (toPlayerLength < 360)
                    NPC.ai[3] = 1;
                else
                    NPC.ai[3] = 0;
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