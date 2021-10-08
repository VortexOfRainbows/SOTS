using Terraria.ModLoader;

namespace SOTS.Projectiles.Pyramid
{
	public class JuryRiggedDrill : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Jury Rigged Drill");
		}
		public override void SetDefaults() 
		{
			projectile.width = 20;
			projectile.height = 42;
			projectile.aiStyle = 20;
			projectile.friendly = true;
			projectile.penetrate = -1;
			projectile.tileCollide = false;
			projectile.hide = true;
			projectile.ownerHitCheck = true;
			projectile.melee = true;
		}
	}
}