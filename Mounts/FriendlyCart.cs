using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using System.Collections.Generic;
using Terraria.DataStructures;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using SOTS.Dusts;
using System;
using static Terraria.ModLoader.ModContent;

namespace SOTS.Mounts
{
	public class FriendlyCart : ModMount
    {
        protected class FriendlyCartSpecificData
        {
            internal float moveCounter = 0;
            internal float Moving = 0;
            public FriendlyCartSpecificData()
            {
                moveCounter = 0;
                Moving = 0;
            }
        }
		public override void SetStaticDefaults()
		{
			MountData.spawnDust = ModContent.DustType<SootDust>();
			MountData.buff = ModContent.BuffType<Buffs.FriendlyCartBuff>();
			MountData.heightBoost = 50;
			MountData.runSpeed = 11f;
			MountData.dashSpeed = 11f;
			MountData.flightTimeMax = 0;
			MountData.fatigueMax = 0;
			MountData.jumpHeight = 15;
			MountData.acceleration = 0.078f;
			MountData.jumpSpeed = 6f;
			MountData.totalFrames = 1;
			MountData.usesHover = false;
			MountData.playerYOffsets = [50];
			MountData.xOffset = 0;
			MountData.bodyFrame = 6;
			MountData.yOffset = 9;
			MountData.playerHeadOffset = 0;
			MountData.fallDamage = 0;
			MountData.constantJump = true;
			if (Main.netMode != NetmodeID.Server)
				MountData.textureWidth = MountData.backTexture.Width();
				MountData.textureHeight = MountData.backTexture.Height();
        }
        public override bool Draw(List<DrawData> playerDrawData, int drawType, Player drawPlayer, ref Texture2D texture, 
			ref Texture2D glowTexture, ref Vector2 drawPosition, ref Rectangle frame, ref Color drawColor, ref Color glowColor, 
			ref float rotation, ref SpriteEffects spriteEffects, ref Vector2 drawOrigin, ref float drawScale, float shadow)
        {
            if (drawType != 2)
                return false;
            var m = (FriendlyCartSpecificData)drawPlayer.mount._mountSpecificData;
            SpriteEffects spriteEffects2 = spriteEffects;
            SpriteEffects spriteEffectsReverse = spriteEffects == SpriteEffects.FlipHorizontally ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
            float drawScale2 = drawScale;
            float rotation2 = rotation;
            int spriteDirection = drawPlayer.direction;
            Vector2 drawPosition2 = drawPosition;
            Texture2D texture2 = texture;
            string Directory = "SOTS/NPCs/AbandonedVillage/CoalCart";
            float sin = MathF.Sin(m.moveCounter * 2.0f) * m.Moving;
            float sin2 = -MathF.Sin(m.moveCounter * 1.0f) * m.Moving;
            float r = rotation2 + MathHelper.ToRadians(5 * sin2);
            Vector2 bobbing = new Vector2(0, 2 * sin);
            //MountData.playerYOffsets = [(int)(50 - 2.5f * sin)]; //doesn't work in MP
            void DrawBody(List<DrawData> playerDrawData, Vector2 screenPos, Color drawColor)
            {
                Texture2D texture = texture2;
                Texture2D head = texture2;
                Texture2D glow = Request<Texture2D>("SOTS/Mounts/FriendlyCartGlow").Value;
                int height = texture.Height;
                Vector2 drawOrigin = new Vector2(texture.Width / 2, height / 2);
                Vector2 drawPos = drawPosition2 - screenPos;
                drawPos += bobbing;
                DrawArmIK(playerDrawData, screenPos - bobbing, 2 * spriteDirection);
                playerDrawData.Add(new DrawData(head, drawPos, null, drawColor, r, drawOrigin, drawScale2, spriteEffectsReverse, 0f));
                playerDrawData.Add(new DrawData(glow, drawPos, null, Color.White, r, drawOrigin, drawScale2, spriteEffectsReverse, 0f));
                DrawArmIK(playerDrawData, screenPos - bobbing, spriteDirection);
            }
            void DrawArmIK(List<DrawData> playerDrawData, Vector2 screenPos, int dir)
            {
                Texture2D leg = Request<Texture2D>(Directory + "Leg").Value;
                Texture2D foot = Request<Texture2D>(Directory + "Foot").Value;
                Texture2D toes = Request<Texture2D>(Directory + "Toes").Value;
                int j = SOTSUtils.SignNoZero(dir);
                int xOffset = MathF.Abs(dir) == 2 ? 36 : 0;
                float sideOffset = (-17 + xOffset) * j;
                Vector2 legOrigin = new Vector2(0, 0);
                Vector2 revLegOrigin = new Vector2(leg.Width - legOrigin.X, legOrigin.Y);
                Vector2 footOrigin = new Vector2(13, foot.Height);
                Vector2 revFootOrigin = new Vector2(foot.Width - footOrigin.X, footOrigin.Y);
                Vector2 armPosition = new Vector2(sideOffset, -6);
                armPosition = armPosition.RotatedBy(rotation2) + drawPosition2;
                Color drawColor = Color.White;

                float r = MathHelper.WrapAngle(m.moveCounter * 1.0f * j + MathHelper.ToRadians(dir * 180));
                //float outwardSize = (isBigArm ? 72 : 38) - 16 * MathF.Sin(MathHelper.ToRadians(r + 90 * j));
                Vector2 targetLegPos = new Vector2(sideOffset, 33).RotatedBy(rotation2);
                targetLegPos = targetLegPos + drawPosition2;
                Vector2 circular = new Vector2(40, 0).RotatedBy(r);
                if(m.Moving < 1)
                {
                    circular = Vector2.Lerp(circular, new Vector2(0, 40), 1 - m.Moving);
                }
                circular = circular.RotatedBy(rotation2);
                if (circular.Y > 0)
                {
                    circular.Y *= 0.2f;
                }
                else
                    circular.Y *= 0.5f;
                targetLegPos += circular;

                float maxDifference = 0;
                float A = foot.Height; //size of hand
                float B = leg.Height; //size of arm
                float maxSize = A + B;
                Vector2 end = targetLegPos;
                Vector2 start = armPosition;
                if (end.Distance(start) < maxDifference)
                {
                    end = start + (end - start).SNormalize() * maxDifference;
                }
                Vector2 startToEnd = end - start;
                float C = startToEnd.Length();
                float angleA = C - A - B > 0 ? 0 : MathF.Acos((B * B + C * C - A * A) / (2 * B * C));
                float angleB = C - A - B > 0 ? 0 : MathF.Acos((A * A + C * C - B * B) / (2 * A * C));
                Vector2 endToMid = -startToEnd.RotatedBy(-angleB * j);
                Vector2 startToMid = startToEnd.RotatedBy(angleA * j);
                float endHandRot = endToMid.ToRotation();
                float endArmRot = startToMid.ToRotation();
                end -= new Vector2(2, -9 * j).RotatedBy(endArmRot);
                Vector2 mid = startToMid.SNormalize() * B + start;
                mid -= new Vector2(2, -9 * j).RotatedBy(endArmRot);
                float stretch = MathF.Max(1, end.Distance(mid) / A);
                //Visual representations of the IK happening
                // playerDrawData.Add(new DrawData(SOTSUtils.WhitePixel, end - screenPos, null, drawColor, endHandRot, new Vector2(0, 1), new Vector2(A * 0.5f, 2), SpriteEffects.None, 0);
                // playerDrawData.Add(new DrawData(SOTSUtils.WhitePixel, start - screenPos, null, drawColor, 0, Vector2.One, drawScale2 * 4, SpriteEffects.None, 0);
                // playerDrawData.Add(new DrawData(SOTSUtils.WhitePixel, start - screenPos, null, drawColor, endArmRot, new Vector2(0, 1), new Vector2(B * 0.5f, 2), SpriteEffects.None, 0);

                playerDrawData.Add(new DrawData(foot, end - screenPos, null, drawColor, endHandRot + MathHelper.PiOver2, j == -1 ? footOrigin : revFootOrigin, new Vector2(1, stretch), j == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0));
                playerDrawData.Add(new DrawData(leg, start - screenPos, null, drawColor, endArmRot + MathHelper.Pi * 1.5f, j == -1 ? legOrigin : revLegOrigin, drawScale2, j == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0));

                footOrigin = new Vector2(10, 16);
                revFootOrigin = new Vector2(foot.Width - footOrigin.X, footOrigin.Y);
                end += new Vector2(6, 4 * j).RotatedBy(endHandRot);
                playerDrawData.Add(new DrawData(toes, end - screenPos, null, drawColor, (endHandRot + MathHelper.PiOver2) * 0.4f, j == -1 ? footOrigin : revFootOrigin, new Vector2(1, stretch), j == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0));
                //playerDrawData.Add(new DrawData(SOTSUtils.WhitePixel, end - screenPos, null, drawColor, 0, Vector2.One, drawScale2 * 2, SpriteEffects.None, 0));
                if (!Main.gameInactive && !Main.gamePaused && m.Moving > 0.2f)
                {
                    PixelDust.Spawn(end + Main.screenPosition + new Vector2(Main.rand.NextFloat(-8, 8), 4), 0, 0, Main.rand.NextVector2Circular(0.5f, 0.5f) + new Vector2(drawPlayer.velocity.X, drawPlayer.velocity.Y * 0.1f), new Color(132, 42, 0), 12).scale = Main.rand.NextFloat(1.25f, 1.6f);
                }
            }

            DrawBody(playerDrawData, Vector2.Zero, drawColor);

            return false;
        }
        public override void UpdateEffects(Player player)
        {
            //SetStaticDefaults();
            var m = (FriendlyCartSpecificData)player.mount._mountSpecificData;
            Vector2 trueVelo = player.position - player.oldPosition;
            float speed = trueVelo.Length();
            if (speed < 1000)
            {
                float speedCalc = MathF.Sqrt(MathF.Abs(MathF.Abs(player.velocity.X) - MathF.Abs(player.velocity.Y)));
                m.moveCounter += MathHelper.ToRadians(4.5f * speedCalc);
                m.moveCounter %= MathHelper.TwoPi;
                m.Moving = MathHelper.Lerp(m.Moving, MathF.Min(speedCalc, 1), 0.1f);
            }
        }
        public override void SetMount(Player player, ref bool skipDust)
        {
            // When this mount is mounted, we initialize _mountSpecificData with a new CarSpecificData object which will track some extra visuals for the mount.
            player.mount._mountSpecificData = new FriendlyCartSpecificData();

            //if (!Main.dedServ)
            //{
            //    for (int i = 0; i < 16; i++)
            //    {
            //        Dust.NewDustPerfect(player.Center + new Vector2(80, 0).RotatedBy(i * Math.PI * 2 / 16f), MountData.spawnDust);
            //    }
            //
            //    skipDust = true;
            //}
        }

    }
}