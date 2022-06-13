using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace SOTS.Projectiles.Crushers
{    
    public class EclipseCrush : ModProjectile 
    {	
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Eclipse Crush");
		}
        public override void SetDefaults()
        {
			Projectile.height = 70;
			Projectile.width = 70;
            Main.projFrames[Projectile.type] = 6;
			Projectile.penetrate = -1;
			Projectile.friendly = true;
			Projectile.DamageType = DamageClass.Melee;
			Projectile.timeLeft = 23;
			Projectile.tileCollide = false;
			Projectile.hostile = false;
			Projectile.alpha = 0;
		}
		bool runOnce = true;
		public override void AI()
        {
			Projectile.alpha += 3;
			Lighting.AddLight(Projectile.Center, (255 - Projectile.alpha) * 1.0f / 255f, (255 - Projectile.alpha) * 0.4f / 255f, (255 - Projectile.alpha) * 1.8f / 255f);
			if(runOnce && Projectile.owner == Main.myPlayer)
			{
				runOnce = false;
				int ogDamage = (int)Projectile.ai[0];
				for (float i = 0; i < Projectile.damage; i += ogDamage * 2.0f)
				{
					Vector2 direction = new Vector2(Main.rand.NextFloat(-2f, 2f), Main.rand.NextFloat(-2f, 2f)) + Projectile.velocity.SafeNormalize(Vector2.Zero) * Main.rand.NextFloat(1f, 2f);
					int proj = Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center.X, Projectile.Center.Y, direction.X, direction.Y, ModContent.ProjectileType<EclipseBubble>(), (int)(Projectile.damage * 0.1f), 0, Projectile.owner);
					Main.projectile[proj].timeLeft = Main.rand.Next(52, 156);
					Main.projectile[proj].netUpdate = true;
				}
			}
            Projectile.frameCounter++;
            if (Projectile.frameCounter >= 4)
            {
				Projectile.friendly = false;
                Projectile.frameCounter = 0;
                Projectile.frame = (Projectile.frame + 1) % 6;
            }
        }
        public override bool ShouldUpdatePosition()
        {
			return false;
        }
    }
}
		