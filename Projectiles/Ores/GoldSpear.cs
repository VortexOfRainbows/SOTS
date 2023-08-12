using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Projectiles.Ores
{    
    public class GoldSpear : ModProjectile 
    {	
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Gold Glaive");
		}
        public override void SetDefaults()
        {
            Projectile.CloneDefaults(ProjectileID.TheRottedFork); //obsidian swordfish
            AIType = ProjectileID.TheRottedFork;
            Projectile.DamageType = DamageClass.Melee;
			Projectile.alpha = 0; 
		}
	}
}