using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using Microsoft.Xna.Framework.Graphics;

namespace SOTS.Items.Fragments
{
	public class DissolvingNature : ModItem
	{
		int frameCounter;
		int frame;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Dissolving Nature");
			Tooltip.SetDefault("Reduces damage dealt by 10% while in the inventory");
			Main.RegisterItemAnimation(item.type, new DrawAnimationVertical(5, 6));
		}
		public override bool PreDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
		{
			Texture2D texture = Main.itemTexture[item.type];
			Color color = new Color(100, 100, 100, 0);
			for (int k = 0; k < 7; k++)
			{
				float x = Main.rand.Next(-10, 11) * 0.15f;
				float y = Main.rand.Next(-10, 11) * 0.15f;
				Main.spriteBatch.Draw(texture,
				new Vector2(position.X + x, position.Y + y),
				new Rectangle(0, 42 * this.frame, 26, 42), color * (1f - (item.alpha / 255f)), 0f, origin, scale, SpriteEffects.None, 0f);
			}
			return false;
		}
		public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
		{
			frameCounter++;
			if (frameCounter >= 5)
			{
				frameCounter = 0;
				frame++;
			}
			if (frame >= 6)
			{
				frame = 0;
			}

			Texture2D texture = Main.itemTexture[item.type];
			Color color = new Color(100, 100, 100, 0);
			Vector2 drawOrigin = new Vector2(Main.itemTexture[item.type].Width * 0.5f, item.height * 0.5f);
			for (int k = 0; k < 7; k++)
			{
				float x = Main.rand.Next(-10, 11) * 0.15f;
				float y = Main.rand.Next(-10, 11) * 0.15f;
				Main.spriteBatch.Draw(texture,
				new Vector2((float)(item.Center.X - (int)Main.screenPosition.X) + x, (float)(item.Center.Y - (int)Main.screenPosition.Y) + y),
				new Rectangle(0, 42 * frame, 26, 42), color * (1f - (item.alpha / 255f)), rotation, drawOrigin, scale, SpriteEffects.None, 0f);
			}
			return false;
		}
		public override void SetDefaults()
		{ 
			item.width = 26;
			item.height = 42;
            item.value = Item.sellPrice(0, 1, 0, 0);
			item.rare = 3;
			item.maxStack = 999;
			ItemID.Sets.ItemNoGravity[item.type] = true; 
		}
		public override void UpdateInventory(Player player)
		{
			frameCounter++;
			if (frameCounter >= 5)
			{
				frameCounter = 0;
				frame++;
			}
			if (frame >= 6)
			{
				frame = 0;
			}
			for (int i = 0; i < item.stack; i++)
			{
				if(player.allDamage > 0f)
				{
					player.allDamage -= 0.1f;
				}
				else
				{
					player.allDamage = 0;
				}
			}
		}
	}
}