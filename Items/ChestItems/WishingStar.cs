using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.ChestItems
{
	public class WishingStar : ModItem
    {
        public override bool PreDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
        {
            Texture2D textureG = (Texture2D)ModContent.Request<Texture2D>("SOTS/Items/ChestItems/WishingStarGlow");
            for (int i = 0; i < 8; i++)
            {
                float sinusoid = 4f + 2f * (float)Math.Sin(SOTSWorld.GlobalCounter * MathHelper.Pi / 90f);
                Vector2 circular = new Vector2(sinusoid * scale, 0).RotatedBy(i * MathHelper.TwoPi / 8f);
                spriteBatch.Draw(textureG, position + circular, frame, new Color(120, 110, 130, 0), 0f, origin, scale, SpriteEffects.None, 0f);
            }
            //spriteBatch.Draw(texture, position, frame, drawColor, 0f, origin, scale, SpriteEffects.None, 0f);
            return true;
        }
        public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
        {
            Texture2D textureG = (Texture2D)ModContent.Request<Texture2D>("SOTS/Items/ChestItems/WishingStarGlow");
            Vector2 origin = Item.Size / 2;
            for (int i = 0; i < 8; i++)
            {
                float sinusoid = 4f + 2f * (float)Math.Sin(SOTSWorld.GlobalCounter * MathHelper.Pi / 90f);
                Vector2 circular = new Vector2(sinusoid * scale, 0).RotatedBy(i * MathHelper.TwoPi / 8f);
                spriteBatch.Draw(textureG, Item.Center - Main.screenPosition + circular, null, new Color(120, 110, 130, 0), rotation, origin, scale, SpriteEffects.None, 0f);
            }
            //spriteBatch.Draw(texture, Item.Center - Main.screenPosition, frame, Color.White, rotation, origin, scale, SpriteEffects.None, 0f);
            return true;
        }
        public override void SetStaticDefaults()
		{
			this.SetResearchCost(1);
		}
		public override void SetDefaults()
		{
            Item.width = 52;     
            Item.height = 52;   
            Item.value = Item.sellPrice(0, 5, 0, 0);
            Item.rare = ItemRarityID.Orange;
			Item.accessory = true;
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			SOTSPlayer modPlayer = SOTSPlayer.ModPlayer(player);
			modPlayer.WishingStar = true;
        }
        public override bool WeaponPrefix()
        {
            return false;
        }
    }
}