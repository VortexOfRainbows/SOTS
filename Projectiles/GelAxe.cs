using Microsoft.Xna.Framework;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria;

namespace SOTS.Projectiles 
{    
    public class GelAxe : ModProjectile 
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Gel Axe");
		}
        public override void SetDefaults()
        {
			Projectile.CloneDefaults(3);
            aiType = 3;
			Projectile.penetrate = 3;
			Projectile.alpha = 25;
			Projectile.width = 32;
			Projectile.height = 20;
		}
        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough)
        {
			width = 16;
			height = 12;
            return base.TileCollideStyle(ref width, ref height, ref fallThrough);
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
		{	
			Projectile.tileCollide = false;
			Projectile.timeLeft = 120;
			Projectile.velocity.X *= 0.3f;
			Projectile.velocity.Y *= 0.3f;
			Projectile.aiStyle = 0;
			return false;
		}
        public override void Kill(int timeLeft)
		{
			for(int i = 0; i < 20; i ++)
			{
				Dust dust = Dust.NewDustDirect(Projectile.position - new Vector2(5), Projectile.width, Projectile.height, DustID.t_Slime, 0, 0, 100, new Color(0, 0, 255, 100), 1f);
				dust.scale *= 1.1f;
				dust.velocity *= 1.15f;
			}
			base.Kill(timeLeft);
        }
        public override void AI()
		{
			if(!Projectile.tileCollide)
			{
				Projectile.velocity.X *= 0.9f;
				Projectile.velocity.Y *= 0.9f;
			}
		}
	}
}
		
			