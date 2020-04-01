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

namespace SOTS.Projectiles.Permafrost
{    
    public class IceCluster : ModProjectile 
    {	
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Ice Cluster");
			
		}
		
        public override void SetDefaults()
        {
			projectile.CloneDefaults(48);
            aiType = 48;
			projectile.penetrate = 1;
			projectile.width = 20;
			projectile.height = 20;
		}
		public override void Kill(int timeLeft)
        {
			if(projectile.owner == Main.myPlayer)
			{
				Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, 0, 0, mod.ProjectileType("IcePulse"), projectile.damage, 0, projectile.owner);
			}
		}
	}
}
		
			