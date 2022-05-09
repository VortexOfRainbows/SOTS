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

namespace SOTS.Projectiles.Lightning
{    
    public class Starsplosion : ModProjectile 
    {	int expand = 4;
		            
		
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Divided");
			
		}
		
        public override void SetDefaults()
        {
			Projectile.CloneDefaults(263);
            aiType = 263; 
			Projectile.height = 64;
			Projectile.width = 64;
			Projectile.penetrate = -1;
			Projectile.friendly = true;
			Projectile.timeLeft = 15;
			Projectile.tileCollide = false;
			Projectile.hostile = false;
			Projectile.alpha = 255;
		}
		public override void AI()
		{
			Projectile.alpha = 255;
			expand += 4;
			for(int i = 0; i < 360; i += 15)
			{
			Vector2 circularLocation = new Vector2(-expand, 0).RotatedBy(MathHelper.ToRadians(i));
			
			int num1 = Dust.NewDust(new Vector2(Projectile.Center.X + circularLocation.X - 4, Projectile.Center.Y + circularLocation.Y - 4), 4, 4, 204);
			Main.dust[num1].noGravity = true;
			Main.dust[num1].velocity *= 0.1f;
			
			}
		}
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.immune[Projectile.owner] = 3;
        }
	}
}
		