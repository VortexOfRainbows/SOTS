using System;
using Microsoft.Xna.Framework;
using SOTS.Buffs.MinionBuffs;
using SOTS.Common.GlobalNPCs;
using Steamworks;
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
        private int target = -1;
        public override void AI()
        {
            int oldMouseX = Main.mouseX;
            int oldMouseY = Main.mouseY;
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

            bool foundTarget = false;
            bool validItem = false;
            if(FakePlayer.heldItem != null)
                validItem = FakePlayer.CheckItemValidityFull(player, FakePlayer.heldItem, FakePlayer.heldItem, FakePlayerTypeID.Tesseract);
            int TrailingType = FakePlayer.ItemTrailingType(player.inventory[FakePlayer.UseItemSlot(player)]);
            bool needsLOS = TrailingType == TrailingID.RANGED || TrailingType == TrailingID.MAGIC;

            if (target != -1)
            {
                NPC prevTarget = Main.npc[target];
                if(!prevTarget.active || prevTarget.life <= 0 || prevTarget.dontTakeDamage || !prevTarget.CanBeChasedBy(this))
                {
                    target = -1;
                }
                else if (FakePlayer.itemAnimation <= 1)
                {
                    target = -1;
                } 
                else if(needsLOS && !Collision.CanHitLine(Projectile.Center - -new Vector2(16, 16), 32, 32, prevTarget.position, prevTarget.width, prevTarget.height))
                {
                    target = -1;
                }
            }
            if (target == -1 && validItem)
            {
                target = SOTSNPCs.FindTarget_Basic(Projectile.Center, nearbyPursuitRange, this, needsLOS);
                if (target == -1)
                    target = SOTSNPCs.FindTarget_Basic(player.Center, maxPursuitRange - nearbyPursuitRange, this, true);
            }
            NPC npc = null;
            if (target != -1)
            {
                foundTarget = true;
                npc = Main.npc[target];
                cursorArea = npc.Center;

                Vector2 result = npc.Center - Main.screenPosition;
                Main.mouseX = FakePlayer.UniqueMouseX = (int)result.X;
                Main.mouseY = FakePlayer.UniqueMouseY = (int)result.Y;

                if (FakePlayer.itemAnimation <= 1)
                {
                    if (TrailingType == TrailingID.MELEE || TrailingType == TrailingID.CLOSERANGE)
                    {
                        Direction = Math.Sign(npc.Center.X - player.Center.X);
                    }
                    else
                    {
                        Direction = Math.Sign(npc.Center.X - Projectile.Center.X);
                    }
                }
            }
            else
            {
                if (Main.myPlayer == player.whoAmI)
                {
                    Direction = player.direction;
                }
            }
            if (!foundTarget)
            {
                TrailingType = 0;
            }
            if (cursorArea != Vector2.Zero || TrailingType == 0)
            {
                bool hasLOS = false;
                if (TrailingType == 0)
                {
                    idlePosition.Y -= 64f;
                }
                if (TrailingType == TrailingID.RANGED || TrailingType == TrailingID.MAGIC)
                {
                    float appropriateRangedDistance = 128;
                    if (npc != null && foundTarget)
                    {
                        hasLOS = Collision.CanHitLine(Projectile.Center - -new Vector2(16, 16), 32, 32, npc.position, npc.width, npc.height);
                        appropriateRangedDistance += npc.Size.Length() / 2f + (float)Math.Sqrt(npc.width * npc.height);
                    }
                    Vector2 toCursor = cursorArea - Projectile.Center;
                    Vector2 fromNPC = cursorArea - toCursor.SafeNormalize(Vector2.Zero) * appropriateRangedDistance;
                    idlePosition = fromNPC;
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
                if (TrailingType == TrailingID.MELEE || TrailingType == TrailingID.CLOSERANGE)
                {
                    hasLOS = true;
                    Vector2 toCursor = cursorArea - player.Center;
                    float length = toCursor.Length();
                    if (length > maxPursuitRange)
                        length = maxPursuitRange;
                    float lengthToCursor = length;
                    toCursor = toCursor.SafeNormalize(Vector2.Zero) * lengthToCursor;
                    idlePosition += toCursor;
                    idlePosition.Y = cursorArea.Y;
                    idlePosition.X -= appropriateMeleeDistance * Math.Sign(Direction);
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

                if(foundTarget)
                {
                    FakePlayer.ForceItemUse = hasLOS;
                }
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