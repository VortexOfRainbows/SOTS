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
    public class SandyCloud : ModProjectile 
    {	int bounce = 2000;
		float up = 1;              
		
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Sandy Water");
			
		}
		
        public override void SetDefaults()
        {
			projectile.CloneDefaults(512);
            aiType = 512; //18 is the demon scythe style
			projectile.height = 40;
			projectile.width = 38;
			projectile.thrown = true;
			projectile.magic = false;
		}
		public override void AI()
		{
			projectile.rotation += (float)(Main.rand.Next(100)/100f);
		}
		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
				target.AddBuff(BuffID.Suffocation, 480, false);
			
		}
		
	}
}
		