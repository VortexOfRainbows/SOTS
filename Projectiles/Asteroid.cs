using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Projectiles 
{    
    public class Asteroid : ModProjectile 
    {	
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Asteroid");
			
		}
		
        public override void SetDefaults()
        {
            projectile.width = 22;
            projectile.height = 22; 
            projectile.timeLeft = 361;
            projectile.penetrate = -1; 
            projectile.friendly = false; 
            projectile.hostile = false; 
            projectile.tileCollide = false;
            projectile.ignoreWater = true; 
            projectile.aiStyle = 0; //18 is the demon scythe style
			projectile.alpha = 0;
		}
		public override void AI()
		{
			Player player  = Main.player[projectile.owner];
			Vector2 vector14;
						
			if (player.gravDir == 1f)
			{
				vector14.Y = (float)Main.mouseY + Main.screenPosition.Y;
			}
			else
			{
				vector14.Y = Main.screenPosition.Y + (float)Main.screenHeight - (float)Main.mouseY;
			}
			vector14.X = (float)Main.mouseX + Main.screenPosition.X;

			double deg = (double) projectile.ai[1];
			double rad = deg * (Math.PI / 180);
			double dist = 48;
			
			projectile.position.X = vector14.X - (int)(Math.Cos(rad) * dist) - projectile.width/2;
			projectile.position.Y = vector14.Y - (int)(Math.Sin(rad) * dist) - projectile.height/2;
		 
			projectile.ai[1] += 1f;
			
			for(int i = 0; i < 1000; i++)
			{
				Projectile reflectProjectile = Main.projectile[i];
				
					float dX = reflectProjectile.Center.X - projectile.Center.X;
					float dY = reflectProjectile.Center.Y - projectile.Center.Y;
					float distance = (float) Math.Sqrt((double)(dX * dX + dY * dY));
				if(distance < 24f && reflectProjectile.hostile && !reflectProjectile.friendly)
				{
					reflectProjectile.friendly = true;
					reflectProjectile.hostile = false;
					reflectProjectile.velocity.X *= -1;
					reflectProjectile.velocity.Y *= -1;
				}
			}
			
		} 
		public override void PostDraw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch, Color lightColor)
        {
            Texture2D texture = ModContent.GetTexture("SOTS/Projectiles/RockChain");    //this where the chain of grappling hook is drawn
                                                      //change YourModName with ur mod name/ and CustomHookPr_Chain with the name of ur one
            Vector2 position = projectile.Center;
            Vector2 mountedCenter = Main.player[projectile.owner].MountedCenter;
            Microsoft.Xna.Framework.Rectangle? sourceRectangle = new Microsoft.Xna.Framework.Rectangle?();
            Vector2 origin = new Vector2((float)texture.Width * 0.5f, (float)texture.Height * 0.5f);
            float num1 = (float)texture.Height;
            Vector2 vector2_4 = mountedCenter - position;
            float rotation = (float)Math.Atan2((double)vector2_4.Y, (double)vector2_4.X) - 1.57f;
            bool flag = true;
            if (float.IsNaN(position.X) && float.IsNaN(position.Y))
                flag = false;
            if (float.IsNaN(vector2_4.X) && float.IsNaN(vector2_4.Y))
                flag = false;
            while (flag)
            {
                if ((double)vector2_4.Length() < (double)num1 + 1.0)
                {
                    flag = false;
                }
                else
                {
                    Vector2 vector2_1 = vector2_4;
                    vector2_1.Normalize();
                    position += vector2_1 * num1;
                    vector2_4 = mountedCenter - position;
                    Microsoft.Xna.Framework.Color color2 = Lighting.GetColor((int)position.X / 16, (int)((double)position.Y / 16.0));
                    color2 = projectile.GetAlpha(color2);
                    Main.spriteBatch.Draw(texture, position - Main.screenPosition, sourceRectangle, color2, rotation, origin, 1f, SpriteEffects.None, 0.0f);
                }
            }
        }
		
	}
	
}