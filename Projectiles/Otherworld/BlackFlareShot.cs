using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ModLoader;

namespace SOTS.Projectiles.Otherworld
{    
    public class BlackFlareShot : ModProjectile 
    {	
        public override void SetDefaults()
        {
            Projectile.tileCollide = true;
            Projectile.netImportant = true;
            Projectile.width = 6;
            Projectile.height = 6;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.alpha = (int)byte.MaxValue;
            Projectile.timeLeft = 36000;
        }
        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            modifiers.ScalingArmorPenetration += 1f;
        }
        int counter = 0;
        public override void AI() //adapted directly from terraria source
        {
            counter++;
            if (Projectile.alpha > 0)
            {
                Projectile.alpha -= 50;
                if (Projectile.alpha < 0)
                {
                    Projectile.alpha = 0;
                }
            }

            var dist = 10f;
            var num366 = Projectile.ai[0];
            var num367 = Projectile.ai[1];
            if (num366 == 0f && num367 == 0f)
            {
                num366 = 1f;
            }

            var num368 = (float)Math.Sqrt((double)(num366 * num366 + num367 * num367));
            num368 = dist / num368;
            num366 *= num368;
            num367 *= num368;
            if (Projectile.alpha < 70)
            {
                var position116 = new Vector2(Projectile.position.X, Projectile.position.Y - 2f);
                Color newColor = default(Color);
                var num371 = Dust.NewDust(position116, 6, 6, ModContent.DustType<Dusts.CopyDust4>(), 0, 0, 100, newColor, 1.6f);
                Dust dust = Main.dust[num371];
                dust.velocity.Y *= 0.4f;
                dust.noGravity = true;
                dust.position.X -= num366 * 1f;
                dust.position.Y -= num367 * 1f;
                dust.velocity.X -= num366;
                dust.velocity.Y -= num367;
                dust.color = Color.Lerp(new Color(220, 60, 10, 40), new Color(255, 250, 130, 40), new Vector2(0.5f, 0).RotatedBy(MathHelper.ToRadians(counter * 4)).X + 0.5f);
                dust.noGravity = true;
                dust.fadeIn = 0.2f;
                dust.scale *= 0.6f;
                dust.alpha = Projectile.alpha;
            }

            if (Projectile.localAI[0] == 0f)
            {
                Projectile.ai[0] = Projectile.velocity.X;
                Projectile.ai[1] = Projectile.velocity.Y;
                Projectile.localAI[1] += 1f;
                if (Projectile.localAI[1] >= 30f)
                {
                    Projectile.velocity.Y += 0.09f;
                    Projectile.localAI[1] = 30f;
                }
            }
            else
            {
                if (!Collision.SolidCollision(Projectile.position, Projectile.width, Projectile.height))
                {
                    Projectile.localAI[0] = 0f;
                    Projectile.localAI[1] = 30f;
                }
                Projectile.damage = 0;
            }
            if (Projectile.velocity.Y > 16f)
            {
                Projectile.velocity.Y = 16f;
            }
            Projectile.rotation = (float)Math.Atan2((double)Projectile.ai[1], (double)Projectile.ai[0]) + 1.57f;
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            if (Projectile.localAI[0] == 0f)
            {
                if (Projectile.wet)
                {
                    Projectile.position = Projectile.position + oldVelocity / 2f;
                }
                else
                {
                    Projectile.position = Projectile.position + oldVelocity;
                }
                Projectile.velocity = Projectile.velocity * 0f;
                Projectile.localAI[0] = 1f;
            }
            return false;
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (Main.rand.Next(3) == 0)
               target.AddBuff(24, 900, false);
            else
               target.AddBuff(24, 600, false);
        }
	}
}
		