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
            projectile.width = 64;
            projectile.height = 64;
			projectile.timeLeft = 24;
            projectile.penetrate = -1; 
            projectile.friendly = true; 
            projectile.tileCollide = true;
			projectile.ranged = true;
			projectile.alpha = 255;
			projectile.usesLocalNPCImmunity = true;
			projectile.localNPCHitCooldown = 40;
			projectile.extraUpdates = 1;
		}
		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			if (projectile.velocity.X != oldVelocity.X)
			{
				projectile.velocity.X = -oldVelocity.X;
			}
			if (projectile.velocity.Y != oldVelocity.Y)
			{
				projectile.velocity.Y = -oldVelocity.Y;
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
				int num = Dust.NewDust(new Vector2(projectile.position.X - 4, projectile.position.Y - 4), projectile.width, projectile.height, mod.DustType("CopyDust4"));
				Dust dust = Main.dust[num];
				dust.velocity = projectile.velocity * 0.3f;
				if (projectile.ai[0] == 2)
					dust.color = new Color(220, 60, 10, 40);
				if (projectile.ai[0] == 1)
					dust.color = new Color(0, 200, 220, 100);
				if (projectile.ai[0] == 0)
					dust.color = new Color(255, 10, 10, 40);
				dust.noGravity = true;
				dust.fadeIn = 0.1f;
				dust.scale *= 2.5f;
			}
		}
	}
}