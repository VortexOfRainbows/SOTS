using Terraria;
using Terraria.ModLoader;

namespace SOTS.Projectiles.Pyramid 
{    
    public class PyramidSpear : ModProjectile 
    {	
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Imperial Pike");
		}
        public override void SetDefaults()
        {
            Projectile.CloneDefaults(64);
            AIType = 64;
            Projectile.DamageType = DamageClass.Melee;
			Projectile.alpha = 0;
		}
		int storeData = -1;
		public override void PostAI()
		{
			if (storeData == -1 && Projectile.owner == Main.myPlayer)
			{
				storeData = Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center.X, Projectile.Center.Y, Projectile.velocity.X, Projectile.velocity.Y, ModContent.ProjectileType<EmeraldTrail>(), (int)(Projectile.damage * 1f) + 1, Projectile.knockBack * 0.75f, Projectile.owner, 0, Projectile.whoAmI);
				Projectile.ai[1] = storeData;
				Projectile.netUpdate = true;
			}
		}
	}
}