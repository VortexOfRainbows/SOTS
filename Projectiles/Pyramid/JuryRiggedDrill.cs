using Terraria.ModLoader;

namespace SOTS.Projectiles.Pyramid
{
	public class JuryRiggedDrill : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Jury Rigged Drill");
		}
		public override void SetDefaults() 
		{
			Projectile.width = 22;
			Projectile.height = 42;
			Projectile.aiStyle = 20;
			Projectile.friendly = true;
			Projectile.penetrate = -1;
			Projectile.tileCollide = false;
			Projectile.hide = true;
			Projectile.ownerHitCheck = true;
			Projectile.DamageType = DamageClass.Melee;
		}
    }
}