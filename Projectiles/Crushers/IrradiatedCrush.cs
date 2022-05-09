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
			Projectile.height = 70;
			Projectile.width = 70;
            Main.projFrames[Projectile.type] = 5;
			Projectile.penetrate = -1;
			Projectile.friendly = true;
			Projectile.melee = true;
			Projectile.timeLeft = 24;
			Projectile.tileCollide = false;
			Projectile.hostile = false;
			Projectile.alpha = 0;
		}
		public override bool ShouldUpdatePosition()
		{
			return false;
		}
		public override void AI()
        {
			Lighting.AddLight(Projectile.Center, (255 - Projectile.alpha) * 0.5f / 255f, (255 - Projectile.alpha) * 1.5f / 255f, (255 - Projectile.alpha) * 0.7f / 255f);
			if(runOnce && Projectile.owner == Main.myPlayer)
			{
				runOnce = false;
				for (int i = 0; i < 2; i++)
				{
					Projectile proj = Projectile.NewProjectileDirect(Projectile.Center, Main.rand.NextVector2Circular(3, 3), ProjectileID.SporeCloud, (int)(Projectile.damage * 0.50f) + 1, 0, Projectile.owner);
					proj.timeLeft = Main.rand.Next(16, 35);
				}
			}
            Projectile.frameCounter++;
            if (Projectile.frameCounter >= 5)
            {
				Projectile.friendly = false;
                Projectile.frameCounter = 0;
                Projectile.frame = (Projectile.frame + 1) % 5;
            }
        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.immune[Projectile.owner] = 10;
        }
	}
}