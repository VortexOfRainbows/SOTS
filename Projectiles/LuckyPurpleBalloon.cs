using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Projectiles 
{    
    public class LuckyPurpleBalloon : ModProjectile 
    {	int wait = 0;
		int plusY = 0;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Lucky Purple Balloon");
			
		}
		
        public override void SetDefaults()
        {
		
			
            projectile.CloneDefaults(198);
            aiType = 198;
            Main.projFrames[projectile.type] = 1;
			projectile.netImportant = true;
            projectile.width = 18;
            projectile.height = 34; 
            projectile.timeLeft = 255;
            projectile.penetrate = -1; 
            projectile.friendly = false; 
            projectile.hostile = false; 
            projectile.tileCollide = false;
            projectile.ignoreWater = true; 
            projectile.magic = true; 
			


		}
        public override bool PreAI()
        {
            Player player = Main.player[projectile.owner];
			player.bunny = false; // Relic from aiType
            return true;
        }
		public override void AI() 
		{
			Player player = Main.player[projectile.owner];
            SOTSPlayer modPlayer = player.GetModPlayer<SOTSPlayer>();
            if (player.dead || (player.ownedProjectileCounts[mod.ProjectileType("LuckyPurpleBalloon")] > 1 && projectile.alpha < 30))
            {
                modPlayer.PurpleBalloon = false;
            }
            if (modPlayer.PurpleBalloon)
            {
                projectile.timeLeft = 2;
            }
			wait++;
			if(wait % 100 == 0)
			{
				plusY = 2;
			}
			if(wait % 100 == 20)
			{
				plusY = 0;
			}
			if(wait % 100 == 40)
			{
				plusY = -2;
			}
			if(wait % 100 == 60)
			{
				plusY = 0;
			}
			if(wait % 100 == 80)
			{
				plusY = 2;
			}
			projectile.velocity *= 0.1f;
			projectile.rotation = player.velocity.X * 0.04f;
			projectile.position.X = player.Center.X - projectile.width/2;
			projectile.position.Y = player.Center.Y - projectile.height/2;
			projectile.position.X += (float)(67 * Main.player[projectile.owner].direction);
            float gravDir = Main.player[projectile.owner].gravDir;
            projectile.position.Y -= (30f + plusY) * gravDir;
			if(gravDir == -1)
			{
				projectile.rotation = MathHelper.ToRadians(180);
			}
		}
	}
	
}
