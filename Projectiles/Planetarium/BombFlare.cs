using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Projectiles.Planetarium
{    
    public class BombFlare : ModProjectile 
    {	
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Bomb Flare");
            Main.projFrames[Projectile.type] = 3;
        }
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
        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
        {
            width = 6;
            height = 6;
            return true;
        }
        Vector2 lastVelo = new Vector2(0, 0);
        int counter = 0;
        public override void AI() //adapted directly from terraria source
        {
            Projectile.frame = (int)Projectile.ai[0];
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
            if (Projectile.alpha < 70)
            {
                var position116 = new Vector2(Projectile.position.X, Projectile.position.Y - 2f);
                Color newColor = default(Color);
                Vector2 rotational = new Vector2(8, 0).RotatedBy(MathHelper.ToRadians(counter * 4));
                for(int i = -1; i < 2; i += 2)
                {
                    var num371 = Dust.NewDust(position116, 6, 6, ModContent.DustType<Dusts.CopyDust4>(), 0, 0, 100, newColor, 1.6f);
                    Dust dust = Main.dust[num371];
                    dust.position += new Vector2(0, rotational.X * i).RotatedBy(lastVelo.ToRotation());
                    dust.velocity.Y *= 0.1f;
                    dust.noGravity = true;
                    dust.position.X -= num366 * 1f;
                    dust.position.Y -= num367 * 1f;
                    dust.velocity.X -= num366;
                    dust.velocity.Y -= num367;
                    if (Projectile.frame == 2)
                        dust.color = Color.Lerp(new Color(220, 60, 10, 40), new Color(255, 250, 130, 40), new Vector2(0.5f, 0).RotatedBy(MathHelper.ToRadians(counter * 4)).X + 0.5f);
                    if (Projectile.frame == 1)
                        dust.color = Color.Lerp(new Color(0, 200, 220, 100), new Color(220, 200, 30, 100), new Vector2(-0.5f, 0).RotatedBy(MathHelper.ToRadians(counter * 3)).X + 0.5f);
                    if (Projectile.frame == 0)
                        dust.color = new Color(255, 10, 10, 40);
                    dust.noGravity = true;
                    dust.fadeIn = 0.2f;
                    dust.scale *= 0.6f;
                    dust.alpha = Projectile.alpha;
                }
            }

            if (Projectile.localAI[0] == 0f)
            {
                lastVelo.X = Projectile.velocity.X;
                lastVelo.Y = Projectile.velocity.Y;
                Projectile.localAI[1] += 1f;
                if (Projectile.localAI[1] >= 30f)
                {
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
            Player player = Main.player[Projectile.owner];
            if (!player.channel)
                Projectile.Kill();
            Projectile.rotation = (float)Math.Atan2((double)lastVelo.Y, (double)lastVelo.X) + 1.57f;
        }
        public override void Kill(int timeLeft)
        {
            SOTSUtils.PlaySound(SoundID.Item14, (int)(Projectile.Center.X), (int)(Projectile.Center.Y), 0.4f);
            for (int i = 0; i < 3; i++)
            {
                int goreIndex = Gore.NewGore(Projectile.GetSource_Death(), new Vector2(Projectile.Center.X, Projectile.Center.Y), default(Vector2), Main.rand.Next(61, 64), 1f);
                Main.gore[goreIndex].scale = 1f;
                Main.gore[goreIndex].velocity.Y *= 0.45f;
                Main.gore[goreIndex].position -= new Vector2(20, 20);
                Main.gore[goreIndex].velocity.X *= 0.45f;
                Main.gore[goreIndex].velocity += Projectile.velocity * 0.1f;
            }
            if (Main.myPlayer == Projectile.owner)
            {
                for(int i = -1; i < 2; i++)
                {
                    Vector2 circular = lastVelo.SafeNormalize(Vector2.Zero).RotatedBy(MathHelper.ToRadians(18 * i)) * (Projectile.frame == 2 ? 5 : Projectile.frame == 1 ? 3.5f : 6.5f) * 1.075f;
                    Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, circular, ModContent.ProjectileType<TravelingFlareFlame>(), Projectile.damage, Projectile.knockBack, Projectile.owner, Projectile.frame);
                }
            }
            base.Kill(timeLeft);
        }
        public override void PostAI()
        {
            Player player = Main.player[Projectile.owner];
            if(Main.myPlayer == Projectile.owner)
            {
                Projectile.netUpdate = true;
                Vector2 projectileSpeed = Projectile.velocity.SafeNormalize(Vector2.Zero) * Projectile.velocity.Length();
                Vector2 add = Main.MouseWorld - player.Center;
                add = add.SafeNormalize(Vector2.Zero) * 1.4f;
                projectileSpeed += add;
                Projectile.velocity = projectileSpeed.SafeNormalize(Vector2.Zero) * Projectile.velocity.Length();
            }
            base.PostAI();
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
		