using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace SOTS.Projectiles.Crushers
{    
    public class WebBolt : ModProjectile 
    {	
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Web Bolt");	
		}
        public override void SetDefaults()
        {
            projectile.friendly = true;
            projectile.melee = true;
            projectile.ignoreWater = false;
            projectile.tileCollide = true;
            projectile.timeLeft = 60;
			projectile.penetrate = 1;
			projectile.alpha = 0;
			projectile.width = 8;
			projectile.height = 12;
		}
        bool runOnce = true;
        public override void AI()
        {
            projectile.rotation = projectile.velocity.ToRotation() + MathHelper.ToRadians(90);
            if(runOnce)
            {
                if(Main.myPlayer == projectile.owner)
                {
                    projectile.timeLeft -= Main.rand.Next(45);
                    projectile.netUpdate = true;
                }
                runOnce = false;
            }
            Dust dust = Dust.NewDustDirect(projectile.Center - new Vector2(5) - projectile.velocity.SafeNormalize(Vector2.Zero) * 2, 0, 0, DustID.Web);
            dust.velocity *= 0f;
            dust.noGravity = true;
            dust.scale = 0.75f;
        }
        public override void Kill(int timeLeft)
        {
            if(Main.myPlayer == projectile.owner)
                Projectile.NewProjectileDirect(projectile.Center, projectile.velocity, ModContent.ProjectileType<Webbing>(), (int)(projectile.damage * 0.5f), projectile.knockBack, projectile.owner);
            base.Kill(timeLeft);
        }
    }
}