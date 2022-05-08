using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items
{
	public class ElectromagneticDeterrent : ModItem
	{
		int frameCounter = 0;
		int frame = 0;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Disruptive Electromagnetic Field Emitter");
			Tooltip.SetDefault("Prevents constructs from spawning while favorited in the inventory");
			Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(5, 15));
		}
		public override void SetDefaults()
		{
            Item.width = 54;     
            Item.height = 30;   
            Item.value = Item.sellPrice(0, 3, 0, 0);
            Item.rare = ItemRarityID.Orange;
		}
		public override bool PreDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
		{
			UpgradeFrame();
			Texture2D texture = Main.itemTexture[Item.type];
			Color color = new Color(100, 100, 100, 0);
			Texture2D texture2 = (Texture2D)ModContent.Request<Texture2D>("SOTS/Items/ElectromagneticDeterrentGlow");
			Main.spriteBatch.Draw(texture, position, new Rectangle(0, 30 * this.frame, 54, 30), drawColor, 0f, origin, scale, SpriteEffects.None, 0f);
			for (int k = 0; k < 3; k++)
			{
				Main.spriteBatch.Draw(texture2, position + Main.rand.NextVector2Circular(1.0f, 1.0f), new Rectangle(0, 30 * this.frame, 54, 30), color * 1.1f * (1f - (Item.alpha / 255f)), 0f, origin, scale, SpriteEffects.None, 0f);
			}
			return false;
		}
		public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
		{
			UpgradeFrame();
			Texture2D texture = Main.itemTexture[Item.type];
			Texture2D texture2 = (Texture2D)ModContent.Request<Texture2D>("SOTS/Items/ElectromagneticDeterrentGlow");
			Color color = new Color(100, 100, 100, 0);
			Vector2 drawOrigin = new Vector2(Main.itemTexture[Item.type].Width * 0.5f, Item.height * 0.5f);
			Main.spriteBatch.Draw(texture, Item.Center - Main.screenPosition, new Rectangle(0, 30 * this.frame, 54, 30), lightColor, rotation, drawOrigin, scale, SpriteEffects.None, 0f);
			for (int k = 0; k < 3; k++)
			{
				Main.spriteBatch.Draw(texture2, Item.Center - Main.screenPosition + Main.rand.NextVector2Circular(1.0f, 1.0f), new Rectangle(0, 30 * this.frame, 54, 30), color * 1.1f * (1f - (Item.alpha / 255f)), rotation, drawOrigin, scale, SpriteEffects.None, 0f);
			}
			return false;
		}
		public void UpgradeFrame()
		{
			frameCounter++;
			if (frameCounter >= 5)
			{
				frameCounter = 0;
				frame++;
			}
			if (frame >= 15)
			{
				frame = 0;
			}
		}
		public override void UpdateInventory(Player player)
		{
			if (Item.favorited)
				SOTSPlayer.ModPlayer(player).noMoreConstructs = true;
        }
    }
}