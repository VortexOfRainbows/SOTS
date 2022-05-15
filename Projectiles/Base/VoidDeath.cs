using Terraria;
using Terraria.ModLoader;

namespace SOTS.Projectiles.Base
{    
    public class VoidDeath : ModProjectile 
    {	
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Voiden Collapse");
			
		}
		
        public override void SetDefaults()
        {
			Projectile.height = 70;
			Projectile.width = 70;
            Main.projFrames[Projectile.type] = 14;
			Projectile.penetrate = -1;
			Projectile.friendly = true;
			Projectile.timeLeft = 70;
			Projectile.tileCollide = false;
			Projectile.hostile = false;
			Projectile.alpha = 30;
		}
		bool sound = true;
		public override void AI()
        {
            Projectile.frameCounter++;
            if (Projectile.frameCounter >= 3)
            {
				Projectile.friendly = false;
                Projectile.frameCounter = 0;
                Projectile.frame = (Projectile.frame + 1) % 14;
				if(Projectile.frame == 0)
				{
					Projectile.alpha = 255;
				}
            }
			if(Projectile.alpha == 255)
			{
				if(sound)
				{
					if (Projectile.owner  == Main.LocalPlayer.whoAmI)
						SoundEngine.PlaySound(SoundLoader.customSoundType, (int)Projectile.Center.X, (int)Projectile.Center.Y, Mod.GetSoundSlot(SoundType.Custom, "Sounds/Void/Void_Death"), 1.05f);
					sound = false;
                }
				for (int i = 0; i < 3; i++)
				{
					int num1 = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 198);
					Main.dust[num1].noGravity = true;
					Main.dust[num1].velocity *= 5;
					Main.dust[num1].scale = 2;

					num1 = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 198);
					Main.dust[num1].noGravity = true;
					Main.dust[num1].velocity *= 4;
					Main.dust[num1].scale = 3;

					num1 = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 198);
					Main.dust[num1].noGravity = true;
					Main.dust[num1].velocity *= 3;
					Main.dust[num1].scale = 4;
				}
			}
        }
	}
}
		