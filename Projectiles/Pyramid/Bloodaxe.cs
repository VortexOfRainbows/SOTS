using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace SOTS.Projectiles.Pyramid
{    
    public class Bloodaxe : ModProjectile 
    {	
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Bloody Hammer");
		}
        public override void SetDefaults()
        {
			projectile.CloneDefaults(274);
            aiType = 274;
			projectile.alpha = 0;
			projectile.timeLeft = 255;
			projectile.width = 40;
			projectile.height = 40;
			projectile.penetrate = 5;
		}
        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
			Texture2D texture = Main.projectileTexture[projectile.type];
			Vector2 origin = new Vector2(texture.Width / 2, texture.Height / 2);
			Color color = Color.Black;
			for (int i = 0; i < 360; i += 30)
			{
				Vector2 circular = new Vector2(Main.rand.NextFloat(3.5f, 5), 0).RotatedBy(MathHelper.ToRadians(i));
				color = new Color(255, 100, 100, 0);
				Main.spriteBatch.Draw(texture, projectile.Center + circular - Main.screenPosition, null, color * ((255f - projectile.alpha) / 255f), projectile.rotation, origin, projectile.scale, SpriteEffects.None, 0.0f);
			}
			color = new Color(55, 0, 0);
			Main.spriteBatch.Draw(texture, projectile.Center - Main.screenPosition, null, color, projectile.rotation, origin, projectile.scale, SpriteEffects.None, 0.0f);
			return false;
        }
        public override bool PreAI()
		{
			Dust dust = Dust.NewDustDirect(projectile.Center + new Vector2(16, -16).RotatedBy(projectile.rotation) - new Vector2(5), 0, 0, 235);
			dust.velocity *= 0.1f;
			dust.scale *= 1.33f;
			dust.noGravity = true;
			return base.PreAI();
        }
        public override void AI()
		{
			projectile.alpha++;
			float minDist = 360;
			int target2 = -1;
			float dX = 0f;
			float dY = 0f;
			float distance = 0;
			float speed = 0.5f;
			if(projectile.friendly == true && projectile.hostile == false && projectile.timeLeft > 110)
			{
				for(int i = 0; i < Main.npc.Length - 1; i++)
				{
					NPC target = Main.npc[i];
					if(!target.friendly && target.dontTakeDamage == false && target.lifeMax > 5 && target.active && target.CanBeChasedBy())
					{
						dX = target.Center.X - projectile.Center.X;
						dY = target.Center.Y - projectile.Center.Y;
						distance = (float) Math.Sqrt((double)(dX * dX + dY * dY));
						if(distance < minDist)
						{
							minDist = distance;
							target2 = i;
						}
					}
				}
				
				if(target2 != -1)
				{
					NPC toHit = Main.npc[target2];
					if(toHit.active == true)
					{
						dX = toHit.Center.X - projectile.Center.X;
						dY = toHit.Center.Y - projectile.Center.Y;
						distance = (float)Math.Sqrt((double)(dX * dX + dY * dY));
						speed /= distance;
						projectile.velocity += new Vector2(dX * speed, dY * speed);
					}
				}
			}
			
		}
        public override void Kill(int timeLeft)
        {
			for(int i = 0; i < 45; i++)
            {
				Dust dust = Dust.NewDustDirect(projectile.Center - new Vector2(5), 0, 0, 235);
				dust.velocity *= 2;
				dust.scale *= 2;
				dust.noGravity = true;
            }
            base.Kill(timeLeft);
        }
    }
}
		
			