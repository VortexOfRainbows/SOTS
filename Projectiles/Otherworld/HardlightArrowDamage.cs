using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using SOTS.Projectiles.Permafrost;

namespace SOTS.Projectiles.Otherworld
{    
    public class HardlightArrowDamage : ModProjectile 
    {	
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Hardlight Arrow");
		}
        public override void SetDefaults()
		{
			Projectile.height = 24;
			Projectile.width = 24;
			Projectile.penetrate = 1;
			Projectile.friendly = true;
			Projectile.DamageType = DamageClass.Ranged;
			Projectile.timeLeft = 6;
			Projectile.tileCollide = false;
			Projectile.hostile = false;
			Projectile.alpha = 255;
		}
        public override bool ShouldUpdatePosition()
        {
			return false;
        }
        public override void Kill(int timeLeft)
		{
			int frostFlake = (int)Projectile.ai[0];
			if(frostFlake > 0 && Main.myPlayer == Projectile.owner)
			{
				float damageMult = 2;
				if (frostFlake == 2)
					damageMult = 6;
				Projectile.NewProjectile(Projectile.Center, Projectile.velocity, ModContent.ProjectileType<FrostflakePulse>(), (int)(Projectile.damage * damageMult), Projectile.knockBack, Main.myPlayer, frostFlake, 0);
			}
			for (int h = 0; h < 20; h++)
			{
				int dust = Dust.NewDust(new Vector2(Projectile.Center.X - 12, Projectile.Center.Y - 12), 16, 16, DustID.Electric);
				Main.dust[dust].scale *= 1f;
				Main.dust[dust].velocity += Projectile.velocity * 0.1f;
				Main.dust[dust].noGravity = true;
			}
		}
	}
}
		