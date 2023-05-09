using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using SOTS.Dusts;
using SOTS.Void;

namespace SOTS.Projectiles.Celestial
{    
    public class SmallStellarHitbox : ModProjectile 
    {	
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Starsplosion");
		}
        public override void SetDefaults()
        {
			Projectile.height = 80;
			Projectile.width = 80;
			Projectile.penetrate = -1;
			Projectile.friendly = true;
			Projectile.timeLeft = 5;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 20;
			Projectile.tileCollide = false;
			Projectile.hostile = false;
			Projectile.alpha = 255;
		}
        public override bool? CanHitNPC(NPC target)
        {
            return target.whoAmI != Projectile.ai[0];
        }
		bool runOnce = true;
        public override void AI()
		{
			if(runOnce)
			{
				float size = 32f;
				float starPosX = Projectile.Center.X - size / 2f;
				float starPosY = Projectile.Center.Y - size / 6f;
				float iterateBy = 2f;
				for (int i = 0; i < 5; i++)
				{
					float rads = MathHelper.ToRadians(144 * i);
					for (float j = 0; j < size; j += iterateBy)
					{
						Vector2 direction = -(Projectile.Center - new Vector2(starPosX, starPosY)).SafeNormalize(Vector2.Zero);
						Dust dust = Dust.NewDustDirect(new Vector2(starPosX, starPosY), 0, 0, ModContent.DustType<CopyDust4>(), 50);
						dust.noGravity = true;
						dust.fadeIn = 0.1f;
						dust.scale = 1.3f;
						dust.color = ColorHelpers.pastelAttempt(MathHelper.ToRadians(i * 60 + j / size * 60), true);
						dust.velocity = direction * 1.5f + Main.rand.NextVector2Circular(0.1f, 0.1f);
						Vector2 rotationDirection = new Vector2(iterateBy, 0).RotatedBy(rads);
						starPosX += rotationDirection.X;
						starPosY += rotationDirection.Y;
					}
				}
				SOTSUtils.PlaySound(SoundID.Item9, (int)Projectile.Center.X, (int)Projectile.Center.Y, 1f, 0.2f);
				runOnce = false;
			}
		}
    }
}
		