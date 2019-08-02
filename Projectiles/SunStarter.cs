using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Projectiles 
{    
    public class SunStarter : ModProjectile 
    {	public int sunSoulTimer = 130;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Lihzahrd Sun");
			
		}
		
        public override void SetDefaults()
        {	
            projectile.width = 44;
            projectile.height = 44; 
            projectile.timeLeft = 200;
            projectile.penetrate = -1; 
            projectile.friendly = false; 
            projectile.hostile = false; 
            projectile.tileCollide = false;
            projectile.ignoreWater = true; 
            projectile.magic = true; 
            projectile.aiStyle = 0; //18 is the demon scythe style
			projectile.alpha = 0;

		}
		public override void AI() //The projectile's AI/ what the projectile does
		{
			sunSoulTimer = projectile.timeLeft;
			Player player = Main.player[projectile.owner];
			player.velocity.X = 0;
			player.velocity.Y = 0;
			
			if(sunSoulTimer == 190)
			Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, 0, -3, mod.ProjectileType("SoulSunStrike"), (int)(projectile.damage * 1), projectile.knockBack, Main.myPlayer);
		
		
			if(sunSoulTimer == 175)
			Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, 2, -2, mod.ProjectileType("SoulSunStrike2"), (int)(projectile.damage * 1), projectile.knockBack, Main.myPlayer);
			
			
			if(sunSoulTimer == 160)
			Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, 3, 0, mod.ProjectileType("SoulSunStrike"), (int)(projectile.damage * 1), projectile.knockBack, Main.myPlayer);
			
			
			if(sunSoulTimer == 145)
			Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, 2, 2,mod.ProjectileType("SoulSunStrike2"), (int)(projectile.damage * 1), projectile.knockBack, Main.myPlayer);
			
			
			if(sunSoulTimer == 130)
			Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, 0, 3, mod.ProjectileType("SoulSunStrike"), (int)(projectile.damage * 1), projectile.knockBack, Main.myPlayer);
			
			
			if(sunSoulTimer == 115)
			Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, -2, 2, mod.ProjectileType("SoulSunStrike2"), (int)(projectile.damage * 1), projectile.knockBack, Main.myPlayer);
			
			if(sunSoulTimer == 100)
			Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, -3, 0, mod.ProjectileType("SoulSunStrike"), (int)(projectile.damage * 1), projectile.knockBack, Main.myPlayer);
			
			
			if(sunSoulTimer == 85)
			Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, -2, -2, mod.ProjectileType("SoulSunStrike2"), (int)(projectile.damage * 1), projectile.knockBack, Main.myPlayer);
		
			if(sunSoulTimer == 3)
			{
            SOTSPlayer modPlayer = (SOTSPlayer)player.GetModPlayer(mod, "SOTSPlayer");
			
			
			modPlayer.sunSoulActivate = 8;
			}
		}
		public override void Kill(int timeLeft)
		{

			
		}
	}
	
}