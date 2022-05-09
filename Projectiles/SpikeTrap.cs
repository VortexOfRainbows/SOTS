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
			Projectile.CloneDefaults(24);
            aiType = 24; 
            Projectile.width = 18;
            Projectile.height = 18; 
            Projectile.timeLeft = 1275;
            Projectile.penetrate = 1; 
            Projectile.friendly = true; 
            Projectile.hostile = false; 
            Projectile.tileCollide = true;
            Projectile.ignoreWater = false; 
            Projectile.melee = true; 
			Projectile.alpha = 0;
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
			if(Projectile.timeLeft % 10 == 0)
				Projectile.alpha++;
        }
		public override void Kill(int timeLeft)
		{
			for(int i = 0; i < 4; i++)
				Gore.NewGore(new Vector2(Projectile.position.X, Projectile.position.Y), default(Vector2), Main.rand.Next(61,64), 0.45f);	
			SoundEngine.PlaySound(SoundID.Item, (int)(Projectile.Center.X), (int)(Projectile.Center.Y), 14, 0.3f);
		}
		public override bool OnTileCollide(Vector2 oldVelocity)
		{	
			Projectile.rotation = 0;
			Projectile.velocity.X = 0;
			Projectile.velocity.Y = 0;
			return false;
		}
	}
}
		
			