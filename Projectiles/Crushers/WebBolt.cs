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
			// DisplayName.SetDefault("Web Bolt");	
		}
        public override void SetDefaults()
        {
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.ignoreWater = false;
            Projectile.tileCollide = true;
            Projectile.timeLeft = 60;
			Projectile.penetrate = 1;
			Projectile.alpha = 0;
			Projectile.width = 8;
			Projectile.height = 12;
		}
        bool runOnce = true;
        public override void AI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.ToRadians(90);
            if(runOnce)
            {
                if(Main.myPlayer == Projectile.owner)
                {
                    Projectile.timeLeft -= Main.rand.Next(45);
                    Projectile.netUpdate = true;
                }
                runOnce = false;
            }
            Dust dust = Dust.NewDustDirect(Projectile.Center - new Vector2(5) - Projectile.velocity.SafeNormalize(Vector2.Zero) * 2, 0, 0, DustID.Web);
            dust.velocity *= 0f;
            dust.noGravity = true;
            dust.scale = 0.75f;
        }
        public override void OnKill(int timeLeft)
        {
            if(Main.myPlayer == Projectile.owner)
                Projectile.NewProjectileDirect(Projectile.GetSource_FromThis(), Projectile.Center, Projectile.velocity, ModContent.ProjectileType<Webbing>(), (int)(Projectile.damage * 0.5f), Projectile.knockBack, Projectile.owner);
            base.OnKill(timeLeft);
        }
    }
}