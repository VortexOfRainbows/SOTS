using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Projectiles 
{    
    public class Tele : ModProjectile 
    {	int Telep = 0;
	float swapX;
	float swapY;
	int delay;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Tele");
			
		}
		
        public override void SetDefaults()
        {
            projectile.width = 30;
            projectile.height = 30; 
            projectile.timeLeft = 13;
            projectile.penetrate = -1; 
            projectile.friendly = true; 
            projectile.hostile = false; 
            projectile.tileCollide = true;
            projectile.ignoreWater = true; 
            projectile.ranged = true; 
            projectile.aiStyle = 0; //18 is the demon scythe style
			projectile.alpha = 255;
		}
		public override void AI()
		{
		projectile.rotation += 30;
        Player player = Main.player[Main.myPlayer];
		if(Telep >= 4)
		{
				  player.position.X = projectile.position.X;
				  player.position.Y = projectile.position.Y;
				  projectile.Kill();
		}
		Telep++;
		
		}
		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{   
		if(target.life <= target.lifeMax / 20)
		{
			
		}
		else
		{
		target.life -= target.lifeMax / 20;
		}
		
			Player player  = Main.player[target.target];	
			
			
			swapX = target.Center.X;
			swapY = target.Center.Y;
			target.position.X = player.Center.X + (target.position.X - target.Center.X);
			target.position.Y = player.Center.Y - (target.Center.Y - target.position.Y);
			player.position.X = swapX + (player.position.X - player.Center.X);
			player.position.Y = swapY - (player.Center.Y - player.position.Y);
				  projectile.Kill();
			
		}
		public override void OnHitPvp(Player target, int damage, bool crit)
		{
            Player player = Main.player[Main.myPlayer];
			
			
			
			swapX = target.Center.X;
			swapY = target.Center.Y;
			target.position.X = player.Center.X + (target.position.X - target.Center.X);
			target.position.Y = player.Center.Y - (target.Center.Y - target.position.Y);
			player.position.X = swapX + (player.position.X - player.Center.X);
			player.position.Y = swapY - (player.Center.Y - player.position.Y);
				  projectile.Kill();
}
	}
	
}