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
    public class OrionChain : ModProjectile 
    {	int wait = 0;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Chain Beam");
			
		}
		
        public override void SetDefaults()
        {
			projectile.CloneDefaults(260);
            aiType = 260; //18 is the demon scythe style
			projectile.magic = true;
			projectile.alpha = 255;
			projectile.penetrate = 1;
			projectile.timeLeft = 7200;
			projectile.friendly = true;

		}
		public override void AI()
		{
			int num1 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 269);
			Main.dust[num1].noGravity = true;
			Main.dust[num1].velocity *= 0.1f;
			if(projectile.velocity.X == 0 && projectile.velocity.Y == 0)
			{
			projectile.Kill();
			}
			wait++;
			if(wait >= 120)
			{
			projectile.friendly = true;
			}
		}
		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
				target.AddBuff(mod.BuffType("FrozenThroughTime"), 6, false);
			
		}
			
}
	
}
		
			