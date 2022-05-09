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
			Projectile.CloneDefaults(274);
            aiType = 274;
			Projectile.alpha = 0;
			Projectile.timeLeft = 255;
			Projectile.width = 40;
			Projectile.height = 40;
			Projectile.penetrate = 5;
		}
        public override bool PreDraw(ref Color lightColor)
        {
			Texture2D texture = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
			Vector2 origin = new Vector2(texture.Width / 2, texture.Height / 2);
			Color color = Color.Black;
			for (int i = 0; i < 360; i += 30)
			{
				Vector2 circular = new Vector2(Main.rand.NextFloat(3.5f, 5), 0).RotatedBy(MathHelper.ToRadians(i));
				color = new Color(255, 100, 100, 0);
				Main.spriteBatch.Draw(texture, Projectile.Center + circular - Main.screenPosition, null, color * ((255f - Projectile.alpha) / 255f), Projectile.rotation, origin, Projectile.scale, SpriteEffects.None, 0.0f);
			}
			color = new Color(55, 0, 0);
			Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, null, color, Projectile.rotation, origin, Projectile.scale, SpriteEffects.None, 0.0f);
			return false;
        }
        public override bool PreAI()
		{
			Dust dust = Dust.NewDustDirect(Projectile.Center + new Vector2(16, -16).RotatedBy(Projectile.rotation) - new Vector2(5), 0, 0, 235);
			dust.velocity *= 0.1f;
			dust.scale *= 1.33f;
			dust.noGravity = true;
			return base.PreAI();
        }
        public override void AI()
		{
			Projectile.alpha++;
			float minDist = 360;
			int target2 = -1;
			float dX = 0f;
			float dY = 0f;
			float distance = 0;
			float speed = 0.5f;
			if(Projectile.friendly == true && Projectile.hostile == false && Projectile.timeLeft > 110)
			{
				for(int i = 0; i < Main.npc.Length - 1; i++)
				{
					NPC target = Main.npc[i];
					if(!target.friendly && target.dontTakeDamage == false && target.lifeMax > 5 && target.active && target.CanBeChasedBy())
					{
						dX = target.Center.X - Projectile.Center.X;
						dY = target.Center.Y - Projectile.Center.Y;
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
						dX = toHit.Center.X - Projectile.Center.X;
						dY = toHit.Center.Y - Projectile.Center.Y;
						distance = (float)Math.Sqrt((double)(dX * dX + dY * dY));
						speed /= distance;
						Projectile.velocity += new Vector2(dX * speed, dY * speed);
					}
				}
			}
			
		}
        public override void Kill(int timeLeft)
        {
			for(int i = 0; i < 45; i++)
            {
				Dust dust = Dust.NewDustDirect(Projectile.Center - new Vector2(5), 0, 0, 235);
				dust.velocity *= 2;
				dust.scale *= 2;
				dust.noGravity = true;
            }
            base.Kill(timeLeft);
        }
    }
}
		
			