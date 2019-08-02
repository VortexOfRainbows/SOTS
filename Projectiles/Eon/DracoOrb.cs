using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Terraria;
using Terraria.ModLoader;

namespace SOTS.Projectiles.Eon
{    
    public class DracoOrb : ModProjectile 
    {	
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Draco Orb");
			
		}
		
        public override void SetDefaults()
        {
            projectile.width = 18;
            projectile.height = 20; 
            projectile.timeLeft = 100000;
            projectile.penetrate = 1; 
            projectile.friendly = true; 
            projectile.hostile = false; 
            projectile.tileCollide = true;;
            projectile.ignoreWater = true; 
            projectile.magic = true; 
            projectile.aiStyle = 1; //18 is the demon scythe style
			projectile.alpha = 100;
		}
		public override void Kill(int timeLeft)
		{
			
				Projectile.NewProjectile((projectile.Center.X), projectile.Center.Y,  Main.rand.Next(-2, 3), Main.rand.Next(-14, -11), mod.ProjectileType("DracoBeacon"), (int)(projectile.damage * 1f), 0, 0);
				Projectile.NewProjectile((projectile.Center.X), projectile.Center.Y,  Main.rand.Next(-2, 3), Main.rand.Next(-14, -11), mod.ProjectileType("DracoBeacon"), (int)(projectile.damage * 1f), 0, 0);
				Projectile.NewProjectile((projectile.Center.X), projectile.Center.Y,  Main.rand.Next(-2, 3), Main.rand.Next(-14, -11), mod.ProjectileType("DracoBeacon"), (int)(projectile.damage * 1f), 0, 0);
				Projectile.NewProjectile((projectile.Center.X), projectile.Center.Y,  Main.rand.Next(-2, 3), Main.rand.Next(-14, -11), mod.ProjectileType("DracoBeacon"), (int)(projectile.damage * 1f), 0, 0);

			
		}
		
	}
	
}