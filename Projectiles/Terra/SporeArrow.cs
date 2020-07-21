using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace SOTS.Projectiles.Terra
{    
    public class SporeArrow : ModProjectile 
    {	
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Spore Arrow");
		}
        public override void SetDefaults()
        {
			projectile.CloneDefaults(1);
            aiType = 1; 
			projectile.width = 14;
			projectile.height = 30;
			projectile.extraUpdates = 2;
		}
		public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough)
		{
			fallThrough = true;
			width = 8;
			height = 20;
			return true;
		}
		public override void Kill(int timeLeft)
		{
			for(int i = 0; i < 4; i++)
				Dust.NewDust(new Vector2(projectile.Center.X - 3, projectile.Center.Y - 3), 2, 2, 41, 0, 0, 250, new Color(100, 100, 100, 250), 0.8f);
		}
		public override void AI()
		{
			projectile.alpha = 0;
			if(Main.rand.Next(100) == 0)
				Dust.NewDust(new Vector2(projectile.Center.X - 3, projectile.Center.Y - 3), 2, 2, 41, 0, 0, 250, new Color(100, 100, 100, 250), 0.8f);
		}
	}
}
		
			