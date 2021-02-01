using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;
using Microsoft.Xna.Framework;
using Terraria.ObjectData;
using Terraria.DataStructures;
using Microsoft.Xna.Framework.Graphics;

namespace SOTS.Items.Pyramid
{
	public class RefractingCrystal : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Refracting Crystal");
			Tooltip.SetDefault("");
		}
		public override bool PreDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frameNotUsed, Color drawColor, Color itemColor, Vector2 origin, float scale)
		{
			Texture2D texture = mod.GetTexture("Items/Pyramid/RefractingCrystal");
			Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
			position += new Vector2(10 * scale, 9 * scale);
			float counter = Main.GlobalTime * 160;
			//int bonus = (int)(counter / 360f);
			float mult = new Vector2(-1f, 0).RotatedBy(MathHelper.ToRadians(counter)).X;
			for (int i = 0; i < 6; i++)
			{ 
				Color color = new Color(255, 0, 0, 0);
				switch (i)
                {
					case 0:
						color = new Color(255, 0, 0, 0);
						break;
					case 1:
						color = new Color(255, 140, 0, 0);
						break;
					case 2:
						color = new Color(255, 255, 0, 0);
						break;
					case 3:
						color = new Color(0, 255, 0, 0);
						break;
					case 4:
						color = new Color(0, 0, 255, 0);
						break;
					case 5:
						color = new Color(140, 0, 255, 0);
						break;
				}
				Rectangle frame = new Rectangle(0, 0, 20, 18);
				Vector2 rotationAround = new Vector2((4 + mult) * scale, 0).RotatedBy(MathHelper.ToRadians(60 * i + counter));
				Main.spriteBatch.Draw(texture, new Vector2((float)(position.X), (float)(position.Y)) + rotationAround, frame, color * 1f, 0f, drawOrigin, scale * 1.1f, SpriteEffects.None, 0f);
			}
			return true;
		}
        public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
        {
			int i = (int)item.Center.X / 16;
			int j = (int)item.Center.Y / 16;
			Tile tile = Framing.GetTileSafely(i, j);
			if (tile.wall > 0 || j > Main.rockLayer)
            {
				return true;
            }
			bool day = Main.dayTime;
			if(Main.raining || !day)
            {
				return true;
			}
			Texture2D texture2 = mod.GetTexture("Items/Pyramid/RefractingCrystal");
			float time = (float)Main.time;
			float maxTime = 54000f;
			float minTime = 0f;
			float midDay = 27000f;
			float percent = (float)time / (float)maxTime;
			float lightIntesity = new Vector2(1, 0).RotatedBy(MathHelper.ToRadians(180f * percent)).Y;
			Texture2D texture = mod.GetTexture("Items/Pyramid/WhitePixel");
			Vector2 drawOrigin;
			float counter = time * 0.024f;
			float mult = new Vector2(-11.8f, 0).RotatedBy(MathHelper.ToRadians(180f * percent)).Y;
			for (i = 0; i < 6; i++)
			{
				drawOrigin = new Vector2(texture2.Width * 0.5f, texture2.Height * 0.5f);
				Color color = new Color(255, 0, 0, 0);
				switch (i)
				{
					case 0:
						color = new Color(255, 0, 0, 0);
						break;
					case 1:
						color = new Color(255, 140, 0, 0);
						break;
					case 2:
						color = new Color(255, 255, 0, 0);
						break;
					case 3:
						color = new Color(0, 255, 0, 0);
						break;
					case 4:
						color = new Color(0, 0, 255, 0);
						break;
					case 5:
						color = new Color(140, 0, 255, 0);
						break;
				}
				Vector2 rotateAroundArea = new Vector2(0, 2) + (new Vector2(6f, 0).RotatedBy(MathHelper.ToRadians(180 * percent)));
				Vector2 rotationAround = rotateAroundArea + new Vector2((12 + mult) * scale, 0).RotatedBy(MathHelper.ToRadians(60 * i + counter));
				Vector2 rotationAround2 = 0.5f * new Vector2((12 + mult * 0.5f) * scale, 0).RotatedBy(MathHelper.ToRadians(60 * i + counter));
				float rotation2 = rotationAround.ToRotation() - MathHelper.ToRadians(90);
				float dist = 2;
				float scale2 = 1 + 0.5f * lightIntesity;
				Main.spriteBatch.Draw(texture2, rotationAround2 + item.Center - Main.screenPosition + new Vector2(0, 2), null, color * lightIntesity, rotation, drawOrigin, scale * 1.1f, SpriteEffects.None, 0f);
				for (int k = 0; k < 51; k++)
				{
					scale2 += 0.1f + 0.01f * lightIntesity;
					dist += 2;
					Vector2 fromCenter = item.Center + new Vector2(0, dist * scale).RotatedBy(rotation2);
					int width = (int)(2 * scale2);
					int height = 2;
					drawOrigin = new Vector2(width * 0.5f, 0);
					Main.spriteBatch.Draw(texture, fromCenter - Main.screenPosition, new Rectangle(0, 0, width, height), color * lightIntesity * ((51f - k) / 51f), rotation + rotation2, drawOrigin, 1, SpriteEffects.None, 0f);
				}
			}

			return base.PreDrawInWorld(spriteBatch, lightColor, alphaColor, ref rotation, ref scale, whoAmI);
        }

        public override void SetDefaults()
		{
			item.width = 20;
			item.height = 18;
			item.maxStack = 999;
			item.value = Item.sellPrice(1, 0, 0, 0);
			item.rare = 6;
		}
	}
}