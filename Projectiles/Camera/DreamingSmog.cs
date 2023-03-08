using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.Audio;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Void;
using SOTS.Dusts;

namespace SOTS.Projectiles.Camera
{    
    public class DreamingSmog : ModProjectile
	{
		public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
		{
			height = 24;
			width = 24;
			return base.TileCollideStyle(ref width, ref height, ref fallThrough, ref hitboxCenterFrac);
		}
        public override void SetDefaults()
        {
			Projectile.DamageType = ModContent.GetInstance<VoidMagic>();
			Projectile.tileCollide = true;
			Projectile.friendly = true;
			Projectile.penetrate = -1;
			Projectile.width = 36;
			Projectile.height = 36;
			Projectile.alpha = 0;
			Projectile.timeLeft = 1200;
		}
        public override void ModifyDamageHitbox(ref Rectangle hitbox)
        {
			int width = (int)(48 * Projectile.scale);
			hitbox = new Rectangle((int)(Projectile.Center.X - width / 2), (int)(Projectile.Center.Y - width / 2), width, width);
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
			if(oldVelocity.X != Projectile.velocity.X)
            {
				Projectile.velocity.X = -oldVelocity.X;
			}
			if (oldVelocity.Y != Projectile.velocity.Y)
			{
				Projectile.velocity.Y = -oldVelocity.Y;
			}
			Projectile.velocity *= 0.8f;
			if(Projectile.velocity.Length() > 64)
				Projectile.velocity = Projectile.velocity.SafeNormalize(Vector2.Zero) * 64;
			return false;
        }
        public override bool PreDraw(ref Color lightColor)
		{
			return true;
        }
		bool runOnce = true;
		public override void Kill(int timeLeft)
		{

		}
		public Color getDustColor => DreamingFrame.Green1;
        public override bool PreAI()
		{
			return true;
        }
        public override void AI()
		{

		}
	}
}
		
			