using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using SOTS.Void;
using Terraria.ID;

namespace SOTS.Projectiles.Planetarium
{    
    public class OriginLightningDamage : ModProjectile 
    {	
        public override void SetDefaults()
		{
			Projectile.height = 24;
			Projectile.width = 24;
			Projectile.penetrate = 1;
			Projectile.friendly = true;
			Projectile.DamageType = ModContent.GetInstance<VoidMagic>();
			Projectile.timeLeft = 6;
			Projectile.tileCollide = false;
			Projectile.alpha = 255;
		}
        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
			if (!target.boss && target.lifeMax <= 3600)
				modifiers.SourceDamage *= 2f;
        }
		public override bool ShouldUpdatePosition()
        {
			return false;
        }
        public override void OnKill(int timeLeft)
		{
			for (int h = 0; h < 8; h++)
			{
				Dust dust = Dust.NewDustDirect(new Vector2(Projectile.Center.X - 12, Projectile.Center.Y - 12), 16, 16, DustID.LifeDrain);
                dust.scale *= 3.5f;
                dust.velocity += Projectile.velocity * 0.004f;
                dust.noGravity = true;
			}
		}
	}
}
		