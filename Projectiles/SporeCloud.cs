using Terraria.ModLoader;
using Terraria.ID;
using Terraria;
using Microsoft.Xna.Framework;

namespace SOTS.Projectiles
{    
    public class SporeCloud : ModProjectile 
    {	
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Spore Cloud");
		}
        public override void SetDefaults()
		{
			Projectile.width = 40;
			Projectile.height = 38;
			Projectile.timeLeft = 120;
			Projectile.friendly = false;
			Projectile.hostile = true;
			Projectile.scale = 1.01f;
			Projectile.alpha = 120;
			Projectile.tileCollide = false;
		}
		public override bool PreAI()
		{
			if(Projectile.scale == 1.01f)
			{
				Projectile.scale = 0.1f;
			}
			Projectile.alpha++;
			Projectile.velocity *= 0.9f;
			Projectile.rotation += 0.1f;
			if(Projectile.scale < 1)
				Projectile.scale += 0.02f;

			if (Main.rand.Next(100) == 0)
			{
				Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 41, 0, 0, 250, new Color(100, 100, 100, 250), 0.8f);
			}
			return true;
		}
	}
}
		