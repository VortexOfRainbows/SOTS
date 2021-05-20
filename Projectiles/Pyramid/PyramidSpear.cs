using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
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
            projectile.CloneDefaults(64);
            aiType = 64;
            projectile.melee = true;
			projectile.alpha = 0;
		}
		int storeData = -1;
		public override void PostAI()
		{
			if (storeData == -1 && projectile.owner == Main.myPlayer)
			{
				storeData = Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, projectile.velocity.X, projectile.velocity.Y, mod.ProjectileType("EmeraldTrail"), (int)(projectile.damage * 1f) + 1, projectile.knockBack * 0.75f, projectile.owner, 0, projectile.whoAmI);
				projectile.ai[1] = storeData;
				projectile.netUpdate = true;
			}
		}
	}
}