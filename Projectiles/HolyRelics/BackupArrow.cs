using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
 
namespace SOTS.Projectiles.HolyRelics
{    
    public class BackupArrow : ModProjectile 
    {	int bounce = 23;
	
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Backup Arrow");
			
		}
		
        public override void SetDefaults()
        {
			projectile.CloneDefaults(616);
            aiType = 616; //18 is the demon scythe style
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
		
		public override void AI()
        {
			
			
        }public override bool OnTileCollide(Vector2 oldVelocity)
		{	
			//If collide with tile, reduce the penetrate.
			//So the projectile can reflect at most 5 times
			bounce--;
			if(bounce <= 0)
			{
				projectile.tileCollide = false;
			}
				if (projectile.velocity.X != oldVelocity.X)
				{
					projectile.velocity.X = -oldVelocity.X;
				}
				if (projectile.velocity.Y != oldVelocity.Y)
				{
					projectile.velocity.Y = -oldVelocity.Y;
				}
				Main.PlaySound(SoundID.Item10, projectile.position);
				
				
			
				return false;
		}
	
		
}
}
			