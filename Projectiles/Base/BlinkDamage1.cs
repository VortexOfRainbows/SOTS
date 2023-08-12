using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Projectiles.Base
{    
    public class BlinkDamage1 : ModProjectile 
    {	
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Blinding Assault");
		}
        public override void SetDefaults()
        {
            Projectile.width = 128;
            Projectile.height = 128;
			Projectile.timeLeft = 5;
            Projectile.penetrate = -1; 
            Projectile.friendly = true; 
            Projectile.tileCollide = false;
            Projectile.aiStyle = 0; 
			Projectile.alpha = 255;
		}
		int randseed = -1;
		public override void AI() //The projectile's AI/ what the projectile does
		{
			Player player = Main.player[Projectile.owner];
			if (randseed == -1)
			{
				Terraria.Audio.SoundEngine.PlaySound(SoundID.Item14, Projectile.Center);
				randseed = Main.rand.Next(360);
				for (int i = 0; i < 360; i += 10)
				{
					Vector2 circularLocation = new Vector2(8, 0).RotatedBy(MathHelper.ToRadians(i));
					int num = Dust.NewDust(new Vector2(Projectile.Center.X - 4, Projectile.Center.Y - 4), 0, 0, 235);
					Main.dust[num].velocity = circularLocation;
					Main.dust[num].scale = 2f;
					Main.dust[num].noGravity = true;
					Main.dust[num].shader = GameShaders.Armor.GetSecondaryShader(player.cBack, player);
				}
				for (int i = 0; i < 360; i += 10)
				{
					Vector2 circularLocation = new Vector2(8, 0).RotatedBy(MathHelper.ToRadians(i));
					circularLocation.X /= 2;
					circularLocation = circularLocation.RotatedBy(MathHelper.ToRadians(randseed));
					int num = Dust.NewDust(new Vector2(Projectile.Center.X - 4, Projectile.Center.Y - 4), 0, 0, 235);
					Main.dust[num].velocity = circularLocation;
					Main.dust[num].scale = 2f;
					Main.dust[num].noGravity = true;
					Main.dust[num].shader = GameShaders.Armor.GetSecondaryShader(player.cBack, player);
				}
				for (int i = 0; i < 360; i += 10)
				{
					Vector2 circularLocation = new Vector2(8, 0).RotatedBy(MathHelper.ToRadians(i));
					circularLocation.X /= 2;
					circularLocation = circularLocation.RotatedBy(MathHelper.ToRadians(randseed + 90));
					int num = Dust.NewDust(new Vector2(Projectile.Center.X - 4, Projectile.Center.Y - 4), 0, 0, 235);
					Main.dust[num].velocity = circularLocation;
					Main.dust[num].scale = 2f;
					Main.dust[num].noGravity = true;
					Main.dust[num].shader = GameShaders.Armor.GetSecondaryShader(player.cBack, player);
				}
			}
		}
	}
}