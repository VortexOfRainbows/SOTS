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

namespace SOTS.Items.ChestItems
{    
    public class WorldgenCapture_Highlight : ModProjectile //stored as a projectile because I had scaling problems otherwise
    {	
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Worldgen Capture Visual");
		}
		
        public override void SetDefaults()
        {
			projectile.width = 16;
			projectile.height = 16;
			projectile.timeLeft = 2;
			projectile.alpha = 255;
			projectile.tileCollide = false;
		}
		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Texture2D texture1 = ModContent.GetTexture("SOTS/Items/ChestItems/WorldgenCapture_Outline");
			Texture2D texture2 = ModContent.GetTexture("SOTS/Items/ChestItems/WorldgenCapture_OutlineInterior");
			Texture2D texture3 = ModContent.GetTexture("SOTS/Items/ChestItems/WorldgenCapture_Highlight");

			//Vector2 drawOrigin = new Vector2(texture1.Width * 0.5f, texture1.Height * 0.5f);
			Vector2 drawOrigin = new Vector2(0, 0);
			Vector2 drawPos = projectile.position - Main.screenPosition;


			if (!new Vector2(projectile.ai[0], projectile.ai[1]).Equals(new Vector2(-1, 0)))
			{
				int differenceX = 1 + Math.Abs((int)(projectile.position.X/16 - projectile.ai[0]));
				int differenceY = 1 + Math.Abs((int)(projectile.position.Y/16 - projectile.ai[1]));
				for (int j = 0; j < differenceY; j++)
				{
					for (int i = 0; i < differenceX; i++)
					{
						drawPos = (projectile.position + (new Vector2(i, j) * 16)) - Main.screenPosition;
						if (j == 0 || i == 0 || j == differenceY - 1 || i == differenceX - 1)
						{
							spriteBatch.Draw(texture1, drawPos, null, new Color(100, 100, 100, 100), 0f, drawOrigin, projectile.scale, SpriteEffects.None, 0f);
							spriteBatch.Draw(texture2, drawPos, null, new Color(100, 100, 100, 50), 0f, drawOrigin, projectile.scale, SpriteEffects.None, 0f);
						}
						else
						{
							spriteBatch.Draw(texture3, drawPos, null, new Color(100, 100, 100, 50), 0f, drawOrigin, projectile.scale, SpriteEffects.None, 0f);
						}
					}
				}
			}
			else
			{
				spriteBatch.Draw(texture1, drawPos, null, new Color(100, 100, 100, 100), 0f, drawOrigin, projectile.scale, SpriteEffects.None, 0f);
				spriteBatch.Draw(texture2, drawPos, null, new Color(100, 100, 100, 50), 0f, drawOrigin, projectile.scale, SpriteEffects.None, 0f);
			}
			return true;
		}
	}
}
		