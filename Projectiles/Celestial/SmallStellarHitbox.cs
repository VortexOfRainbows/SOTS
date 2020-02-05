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

namespace SOTS.Projectiles.Celestial
{    
    public class SmallStellarHitbox : ModProjectile 
    {	int expand = -1;
		            
		
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Starsplosion");
			
		}
		
        public override void SetDefaults()
        {
			projectile.CloneDefaults(263);
            aiType = 263; 
			projectile.height = 60;
			projectile.width = 60;
			projectile.penetrate = -1;
			projectile.friendly = true;
			projectile.timeLeft = 1;
			projectile.tileCollide = false;
			projectile.hostile = false;
			projectile.alpha = 255;
			projectile.minion = true;
		}
		public override void Kill(int timeLeft)
        {
			int color = Main.rand.Next(2);
			float size = 60f;
			float starPosX = projectile.Center.X - size/2f;
			float starPosY = projectile.Center.Y - size/6f;
			for(int i = 0; i < 5; i ++)
			{
				float rads = MathHelper.ToRadians(144 * i);
				for(float j = 0; j < size; j += 3f)
				{
					int num1 = Dust.NewDust(new Vector2(starPosX, starPosY), 0, 0, color == 0 ? 88 : 21);
					Main.dust[num1].noGravity = true;
					Main.dust[num1].velocity *= 0.1f;
					
					Vector2 rotationDirection = new Vector2(3f, 0).RotatedBy(rads);
					starPosX += rotationDirection.X;
					starPosY += rotationDirection.Y;
				}
			}
			Main.PlaySound(SoundID.Item9, (int)(projectile.Center.X), (int)(projectile.Center.Y));
		}
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.immune[projectile.owner] = 4;
        }
	}
}
		