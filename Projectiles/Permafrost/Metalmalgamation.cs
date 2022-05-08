using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
 
namespace SOTS.Projectiles.Permafrost
{
    public class Metalmalgamation : ModProjectile 
    {	int counter = 0;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Metalmalgamation");
		}
        public override void SetDefaults()
        {
            projectile.extraUpdates = 0;
            projectile.width = 30;
            projectile.height = 30;	       
            projectile.aiStyle = 99; 
            projectile.friendly = true;	
            projectile.penetrate = -1;	
			projectile.melee = true;	        
            ProjectileID.Sets.YoyosLifeTimeMultiplier[projectile.type] = 10f;
            ProjectileID.Sets.YoyosMaximumRange[projectile.type] = 196f;
            ProjectileID.Sets.YoyosTopSpeed[projectile.type] = 12f;
        }
        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough)
        {
            width = 16;
            height = 16;
            return true;
        }
        public override void AI()
		{
			Player player = Main.player[projectile.owner];
			counter++;
			if(counter % 6 == 0 && projectile.owner == Main.myPlayer)
			{
				for(int i = 0; i < 4; i++)
                {
                    Vector2 rotateArea = new Vector2(5.4f, 0).RotatedBy(MathHelper.ToRadians(counter * 6 + i * 90));
                    Projectile.NewProjectile(projectile.Center, rotateArea, ModContent.ProjectileType<FriendlyPolarBullet>(), (int)(projectile.damage * .35f), projectile.knockBack, player.whoAmI, -3);
                }
				//SoundEngine.PlaySound(SoundID.Item11, (int)projectile.Center.X, (int)projectile.Center.Y);
			}
		}
    }
}