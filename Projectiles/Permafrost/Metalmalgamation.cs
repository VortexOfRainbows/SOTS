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
            Projectile.extraUpdates = 0;
            Projectile.width = 30;
            Projectile.height = 30;	       
            Projectile.aiStyle = 99; 
            Projectile.friendly = true;	
            Projectile.penetrate = -1;	
			Projectile.DamageType = DamageClass.Melee;	        
            ProjectileID.Sets.YoyosLifeTimeMultiplier[Projectile.type] = 10f;
            ProjectileID.Sets.YoyosMaximumRange[Projectile.type] = 196f;
            ProjectileID.Sets.YoyosTopSpeed[Projectile.type] = 12f;
        }
        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
        {
            width = 16;
            height = 16;
            return true;
        }
        public override void AI()
		{
			Player player = Main.player[Projectile.owner];
			counter++;
			if(counter % 6 == 0 && Projectile.owner == Main.myPlayer)
			{
				for(int i = 0; i < 4; i++)
                {
                    Vector2 rotateArea = new Vector2(5.4f, 0).RotatedBy(MathHelper.ToRadians(counter * 6 + i * 90));
                    Projectile.NewProjectile(Projectile.Center, rotateArea, ModContent.ProjectileType<FriendlyPolarBullet>(), (int)(Projectile.damage * .35f), Projectile.knockBack, player.whoAmI, -3);
                }
				//SoundEngine.PlaySound(SoundID.Item11, (int)Projectile.Center.X, (int)Projectile.Center.Y);
			}
		}
    }
}