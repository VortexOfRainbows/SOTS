using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using System.IO;

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
			Projectile.hide = true;
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
			int TrailingType = FakePlayer.TrailingType;
			Vector2 idlePosition = player.Center;
			if (Main.myPlayer == player.whoAmI)
			{
				cursorArea = Main.MouseWorld;
				if (FakePlayer.itemAnimation <= 0 || FakePlayer.heldItem.IsAir)
				{
					Direction = player.direction;
				}
				if(FakePlayer.itemAnimation == FakePlayer.itemAnimationMax && FakePlayer.itemAnimationMax != 0)
                {
					Direction = Math.Sign(cursorArea.X - Projectile.Center.X);
                }
			}
			if (cursorArea != Vector2.Zero)
            {
				float speed = 0;
				Vector2 toIdle = Vector2.Zero;
                if (FakePlayer.itemAnimation > 0 || FakePlayer.SkipDrawing)
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
					speed = 3.25f + dist * 0.01f;
                    if (FakePlayer.SkipDrawing)
                    {
                        speed *= 2;
                    }
                    if (dist < speed)
                    {
                        speed = toIdle.Length();
                    }
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
			if (Main.myPlayer == player.whoAmI) //might be excessive but is the easiest way to sync everything
				Projectile.netUpdate = true;
			if(!FakePlayer.SkipDrawing)
				Lighting.AddLight(Projectile.Center, new Vector3(0.65f, 0.8f, 0.75f));
		}
        public override bool PreDraw(ref Color lightColor)
		{
			return false;
        }
    }
}