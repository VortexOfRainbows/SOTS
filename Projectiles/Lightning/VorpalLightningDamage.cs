using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using System;
using SOTS.Projectiles.Blades;

namespace SOTS.Projectiles.Lightning
{    
    public class VorpalLightningDamage : ModProjectile 
    {
		public override string Texture => "SOTS/Projectiles/Lightning/PurpleLightningDamage";
        public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Origin Thunder");
		}
        public override void SetDefaults()
		{
			Projectile.height = 24;
			Projectile.width = 24;
			Projectile.penetrate = 1;
			Projectile.friendly = true;
			Projectile.DamageType = ModContent.GetInstance<Void.VoidMelee>();
			Projectile.timeLeft = 6;
			Projectile.tileCollide = false;
			Projectile.alpha = 255;
		}
		public override bool ShouldUpdatePosition()
        {
			return false;
        }
        public override void OnKill(int timeLeft)
		{
			for (int i = 0; i < 360; i += 5)
			{
				if (Main.rand.NextBool(5))
				{
					Vector2 circularLocation = new Vector2(6, 0).RotatedBy(MathHelper.ToRadians(i));
					Dust dust = Dust.NewDustDirect(Projectile.Center, 0, 0, ModContent.DustType<Dusts.CopyDust4>());
					dust.noGravity = true;
					dust.velocity *= 0.8f;
					dust.velocity += circularLocation;
					dust.scale *= 1.5f;
					dust.fadeIn = 0.1f;
					dust.color = Color.Lerp(VorpalThrow.VorpalColor1, VorpalThrow.VorpalColor2, 1 - Main.rand.NextFloat(1f) * Main.rand.NextFloat(1f));
				}
			}
		}
	}
}
		