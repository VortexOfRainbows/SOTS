using Microsoft.Xna.Framework;
using SOTS.Dusts;
using Terraria;
using Terraria.ModLoader;

namespace SOTS.Projectiles.Minions
{    
    public class VoidspaceExplosion : ModProjectile 
    {
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Voidspace Explosion");
		}
        public override void SetDefaults()
        {
			Projectile.height = 24;
			Projectile.width = 24;
			Projectile.penetrate = -1;
			Projectile.friendly = true;
			Projectile.timeLeft = 2;
			Projectile.tileCollide = false;
			Projectile.hostile = false;
			Projectile.alpha = 255;
			Projectile.DamageType = DamageClass.Summon;
		}
        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
			crit = false;
        }
        public override void Kill(int timeLeft)
		{
			for (int i = 0; i < 360; i += 20)
			{
				Vector2 circularLocation = new Vector2(-12, 0).RotatedBy(MathHelper.ToRadians(i));
				Dust dust = Dust.NewDustDirect(new Vector2(Projectile.Center.X + circularLocation.X - 4, Projectile.Center.Y + circularLocation.Y - 4), 4, 4, 107);
				dust.noGravity = true;
				dust.velocity *= 0.1f;
				dust.velocity += circularLocation * 0.25f;
				dust.scale *= 1.25f;
			}
			for (int i = 0; i < 360; i += 40)
			{
				Vector2 circularLocation = new Vector2(-12, 0).RotatedBy(MathHelper.ToRadians(i));
				Dust dust = Dust.NewDustDirect(new Vector2(Projectile.Center.X + circularLocation.X - 4, Projectile.Center.Y + circularLocation.Y - 4), 4, 4, ModContent.DustType<CopyDust4>());
				dust.noGravity = true;
				dust.velocity *= 0.5f;
				dust.velocity += circularLocation * 0.125f;
				dust.scale *= 2.5f;
				dust.fadeIn = 0.1f;
				dust.color = new Color(33, 100, 33);
			}
		}
		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.immune[Projectile.owner] = 0;
			Projectile.friendly = false;
        }
	}
}
		