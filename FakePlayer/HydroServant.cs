using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using System.IO;
using SOTS.Common;
using SOTS.Projectiles.Pyramid;
using Terraria.ID;

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
		public float timeSinceTransforming = -1f;
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
			if(!FakeModPlayer.ModPlayer(player).hasHydroFakePlayer || player.gravDir != 1)
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
				bool valid = FakePlayer.CheckItemValidityFull(player, player.HeldItem, player.HeldItem, 1);
                if (!valid && FakePlayer.BonusItemAnimationTime <= 0)
                {
					Vector2 position = player.Center + new Vector2(12, 2 * player.direction).RotatedBy((cursorArea - player.Center).ToRotation());
					Projectile.Center = position;
					timeSinceTransforming = 0f;
                }
				else if(timeSinceTransforming != -1)
				{
					if(FakePlayer.SkipDrawing || FakePlayer.BonusItemAnimationTime <= 0)
                        timeSinceTransforming += 1f / 30f;
                    else if(FakePlayer.BonusItemAnimationTime > 0)
					{
						if (timeSinceTransforming > 0.5f)
							timeSinceTransforming -= 1f / 240f;
                        else
                            timeSinceTransforming += 1f / 120f;
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
                        speed = 6.75f + (float)Math.Pow(dist, 1.5f) * 0.001f;
						if (dist < speed)
                        {
                            speed = toIdle.Length();
                        }
                    }
                }
                if(timeSinceTransforming != -1)
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
				float sinusoid = 1f * (float)Math.Sin(MathHelper.ToRadians(12 * Projectile.ai[1]));
				Projectile.ai[1] += 0.65f;
                LastDrawSkipped = FakePlayer.SkipDrawing;
                UpdateItems(player);
                if (valid)
                {
                    if (LastDrawSkipped != FakePlayer.SkipDrawing && timeSinceTransforming > 0)
				{
					if(LastDrawSkipped) //was previously incomplete
                    {
						SOTSUtils.PlaySound(SoundID.Item80, Projectile.Center, 1.0f, -0.8f);
						for(int j = 0; j < 2; j++)
                        {
                            for (int i = 0; i < 40; i++)
                            {
                                Vector2 circular = new Vector2(2.0f + Main.rand.NextFloat(2.0f), 0).RotatedBy(i / 40f * MathHelper.TwoPi) * (1.1f - 0.4f * j);
								circular.Y *= 1.4f;
                                WaterParticle.NewWaterParticle(Projectile.Center, circular + Projectile.velocity * 0.75f, j * 1f + Main.rand.NextFloat(1.6f, 2.1f));
                            }
                        }
                    }
					else //is now incomplete
                    {
                        SOTSUtils.PlaySound(SoundID.Item130, Projectile.Center, 1.1f, -0.8f);
                        for (int i = 0; i < 80; i++)
                        {
                            Vector2 circular = new Vector2(1f, 0).RotatedBy(i / 80f * MathHelper.TwoPi);
                            circular.Y *= 1.4f;
                            float rand = Main.rand.NextFloat(1f);
                            WaterParticle.NewWaterParticle(Projectile.Center + circular * 48 * rand, -circular * (Main.rand.NextFloat(3f) + rand * Main.rand.NextFloat(1f, 2f)) + Projectile.velocity * 0.75f, Main.rand.NextFloat(1.7f, 2.2f));
                        }
                    }
                }
                    Lighting.AddLight(Projectile.Center, new Vector3(0.65f, 0.7f, 0.8f));
                    if (!FakePlayer.SkipDrawing)
                    {
                        WaterParticle.NewWaterParticle(Projectile.Center + new Vector2(-1 * FakePlayer.direction, 6), Projectile.velocity * 0.75f + new Vector2(Main.rand.NextFloat(-0.5f, 0.5f), 2.2f), Main.rand.NextFloat(0.8f, 1.1f));
                        WaterParticle.NewWaterParticle(Projectile.Center, Projectile.velocity * 0.75f + Main.rand.NextVector2Circular(4, 4), Main.rand.NextFloat(1.0f, 1.2f));
                    }
					else
                    {
						for(int j = 0; j < 3; j++)
                        {
                            WaterParticle.NewWaterParticle(Projectile.Center, Projectile.velocity * 0.75f * (0.45f + j * 0.35f) + Main.rand.NextVector2Circular(1.5f, 1.5f) * (1.0f - 0.4f * j), Main.rand.NextFloat(0.9f, 1.1f));
                        }
                    }
                    for (int j = 0; j < 2; j++)
                    {
                        if(Main.rand.NextBool(3))
                            WaterParticle.NewWaterParticle(FakePlayerDrawing.PlayerBallCenter(player), player.velocity * 0.65f * (0.5f + j * 0.5f) + Main.rand.NextVector2Circular(1.5f, 1.5f) * (1.0f - 0.8f * j), Main.rand.NextFloat(0.8f, 1.1f));
                    }
                }
                if(timeSinceTransforming < 0)
                    timeSinceTransforming += 0.5f;
                Projectile.velocity.Y += sinusoid * 0.5f;
            }
			if (Main.myPlayer == player.whoAmI) //might be excessive but is the easiest way to sync everything
				Projectile.netUpdate = true;
		}
        public override bool PreDraw(ref Color lightColor)
        {
			if (Main.player[Projectile.owner].heldProj == Projectile.whoAmI)
				GreenScreenManager.DrawWaterLayer(Main.spriteBatch, ref MagicWaterLayer.RenderTargetPlayerHoldsWaterBall, true, 0);
            return false;
        }
    }
}