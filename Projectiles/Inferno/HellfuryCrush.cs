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

namespace SOTS.Projectiles.Inferno
{    
    public class HellfuryCrush : ModProjectile 
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Hellfury Crush");
		}
        public override void SetDefaults()
        {
			Projectile.height = 70;
			Projectile.width = 70;
            Main.projFrames[Projectile.type] = 5;
			Projectile.penetrate = -1;
			Projectile.DamageType = DamageClass.Melee;
			Projectile.friendly = true;
			Projectile.timeLeft = 24;
			Projectile.tileCollide = false;
			Projectile.hostile = false;
			Projectile.alpha = 0;
		}
		public override void ModifyDamageHitbox(ref Rectangle hitbox) 
		{
			hitbox = new Rectangle((int)(Projectile.position.X - Projectile.width/2), (int)(Projectile.position.Y - Projectile.height/2), Projectile.width * 2, Projectile.height * 2);
		}
		bool runOnce = true;
		public override void AI()
		{
			if(runOnce)
			{
				SoundEngine.PlaySound(SoundID.Item14, (int)(Projectile.Center.X), (int)(Projectile.Center.Y));
				runOnce = false;
            }
			Lighting.AddLight(Projectile.Center, (255 - Projectile.alpha) * 1.5f / 255f, (255 - Projectile.alpha) * 1.5f / 255f, (255 - Projectile.alpha) * 1.5f / 255f);
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
			target.AddBuff(BuffID.OnFire, 180, false);
        }
	}
}