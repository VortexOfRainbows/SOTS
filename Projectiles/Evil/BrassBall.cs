using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using SOTS.Dusts;
using System.Net.Mail;

namespace SOTS.Projectiles.Evil
{    
    public class BrassBall : ModProjectile 
    {
        public override void SetDefaults()
        {
			Projectile.CloneDefaults(3);
            AIType = 3;
            Projectile.DamageType = DamageClass.Summon;
			Projectile.penetrate = -1;
			Projectile.width = 18;
			Projectile.height = 18;
			Projectile.alpha = 0;
			Projectile.timeLeft = 210;
            Projectile.idStaticNPCHitCooldown = 30;
            Projectile.usesIDStaticNPCImmunity = true;
		}
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            if (Projectile.velocity.X != oldVelocity.X)
            {
                Projectile.velocity.X = -oldVelocity.X;
            }
            if (Projectile.velocity.Y != oldVelocity.Y)
            {
                Projectile.velocity.Y = -oldVelocity.Y;
            }
            Projectile.velocity += Main.rand.NextVector2Circular(1, 1);
            Projectile.velocity *= 0.925f;
            return false;
        }
        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
        {
			width = 14;
			height = 14;
			return true;
        }
        public override bool? CanHitNPC(NPC target)
        {
            return (target.whoAmI != (int)Projectile.ai[0] || Projectile.timeLeft < 180) ? null : false;
        }
        public override void OnKill(int timeLeft)
		{
			for (int i = 0; i < 8; i++)
            {
                SOTSUtils.PlaySound(SoundID.Item17, (int)Projectile.Center.X, (int)Projectile.Center.Y, 0.25f, 0.5f);
                Dust dust = Dust.NewDustDirect(Projectile.Center - new Vector2(5), 0, 0, ModContent.DustType<SootDust>());
                dust.noGravity = true;
                dust.velocity *= 1.75f;
                dust.scale = dust.scale * 0.5f + 0.75f;
                dust.color = new Color(150, 170, 190, 0);
                dust.alpha = 100;
                if (Main.rand.NextBool(2))
                {
                    dust = Dust.NewDustDirect(Projectile.Center - new Vector2(5), 0, 0, ModContent.DustType<SootDust>());
                    dust.noGravity = false;
                    dust.velocity *= 1.1f;
                    dust.velocity.Y -= 0.5f;
                    dust.scale = dust.scale * 0.5f + 0.9f * Projectile.scale;
                    dust.color = new Color(150, 170, 190, 0);
                    dust.alpha = 100;
                }
            }
		}
        private bool RunOnce = true;
		public override void AI()
        {
            if (RunOnce)
            {
                SOTSUtils.PlaySound(SoundID.Item14, (int)Projectile.Center.X, (int)Projectile.Center.Y, 0.5f, 0.8f);
                for (int i = 0; i < 5; i++)
                {
                    Dust dust2 = Dust.NewDustDirect(Projectile.Center - new Vector2(5), 0, 0, ModContent.DustType<SootDust>());
                    dust2.noGravity = true;
                    dust2.velocity *= 1.25f;
                    dust2.velocity += Projectile.velocity;
                    dust2.scale = dust2.scale * 0.25f + 1.5f * Projectile.scale;
                    dust2.color = new Color(150, 170, 190, 0);
                    dust2.alpha = 10;
                }
                RunOnce = false;
            }
            if (Main.rand.NextBool(2))
            {
                Dust dust = Dust.NewDustDirect(Projectile.Center - new Vector2(5), 0, 0, ModContent.DustType<SootDust>());
                dust.noGravity = true;
                dust.velocity *= 0.35f;
                dust.scale = dust.scale * 0.5f + 0.7f * Projectile.scale;
                dust.color = new Color(150, 170, 190, 0);
                dust.alpha = 100;
                if (Main.rand.NextBool(5))
                {
                    dust = Dust.NewDustDirect(Projectile.Center - new Vector2(5), 0, 0, ModContent.DustType<SootDust>());
                    dust.noGravity = false;
                    dust.velocity *= 0.5f;
                    dust.scale = dust.scale * 0.5f + 0.5f * Projectile.scale;
                    dust.color = new Color(150, 170, 190, 0);
                    dust.alpha = 100; 
                }
            }
        }
    }
}
		
			