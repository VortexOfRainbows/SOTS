using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Projectiles.Otherworld
{    
    public class CodeBurst : ModProjectile 
    {	
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Code Burst");
		}
        public override void SetDefaults()
        {
            projectile.width = 32;
            projectile.height = 32;
			projectile.timeLeft = 72;
            projectile.penetrate = 1; 
            projectile.friendly = true; 
            projectile.tileCollide = true;
			projectile.magic = true;
			projectile.alpha = 255;
		}
        public override void Kill(int timeLeft)
		{
			for (int i = 0; i < 30; i++)
			{
				int num = Dust.NewDust(new Vector2(projectile.position.X - 4, projectile.position.Y - 4), projectile.width, projectile.height, mod.DustType("CodeDust"));
				Dust dust = Main.dust[num];
				dust.velocity *= 1.3f;
				dust.velocity += projectile.velocity * 0.3f;
				dust.noGravity = true;
				dust.scale *= 2.75f;
			}
		}
        public override bool OnTileCollide(Vector2 oldVelocity)
		{
			return true;
		}
        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough)
        {
			width = 16;
			height = 16;
            return base.TileCollideStyle(ref width, ref height, ref fallThrough);
        }
		public override void AI() //The projectile's AI/ what the projectile does
		{
			for(int i = 0; i < Main.rand.Next(2) + 1; i++)
			{
				int num = Dust.NewDust(new Vector2(projectile.Center.X - 4, projectile.Center.Y - 4), 4, 4, mod.DustType("CodeDust"));
				Dust dust = Main.dust[num];
				dust.velocity *= 0.6f;
				dust.velocity += projectile.velocity * 0.1f;
				dust.noGravity = true;
				dust.scale *= 1.75f;
			}
		}
	}
}