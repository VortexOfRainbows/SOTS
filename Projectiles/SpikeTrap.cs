using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace SOTS.Projectiles 
{    
    public class SpikeTrap : ModProjectile 
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Spike Trap");
		}
        public override void SetDefaults()
        {
			projectile.CloneDefaults(24);
            aiType = 24; 
            projectile.width = 18;
            projectile.height = 18; 
            projectile.timeLeft = 1275;
            projectile.penetrate = 1; 
            projectile.friendly = true; 
            projectile.hostile = false; 
            projectile.tileCollide = true;
            projectile.ignoreWater = false; 
            projectile.melee = true; 
			projectile.alpha = 0;
		}
		public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough)
		{
			width = 14;
			height = 14;
			fallThrough = false;
			return true;
		}
		public override void AI()
        {
			if(projectile.timeLeft % 10 == 0)
				projectile.alpha++;
        }
		public override void Kill(int timeLeft)
		{
			for(int i = 0; i < 4; i++)
				Gore.NewGore(new Vector2(projectile.position.X, projectile.position.Y), default(Vector2), Main.rand.Next(61,64), 0.45f);	
			SoundEngine.PlaySound(SoundID.Item, (int)(projectile.Center.X), (int)(projectile.Center.Y), 14, 0.3f);
		}
		public override bool OnTileCollide(Vector2 oldVelocity)
		{	
			projectile.rotation = 0;
			projectile.velocity.X = 0;
			projectile.velocity.Y = 0;
			return false;
		}
	}
}
		
			