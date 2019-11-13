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

namespace SOTS.Projectiles.Minions
{    
    public class PearlescentShot : ModProjectile 
    {	int bounce = 24;
		int wait = 1;              
		
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Cryptic Bolt");
			
		}
		
        public override void SetDefaults()
        {
			projectile.CloneDefaults(14);
            aiType = 14; 
			projectile.height = 20;
			projectile.width = 16;
			projectile.magic = true;
			projectile.minion = true;
			projectile.tileCollide = false;
			projectile.timeLeft = 1800;
			projectile.friendly = true;
			projectile.hostile = false;
			projectile.penetrate = 40;
		}
		public override void AI()
		{ 
			if(Main.rand.Next(3) == 0)
			{
			int num1 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), 16, 20, 235);
			Main.dust[num1].noGravity = true;
			Main.dust[num1].velocity *= 0.1f;
			}
		}
		public override void Kill(int timeLeft)
		{
			for(int i = 0; i < 24; i++)
			{
			int num1 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), 8, 8, 235);
			Main.dust[num1].noGravity = true;
			}
		} 
		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.immune[projectile.owner] = Main.rand.Next(3);
		}
	}
}
		