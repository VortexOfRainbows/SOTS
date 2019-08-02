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

namespace SOTS.Projectiles 
{    
    public class ExplosiveKnife : ModProjectile 
    {	
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Explosive Knife");
			
		}
		
        public override void SetDefaults()
        {
			projectile.CloneDefaults(48);
            aiType = 48; //18 is the demon scythe style
			projectile.penetrate = 1;
		}
		public override void AI()
		{
		}
		public override void Kill(int timeLeft)
        {
			if(projectile.owner == Main.myPlayer)
			{
				Projectile.NewProjectile((projectile.Center.X), projectile.Center.Y, Main.rand.Next(-5, 6), Main.rand.Next(-5, 6), 249, projectile.damage, 0, 0);
				Projectile.NewProjectile((projectile.Center.X), projectile.Center.Y, Main.rand.Next(-5, 6), Main.rand.Next(-5, 6), 249, projectile.damage, 0, 0);
				Projectile.NewProjectile((projectile.Center.X), projectile.Center.Y, Main.rand.Next(-5, 6), Main.rand.Next(-5, 6), 249, projectile.damage, 0, 0);
			}
		}
	}
}
		
			