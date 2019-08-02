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

namespace SOTS.Projectiles.HolyRelics
{    
    public class Oxygen : ModProjectile 
    {	int wait = -1;
			int num1 = 1;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Oxygen");
			
		}
		
        public override void SetDefaults()
        {
			projectile.CloneDefaults(440);
            aiType = 440; //18 is the demon scythe style
			projectile.alpha = 255;
			projectile.width = 8;
			projectile.height = 8;
		}
		
		public override void AI()
		{	
			projectile.alpha = 255;
			
		
			num1 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), 8, 8, 219);
		
			Main.dust[num1].noGravity = true;
			Main.dust[num1].velocity *= 0.1f;
			
			
		}
		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
				target.AddBuff(mod.BuffType("CosmicRadiation"), 3600000, false);
			
		}
		public override void OnHitPvp(Player target, int damage, bool crit)
		{
				target.AddBuff(mod.BuffType("CosmicRadiation"), 3600000, false);
			
		}		
	}
}
		
			