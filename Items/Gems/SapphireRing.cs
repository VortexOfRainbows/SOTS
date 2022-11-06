using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Void;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.Gems
{
	public class SapphireRing : ModItem
	{
		public override bool PreDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
		{
			Texture2D texture = ModContent.Request<Texture2D>(this.Texture + "Glow").Value;
			for (int k = 0; k < 6; k++)
			{
				Vector2 circular = new Vector2(0, 2).RotatedBy(MathHelper.ToRadians(k * 60 + SOTSWorld.GlobalCounter * 2));
				Color color = new Color(120 - k * 7, 110 - k * 2, 100 + k * 4, 0);
				Main.spriteBatch.Draw(texture, position + circular, null, color * (1f - (Item.alpha / 255f)) * 1.2f, 0f, origin, scale, SpriteEffects.None, 0f);
			}
			return true;
		}
		public override void PostDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
		{
			Texture2D texture = ModContent.Request<Texture2D>(this.Texture + "Glow").Value;
			Main.spriteBatch.Draw(texture, position, null, Color.Lerp(drawColor, Color.White, 0.5f), 0f, origin, scale, SpriteEffects.None, 0f);
		}
		public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
		{
			Texture2D texture = ModContent.Request<Texture2D>(this.Texture + "Glow").Value;
			Vector2 drawOrigin = new Vector2(Terraria.GameContent.TextureAssets.Item[Item.type].Value.Width * 0.5f, Item.height * 0.5f);
			for (int k = 0; k < 6; k++)
			{
				Vector2 circular = new Vector2(0, 2).RotatedBy(MathHelper.ToRadians(k * 60 + SOTSWorld.GlobalCounter * 2));
				Color color = new Color(120 - k * 7, 110 - k * 2, 100 + k * 4, 0);
				Main.spriteBatch.Draw(texture, Item.Center - Main.screenPosition + circular, null, color * (1f - (Item.alpha / 255f)) * 1.2f, rotation, drawOrigin, scale, SpriteEffects.None, 0f);
			}
			return true;
		}
		public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
		{
			Texture2D texture = ModContent.Request<Texture2D>(this.Texture + "Glow").Value;
			Vector2 drawOrigin = new Vector2(Terraria.GameContent.TextureAssets.Item[Item.type].Value.Width * 0.5f, Item.height * 0.5f);
			Main.spriteBatch.Draw(texture, Item.Center - Main.screenPosition, null, Color.Lerp(lightColor, Color.White, 0.5f), rotation, drawOrigin, scale, SpriteEffects.None, 0f);
		}
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Mammon's Ring");
			Tooltip.SetDefault("Generate income when the void is gained");
			this.SetResearchCost(1);
		}
		public override void SetDefaults()
		{
			Item.maxStack = 1;
            Item.width = 22;     
            Item.height = 20;   
            Item.value = Item.sellPrice(0, 1, 0, 0);
            Item.rare = ItemRarityID.Green;
			Item.accessory = true;
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			VoidPlayer vPlayer = VoidPlayer.ModPlayer(player);
			vPlayer.VoidGenerateMoney += 1f;
		}
	}
}