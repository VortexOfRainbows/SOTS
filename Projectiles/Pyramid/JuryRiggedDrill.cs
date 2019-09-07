using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
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
			projectile.height = 32;
			projectile.aiStyle = 20;
			projectile.friendly = true;
			projectile.penetrate = -1;
			projectile.tileCollide = false;
			projectile.hide = true;
			projectile.ownerHitCheck = true;
			projectile.melee = true;
		}
		public override void Kill(int timeLeft)
		{
			
		}
	}
}