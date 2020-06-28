using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
 

namespace SOTS.Projectiles 
{    
    public class SpikeTrap : ModProjectile 
    {	int deteriorate = 0;
		
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Spike Trap");
			
		}
		
        public override void SetDefaults()
        {
			projectile.CloneDefaults(24);
            aiType = 24; //18 is the demon scythe style
            projectile.width = 18;
            projectile.height = 16; 
            projectile.timeLeft = 1275;
            projectile.penetrate = 1; 
            projectile.friendly = true; 
            projectile.hostile = false; 
            projectile.tileCollide = true;
            projectile.ignoreWater = false; 
            projectile.melee = true; 
			projectile.alpha = 0;
		}
		public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough)
		{
			fallThrough = false;
			return true;
		}
		public override void AI()
        {
			
			deteriorate++;
			if(deteriorate >= 10)
			{
				projectile.alpha++;
				deteriorate = 0;
			}
			
        }
		public override void Kill(int timeLeft)
		{
			for(int i = 0; i < 5; i++)
			{
				int goreIndex = Gore.NewGore(new Vector2(projectile.position.X, projectile.position.Y), default(Vector2), Main.rand.Next(61,64), 1f);	
				Main.gore[goreIndex].scale = 0.45f;
			}
			Main.PlaySound(2, (int)(projectile.Center.X), (int)(projectile.Center.Y), 14, 0.4f);

		}
		public override bool OnTileCollide(Vector2 oldVelocity)
		{	
			projectile.rotation = 0;
			projectile.velocity.X = 0;
			projectile.velocity.Y = 0;
			return false;
		}
	}
}
		
			