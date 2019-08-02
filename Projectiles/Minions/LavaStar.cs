using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
 
namespace SOTS.Projectiles.Minions
{  
    public class LavaStar : MinionINFO3
    {	
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Lava");
		}
        public override void SetDefaults()
        {
            projectile.netImportant = true;
            projectile.width = 30;
            projectile.height = 30;
            Main.projFrames[projectile.type] = 4;
            projectile.friendly = true;
            Main.projPet[projectile.type] = true;
            projectile.minion = true;
            projectile.netImportant = true;
            projectile.minionSlots = 1;
            projectile.penetrate = 1;
            projectile.timeLeft = 6;
            projectile.tileCollide = false;
            projectile.ignoreWater = true;
            ProjectileID.Sets.MinionSacrificable[projectile.type] = true;
            ProjectileID.Sets.Homing[projectile.type] = true;
            inertia = 30f;
            shoot = 85;
            shootSpeed = 16;
            ProjectileID.Sets.LightPet[projectile.type] = true;
            Main.projPet[projectile.type] = true;
        }
 
        public override void CheckActive()
        {
            Player player = Main.player[projectile.owner];
            SOTSPlayer modPlayer = (SOTSPlayer)player.GetModPlayer(mod, "SOTSPlayer");
            if (player.dead)
            {
                modPlayer.LavaStar = false;
            }
            if (modPlayer.LavaStar)
            {
                projectile.timeLeft = 6;
            }
        }
 
        public override void CreateDust()
        {
            Lighting.AddLight((int)(projectile.Center.X / 16f), (int)(projectile.Center.Y / 16f), 0.6f, 0.9f, 0.3f);
        }
		public override void SelectFrame()
        {
            projectile.frameCounter++;
            if (projectile.frameCounter >= 4)
            {
                projectile.frameCounter = 0;
            }
        }
	}
}