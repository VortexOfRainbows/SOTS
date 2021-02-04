using Terraria;
using Terraria.ModLoader;

namespace SOTS.Projectiles.Pyramid
{    
    public class CurseSpear : ModProjectile 
    {	
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Cursed Impale");
			
		}
        public override void SetDefaults()
        {
            projectile.CloneDefaults(64);
            aiType = 64;
            projectile.melee = true;
			projectile.alpha = 0; 
		}
        int counter = 0;
        public override void AI()
        {
            counter++;
            if(counter == 10)
            {
                if(Main.myPlayer == projectile.owner)
                   Projectile.NewProjectile(projectile.Center, projectile.velocity, ModContent.ProjectileType<RubyBurst>(), projectile.damage, projectile.knockBack, projectile.owner);
            }
        }
    }
}