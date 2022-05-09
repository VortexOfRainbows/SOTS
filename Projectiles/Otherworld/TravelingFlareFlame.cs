using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Projectiles.Otherworld
{    
    public class TravelingFlareFlame : ModProjectile 
    {	
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Flare Flame");
		}
        public override void SetDefaults()
        {
            Projectile.width = 64;
            Projectile.height = 64;
			Projectile.timeLeft = 24;
            Projectile.penetrate = -1; 
            Projectile.friendly = true; 
            Projectile.tileCollide = true;
			Projectile.ranged = true;
			Projectile.alpha = 255;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 40;
			Projectile.extraUpdates = 1;
		}
		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			if (Projectile.velocity.X != oldVelocity.X)
			{
				Projectile.velocity.X = -oldVelocity.X;
			}
			if (Projectile.velocity.Y != oldVelocity.Y)
			{
				Projectile.velocity.Y = -oldVelocity.Y;
			}
			return false;
		}
        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough)
        {
			width = 4;
			height = 4;
            return base.TileCollideStyle(ref width, ref height, ref fallThrough);
        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			target.AddBuff(BuffID.OnFire, 3600, false);
		}
		public override void AI() //The projectile's AI/ what the projectile does
		{
			for(int i = 0; i < Main.rand.Next(2) + 1; i++)
			{
				int num = Dust.NewDust(new Vector2(Projectile.position.X - 4, Projectile.position.Y - 4), Projectile.width, Projectile.height, mod.DustType("CopyDust4"));
				Dust dust = Main.dust[num];
				dust.velocity = Projectile.velocity * 0.3f;
				if (Projectile.ai[0] == 2)
					dust.color = new Color(220, 60, 10, 40);
				if (Projectile.ai[0] == 1)
					dust.color = new Color(0, 200, 220, 100);
				if (Projectile.ai[0] == 0)
					dust.color = new Color(255, 10, 10, 40);
				dust.noGravity = true;
				dust.fadeIn = 0.1f;
				dust.scale *= 2.5f;
			}
		}
	}
}