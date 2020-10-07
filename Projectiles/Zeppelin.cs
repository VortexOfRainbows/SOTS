using System;
using System.Collections.Generic;
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
            ProjectileID.Sets.YoyosLifeTimeMultiplier[projectile.type] = 11f;
			// YoyosLifeTimeMultiplier is how long in seconds the yoyo will stay out before automatically returning to the player. 
			// Vanilla values range from 3f(Wood) to 16f(Chik), and defaults to -1f. Leaving as -1 will make the time infinite.
			
            ProjectileID.Sets.YoyosMaximumRange[projectile.type] = 215f;
            // YoyosMaximumRange is the maximum distance the yoyo sleep away from the player.
            // Vanilla values range from 130f(Wood) to 400f(Terrarian), and defaults to 200f
			
            ProjectileID.Sets.YoyosTopSpeed[projectile.type] = 13f;
            // YoyosTopSpeed is top speed of the yoyo projectile.
            // Vanilla values range from 9f(Wood) to 17.5f(Terrarian), and defaults to 10f
		}
        public override void SetDefaults()
        {
            projectile.extraUpdates = 0;
            projectile.width = 18;
            projectile.height = 18;            
            projectile.aiStyle = 99;
            projectile.friendly = true; 
            projectile.penetrate = -1; 
            projectile.melee = true; 
        }
		public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough) 
		{
			width = 14;
			height = 14;
			fallThrough = true;
			return true;
		}
        public override void PostAI()
        {
			Lighting.AddLight(projectile.Center, (255 - projectile.alpha) * 0.7f / 255f, (255 - projectile.alpha) * 1f / 255f, (255 - projectile.alpha) * 1.45f / 255f);
			if(storeData == -1 && projectile.owner == Main.myPlayer)
			{
				storeData = Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, 0, 0, mod.ProjectileType("Razorwater"), (int)(projectile.damage * 0.75f) + 1, 0, Main.myPlayer, 0, projectile.whoAmI);
			}
        }
    }
}