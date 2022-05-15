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
            hitbox = new Rectangle((int)Projectile.Center.X - width / 2, (int)Projectile.Center.Y - width / 2, width, width);
        }
        public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Shattershine Pulse");
		}
        public override void SetDefaults()
        {
			Projectile.friendly = false;
			Projectile.DamageType = DamageClass.Melee;
			Projectile.tileCollide = true;
			Projectile.penetrate = -1;
			Projectile.width = 34;
			Projectile.height = 34;
			Projectile.alpha = 0;
			Projectile.timeLeft = 30;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 10;
            Projectile.scale = 0.8f;
        }
        public override Color? GetAlpha(Color lightColor)
        {
            return new Color(100, 100, 100, 0) * (1 - Projectile.alpha / 255f) * 2.5f;
        }
        bool runOnce = true;
        public override bool PreAI()
        {
            if(runOnce)
            {
                SoundEngine.PlaySound(SoundID.Item, (int)Projectile.Center.X, (int)Projectile.Center.Y, 27, 0.85f, -0.1f);
                Projectile.rotation = Projectile.ai[0];
                if(Projectile.ai[1] > Projectile.timeLeft)
                    Projectile.timeLeft = (int)Projectile.ai[1];
                runOnce = false;
            }
            Projectile.alpha = (int)(150 - 150 * Projectile.timeLeft / Projectile.ai[1]);
            Projectile.rotation = Projectile.velocity.ToRotation();
            Projectile.velocity *= 0.925f;
            if(Projectile.timeLeft < 3)
            {
                Projectile.friendly = true;
            }
            return true;
        }
        public void StarDust()
        {
            Vector2 startingLocation;
            float degrees = Main.rand.NextFloat(360);
            for (int j = 0; j < 4; j++)
            {
                Vector2 offset = new Vector2(0, 8).RotatedBy(MathHelper.ToRadians(j * 90) + Projectile.rotation);
                for (int i = -5; i < 5; i++)
                {
                    degrees += 360f / 40f;
                    startingLocation = new Vector2(i, 15 - Math.Abs(i) * 3).RotatedBy(MathHelper.ToRadians(j * 90) + Projectile.rotation);
                    Vector2 velo = offset + startingLocation;
                    Dust dust = Dust.NewDustPerfect(Projectile.Center + velo * 0.4f, ModContent.DustType<CopyDust4>());
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
            SoundEngine.PlaySound(SoundID.Item, (int)Projectile.Center.X, (int)Projectile.Center.Y, 105, 0.7f, -0.2f);
            StarDust();
        }
    }
}
		
			