using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Projectiles.Ores
{    
    public class GoldSpear : ModProjectile 
    {	
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Gold Glaive");
		}
        public override void SetDefaults()
        {
            projectile.CloneDefaults(ProjectileID.TheRottedFork); //obsidian swordfish
            aiType = ProjectileID.TheRottedFork;
            projectile.melee = true;
			projectile.alpha = 0; 
		}
	}
}