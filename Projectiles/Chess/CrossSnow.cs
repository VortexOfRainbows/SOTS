using System;
using System.IO;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace SOTS.Projectiles.Chess
{    
    public class CrossSnow : ModProjectile 
    {	int bounce = 24;
		int wait = 1;              
		
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Cross Snow");
			
		}
		
        public override void SetDefaults()
        {
			projectile.CloneDefaults(166);
            aiType = 166; 
			projectile.height = 22;
			projectile.width = 22;
			projectile.ranged = true;
			projectile.timeLeft = 360;
			projectile.friendly = false;
			projectile.hostile = false;
		}
		
		public override void AI()
		{ 
		}
		
		public override void Kill(int timeLeft)
		{
			Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, -4, -4, mod.ProjectileType("IceSpike"), 25, 0, 0);
			Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, 4, 4, mod.ProjectileType("IceSpike"), 25, 0, 0);
			Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, -4, 4, mod.ProjectileType("IceSpike"), 25, 0, 0);
			Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, 4, -4, mod.ProjectileType("IceSpike"), 25, 0, 0);
			Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, -5, 0, mod.ProjectileType("IceSpike"), 25, 0, 0);
			Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, 0, -5, mod.ProjectileType("IceSpike"), 25, 0, 0);
			Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, 5, 0, mod.ProjectileType("IceSpike"), 25, 0, 0);
			Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, 0, 5, mod.ProjectileType("IceSpike"), 25, 0, 0);
			
		}
			
}
	
}
		