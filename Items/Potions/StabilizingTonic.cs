using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Linq;
using SOTS.Items.Conduit;
using SOTS.Buffs;

namespace SOTS.Items.Potions
{
	public class StabilizingTonic : ModItem
	{
		public override void SetStaticDefaults()
		{
			this.SetResearchCost(20);
		}
		public override void PostDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
		{
			Texture2D texture = Terraria.GameContent.TextureAssets.Item[Item.type].Value;
			Color color = new(20, 20, 20, 0);
			for (int k = 0; k < 12; k++)
			{
				Vector2 circular = new Vector2(4, 0).RotatedBy(MathHelper.ToRadians(Main.GameUpdateCount + k * 30));
				Main.spriteBatch.Draw(texture, position + circular, null, color * (1f - (Item.alpha / 255f)), 0f, origin, scale, SpriteEffects.None, 0f);
			}
			Main.spriteBatch.Draw(texture, position, null, Color.Black * 0.2f * (1f - (Item.alpha / 255f)), 0f, origin, scale * 0.9f, SpriteEffects.None, 0f);
        }
        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
		{
			Texture2D texture = Terraria.GameContent.TextureAssets.Item[Item.type].Value;
			Color color = new(20, 20, 20, 0);
			Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, Item.height * 0.5f);
			Vector2 pos = new Vector2((float)(Item.Center.X - (int)Main.screenPosition.X), (float)(Item.Center.Y - (int)Main.screenPosition.Y));
            for (int k = 0; k < 12; k++)
            {
                Vector2 circular = new Vector2(4, 0).RotatedBy(MathHelper.ToRadians(Main.GameUpdateCount + k * 30));
				Main.spriteBatch.Draw(texture,
                pos + circular,
				null, color * (1f - (Item.alpha / 255f)), rotation, drawOrigin, scale, SpriteEffects.None, 0f);
            }
            Main.spriteBatch.Draw(texture,
            pos,
            null, Color.Black * 0.2f * (1f - (Item.alpha / 255f)), rotation, drawOrigin, scale * 0.9f, SpriteEffects.None, 0f);
        }
		public override void SetDefaults()
		{
			Item.width = 16;
			Item.height = 28;
            Item.value = Item.sellPrice(0, 1, 0, 0);
			Item.rare = ModContent.RarityType<StrangeWhiteRarity>();
			Item.maxStack = 9999;
            Item.UseSound = SoundID.Item3;
            Item.useStyle = ItemUseStyleID.DrinkLiquid;
            Item.useTurn = true;
            Item.useAnimation = 16;
            Item.useTime = 16;
            Item.consumable = true;     
			Item.buffType = ModContent.BuffType<Hyperphantasia>();
            Item.buffTime = 3600 * 20 + 30;
		}
		public override bool ConsumeItem(Player player) 
		{
			return true;
		}
		public override bool? UseItem(Player player)
		{
			return true;
		}
		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient<DissolvingNihility>().Register();
		}
	}
}