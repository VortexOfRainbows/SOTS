using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
 
namespace SOTS.Projectiles.Base
{    
    public class BackupArrow : ModProjectile 
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Backup Arrow");
		}
        public override void SetDefaults()
        {
			projectile.CloneDefaults(ProjectileID.VortexBeaterRocket);
            aiType = ProjectileID.VortexBeaterRocket; 
            projectile.width = 18;
            projectile.height = 22; 
            projectile.timeLeft = 1800;
            projectile.penetrate = 1; 
            projectile.friendly = true; 
            projectile.hostile = false; 
            projectile.tileCollide = true;
            projectile.ignoreWater = true; 
            projectile.ranged = true; 
		}
		int bounceCounter = 0;
        public override bool OnTileCollide(Vector2 oldVelocity)
		{
			bounceCounter++;
			if (bounceCounter > 3)
				return true;
			if (projectile.velocity.X != oldVelocity.X)
				projectile.velocity.X = -oldVelocity.X;
			if (projectile.velocity.Y != oldVelocity.Y)
				projectile.velocity.Y = -oldVelocity.Y;
			Main.PlaySound(SoundID.Item10, projectile.Center);
			return false;
		}
	}
}
			