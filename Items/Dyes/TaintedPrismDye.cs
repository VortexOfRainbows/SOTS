using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Items.Pyramid;

namespace SOTS.Items.Dyes
{
	public class TaintedPrismDye : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Tainted Prism Dye");
			Tooltip.SetDefault("");
		}
		public override void SetDefaults()
		{
			Item.width = 16;
			Item.height = 24;
			Item.value = Item.sellPrice(0, 5, 0, 0);
			Item.rare = ItemRarityID.LightRed;
		}
		public override void PostUpdate()
		{
			Lighting.AddLight(Item.Center, 10 / 255f, 10 / 255f, 10 / 255f);
		}
		public override bool PreDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frameNotUsed, Color drawColor, Color itemColor, Vector2 origin, float scale)
		{
			Texture2D texture = Mod.Assets.Request<Texture2D>("Items/Dyes/TaintedPrismDyeGlow").Value;
			Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
			position += drawOrigin * scale;
			float counter = Main.GlobalTimeWrappedHourly * 160;
			float mult = new Vector2(-1f, 0).RotatedBy(MathHelper.ToRadians(counter)).X;
			for (int i = 0; i < 6; i++)
			{
				Color color = new Color(255, 0, 0, 0);
				switch (i)
				{
					case 0:
						color = new Color(50, 255, 255, 0);
						break;
					case 1:
						color = new Color(255, 50, 255, 0);
						break;
					case 2:
						color = new Color(255, 255, 50, 0);
						break;
					case 3:
						color = new Color(100, 255, 255, 0);
						break;
					case 4:
						color = new Color(255, 100, 255, 0);
						break;
					case 5:
						color = new Color(255, 255, 100, 0);
						break;
				}
				Vector2 rotationAround = new Vector2((3 + mult) * scale, 0).RotatedBy(MathHelper.ToRadians(60 * i + counter));
				Main.spriteBatch.Draw(texture, new Vector2(position.X, position.Y) + rotationAround, null, color, 0f, drawOrigin, scale * 1.1f, SpriteEffects.None, 0f);
			}
			texture = Terraria.GameContent.TextureAssets.Item[Item.type].Value;
			Main.spriteBatch.Draw(texture, new Vector2(position.X, position.Y), null, Color.Lerp(drawColor, Color.Black, 0.1f), 0f, drawOrigin, scale * 1.0f, SpriteEffects.None, 0f);
			return true;
		}
		public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
		{
			Texture2D texture2 = Mod.Assets.Request<Texture2D>("Items/Dyes/TaintedPrismDyeGlow").Value;
			Vector2 drawOrigin = new Vector2(texture2.Width * 0.5f, texture2.Height * 0.5f);
			float counter = Main.GlobalTimeWrappedHourly * 160;
			float mult = new Vector2(-2.5f, 0).RotatedBy(MathHelper.ToRadians(counter)).X;
			for (int i = 0; i < 6; i++)
			{
				Color color = new Color(255, 0, 0, 0);
				switch (i)
				{
					case 0:
						color = new Color(50, 255, 255, 0);
						break;
					case 1:
						color = new Color(255, 50, 255, 0);
						break;
					case 2:
						color = new Color(255, 255, 50, 0);
						break;
					case 3:
						color = new Color(100, 255, 255, 0);
						break;
					case 4:
						color = new Color(255, 100, 255, 0);
						break;
					case 5:
						color = new Color(255, 255, 100, 0);
						break;
				}
				Vector2 rotationAround2 = 0.5f * new Vector2((6 + mult) * scale, 0).RotatedBy(MathHelper.ToRadians(60 * i + counter));
				Main.spriteBatch.Draw(texture2, rotationAround2 + Item.Center - Main.screenPosition + new Vector2(0, 2), null, color, rotation, drawOrigin, scale * 1.1f, SpriteEffects.None, 0f);
			}
			texture2 = Terraria.GameContent.TextureAssets.Item[Item.type].Value;
			Main.spriteBatch.Draw(texture2, Item.Center - Main.screenPosition + new Vector2(0, 2), null, Color.Lerp(lightColor, Color.Black, 0.7f), rotation, drawOrigin, scale * 1.1f, SpriteEffects.None, 0f);
			return true;
		}
		public override void AddRecipes()
		{
			Recipe recipe = new Recipe(mod);
			recipe.AddIngredient(ModContent.ItemType<TaintedKeystoneShard>(), 1);
			recipe.AddIngredient(ItemID.BottledWater, 1);
			recipe.AddTile(TileID.DyeVat);
			recipe.SetResult(this, 1);
			recipe.AddRecipe();
		}
	}
}