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
			this.SetResearchCost(1);
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
		private static Texture2D GlowTexture;
		public override bool PreDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frameNotUsed, Color drawColor, Color itemColor, Vector2 origin, float scale)
		{
            GlowTexture ??= Mod.Assets.Request<Texture2D>("Items/Dyes/TaintedPrismDyeGlow", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
			Vector2 drawOrigin = new(GlowTexture.Width * 0.5f, GlowTexture.Height * 0.5f);
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
				Main.spriteBatch.Draw(GlowTexture, position + rotationAround, null, color, 0f, drawOrigin, scale * 1.1f, SpriteEffects.None, 0f);
			}
            Texture2D mainTex = Terraria.GameContent.TextureAssets.Item[Item.type].Value;
			Main.spriteBatch.Draw(mainTex, position, null, Color.Lerp(drawColor, Color.Black, 0.1f), 0f, drawOrigin, scale * 1.0f, SpriteEffects.None, 0f);
			return false;
		}
		public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
		{
            GlowTexture ??= Mod.Assets.Request<Texture2D>("Items/Dyes/TaintedPrismDyeGlow", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
			Vector2 drawOrigin = new Vector2(GlowTexture.Width * 0.5f, GlowTexture.Height * 0.5f);
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
				Main.spriteBatch.Draw(GlowTexture, rotationAround2 + Item.Center - Main.screenPosition, null, color, rotation, drawOrigin, scale * 1.1f, SpriteEffects.None, 0f);
			}
            Texture2D mainTex = Terraria.GameContent.TextureAssets.Item[Item.type].Value;
			Main.spriteBatch.Draw(mainTex, Item.Center - Main.screenPosition, null, Color.Lerp(lightColor, Color.Black, 0.7f), rotation, drawOrigin, scale * 1.1f, SpriteEffects.None, 0f);
			return true;
		}
		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(ModContent.ItemType<TaintedKeystoneShard>(), 1).AddIngredient(ItemID.BottledWater, 1).AddTile(TileID.DyeVat).Register();
		}
	}
}