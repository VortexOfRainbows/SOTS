using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
 

namespace SOTS.Projectiles 
{    
    public class CrackTheSkyProj : ModProjectile 
    {	
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Crack The Sky");
			
		}
		
        public override void SetDefaults()
        {
            projectile.width = 28;
            projectile.height = 64; 
            projectile.timeLeft = 60000;
            projectile.penetrate = -1; 
            projectile.friendly = true; 
            projectile.hostile = false; 
            projectile.tileCollide = true;
            projectile.ignoreWater = false; 
            projectile.thrown = true; 
            projectile.aiStyle = 1; //18 is the demon scythe style
		}
		
		public override void Kill(int timeLeft)
        {
			if(projectile.owner == Main.myPlayer)
			Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y,0, 0,  mod.ProjectileType("CrackedGate"), projectile.damage, 2,0);
		}
		
	}
	
}