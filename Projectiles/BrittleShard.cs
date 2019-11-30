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
    public class BrittleShard : ModProjectile 
    {	
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Brittle Shard");	
		}
        public override void SetDefaults()
        {
			projectile.CloneDefaults(1);
            aiType = 1; 
			projectile.penetrate = 1;
			projectile.alpha = 0;
			projectile.width = 10;
			projectile.height = 12;
			projectile.melee = true;
		}
		public override void AI()
		{
			projectile.rotation = Main.rand.Next(38);
		}
		public override void Kill(int timeLeft)
		{
			for(int i = 0; i < 12; i++)
			{
				int num1 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 67);
				Main.dust[num1].noGravity = true;
			}
		}
	}
}