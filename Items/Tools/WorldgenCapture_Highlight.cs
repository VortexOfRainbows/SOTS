using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace SOTS.Items.Tools
{    
    public class WorldgenCapture_Highlight : ModProjectile //stored as a projectile because I had scaling problems otherwise
    {	
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Worldgen Capture Visual");
		}
		
        public override void SetDefaults()
        {
			Projectile.width = 16;
			Projectile.height = 16;
			Projectile.timeLeft = 2;
			Projectile.alpha = 255;
			Projectile.tileCollide = false;
		}
		public override bool PreDraw(ref Color lightColor)
		{
			Texture2D texture1 = (Texture2D)ModContent.Request<Texture2D>("SOTS/Items/Tools/WorldgenCapture_Outline");
			Texture2D texture2 = (Texture2D)ModContent.Request<Texture2D>("SOTS/Items/Tools/WorldgenCapture_OutlineInterior");
			Texture2D texture3 = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;

			//Vector2 drawOrigin = new Vector2(texture1.Width * 0.5f, texture1.Height * 0.5f);
			Vector2 drawOrigin = new Vector2(0, 0);
			Vector2 drawPos = Projectile.position - Main.screenPosition;


			if (!new Vector2(Projectile.ai[0], Projectile.ai[1]).Equals(new Vector2(-1, 0)))
			{
				int differenceX = 1 + Math.Abs((int)(Projectile.position.X/16 - Projectile.ai[0]));
				int differenceY = 1 + Math.Abs((int)(Projectile.position.Y/16 - Projectile.ai[1]));
				for (int j = 0; j < differenceY; j++)
				{
					for (int i = 0; i < differenceX; i++)
					{
						drawPos = (Projectile.position + (new Vector2(i, j) * 16)) - Main.screenPosition;
						if (j == 0 || i == 0 || j == differenceY - 1 || i == differenceX - 1)
						{
							Main.spriteBatch.Draw(texture1, drawPos, null, new Color(100, 100, 100, 100), 0f, drawOrigin, Projectile.scale, SpriteEffects.None, 0f);
							Main.spriteBatch.Draw(texture2, drawPos, null, new Color(100, 100, 100, 50), 0f, drawOrigin, Projectile.scale, SpriteEffects.None, 0f);
						}
						else
						{
							Main.spriteBatch.Draw(texture3, drawPos, null, new Color(100, 100, 100, 50), 0f, drawOrigin, Projectile.scale, SpriteEffects.None, 0f);
						}
					}
				}
			}
			else
			{
				Main.spriteBatch.Draw(texture1, drawPos, null, new Color(100, 100, 100, 100), 0f, drawOrigin, Projectile.scale, SpriteEffects.None, 0f);
				Main.spriteBatch.Draw(texture2, drawPos, null, new Color(100, 100, 100, 50), 0f, drawOrigin, Projectile.scale, SpriteEffects.None, 0f);
			}
			return true;
		}
	}
}
		