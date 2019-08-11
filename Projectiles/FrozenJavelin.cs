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
    public class FrozenJavelin : ModProjectile 
    {	int wait = 0;
	int oldDamage = 0;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Frozen Javelin");
			
		}
		
        public override void SetDefaults()
        {
			projectile.CloneDefaults(507);
            aiType = 507; //18 is the demon scythe style
			projectile.height = 56;
		}
		public override void AI()
		{
		}
		public override void Kill(int timeLeft)
        {
			if(wait == 0)
			{
				oldDamage = projectile.damage;
				wait++;
			}
				if(projectile.owner == Main.myPlayer)
				{
					Projectile.NewProjectile((projectile.Center.X), projectile.Center.Y, Main.rand.Next(-8, 9), Main.rand.Next(-8, 9), 520, (int)(oldDamage * 0.75) + 1, 0, 0);
					Projectile.NewProjectile((projectile.Center.X), projectile.Center.Y, Main.rand.Next(-8, 9), Main.rand.Next(-8, 9), 520, (int)(oldDamage * 0.75) + 1, 0, 0);
					Projectile.NewProjectile((projectile.Center.X), projectile.Center.Y, Main.rand.Next(-8, 9), Main.rand.Next(-8, 9), 520, (int)(oldDamage * 0.75) + 1, 0, 0);
					Projectile.NewProjectile((projectile.Center.X), projectile.Center.Y, Main.rand.Next(-8, 9), Main.rand.Next(-8, 9), 520, (int)(oldDamage * 0.75) + 1, 0, 0);
				}

		}
	}
}
		
			