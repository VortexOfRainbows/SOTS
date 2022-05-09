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
            Projectile.CloneDefaults(64);
            aiType = 64;
            Projectile.melee = true;
			Projectile.alpha = 0; 
		}
        int counter = 0;
        public override void AI()
        {
            counter++;
            if(counter == 10)
            {
                if(Main.myPlayer == Projectile.owner)
                   Projectile.NewProjectile(Projectile.Center, Projectile.velocity, ModContent.ProjectileType<RubyBurst>(), Projectile.damage, Projectile.knockBack, Projectile.owner);
            }
        }
    }
}