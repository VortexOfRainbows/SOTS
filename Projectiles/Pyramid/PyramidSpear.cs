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
            Projectile.CloneDefaults(64);
            aiType = 64;
            Projectile.melee = true;
			Projectile.alpha = 0;
		}
		int storeData = -1;
		public override void PostAI()
		{
			if (storeData == -1 && Projectile.owner == Main.myPlayer)
			{
				storeData = Projectile.NewProjectile(Projectile.Center.X, Projectile.Center.Y, Projectile.velocity.X, Projectile.velocity.Y, mod.ProjectileType("EmeraldTrail"), (int)(Projectile.damage * 1f) + 1, Projectile.knockBack * 0.75f, Projectile.owner, 0, Projectile.whoAmI);
				Projectile.ai[1] = storeData;
				Projectile.netUpdate = true;
			}
		}
	}
}