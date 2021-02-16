using Microsoft.Xna.Framework;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria;

namespace SOTS.Projectiles 
{    
    public class GelAxe : ModProjectile 
    {	int wait = 0;
		float rotate = 0;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Gel Axe");
			
		}
		
        public override void SetDefaults()
        {
			projectile.CloneDefaults(3);
            aiType = 3; //18 is the demon scythe style
			projectile.penetrate = 3;
			projectile.alpha = 0;
			projectile.width = 32;
			projectile.height = 20;
		}
		public override bool OnTileCollide(Vector2 oldVelocity)
		{	
			projectile.tileCollide = false;
			projectile.timeLeft = 120;
			projectile.velocity.X *= 0.3f;
			projectile.velocity.Y *= 0.5f;
			projectile.aiStyle = 0;
			wait = 1;
			return false;
		}
        public override void Kill(int timeLeft)
		{
			for(int i = 0; i < 20; i ++)
			{
				Dust dust = Dust.NewDustDirect(projectile.position - new Vector2(5), projectile.width, projectile.height, DustID.t_Slime, 0, 0, projectile.alpha, new Color(0, 0, 255, 100), 1f);
				dust.scale *= 1.5f;
				dust.velocity *= 1.5f;
			}
			base.Kill(timeLeft);
        }
        public override void AI()
		{
			if(wait == 1)
			{
				projectile.velocity.X *= 0.9f;
				projectile.velocity.Y *= 0.9f;
			}
		}
	}
}
		
			