using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
 
namespace SOTS.Projectiles.Minions
{  
    public class BVP : MinionINFO
    {	
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Vampire");
		}
        public override void SetDefaults()
        {
            projectile.netImportant = true;
            projectile.width = 14;
            projectile.height = 42;
            Main.projFrames[projectile.type] = 2;
            projectile.friendly = true;
            Main.projPet[projectile.type] = true;
            projectile.minion = true;
            projectile.netImportant = true;
            projectile.minionSlots = 0;
            projectile.penetrate = 1;
            projectile.timeLeft = 300000;
            projectile.tileCollide = false;
            projectile.ignoreWater = true;
            ProjectileID.Sets.MinionSacrificable[projectile.type] = false;
            ProjectileID.Sets.Homing[projectile.type] = true;
            inertia = 30f;
            shoot = mod.ProjectileType("VampireShot");
            shootSpeed = 28f;
            ProjectileID.Sets.LightPet[projectile.type] = true;
            Main.projPet[projectile.type] = true;
        }
 
        public override void CheckActive()
        {
            Player player = Main.player[projectile.owner];
            SOTSPlayer modPlayer = (SOTSPlayer)player.GetModPlayer(mod, "SOTSPlayer");
            if (player.dead)
            {
                modPlayer.Phankin = false;
            }
            if (modPlayer.Phankin)
            {
                projectile.timeLeft = 2;
            }
        }
 
        public override void CreateDust()
        {
            Lighting.AddLight((int)(projectile.Center.X / 16f), (int)(projectile.Center.Y / 16f), 0.6f, 0.9f, 0.3f);
        }
 
        public override void SelectFrame()
        {
            projectile.frameCounter++;
            if (projectile.frameCounter >= 8)
            {
                projectile.frameCounter = 0;
                projectile.frame = (projectile.frame + 1) % 2;
            }
        }
    }
}