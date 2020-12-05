using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
 
namespace SOTS.Projectiles.Otherworld
{
    public class Poyoyo : ModProjectile 
    {	
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Poyo-yo");
            ProjectileID.Sets.YoyosLifeTimeMultiplier[projectile.type] = 15f;
			// YoyosLifeTimeMultiplier is how long in seconds the yoyo will stay out before automatically returning to the player. 
			// Vanilla values range from 3f(Wood) to 16f(Chik), and defaults to -1f. Leaving as -1 will make the time infinite.
			
            ProjectileID.Sets.YoyosMaximumRange[projectile.type] = 240f;
            // YoyosMaximumRange is the maximum distance the yoyo sleep away from the player.
            // Vanilla values range from 130f(Wood) to 400f(Terrarian), and defaults to 200f
			
            ProjectileID.Sets.YoyosTopSpeed[projectile.type] = 18f;
            // YoyosTopSpeed is top speed of the yoyo projectile.
            // Vanilla values range from 9f(Wood) to 17.5f(Terrarian), and defaults to 10f
		}
        public override void SetDefaults()
        {
            projectile.extraUpdates = 0;
            projectile.width = 16;
            projectile.height = 16;            
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
        int storeData = -1;
        public override void PostAI()
        {
            if (storeData == -1 && projectile.owner == Main.myPlayer)
            {
                storeData = Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, 0, 0, mod.ProjectileType("RainbowTrail"), (int)(projectile.damage * 0.55f) + 1, 0, projectile.owner, 0, projectile.whoAmI);
            }
        }
    }
}