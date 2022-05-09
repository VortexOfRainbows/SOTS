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
            Projectile.CloneDefaults(ProjectileID.TheRottedFork); //obsidian swordfish
            aiType = ProjectileID.TheRottedFork;
            Projectile.melee = true;
			Projectile.alpha = 0; 
		}
	}
}