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
			if(!SubspacePlayer.ModPlayer(player).hasHydroFakePlayer)
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
				if (TrailingType == 3) //melee
                {
                    Vector2 toCursor = cursorArea - player.Center;
                    float length = toCursor.Length();
                    if (length > 720)
                        length = 720;
					float lengthToCursor = -32 + length;
					toCursor = toCursor.SafeNormalize(Vector2.Zero) * lengthToCursor;
					idlePosition += toCursor;
				}
				if (TrailingType == 4) //melee, but limitted range
                {
                    Vector2 toCursor = cursorArea - player.Center;
                    float length = toCursor.Length();
					if (length > 480)
						length = 480;
                    idlePosition.Y += 8f;
                    float lengthToCursor = -64 + length;
                    toCursor = toCursor.SafeNormalize(Vector2.Zero) * lengthToCursor;
                    idlePosition += toCursor;
                }
				Vector2 toIdle = idlePosition - Projectile.Center;
				float dist = toIdle.Length();
				float speed = 3 + (float)Math.Pow(dist, 1.45) * 0.002f;
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
			if (Main.myPlayer == player.whoAmI) //might be excessive but is the easiest way to sync everything
				Projectile.netUpdate = true;
			Lighting.AddLight(Projectile.Center, new Vector3(0.65f, 0.8f, 0.75f));
		}
        public override bool PreDraw(ref Color lightColor)
		{
			return false;
        }
    }
}