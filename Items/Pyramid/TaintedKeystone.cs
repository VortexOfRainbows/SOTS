using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework.Graphics;

namespace SOTS.Items.Pyramid
{
	public class TaintedKeystone : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Tainted Keystone");
			Tooltip.SetDefault("'What a regal crystallization of void energy!'");
		}
		public override void SetDefaults()
		{
			item.width = 18;
			item.height = 42;
			item.value = Item.sellPrice(0, 25, 0, 0);
			item.rare = ItemRarityID.LightRed;
			item.maxStack = 999;
			ItemID.Sets.ItemNoGravity[item.type] = true;
		}
		public override void PostUpdate()
		{
			Lighting.AddLight(item.Center, 10 / 255f, 10 / 255f, 10 / 255f);
		}
		public override bool PreDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frameNotUsed, Color drawColor, Color itemColor, Vector2 origin, float scale)
		{
			Texture2D texture = Main.itemTexture[item.type];
			Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
			position += drawOrigin * scale;
			float counter = Main.GlobalTime * 160;
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
			Main.spriteBatch.Draw(texture, new Vector2(position.X, position.Y), null, Color.Lerp(drawColor, Color.Black, 0.7f), 0f, drawOrigin, scale * 1.0f, SpriteEffects.None, 0f);
			return false;
		}
		public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
		{
			Texture2D texture = Main.itemTexture[item.type];
			Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
			float counter = Main.GlobalTime * 160;
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
				Main.spriteBatch.Draw(texture, rotationAround2 + item.Center - Main.screenPosition + new Vector2(0, 2), null, color, rotation, drawOrigin, scale * 1.1f, SpriteEffects.None, 0f);
			}
			Main.spriteBatch.Draw(texture, item.Center - Main.screenPosition + new Vector2(0, 2), null, Color.Lerp(lightColor, Color.Black, 0.7f), rotation, drawOrigin, scale * 1.1f, SpriteEffects.None, 0f);
			return false;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<TaintedKeystoneShard>(), 100);
			recipe.AddTile(TileID.DemonAltar);
			recipe.SetResult(this, 1);
			recipe.AddRecipe();
		}
	}
}