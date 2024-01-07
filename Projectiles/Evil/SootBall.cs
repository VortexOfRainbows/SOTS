using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.Audio;
using SOTS.Dusts;
using SOTS.Void;
using System.Security.Permissions;
using SOTS.Buffs.Debuffs;

namespace SOTS.Projectiles.Evil
{    
    public class SootBall : ModProjectile 
    {
        public override void SetDefaults()
        {
			Projectile.CloneDefaults(3);
            AIType = 3;
            Projectile.DamageType = ModContent.GetInstance<VoidRanged>();
			Projectile.penetrate = -1;
			Projectile.width = 20;
			Projectile.height = 20;
			Projectile.alpha = 0;
			Projectile.timeLeft = 640;
            Projectile.idStaticNPCHitCooldown = 30;
            Projectile.usesIDStaticNPCImmunity = true;
		}
        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
        {
			width = 14;
			height = 14;
			return true;
        }
        public override void OnKill(int timeLeft)
		{
			SOTSUtils.PlaySound(SoundID.Item14, (int)Projectile.Center.X, (int)Projectile.Center.Y, 0.55f, 0.7f);
			for (int i = 0; i < 8; i++)
            {
                Dust dust = Dust.NewDustDirect(Projectile.Center - new Vector2(5), 0, 0, ModContent.DustType<SootDust>());
                dust.noGravity = true;
                dust.velocity *= 2.0f;
                dust.scale = dust.scale * 0.5f + 1f;
                if (Main.rand.NextBool(2))
                {
                    dust = Dust.NewDustDirect(Projectile.Center - new Vector2(5), 0, 0, ModContent.DustType<SootDust>());
                    dust.noGravity = false;
                    dust.velocity *= 1.1f;
                    dust.velocity.Y -= 0.5f;
                    dust.scale = dust.scale * 0.5f + 0.9f * Projectile.scale;
                }
            }
		}
        private bool RunOnce = true;
		public override void AI()
        {
            if (RunOnce)
            {
                Projectile.scale = Projectile.ai[1];
                for (int i = 0; i < 5; i++)
                {
                    Dust dust2 = Dust.NewDustDirect(Projectile.Center - new Vector2(5), 0, 0, ModContent.DustType<SootDust>());
                    dust2.noGravity = true;
                    dust2.velocity *= 1.25f;
                    dust2.velocity += Projectile.velocity;
                    dust2.scale = dust2.scale * 0.25f + 1.5f * Projectile.scale;
                }
                RunOnce = false;
            }
			Dust dust = Dust.NewDustDirect(Projectile.Center - new Vector2(5), 0, 0, ModContent.DustType<SootDust>());
			dust.noGravity = true;
			dust.velocity *= 0.35f;
            dust.scale = dust.scale * 0.5f + 0.7f * Projectile.scale;
			if(Main.rand.NextBool(5))
			{
                dust = Dust.NewDustDirect(Projectile.Center - new Vector2(5), 0, 0, ModContent.DustType<SootDust>());
                dust.noGravity = false;
                dust.velocity *= 0.5f;
                dust.scale = dust.scale * 0.45f + 0.45f * Projectile.scale;
            }
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(ModContent.BuffType<EmberOiled>(), 1200, false);
        }
    }
}
		
			