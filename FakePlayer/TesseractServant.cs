using System;
using Microsoft.Xna.Framework;
using SOTS.Buffs.MinionBuffs;
using SOTS.Common.GlobalNPCs;
using Terraria;
using Terraria.ModLoader;

namespace SOTS.FakePlayer
{
	public class TesseractServant : FakePlayerPossessingProjectile
	{
        public override void SafeSetStaticDefaults()
        {
            Main.projPet[Type] = false;
        }
        public override string Texture => "SOTS/FakePlayer/SubspaceServant";
        public sealed override void SetDefaults()
		{
			Projectile.width = 20;
			Projectile.height = 42;
			Projectile.tileCollide = false;
			Projectile.friendly = true;
			Projectile.penetrate = -1;
			Projectile.ignoreWater = true;
			Projectile.netImportant = true;
			Projectile.hide = false;
		}
        public override bool? CanHitNPC(NPC target)
        {
            return false;
        }
        bool runOnce = true;
        public override bool PreAI()
		{
			if (Main.myPlayer != Projectile.owner)
				Projectile.timeLeft = 20;
			if (runOnce)
			{
				Projectile.ai[1] = 80f;
				runOnce = false;
			}
			if (FakePlayer == null)
				FakePlayer = new FakePlayer(FakePlayerTypeID.Tesseract, Projectile.identity);
			return base.PreAI();
		}
        public override void AI()
		{
            if (FakePlayer == null)
                return;
			Player player = Main.player[Projectile.owner];
			if(!player.HasBuff<TesseractBuff>())
            {
				Projectile.Kill();
            }
            else
            {
                Projectile.timeLeft = 6;
            }
			Vector2 idlePosition = player.Center;

            float maxPursuitRange = 1800;
            float nearbyPursuitRange = 240;

            int target = SOTSNPCs.FindTarget_Basic(Projectile.Center, nearbyPursuitRange, this, false);
            if(target == -1 && (FakePlayer.itemAnimation <= 0 || FakePlayer.heldItem.IsAir))
                target = SOTSNPCs.FindTarget_Basic(player.Center, maxPursuitRange - nearbyPursuitRange, this, true);
            int oldMouseX = Main.mouseX;
            int oldMouseY = Main.mouseY;
            bool foundTarget = false;
            if (target != -1)
            {
                foundTarget = true;
                NPC npc = Main.npc[target];
                cursorArea = npc.Center;

                Vector2 result = npc.Center - Main.screenPosition;
                Main.mouseX = (int)result.X;
                Main.mouseY = (int)result.Y;

                FakePlayer.ForceItemUse = true;

                Direction = Math.Sign(npc.Center.X - Projectile.Center.X);
            }
            else
            {
                if (Main.myPlayer == player.whoAmI)
                {
                    if (FakePlayer.itemAnimation <= 0 || FakePlayer.heldItem.IsAir)
                    {
                        Direction = player.direction;
                    }
                }
            }
            if (!foundTarget)
            {
                //FakePlayer.KillMyOwnedProjectiles = true;
            }
            int TrailingType = FakePlayer.TrailingType;
            if (cursorArea != Vector2.Zero || TrailingType == 0)
            {
                if (TrailingType == 0)
                {
                    idlePosition.X -= player.direction * 64f;
                }
                if (TrailingType == 1) //magic
                {
                    idlePosition.Y -= 48f;
                    Vector2 toCursor = cursorArea - player.Center;
                    toCursor = toCursor.SafeNormalize(Vector2.Zero) * -128f;
                    toCursor.Y *= 0.375f;
                    toCursor.Y = -Math.Abs(toCursor.Y);
                    idlePosition += toCursor;
                }
                if (TrailingType == 2) //ranged
                {
                    idlePosition.Y -= 64f;
                    Vector2 toCursor = cursorArea - player.Center;
                    toCursor = toCursor.SafeNormalize(Vector2.Zero) * 128f;
                    toCursor.Y *= 0.4125f;
                    idlePosition += toCursor;
                }
                float appropriateMeleeDistance = -4;
                if(FakePlayer.heldItem != null && !FakePlayer.heldItem.IsAir)
                {
                    appropriateMeleeDistance += FakePlayer.heldItem.Size.Length() * FakePlayer.heldItem.scale;
                    if(FakePlayer.heldItem.CountsAsClass(DamageClass.SummonMeleeSpeed))
                    {
                        appropriateMeleeDistance += 128f;
                    }
                }
                if (appropriateMeleeDistance < 50)
                    appropriateMeleeDistance = 50;
                if (TrailingType == 3 || TrailingType == 4) //melee
                {
                    Vector2 toCursor = cursorArea - player.Center;
                    float length = toCursor.Length();
                    if (length > maxPursuitRange)
                        length = maxPursuitRange;
                    float lengthToCursor = length;
                    toCursor = toCursor.SafeNormalize(Vector2.Zero) * lengthToCursor;
                    idlePosition += toCursor;
                    idlePosition.Y = cursorArea.Y;
                    idlePosition.X -= appropriateMeleeDistance * Math.Sign(toCursor.X);
                }
                Vector2 toIdle = idlePosition - Projectile.Center;
                float dist = toIdle.Length();
                float speed = 8f + dist * 0.0025f;
                if (dist > nearbyPursuitRange)
                {
                    speed = dist;
                }
                if (dist < speed)
                {
                    speed = toIdle.Length();
                }
                Projectile.velocity = toIdle.SafeNormalize(Vector2.Zero) * speed;
                if (Direction == 1)
                {
                    if (Projectile.ai[0] < Direction)
                        Projectile.ai[0] += 0.1f;
                }
                else
                {
                    if (Projectile.ai[0] > Direction)
                        Projectile.ai[0] -= 0.1f;
                }
                if (Projectile.ai[1] >= 24)
                {
                    Projectile.ai[1] -= 24f;
                }
                Vector2 circular = new Vector2(2f, 0).RotatedBy(MathHelper.ToRadians(15 * Projectile.ai[1]));
                Projectile.ai[1] += 0.75f;
                if (circular.Y > 0)
                    circular.Y *= 0.5f;
                Projectile.velocity.Y += circular.Y;
                UpdateItems(player);
                //Projectile.velocity = FakePlayer.Velocity;
            }

            Main.mouseX = oldMouseX;
            Main.mouseY = oldMouseY;
            if (Main.myPlayer == player.whoAmI) //might be excessive but is the easiest way to sync everything
                Projectile.netUpdate = true;
        }
        public override bool PreDraw(ref Color lightColor)
        {
			//if (Main.player[Projectile.owner].heldProj == Projectile.whoAmI)
			//	GreenScreenManager.DrawWaterLayer(Main.spriteBatch, ref MagicWaterLayer.RenderTargetPlayerHoldsWaterBall, true);
            return false;
        }
    }
}