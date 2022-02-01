using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Void;
using SOTS.Dusts;
using Terraria.ID;
using SOTS.Utilities;
using System;

namespace SOTS.Projectiles.Earth 
{    
    public class ShattershinePulse : ModProjectile
    {
        public override void ModifyDamageHitbox(ref Rectangle hitbox)
        {
            int width = 70;
            hitbox = new Rectangle((int)projectile.Center.X - width / 2, (int)projectile.Center.Y - width / 2, width, width);
        }
        public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Shattershine Pulse");
		}
        public override void SetDefaults()
        {
			projectile.friendly = false;
			projectile.melee = true;
			projectile.tileCollide = true;
			projectile.penetrate = -1;
			projectile.width = 34;
			projectile.height = 34;
			projectile.alpha = 0;
			projectile.timeLeft = 30;
			projectile.usesLocalNPCImmunity = true;
			projectile.localNPCHitCooldown = 10;
            projectile.scale = 0.8f;
        }
        public override Color? GetAlpha(Color lightColor)
        {
            return new Color(100, 100, 100, 0) * (1 - projectile.alpha / 255f) * 2.5f;
        }
        bool runOnce = true;
        public override bool PreAI()
        {
            if(runOnce)
            {
                Main.PlaySound(SoundID.Item, (int)projectile.Center.X, (int)projectile.Center.Y, 27, 0.85f, -0.1f);
                projectile.rotation = projectile.ai[0];
                if(projectile.ai[1] > projectile.timeLeft)
                    projectile.timeLeft = (int)projectile.ai[1];
                runOnce = false;
            }
            projectile.alpha = (int)(150 - 150 * projectile.timeLeft / projectile.ai[1]);
            projectile.rotation = projectile.velocity.ToRotation();
            projectile.velocity *= 0.925f;
            if(projectile.timeLeft < 3)
            {
                projectile.friendly = true;
            }
            return true;
        }
        public void StarDust()
        {
            Vector2 startingLocation;
            float degrees = Main.rand.NextFloat(360);
            for (int j = 0; j < 4; j++)
            {
                Vector2 offset = new Vector2(0, 8).RotatedBy(MathHelper.ToRadians(j * 90) + projectile.rotation);
                for (int i = -5; i < 5; i++)
                {
                    degrees += 360f / 40f;
                    startingLocation = new Vector2(i, 15 - Math.Abs(i) * 3).RotatedBy(MathHelper.ToRadians(j * 90) + projectile.rotation);
                    Vector2 velo = offset + startingLocation;
                    Dust dust = Dust.NewDustPerfect(projectile.Center + velo * 0.4f, ModContent.DustType<CopyDust4>());
                    dust.noGravity = true;
                    dust.velocity *= 0.2f;
                    dust.scale = 1.4f + Main.rand.NextFloat(-0.1f, 0.1f);
                    dust.fadeIn = 0.1f;
                    dust.alpha = 100;
                    dust.color = VoidPlayer.VibrantColorAttempt(degrees);
                    dust.velocity += velo * 0.12f;
                }
            }
        }
        public override void Kill(int timeLeft)
        {
            Main.PlaySound(SoundID.Item, (int)projectile.Center.X, (int)projectile.Center.Y, 105, 0.7f, -0.2f);
            StarDust();
        }
    }
}
		
			