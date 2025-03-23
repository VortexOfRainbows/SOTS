using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.Invidia
{
	public class HardlightHook : ModItem
    {
        public override bool PreDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
        {
            Texture2D texture2 = ModContent.Request<Texture2D>(Texture + "Base").Value;
            Main.spriteBatch.Draw(texture2, position, null, drawColor, 0f, origin, scale, SpriteEffects.None, 0f);
            return false;
        }
        public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
        {
            Texture2D texture2 = ModContent.Request<Texture2D>(Texture + "Base").Value;
            Vector2 drawOrigin = new Vector2(TextureAssets.Item[Item.type].Value.Width * 0.5f, TextureAssets.Item[Type].Value.Height * 0.5f);
            Main.spriteBatch.Draw(texture2, Item.Center - Main.screenPosition, null, Item.GetAlpha(lightColor), rotation, drawOrigin, scale, SpriteEffects.None, 0f);
            return false;
        }
        public override void PostDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
        {
            Texture2D texture = ModContent.Request<Texture2D>(Texture + "Outline").Value;
            Texture2D texture2 = ModContent.Request<Texture2D>(Texture + "Fill").Value;
            Color color = new Color(110, 110, 110, 0);
            for (int k = 0; k < 5; k++)
            {
                float x = Main.rand.Next(-10, 11) * 0.03f;
                float y = Main.rand.Next(-10, 11) * 0.03f;
                if (k == 0)
                    Main.spriteBatch.Draw(texture2, position, null, color * 0.5f, 0f, origin, scale, SpriteEffects.None, 0f);
                Main.spriteBatch.Draw(texture, position + new Vector2(x, y), null, Item.GetAlpha(color), 0f, origin, scale, SpriteEffects.None, 0f);
            }
        }
        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
        {
            Texture2D texture = ModContent.Request<Texture2D>(Texture + "Outline").Value;
            Texture2D texture2 = ModContent.Request<Texture2D>(Texture + "Fill").Value;
            Color color = new Color(110, 110, 110, 0);
            Vector2 drawOrigin = new Vector2(TextureAssets.Item[Type].Value.Width * 0.5f, TextureAssets.Item[Type].Value.Height * 0.5f);
            for (int k = 0; k < 5; k++)
            {
                float x = Main.rand.Next(-10, 11) * 0.03f;
                float y = Main.rand.Next(-10, 11) * 0.03f;
                if (k == 0)
                    Main.spriteBatch.Draw(texture2, Item.Center - Main.screenPosition, null, color * 0.5f, rotation, drawOrigin, scale, SpriteEffects.None, 0f);
                Main.spriteBatch.Draw(texture, Item.Center - Main.screenPosition + new Vector2(x, y), null, Item.GetAlpha(color), rotation, drawOrigin, scale, SpriteEffects.None, 0f);
            }
        }
        public override void SetStaticDefaults()
		{
			this.SetResearchCost(1);
		}
		public override void SetDefaults()
		{
			Item.CloneDefaults(ItemID.AmethystHook);
            Item.width = 28;  
            Item.height = 32;
            Item.value = Item.sellPrice(0, 1, 0, 0);
            Item.rare = ItemRarityID.Green;
            Item.shoot = ModContent.ProjectileType<Projectiles.Sanctuary.HardlightHook>(); 
            Item.shootSpeed = 16f;
		}
	}
}
