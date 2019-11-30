using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Terraria;
using Terraria.ModLoader;

namespace SOTS.Projectiles.Ores
{    
    public class PlatinumDart : ModProjectile 
    {	
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Platinum Dart");
			
		}
		
        public override void SetDefaults()
        {
			projectile.aiStyle = 1;
			projectile.width = 26;
			projectile.height = 26;
            projectile.magic = true;
			projectile.penetrate = 2;
			projectile.ranged = false;
			projectile.alpha = 0; 
			projectile.friendly = true;
			projectile.tileCollide = true;
			projectile.timeLeft = 9000;
		}
		bool latch = false;
		int enemyIndex = -1;
		float diffPosX = 0;
		float diffPosY = 0;
		public override void AI()
        {
			Player player = Main.player[projectile.owner];
			projectile.rotation = (float)Math.Atan2((double)projectile.velocity.Y, (double)projectile.velocity.X) + MathHelper.ToRadians(45);
			projectile.spriteDirection = 1;
			
			if(latch && enemyIndex != -1)
			{
				NPC target = Main.npc[enemyIndex];
				projectile.alpha += projectile.timeLeft % 2 == 0 ? 1 : 0;
				if(projectile.alpha >= 210)
				{
					projectile.Kill();
				}
				if(target.active && !target.friendly)
				{
					projectile.aiStyle = 0;
					projectile.position.X = target.Center.X - projectile.width/2 - diffPosX;
					projectile.position.Y = target.Center.Y - projectile.height/2 - diffPosY;
					target.velocity.X *= target.boss ? 0.995f : 0.985f;
					target.velocity.Y += target.boss ? 0.005f : target.lifeMax > 200 ? 0.02f : 0.04f;
				}
				else
				{
					projectile.Kill();
				}
			}
		}
		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
			Player player = Main.player[projectile.owner];
			projectile.friendly = false;
            target.immune[projectile.owner] = 0;
			projectile.tileCollide = false;
			latch = true;
			
			if(diffPosX == 0)
			diffPosX = target.Center.X - projectile.Center.X;
				
			if(diffPosY == 0)
			diffPosY = target.Center.Y - projectile.Center.Y;
				
			enemyIndex = target.whoAmI;
			
			if(target.life <= 0)
			{
				projectile.Kill();
			}
        }
		public override void Kill(int timeLeft)
        {
			for(int i = 0; i < 15; i++)
			{
			int num1 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 16);
			Main.dust[num1].noGravity = true;
			}
		}
	}
}