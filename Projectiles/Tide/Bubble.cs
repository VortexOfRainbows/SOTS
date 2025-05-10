using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
 
namespace SOTS.Projectiles.Tide
{    
    public class Bubble : ModProjectile 
    {
		public override void SetStaticDefaults()
		{
            Main.projFrames[Projectile.type] = 8;
        }
		
		public override void SetDefaults()
        {
            Projectile.width = 34;
            Projectile.height = 34;
			Projectile.friendly = true;
            Projectile.hostile = false; 
			Projectile.timeLeft = 18000;
			Projectile.alpha = 0;
		}
		public override bool? CanDamage() => false;
        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
        {
			width = 22;
			height = 22;
            return true;
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
			if (Projectile.timeLeft > 15)
			{
				Projectile.timeLeft = 15;
			}
			return false;
        }
		private bool runOnce = true;
		public override bool PreAI()
		{
			if (runOnce)
			{
				Projectile.velocity.X = 0;
				runOnce = false;
			}
			return true;
		}
        float n = 0.03f;
		float u = 0;
        public override void AI()
        {
			Projectile.ai[0]++;
			Projectile.velocity.Y = -1;
			if (true) //ToDo: Skip if standing on bubble
			{
				if (Projectile.velocity.X >= 0.9 || Projectile.velocity.X <= -0.9)
				{
					n *= -1;
				}
				u += n;
				Projectile.velocity.X = u;
			}
			else
			{
				Projectile.velocity.X = 0;
			}
			
            if (Projectile.ai[0] >= 3 && Projectile.timeLeft <= 15 && Projectile.frame < 5)
            {
                Projectile.velocity.X = 0;
                n = 0;
                u = 0;
                Projectile.frame++;
                Projectile.ai[0] = 0;
            }
        }
		public override void OnKill(int timeLeft)
		{
			Terraria.Audio.SoundEngine.PlaySound(SoundID.Item14, Projectile.Center);
			Player owner = Main.player[Projectile.owner];
            SOTSPlayer modPlayer = SOTSPlayer.ModPlayer(owner);
			int RandMod = (int)Projectile.ai[0];
					
        }
	}
}
			