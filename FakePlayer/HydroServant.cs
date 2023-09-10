using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using System.IO;
using SOTS.Common;
using SOTS.Projectiles.Pyramid;

namespace SOTS.FakePlayer
{
	public class HydroServant : FakePlayerPossessingProjectile
	{
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
		bool runOnce = true;
		public float timeSinceTransforming = 0f;
		public bool LastDrawSkipped = false;
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
				FakePlayer = new FakePlayer(1, Projectile.identity);
			return base.PreAI();
		}
        public override void AI()
		{
			Player player = Main.player[Projectile.owner];
			if(!FakeModPlayer.ModPlayer(player).hasHydroFakePlayer)
            {
				Projectile.Kill();
            }
			Vector2 idlePosition = player.Center;
			if (Main.myPlayer == player.whoAmI)
			{
				cursorArea = Main.MouseWorld;
                Direction = Math.Sign(cursorArea.X - Projectile.Center.X);
            }
			if (cursorArea != Vector2.Zero)
            {
				float speed = 0;
				Vector2 toIdle = Vector2.Zero;
                if (!FakePlayer.CheckItemValidityFull(player, player.HeldItem, player.HeldItem, 1) && FakePlayer.BonusItemAnimationTime <= 0)
                {
					Vector2 position = player.Center + new Vector2(12, 2 * player.direction).RotatedBy((cursorArea - player.Center).ToRotation());
					Projectile.Center = position;
					timeSinceTransforming = 0f;
                }
				else
				{
					if(FakePlayer.SkipDrawing || FakePlayer.BonusItemAnimationTime <= 0)
                        timeSinceTransforming += 1f / 90f;
                    else if(FakePlayer.BonusItemAnimationTime > 0)
					{
						if (timeSinceTransforming > 0.35f)
							timeSinceTransforming -= 1f / 210f;
                    }
					timeSinceTransforming = Math.Clamp(timeSinceTransforming, 0.0f, 1f);
                    player.heldProj = Projectile.whoAmI;
                    if (FakePlayer.itemAnimation > 0 || FakePlayer.SkipDrawing || FakePlayer.BonusItemAnimationTime <= 0)
                    {
                        Vector2 toCursor = cursorArea - player.Center;
                        float length = toCursor.Length();
                        if (length > 320)
                            length = 320;
                        float lengthToCursor = -32 + length;
                        toCursor = toCursor.SafeNormalize(Vector2.Zero) * lengthToCursor;
                        idlePosition += toCursor;
                        toIdle = idlePosition - Projectile.Center;
                        float dist = toIdle.Length();
                        speed = 5f + (float)Math.Pow(dist, 1.5f) * 0.00075f;
						if (dist < speed)
                        {
                            speed = toIdle.Length();
                        }
                    }
                }
				speed *= timeSinceTransforming;
                Projectile.velocity = toIdle.SafeNormalize(Vector2.Zero) * speed;
				if (Direction == 1)
				{
					if (Projectile.ai[0] < Direction)
						Projectile.ai[0] += 0.07f;
				}
				else
				{
					if (Projectile.ai[0] > Direction)
						Projectile.ai[0] -= 0.07f;
				}
				if (Projectile.ai[1] >= 24)
				{
					Projectile.ai[1] -= 24f;
				}
				Vector2 circular = new Vector2(1f, 0).RotatedBy(MathHelper.ToRadians(12 * Projectile.ai[1]));
				Projectile.ai[1] += 0.65f;
				if (circular.Y > 0)
					circular.Y *= 0.5f;
				Projectile.velocity.Y += circular.Y;
                LastDrawSkipped = FakePlayer.SkipDrawing;
                UpdateItems(player);
				if(LastDrawSkipped != FakePlayer.SkipDrawing && timeSinceTransforming != 0)
				{

                }
			}
			if (Main.myPlayer == player.whoAmI) //might be excessive but is the easiest way to sync everything
				Projectile.netUpdate = true;
			if(!FakePlayer.SkipDrawing)
				Lighting.AddLight(Projectile.Center, new Vector3(0.65f, 0.8f, 0.75f));
		}
        public override bool PreDraw(ref Color lightColor)
        {
			if (Main.player[Projectile.owner].heldProj == Projectile.whoAmI)
				GreenScreenManager.DrawWaterLayer(Main.spriteBatch, ref MagicWaterLayer.RenderTargetPlayerHoldsWaterBall, true);
            return false;
        }
    }
}