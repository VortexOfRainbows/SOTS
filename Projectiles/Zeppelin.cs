using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
 
namespace SOTS.Projectiles 
{
    public class Zeppelin : ModProjectile 
    {	
		int storeData = -1;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Zephyrous Zeppelin");
            ProjectileID.Sets.YoyosLifeTimeMultiplier[Projectile.type] = 11f;
			// YoyosLifeTimeMultiplier is how long in seconds the yoyo will stay out before automatically returning to the player. 
			// Vanilla values range from 3f(Wood) to 16f(Chik), and defaults to -1f. Leaving as -1 will make the time infinite.
            ProjectileID.Sets.YoyosMaximumRange[Projectile.type] = 215f;
            // YoyosMaximumRange is the maximum distance the yoyo sleep away from the player.
            // Vanilla values range from 130f(Wood) to 400f(Terrarian), and defaults to 200f
            ProjectileID.Sets.YoyosTopSpeed[Projectile.type] = 13f;
            // YoyosTopSpeed is top speed of the yoyo Projectile.
            // Vanilla values range from 9f(Wood) to 17.5f(Terrarian), and defaults to 10f
		}
        public override void SetDefaults()
        {
            Projectile.extraUpdates = 0;
            Projectile.width = 18;
            Projectile.height = 18;            
            Projectile.aiStyle = 99;
            Projectile.friendly = true; 
            Projectile.penetrate = -1; 
            Projectile.DamageType = DamageClass.Melee; 
        }
		public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac) 
		{
			width = 14;
			height = 14;
			fallThrough = true;
			return true;
		}
        public override void PostAI()
        {
			Lighting.AddLight(Projectile.Center, (255 - Projectile.alpha) * 0.7f / 255f, (255 - Projectile.alpha) * 1f / 255f, (255 - Projectile.alpha) * 1.45f / 255f);
			if(storeData == -1 && Projectile.owner == Main.myPlayer)
			{
				storeData = Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, Vector2.Zero, ModContent.ProjectileType<Razorwater>(), (int)(Projectile.damage * 0.75f) + 1, 0, Main.myPlayer, 0, Projectile.whoAmI);
			}
        }
    }
}