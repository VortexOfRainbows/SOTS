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
            projectile.width = 36;
            projectile.height = 64; 
            projectile.timeLeft = 255;
            projectile.penetrate = -1; 
            projectile.friendly = true; 
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
		}
	}
	
}
