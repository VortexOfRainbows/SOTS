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
    public class Ghost : ModProjectile 
    {	
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Ghosts");
			
		}
		
        public override void SetDefaults()
        {
			projectile.CloneDefaults(616);
            aiType = 616; //18 is the demon scythe style
			projectile.alpha = 0;
			projectile.timeLeft = 150000;
			projectile.magic = true;
			projectile.width = 9;
			projectile.height = 16;
		}
		
		public override void OnHitNPC(NPC n, int damage, float knockback, bool crit)
		{
            
		}
		public override void AI()
		{
		Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), 9, 16, 206);
}
	}
}
		
			