using System;
using System.IO;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace SOTS.Projectiles.Legendary
{    
    public class PhantomExplosion : ModProjectile 
    {	int expand = -1;
		            
		
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Poltergeist");
			
		}
		
        public override void SetDefaults()
        {
			projectile.CloneDefaults(263);
            aiType = 263; 
			projectile.height = 56;
			projectile.width = 56;
			projectile.penetrate = -1;
			projectile.friendly = true;
			projectile.timeLeft = 1;
			projectile.tileCollide = false;
			projectile.hostile = false;
			projectile.alpha = 255;
		}
		public override void AI()
		{
			for(int i = 0; i < 360; i += 15)
			{
			Vector2 circularLocation = new Vector2(28, 0).RotatedBy(MathHelper.ToRadians(i));
			
			int num1 = Dust.NewDust(new Vector2(projectile.Center.X + circularLocation.X - 4, projectile.Center.Y + circularLocation.Y - 4), 4, 4, 57);
			Main.dust[num1].noGravity = true;
			Main.dust[num1].velocity *= 0.1f;
			
			}
		}
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
			if(projectile.knockBack >= 0)
            target.immune[projectile.owner] = 10;
		
			if(projectile.knockBack <= -1)
            target.immune[projectile.owner] = 0;
		
        }
	}
}
		