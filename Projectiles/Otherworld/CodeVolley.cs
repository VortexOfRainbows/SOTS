using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Projectiles.Otherworld
{    
    public class CodeVolley : ModProjectile 
    {	
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Code Burst");
		}
        public override void SetDefaults()
        {
            projectile.width = 16;
            projectile.height = 16;
			projectile.timeLeft = 10;
            projectile.penetrate = -1; 
            projectile.friendly = true; 
            projectile.tileCollide = true;
			projectile.ownerHitCheck = true;
			projectile.magic = true;
			projectile.alpha = 255;
		}
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
			projectile.velocity *= 0.1f;
            return base.OnTileCollide(oldVelocity);
        }
        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough)
        {
			width = projectile.width / 2;
			height = projectile.height / 2;
            return base.TileCollideStyle(ref width, ref height, ref fallThrough);
        }
		float counter = 0;
		public override void AI() //The projectile's AI/ what the projectile does
		{
			projectile.width += 12;
			projectile.height += 12;
			projectile.position.X -= 6f;
			projectile.position.Y -= 6f;
			counter += 0.5f;
			for (int i = 0; i < 2 + counter; i++)
			{
				int num = Dust.NewDust(new Vector2(projectile.position.X - 4, projectile.position.Y - 4), projectile.width, projectile.height, mod.DustType("CodeDust"));
				Dust dust = Main.dust[num];
				dust.velocity *= 0.5f;
				dust.velocity += projectile.velocity * 0.3f;
				dust.noGravity = true;
				dust.scale *= 2.8f;
			}
		}
	}
}