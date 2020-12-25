using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ModLoader;

namespace SOTS.Projectiles.Otherworld
{    
    public class BombFlare : ModProjectile 
    {	
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Bomb Flare");
            Main.projFrames[projectile.type] = 3;
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
        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough)
        {
            width = 6;
            height = 6;
            return base.TileCollideStyle(ref width, ref height, ref fallThrough);
        }
        Vector2 lastVelo = new Vector2(0, 0);
        int counter = 0;
        public override void AI() //adapted directly from terraria source
        {
            projectile.frame = (int)projectile.ai[0];
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
            var num366 = lastVelo.X;
            var num367 = lastVelo.Y;
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
                Vector2 rotational = new Vector2(8, 0).RotatedBy(MathHelper.ToRadians(counter * 4));
                for(int i = -1; i < 2; i += 2)
                {
                    var num371 = Dust.NewDust(position116, 6, 6, mod.DustType("CopyDust4"), 0, 0, 100, newColor, 1.6f);
                    Dust dust = Main.dust[num371];
                    dust.position += new Vector2(0, rotational.X * i).RotatedBy(lastVelo.ToRotation());
                    dust.velocity.Y *= 0.1f;
                    dust.noGravity = true;
                    dust.position.X -= num366 * 1f;
                    dust.position.Y -= num367 * 1f;
                    dust.velocity.X -= num366;
                    dust.velocity.Y -= num367;
                    if (projectile.frame == 2)
                        dust.color = Color.Lerp(new Color(220, 60, 10, 40), new Color(255, 250, 130, 40), new Vector2(0.5f, 0).RotatedBy(MathHelper.ToRadians(counter * 4)).X + 0.5f);
                    if (projectile.frame == 1)
                        dust.color = Color.Lerp(new Color(0, 200, 220, 100), new Color(220, 200, 30, 100), new Vector2(-0.5f, 0).RotatedBy(MathHelper.ToRadians(counter * 3)).X + 0.5f);
                    if (projectile.frame == 0)
                        dust.color = new Color(255, 10, 10, 40);
                    dust.noGravity = true;
                    dust.fadeIn = 0.2f;
                    dust.scale *= 0.6f;
                    dust.alpha = projectile.alpha;
                }
            }

            if (projectile.localAI[0] == 0f)
            {
                lastVelo.X = projectile.velocity.X;
                lastVelo.Y = projectile.velocity.Y;
                projectile.localAI[1] += 1f;
                if (projectile.localAI[1] >= 30f)
                {
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
            Player player = Main.player[projectile.owner];
            if (!player.channel)
                projectile.Kill();
            projectile.rotation = (float)Math.Atan2((double)lastVelo.Y, (double)lastVelo.X) + 1.57f;
        }
        public override void Kill(int timeLeft)
        {
            Main.PlaySound(2, (int)(projectile.Center.X), (int)(projectile.Center.Y), 14, 0.4f);
            for (int i = 0; i < 3; i++)
            {
                int goreIndex = Gore.NewGore(new Vector2(projectile.Center.X, projectile.Center.Y), default(Vector2), Main.rand.Next(61, 64), 1f);
                Main.gore[goreIndex].scale = 1f;
                Main.gore[goreIndex].velocity.Y *= 0.45f;
                Main.gore[goreIndex].position -= new Vector2(20, 20);
                Main.gore[goreIndex].velocity.X *= 0.45f;
                Main.gore[goreIndex].velocity += projectile.velocity * 0.1f;
            }
            if (Main.myPlayer == projectile.owner)
            {
                for(int i = -1; i < 2; i++)
                {
                    Vector2 circular = lastVelo.SafeNormalize(Vector2.Zero).RotatedBy(MathHelper.ToRadians(18 * i)) * (projectile.frame == 2 ? 5 : projectile.frame == 1 ? 3.5f : 6.5f) * 1.075f;
                    Projectile.NewProjectile(projectile.Center, circular, mod.ProjectileType("TravelingFlareFlame"), projectile.damage, projectile.knockBack, projectile.owner, projectile.frame);
                }
            }
            base.Kill(timeLeft);
        }
        public override void PostAI()
        {
            Player player = Main.player[projectile.owner];
            if(Main.myPlayer == projectile.owner)
            {
                projectile.netUpdate = true;
                Vector2 projectileSpeed = projectile.velocity.SafeNormalize(Vector2.Zero) * projectile.velocity.Length();
                Vector2 add = Main.MouseWorld - player.Center;
                add = add.SafeNormalize(Vector2.Zero) * 1.4f;
                projectileSpeed += add;
                projectile.velocity = projectileSpeed.SafeNormalize(Vector2.Zero) * projectile.velocity.Length();
            }
            base.PostAI();
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
		