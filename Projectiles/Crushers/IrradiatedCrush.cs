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

namespace SOTS.Projectiles.Crushers
{    
    public class IrradiatedCrush : ModProjectile 
    {
		bool runOnce = true;
        public override void SetDefaults()
        {
			projectile.height = 70;
			projectile.width = 70;
            Main.projFrames[projectile.type] = 5;
			projectile.penetrate = -1;
			projectile.friendly = true;
			projectile.melee = true;
			projectile.timeLeft = 24;
			projectile.tileCollide = false;
			projectile.hostile = false;
			projectile.alpha = 0;
		}
		public override bool ShouldUpdatePosition()
		{
			return false;
		}
		public override void AI()
        {
			Lighting.AddLight(projectile.Center, (255 - projectile.alpha) * 0.5f / 255f, (255 - projectile.alpha) * 1.5f / 255f, (255 - projectile.alpha) * 0.7f / 255f);
			if(runOnce && projectile.owner == Main.myPlayer)
			{
				runOnce = false;
				for (int i = 0; i < 2; i++)
				{
					Projectile proj = Projectile.NewProjectileDirect(projectile.Center, Main.rand.NextVector2Circular(3, 3), ProjectileID.SporeCloud, (int)(projectile.damage * 0.50f) + 1, 0, projectile.owner);
					proj.timeLeft = Main.rand.Next(16, 35);
				}
			}
            projectile.frameCounter++;
            if (projectile.frameCounter >= 5)
            {
				projectile.friendly = false;
                projectile.frameCounter = 0;
                projectile.frame = (projectile.frame + 1) % 5;
            }
        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.immune[projectile.owner] = 10;
        }
	}
}