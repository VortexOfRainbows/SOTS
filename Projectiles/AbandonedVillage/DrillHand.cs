using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static SOTS.NPCs.Boss.Excavator.Excavator;

namespace SOTS.Projectiles.AbandonedVillage
{
	public class DrillHand : ModProjectile
	{
		public static Texture2D stemTexture;
        public sealed override void SetDefaults()
		{
			Projectile.width = 30;
			Projectile.height = 64;
			Projectile.tileCollide = false;
			Projectile.friendly = false;
			Projectile.penetrate = -1;
			Projectile.timeLeft = 300;
			Projectile.netImportant = true;
			Projectile.alpha = 255;
		}
        public override void PostDraw(Color lightColor)
		{
            DrawArm();
		}
		public Vector2 armTargetPosition = new(-13f, -40f);
        public float Recoil = 0f;
        public float lastDir = 1f;
        public void PickTargetPosition()
		{
            Player p = Main.player[Projectile.owner];
            SOTSPlayer sP = p.SOTSPlayer();
            int j = SOTSUtils.SignNoZero(p.direction);
            Vector2 target = new(-13f, -40f);
            if (Projectile.ai[0] == -2)
            {
                SOTSUtils.PlaySound(SoundID.Item23, armTargetPosition + p.MountedCenter, 0.8f, 0.1f);
                Recoil = 3;
                Projectile.ai[0] = 0;
            }
            if (Main.myPlayer == Projectile.owner)
            {
                bool lockPosition = Main.SmartCursorIsUsed || p.HeldItem.pick <= 0 || !sP.DrillHand;
                int targetAI = -1;
                if(lockPosition)
                {
                    targetAI = -1; //default position (resting position)
                }
                else
                    targetAI = 0;
                if (Projectile.ai[0] != targetAI)
                {
                    Projectile.ai[0] = targetAI;
                    Projectile.netUpdate = true;
                }
            }
            if (Projectile.ai[0] == 0)
            {
                target = new Vector2(Projectile.ai[1], Projectile.ai[2]) - p.MountedCenter;
                target.X *= p.direction;
                target.Y *= p.gravDir;
                float maxDist = 76f;
                float myLen = target.Length();
                if(myLen > maxDist)
                {
                    target = target / myLen * maxDist;
                }
            }
            float lerpAmt = 0.08f;
            float lerpLenAmt = 0.12f;
            if(lastDir != p.direction)
            {
                lerpAmt = lerpLenAmt = 1;
                lastDir = p.direction;
            }
            float angle = target.ToRotation();
            float currentAngle = armTargetPosition.ToRotation();
            float lerpAngle = SOTSUtils.AngularLerp(currentAngle, angle, lerpAmt);
            float length = target.Length();
            float currentLength = armTargetPosition.Length();
            float lerpLength = MathHelper.Lerp(currentLength, length, lerpLenAmt);
            armTargetPosition = new Vector2(lerpLength, 0).RotatedBy(lerpAngle);
            if (armTargetPosition.Distance(target) < 0.7f)
            {
                armTargetPosition = target;
                if(Projectile.ai[0] == -3)
                {
                    Projectile.ai[0] = -1;
                    Projectile.netUpdate = true;
                }
            }
            if (sP.HasPick3x3ThisFrame && Projectile.ai[0] == 0) //When the 3x3 mining gets used
            {
                Projectile.ai[0] = -2;
                Projectile.netUpdate = true;
            }
            Recoil *= 0.96f;
            if (Recoil <= 0.01f)
                Recoil = 0f;
        }
        public void DrawArm()
        {
            stemTexture ??= ModContent.Request<Texture2D>("SOTS/Projectiles/AbandonedVillage/DrillHandConnector", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
            Texture2D hand = Terraria.GameContent.TextureAssets.Projectile[Type].Value;
            Texture2D arm = stemTexture;

            Vector2 screenPos = Main.screenPosition;
            Player p = Main.player[Projectile.owner];
            int j = SOTSUtils.SignNoZero(p.direction);
            Vector2 armPosition = new Vector2(7 * j, -7 * p.gravDir);
            armPosition = armPosition + p.MountedCenter;
            float A = 48; //size of hand
            float B = 25; //size of arm
			Vector2 target = p.MountedCenter + new Vector2(armTargetPosition.X * j, armTargetPosition.Y * p.gravDir);// Main.MouseWorld - new Vector2(0, p.gfxOffY);
			//Main.NewText(p.MountedCenter - target);
            Vector2 end = target;
            Vector2 start = armPosition;
            int dirJ = j * (int)p.gravDir;
            ExcavatorArm.DoArmJoint(ref start, ref end, A, B, dirJ, out float endArmRot, out float endHandRot);
            end += new Vector2(0, p.gfxOffY);
			start += new Vector2(0, p.gfxOffY);
            //Main.EntitySpriteDraw(SOTSUtils.WhitePixel, end - screenPos, null, Color.White, 0, Vector2.One, 4, SpriteEffects.None, 0);
            //Main.EntitySpriteDraw(SOTSUtils.WhitePixel, end - screenPos, null, Color.White, endHandRot, new Vector2(0, 1), new Vector2(A * 0.5f, 2), SpriteEffects.None, 0);
            //Main.EntitySpriteDraw(SOTSUtils.WhitePixel, target - screenPos, null, Color.White, 0, Vector2.One, 4, SpriteEffects.None, 0);
            //Main.EntitySpriteDraw(SOTSUtils.WhitePixel, start - screenPos, null, Color.White, 0, Vector2.One, 4, SpriteEffects.None, 0);
            //Main.EntitySpriteDraw(SOTSUtils.WhitePixel, start - screenPos, null, Color.White, endArmRot, new Vector2(0, 1), new Vector2(B * 0.5f, 2), SpriteEffects.None, 0);
            Vector2 armOrigin = new(5, 24);
            Vector2 revArmOrigin = new(arm.Width - armOrigin.X, armOrigin.Y);
            Vector2 handOrigin = new(hand.Width / 2, dirJ == 1 ? 9 : 55);
            Color drawColor = Lighting.GetColor(start.ToTileCoordinates(), new Color(210, 210, 210));
            Main.EntitySpriteDraw(arm, start.ToVector2Int() - screenPos, null, drawColor, endArmRot + MathHelper.PiOver2, dirJ == -1 ? armOrigin : revArmOrigin, 1, dirJ == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0);
            Main.EntitySpriteDraw(hand, end.ToVector2Int() + Main.rand.NextVector2Circular(Recoil, Recoil) - screenPos, null, drawColor, endHandRot - MathHelper.PiOver2 * dirJ, handOrigin, 1, dirJ == 1 ? SpriteEffects.None : SpriteEffects.FlipVertically, 0);
        }
		public override bool PreDraw(ref Color lightColor)
		{
			return false;
		}
		public void FindPosition()
		{
            Player p = Main.player[Projectile.owner];
			Projectile.Center = p.Center;
            PickTargetPosition();
        }
		public override void AI()
		{
			FindPosition();
			if (Main.myPlayer == Projectile.owner)
            {
                if (Main.MouseWorld.Distance(new Vector2(Projectile.ai[1], Projectile.ai[2])) > 2)
                    Projectile.netUpdate = true;
                Projectile.ai[1] = Main.MouseWorld.X;
				Projectile.ai[2] = Main.MouseWorld.Y;
            }
            else
            {
                Projectile.timeLeft = 100;
            }
			Projectile.alpha = Math.Max(0, Projectile.alpha - 25);
            Lighting.AddLight(Projectile.Center, ExcavatorOrb.Color.ToVector3() * 0.1f);
		}
	}
}