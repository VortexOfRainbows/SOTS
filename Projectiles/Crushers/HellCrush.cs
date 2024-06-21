using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using SOTS.Void;

namespace SOTS.Projectiles.Crushers
{    
    public class HellCrush : ModProjectile 
    {
		public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 5;
        }
        public override void SetDefaults()
        {
			Projectile.height = 70;
			Projectile.width = 70;
			Projectile.penetrate = -1;
			Projectile.DamageType = ModContent.GetInstance<VoidMelee>();
			Projectile.friendly = true;
			Projectile.timeLeft = 24;
			Projectile.tileCollide = false;
			Projectile.hostile = false;
			Projectile.alpha = 0;
			Projectile.ownerHitCheck = true;
		}
		bool runOnce = true;
        public override bool ShouldUpdatePosition()
        {
			return false;
        }
        public override void AI()
        {
			Lighting.AddLight(Projectile.Center, (255 - Projectile.alpha) * 1.5f / 255f, (255 - Projectile.alpha) * 1.5f / 255f, (255 - Projectile.alpha) * 1.5f / 255f);
			if(runOnce && Projectile.owner == Main.myPlayer)
			{
				runOnce = false;
				int ogDamage = (int)Projectile.ai[0];
				for (float i = 0; i < Projectile.damage; i += ogDamage * 2.5f)
				{ 
					Projectile proj = Projectile.NewProjectileDirect(Projectile.GetSource_FromThis(), Projectile.Center, Main.rand.NextVector2Circular(1.0f, 1.0f), ProjectileID.Flames, (int)(Projectile.damage * 0.1f), 0, Projectile.owner);
					proj.DamageType = ModContent.GetInstance<VoidMelee>();
				}
			}
            Projectile.frameCounter++;
            if (Projectile.frameCounter >= 5)
            {
				Projectile.friendly = false;
                Projectile.frameCounter = 0;
                Projectile.frame = (Projectile.frame + 1) % 5;
            }
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.immune[Projectile.owner] = 10;
			if(Main.rand.NextBool(3))
				target.AddBuff(BuffID.OnFire, 360, false);
        }
	}
}