using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ModLoader;

namespace SOTS.Projectiles.Otherworld
{    
    public class BlackFlareShot : ModProjectile 
    {	
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Black Flare");
			
		}
        public override void SetDefaults()
        {
            projectile.tileCollide = true;
            projectile.netImportant = true;
            projectile.width = 6;
            projectile.height = 6;
            projectile.friendly = true;
            projectile.penetrate = -1;
            projectile.alpha = (int)byte.MaxValue;
            projectile.timeLeft = 36000;
        }
        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            damage += (int)(target.defense * 0.5f);
        }
        int counter = 0;
        public override void AI() //adapted directly from terraria source
        {
            counter++;
            if (projectile.alpha > 0)
            {
                projectile.alpha -= 50;
                if (projectile.alpha < 0)
                {
                    projectile.alpha = 0;
                }
            }

            var dist = 10f;
            var num366 = projectile.ai[0];
            var num367 = projectile.ai[1];
            if (num366 == 0f && num367 == 0f)
            {
                num366 = 1f;
            }

            var num368 = (float)Math.Sqrt((double)(num366 * num366 + num367 * num367));
            num368 = dist / num368;
            num366 *= num368;
            num367 *= num368;
            if (projectile.alpha < 70)
            {
                var position116 = new Vector2(projectile.position.X, projectile.position.Y - 2f);
                Color newColor = default(Color);
                var num371 = Dust.NewDust(position116, 6, 6, mod.DustType("CopyDust4"), 0, 0, 100, newColor, 1.6f);
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
                dust.alpha = projectile.alpha;
            }

            if (projectile.localAI[0] == 0f)
            {
                projectile.ai[0] = projectile.velocity.X;
                projectile.ai[1] = projectile.velocity.Y;
                projectile.localAI[1] += 1f;
                if (projectile.localAI[1] >= 30f)
                {
                    projectile.velocity.Y += 0.09f;
                    projectile.localAI[1] = 30f;
                }
            }
            else
            {
                if (!Collision.SolidCollision(projectile.position, projectile.width, projectile.height))
                {
                    projectile.localAI[0] = 0f;
                    projectile.localAI[1] = 30f;
                }
                projectile.damage = 0;
            }
            if (projectile.velocity.Y > 16f)
            {
                projectile.velocity.Y = 16f;
            }
            projectile.rotation = (float)Math.Atan2((double)projectile.ai[1], (double)projectile.ai[0]) + 1.57f;
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            if (projectile.localAI[0] == 0f)
            {
                if (projectile.wet)
                {
                    projectile.position = projectile.position + oldVelocity / 2f;
                }
                else
                {
                    projectile.position = projectile.position + oldVelocity;
                }
                projectile.velocity = projectile.velocity * 0f;
                projectile.localAI[0] = 1f;
            }
            return false;
        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            if (Main.rand.Next(3) == 0)
               target.AddBuff(24, 900, false);
            else
               target.AddBuff(24, 600, false);
        }
	}
}
		