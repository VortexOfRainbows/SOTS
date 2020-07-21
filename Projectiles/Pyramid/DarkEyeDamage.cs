using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Projectiles.Pyramid
{    
    public class DarkEyeDamage : ModProjectile 
    {	
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("The Dark Eye");
		}
        public override void SetDefaults()
        {
            projectile.width = 128;
            projectile.height = 128;
			projectile.timeLeft = 5;
            projectile.penetrate = -1; 
            projectile.friendly = true; 
            projectile.tileCollide = false;
            projectile.aiStyle = 0; 
			projectile.alpha = 255;
		}
		int randseed = -1;
		public override void AI() //The projectile's AI/ what the projectile does
		{
			if(randseed == -1)
			{
				Main.PlaySound(SoundID.Item14, projectile.Center);
				randseed = Main.rand.Next(360);
				for (int i = 0; i < 360; i += 5)
				{
					Vector2 circularLocation = new Vector2(16, 0).RotatedBy(MathHelper.ToRadians(i));
					int num = Dust.NewDust(new Vector2(projectile.Center.X - 4, projectile.Center.Y - 4), 0, 0, 21);
					Main.dust[num].velocity = circularLocation;
					Main.dust[num].scale = 3f;
					Main.dust[num].noGravity = true;
				}
				for (int i = 0; i < 360; i += 10)
				{
					Vector2 circularLocation = new Vector2(16, 0).RotatedBy(MathHelper.ToRadians(i));
					circularLocation.X /= 2;
					circularLocation = circularLocation.RotatedBy(MathHelper.ToRadians(randseed));
					int num = Dust.NewDust(new Vector2(projectile.Center.X - 4, projectile.Center.Y - 4), 0, 0, 21);
					Main.dust[num].velocity = circularLocation;
					Main.dust[num].scale = 3f;
					Main.dust[num].noGravity = true;
				}
				for (int i = 0; i < 360; i += 10)
				{
					Vector2 circularLocation = new Vector2(16, 0).RotatedBy(MathHelper.ToRadians(i));
					circularLocation.X /= 2;
					circularLocation = circularLocation.RotatedBy(MathHelper.ToRadians(randseed + 90));
					int num = Dust.NewDust(new Vector2(projectile.Center.X - 4, projectile.Center.Y - 4), 0, 0, 21);
					Main.dust[num].velocity = circularLocation;
					Main.dust[num].scale = 3f;
					Main.dust[num].noGravity = true;
				}
			}
		}
	}
}