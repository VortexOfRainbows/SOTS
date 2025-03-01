using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.Invidia
{
	public class InvidiaPetal : ModItem
    {
        public override Color? GetAlpha(Color lightColor)
        {
            return Color.Lerp(lightColor, Color.White, 0.5f);
        }
        public override bool PreDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
        {
            Texture2D texture = Terraria.GameContent.TextureAssets.Item[Item.type].Value;
            Color color = new Color(100, 100, 100, 0);
            for (int k = 0; k < 4; k++)
            {
                Vector2 offset = new Vector2(2.5f, 0).RotatedBy(MathHelper.ToRadians(Main.GameUpdateCount * 3 + k * 90));
                Main.spriteBatch.Draw(texture, position + Main.rand.NextVector2Circular(.5f, .5f) + offset, frame, color * 1.1f * (1f - (Item.alpha / 255f)), 0f, origin, scale, SpriteEffects.None, 0f);
            }
            return true;
        }
        public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
        {
            Texture2D texture = Terraria.GameContent.TextureAssets.Item[Item.type].Value;
            Color color = new Color(100, 100, 100, 0);
            Vector2 drawOrigin = new Vector2(Terraria.GameContent.TextureAssets.Item[Item.type].Value.Width * 0.5f, Item.height * 0.5f);
            for (int k = 0; k < 4; k++)
            {
                Vector2 offset = new Vector2(2.5f, 0).RotatedBy(MathHelper.ToRadians(Main.GameUpdateCount * 3 + k * 90));
                Main.spriteBatch.Draw(texture, Item.Center - Main.screenPosition + Main.rand.NextVector2Circular(.5f, .5f) + offset, null, color * 1.1f * (1f - (Item.alpha / 255f)), rotation, drawOrigin, scale, SpriteEffects.None, 0f);
            }
            return true;
        }
        public override void SetStaticDefaults()
		{
			this.SetResearchCost(100);
		}
		public override void SetDefaults()
		{
			Item.width = 22;
			Item.height = 18;
            Item.value = Item.sellPrice(0, 0, 1, 0);
			Item.rare = ItemRarityID.Orange;
			Item.maxStack = 9999;
		}
	}
}