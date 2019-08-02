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
    public class GelAxe : ModProjectile 
    {	int wait = 0;
		float rotate = 0;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Gel Axe");
			
		}
		
        public override void SetDefaults()
        {
			projectile.CloneDefaults(3);
            aiType = 3; //18 is the demon scythe style
			projectile.penetrate = 3;
			projectile.alpha = 0;
			projectile.width = 30;
			projectile.height = 30;

		}
		public override bool OnTileCollide(Vector2 oldVelocity)
		{	
			projectile.tileCollide = false;
			projectile.timeLeft = 120;
			projectile.velocity.X *= 0.3f;
			projectile.velocity.Y *= 0.5f;
			projectile.aiStyle = 0;
			wait = 1;
			return false;
		}
		public override void AI()
		{
			if(wait == 1)
			{
			projectile.velocity.X *= 0.9f;
			projectile.velocity.Y *= 0.9f;
			}
		}
	}
}
		
			