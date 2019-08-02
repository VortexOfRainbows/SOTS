using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Projectiles 
{    
    public class SoulSunStrike2 : ModProjectile 
    {	int wait = 0;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Lihzahrd Flare");
			
		}
		
        public override void SetDefaults()
        {
			projectile.CloneDefaults(274);
            aiType = 274; //18 is the demon scythe style
			projectile.alpha = 0;
			projectile.timeLeft = 240;
			projectile.width = 22;
			projectile.height = 22;
            projectile.penetrate = -1; 
            projectile.friendly = false; 
            projectile.hostile = false; 
			

		}
		public override void AI()
		{
			Player player = Main.player[projectile.owner];
            SOTSPlayer modPlayer = (SOTSPlayer)player.GetModPlayer(mod, "SOTSPlayer");
			
			
			
			if(modPlayer.sunSoulActivate >= 1)
			{
			projectile.timeLeft = 3;
			modPlayer.sunSoulActivate--;
			}
}
public override void Kill(int timeLeft)
        {
 
			
			
			Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, 10, 10, 85, (int)(projectile.damage * 1), projectile.knockBack, Main.myPlayer);
			Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, -10, 10, 85, (int)(projectile.damage * 1), projectile.knockBack, Main.myPlayer);
			Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, 10, -10, 85, (int)(projectile.damage * 1), projectile.knockBack, Main.myPlayer);
			Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, -10, -10, 85, (int)(projectile.damage * 1), projectile.knockBack, Main.myPlayer);
        }
	
		
	}
}
		
			