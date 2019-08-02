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
    public class BoreBullet : ModProjectile 
    {	int enemybore = 0;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Bore Bullet");
			
		}
		
        public override void SetDefaults()
        {
			projectile.CloneDefaults(14);
            aiType = 14; 
			projectile.penetrate = 20;
			projectile.width = 18;
			projectile.height = 26;
			projectile.alpha = 255;


		}
		public override void OnHitNPC(NPC n, int damage, float knockback, bool crit)
		{
			enemybore++;
			projectile.damage = (int)(projectile.damage * (Math.Pow(1.25, -enemybore) + 1));
			
			
		Main.PlaySound(2, (int)projectile.Center.X, (int)projectile.Center.Y, 22);
            
		}
		public override void AI()
		{
			int num1 = Dust.NewDust(new Vector2(projectile.Center.X - 4, projectile.Center.Y - 4), 2, 2, 1);

			
			Main.dust[num1].noGravity = true;
			Main.dust[num1].velocity *= 0.1f;
			
			
		}
	}
}
		
			