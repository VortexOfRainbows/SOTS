using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace SOTS.Projectiles.Crushers
{    
    public class BubbleCrush : ModProjectile 
    {	
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Bubble Crush");
		}
        public override void SetDefaults()
        {
			Projectile.height = 70;
			Projectile.width = 70;
            Main.projFrames[Projectile.type] = 5;
			Projectile.penetrate = -1;
			Projectile.friendly = true;
			Projectile.DamageType = DamageClass.Melee;
			Projectile.timeLeft = 24;
			Projectile.tileCollide = false;
			Projectile.hostile = false;
			Projectile.alpha = 0;
			Projectile.ownerHitCheck = true;
		}
		bool runOnce = true;
		public override void AI()
        {
			Lighting.AddLight(Projectile.Center, (255 - Projectile.alpha) * 1.0f / 255f, (255 - Projectile.alpha) * 1.75f / 255f, (255 - Projectile.alpha) * 1.75f / 255f);
			if(runOnce)
			{
				runOnce = false;
				int ogDamage = (int)Projectile.ai[0];
				for (float i = Main.rand.NextFloat(-1, 0); i < Projectile.damage; i += ogDamage * 1.5f)
				{
					int proj = Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center.X, Projectile.Center.Y, Main.rand.NextFloat(-4f, 4f), Main.rand.NextFloat(-4f, 4f), ProjectileID.Bubble, 0, 0, 0);
					Main.projectile[proj].friendly = false;
					Main.projectile[proj].hostile = false;
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
        public override bool ShouldUpdatePosition()
        {
            return false;
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.immune[Projectile.owner] = 10;
        }
	}
}