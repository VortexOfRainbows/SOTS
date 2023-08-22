using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Projectiles.Planetarium
{    
    public class CodeVolley : ModProjectile 
    {	
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Code Burst");
		}
        public override void SetDefaults()
        {
            Projectile.width = 16;
            Projectile.height = 16;
			Projectile.timeLeft = 10;
            Projectile.penetrate = -1; 
            Projectile.friendly = true; 
            Projectile.tileCollide = true;
			Projectile.ownerHitCheck = true;
			Projectile.DamageType = DamageClass.Magic;
			Projectile.alpha = 255;
		}
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
			Projectile.velocity *= 0.1f;
            return base.OnTileCollide(oldVelocity);
        }
        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
        {
			width = Projectile.width / 2;
			height = Projectile.height / 2;
            return true;
        }
		float counter = 0;
		public override void AI() //The projectile's AI/ what the projectile does
		{
			Projectile.width += 12;
			Projectile.height += 12;
			Projectile.position.X -= 6f;
			Projectile.position.Y -= 6f;
			counter += 0.5f;
			for (int i = 0; i < 2 + counter; i++)
			{
				int num = Dust.NewDust(new Vector2(Projectile.position.X - 4, Projectile.position.Y - 4), Projectile.width, Projectile.height, ModContent.DustType<Dusts.CodeDust>());
				Dust dust = Main.dust[num];
				dust.velocity *= 0.5f;
				dust.velocity += Projectile.velocity * 0.3f;
				dust.noGravity = true;
				dust.scale *= 2.8f;
			}
		}
	}
}