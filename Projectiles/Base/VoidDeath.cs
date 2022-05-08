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
			projectile.height = 70;
			projectile.width = 70;
            Main.projFrames[projectile.type] = 14;
			projectile.penetrate = -1;
			projectile.friendly = true;
			projectile.timeLeft = 70;
			projectile.tileCollide = false;
			projectile.hostile = false;
			projectile.alpha = 30;
		}
		bool sound = true;
		public override void AI()
        {
            projectile.frameCounter++;
            if (projectile.frameCounter >= 3)
            {
				projectile.friendly = false;
                projectile.frameCounter = 0;
                projectile.frame = (projectile.frame + 1) % 14;
				if(projectile.frame == 0)
				{
					projectile.alpha = 255;
				}
            }
			if(projectile.alpha == 255)
			{
				if(sound)
				{
					if (projectile.owner  == Main.LocalPlayer.whoAmI)
						SoundEngine.PlaySound(SoundLoader.customSoundType, (int)projectile.Center.X, (int)projectile.Center.Y, mod.GetSoundSlot(SoundType.Custom, "Sounds/Void/Void_Death"), 1.05f);
					sound = false;
                }
				for (int i = 0; i < 3; i++)
				{
					int num1 = Dust.NewDust(projectile.position, projectile.width, projectile.height, 198);
					Main.dust[num1].noGravity = true;
					Main.dust[num1].velocity *= 5;
					Main.dust[num1].scale = 2;

					num1 = Dust.NewDust(projectile.position, projectile.width, projectile.height, 198);
					Main.dust[num1].noGravity = true;
					Main.dust[num1].velocity *= 4;
					Main.dust[num1].scale = 3;

					num1 = Dust.NewDust(projectile.position, projectile.width, projectile.height, 198);
					Main.dust[num1].noGravity = true;
					Main.dust[num1].velocity *= 3;
					Main.dust[num1].scale = 4;
				}
			}
        }
	}
}
		