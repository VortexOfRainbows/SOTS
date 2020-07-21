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
			DisplayName.SetDefault("Spore Cloud");
		}
        public override void SetDefaults()
		{
			projectile.width = 40;
			projectile.height = 38;
			projectile.timeLeft = 120;
			projectile.friendly = false;
			projectile.hostile = true;
			projectile.scale = 1.01f;
			projectile.alpha = 120;
			projectile.tileCollide = false;
		}
		public override bool PreAI()
		{
			if(projectile.scale == 1.01f)
			{
				projectile.scale = 0.1f;
			}
			projectile.alpha++;
			projectile.velocity *= 0.9f;
			projectile.rotation += 0.1f;
			if(projectile.scale < 1)
				projectile.scale += 0.02f;

			if (Main.rand.Next(100) == 0)
			{
				Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 41, 0, 0, 250, new Color(100, 100, 100, 250), 0.8f);
			}
			return true;
		}
	}
}
		