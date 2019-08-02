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

namespace SOTS.Projectiles 
{    
    public class MourningStarChain : ModProjectile 
    {	int down = 0;
		bool swap = false;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("-");
			
		}
		
        public override void SetDefaults()
        {
            projectile.width = 1;
            projectile.height = 1; 
            projectile.timeLeft = 360;
            projectile.penetrate = 1; 
            projectile.friendly = false; 
            projectile.hostile = false; 
            projectile.tileCollide = false;
            projectile.ignoreWater = false; 
            projectile.melee = false; 
            projectile.aiStyle = 1; //18 is the demon scythe style
			projectile.alpha = 0;
		}
		public override void AI()
		{
			
    //Making player variable "p" set as the projectile's owner
    Player owner  = Main.player[projectile.owner];
	projectile.rotation = 0;
	if(owner.Center.X - 220 > projectile.Center.X)
			{
				projectile.velocity.X += .09f;
			}
			if(owner.Center.X + 220 < projectile.Center.X)
			{
				projectile.velocity.X -= .09f;
			}
			if(owner.Center.Y - 220 > projectile.Center.Y)
			{
				projectile.velocity.Y += .35f;
			}
			if(owner.Center.Y + 220 < projectile.Center.Y)
			{
				projectile.velocity.Y -= .35f;
			}
	
		}
		public override void PostDraw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch, Color lightColor)
        {
            Texture2D texture = ModContent.GetTexture("SOTS/Projectiles/MourningStarChain2");    //this where the chain of grappling hook is drawn
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
		
			