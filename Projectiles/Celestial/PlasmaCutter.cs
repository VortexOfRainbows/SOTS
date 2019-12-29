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
    public class PlasmaCutter : ModProjectile 
    {	int wait = 0;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Time to die");
			ProjectileID.Sets.TrailCacheLength[projectile.type] = 12;  
			ProjectileID.Sets.TrailingMode[projectile.type] = 2;    
			
		}
		
        public override void SetDefaults()
        {
            projectile.width = 50;
            projectile.height = 50; 
            projectile.timeLeft = 360000;
            projectile.penetrate = -1; 
            projectile.friendly = true; 
            projectile.hostile = false; 
            projectile.tileCollide = false;
            projectile.ignoreWater = true; 
            projectile.melee = true; 
            projectile.aiStyle = 0; 
			projectile.alpha = 0;
			projectile.extraUpdates = 3;
		}
		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Player player  = Main.player[projectile.owner];
			Vector2 drawOrigin = new Vector2(Main.projectileTexture[projectile.type].Width * 0.5f, projectile.height * 0.5f);
			for (int k = 0; k < projectile.oldPos.Length; k++) {
				Vector2 drawPos = projectile.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(0f, projectile.gfxOffY);
				Color color = projectile.GetAlpha(lightColor) * ((float)(projectile.oldPos.Length - k) / (float)projectile.oldPos.Length);
					
				float disX = player.Center.X - projectile.oldPos[k].X;
				float disY = player.Center.Y - projectile.oldPos[k].Y;
				float rotation2 = (float)Math.Atan2(disY,disX) + MathHelper.ToRadians(225f);
				
				spriteBatch.Draw(Main.projectileTexture[projectile.type], drawPos, null, color, rotation2, drawOrigin, projectile.scale, SpriteEffects.None, 0f);
			}
			Texture2D texture = ModContent.GetTexture("SOTS/Projectiles/Celestial/PlasmaCutterChain");  
			
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
			return true;
		}
		bool ReturnTo = false;
		public override void AI()
		{
			Player player  = Main.player[projectile.owner];
			if(player.whoAmI == Main.myPlayer)
			{
				projectile.netUpdate = true;
				Vector2 cursorArea = Main.MouseWorld;
					
				float cursorX = cursorArea.X - player.Center.X;
				float cursorY = cursorArea.Y - player.Center.Y;
				
				float disX = player.Center.X - projectile.Center.X;
				float disY = player.Center.Y - projectile.Center.Y;
				projectile.rotation = (float)Math.Atan2(disY,disX) + MathHelper.ToRadians(225f);
				
				double deg = (double) projectile.ai[0]; 
				double rad = deg * (Math.PI / 180);
				
				Vector2 ovalArea = new Vector2(96, 0).RotatedBy((float)Math.Atan2(cursorY,cursorX));
				Vector2 ovalArea2 = new Vector2(232, 0).RotatedBy((float)rad);
				ovalArea2.Y *= 0.185f;
				ovalArea2 = ovalArea2.RotatedBy((float)Math.Atan2(cursorY,cursorX));
				ovalArea.X += ovalArea2.X;
				ovalArea.Y += ovalArea2.Y;
				if(player.channel && !ReturnTo)
				{
				projectile.position.X = player.Center.X + ovalArea.X - projectile.width/2;
				projectile.position.Y = player.Center.Y + ovalArea.Y - projectile.height/2;
				}
				else
				{
					ReturnTo = true;
					
					Vector2 newVelocity = new Vector2(5, 0).RotatedBy(Math.Atan2(disY, disX));
					projectile.velocity = newVelocity;
					if(Math.Abs(disX) < 22f && Math.Abs(disY) < 22f)
					{
						projectile.Kill();
					}
				}
			}
			projectile.ai[0] += 5f;
			
		}
	}
}
		
			