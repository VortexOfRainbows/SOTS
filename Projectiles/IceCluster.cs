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
				for(int i = 0; i < 360; i += 45)
				{
					Vector2 spinLocation = new Vector2(-5,0).RotatedBy(MathHelper.ToRadians(i + Main.rand.Next(-10,11)));
					int proj = Projectile.NewProjectile(projectile.Center.X - spinLocation.X * 4, projectile.Center.Y - spinLocation.Y * 4, spinLocation.X, spinLocation.Y + 1.1f, 520, projectile.damage, 0, projectile.owner);
					Main.projectile[proj].penetrate = -1;
					Main.projectile[proj].timeLeft = 9;
					Main.projectile[proj].magic = false;
					Main.projectile[proj].thrown = false;
					Main.projectile[proj].ranged = true;
					Main.projectile[proj].tileCollide = false;
					Main.projectile[proj].type = 337;
				}
			}
		}
	}
}
		
			