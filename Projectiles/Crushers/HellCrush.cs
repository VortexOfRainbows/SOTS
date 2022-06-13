using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace SOTS.Projectiles.Crushers
{    
    public class HellCrush : ModProjectile 
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Hell Crush");
		}
        public override void SetDefaults()
        {
			Projectile.height = 70;
			Projectile.width = 70;
            Main.projFrames[Projectile.type] = 5;
			Projectile.penetrate = -1;
			Projectile.DamageType = DamageClass.Melee;
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
					int proj = Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center.X, Projectile.Center.Y, Main.rand.Next(-100, 101) * 0.015f, Main.rand.Next(-100, 101) * 0.015f, ProjectileID.Flames, (int)(Projectile.damage * 0.1f), 0, Projectile.owner);
					Main.projectile[proj].DamageType = DamageClass.Melee;
					Main.projectile[proj].timeLeft = Main.rand.Next(24, 60);
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
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.immune[Projectile.owner] = 10;
			if(Main.rand.NextBool(3))
				target.AddBuff(BuffID.OnFire, 360, false);
        }
	}
}