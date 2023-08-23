using Microsoft.Xna.Framework;
using SOTS.Dusts;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Projectiles.Tide
{    
    public class PogBubble : ModProjectile 
    {
        public override void SetDefaults()
        {
			Projectile.height = 20;
			Projectile.width = 20;
			Projectile.DamageType = DamageClass.Ranged;
			Projectile.friendly = true;
			Projectile.penetrate = 2;
			Projectile.alpha = 105;
			Projectile.timeLeft = 125;
			Projectile.tileCollide = true;
            Projectile.hide = true;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 20;
        }
		public override void AI()
		{
			Projectile.hide = false;
			float slowdownMult = 0.925f + Projectile.ai[1] * 0.00125f;
			if(slowdownMult > 1)
				slowdownMult = 1;
            Projectile.velocity *= slowdownMult;
			Projectile.velocity.Y -= 0.08f + Projectile.ai[1] * 0.00175f;
			Projectile.ai[1]++;

            if (Projectile.ai[1] > 2)
            {
                Dust dust = Dust.NewDustDirect(Projectile.Center - new Vector2(12, 12), 14, 14, ModContent.DustType<CopyDust4>());
                dust.noGravity = true;
                dust.velocity *= 0.15f;
                if (Projectile.ai[1] > 10)
                    dust.velocity -= Projectile.velocity * Main.rand.NextFloat(0.4f, 1f);
                dust.scale *= 0.7f;
                dust.fadeIn = 0.2f;
                dust.color = Color.Lerp(new Color(53, 217, 241, 0), new Color(51, 238, 195, 0), Main.rand.NextFloat(1));
                dust.alpha = 200;
            }
        }
		public override void Kill(int timeLeft)
		{
			SOTSUtils.PlaySound(SoundID.Item54, Projectile.Center, 0.75f, -0.1f, 0.05f);
			for(int i = 0; i < 24; i++)
			{
				Vector2 circular = new Vector2(3, 0).RotatedBy(i / 24f * MathHelper.TwoPi);
				Dust dust = Dust.NewDustDirect(Projectile.Center - new Vector2(5, 5) + circular * 2, 0, 0, ModContent.DustType<CopyDust4>());
                dust.noGravity = true;
				dust.velocity *= 0.15f;
				dust.velocity += circular * Main.rand.NextFloat(0.4f, 1f);
                dust.scale *= 0.5f;
				dust.fadeIn = 0.2f;
				dust.color = Color.Lerp(new Color(53, 217, 241, 0), new Color(51, 238, 195, 0), Main.rand.NextFloat(1));
				dust.alpha = 100;
			}
		}
	}
}
		