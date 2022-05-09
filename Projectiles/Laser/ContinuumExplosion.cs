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

namespace SOTS.Projectiles.Laser
{    
    public class ContinuumExplosion : ModProjectile 
    {	int expand = -1;
		            
		
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Collapse Laser");
			
		}
		
        public override void SetDefaults()
        {
			Projectile.CloneDefaults(263);
            aiType = 263; 
			Projectile.height = 24;
			Projectile.width = 24;
			Projectile.penetrate = -1;
			Projectile.friendly = true;
			Projectile.magic = true;
			Projectile.timeLeft = 2;
			Projectile.tileCollide = false;
			Projectile.hostile = false;
			Projectile.alpha = 255;
		}
		public override void Kill(int timeLeft)
        {
			if(Projectile.ai[0] == 1)
			{
				for(int i = 0; i < 360; i += 40)
				{
					Vector2 circularLocation = new Vector2(-12, 0).RotatedBy(MathHelper.ToRadians(i + Projectile.ai[1]));
					
					int num1 = Dust.NewDust(new Vector2(Projectile.Center.X + circularLocation.X - 4, Projectile.Center.Y + circularLocation.Y - 4), 4, 4, 66);
					Main.dust[num1].noGravity = true;
					Main.dust[num1].velocity = circularLocation * 0.35f;
					
					float newAi = Projectile.ai[1] * 2 / 13f;
					double frequency = 0.3;
					double center = 130;
					double width = 125;
					double red = Math.Sin(frequency * (double)newAi) * width + center;
					double grn = Math.Sin(frequency * (double)newAi + 2.0) * width + center;
					double blu = Math.Sin(frequency * (double)newAi + 4.0) * width + center;
					Color color2 = new Color((int)red, (int)grn, (int)blu);
					Main.dust[num1].scale = 2f;
					Main.dust[num1].color = color2;
				}
			}
		}
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.immune[Projectile.owner] = 5;
			Projectile.ai[0] = 1;
			Projectile.netUpdate = true;
        }
	}
}
		