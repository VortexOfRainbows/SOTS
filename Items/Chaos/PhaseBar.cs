using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SOTS.Items.Fragments;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SOTS.Items.Chaos
{
	public class PhaseBar : ModItem
	{
		public override bool PreDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
		{
			Texture2D texture = Main.itemTexture[item.type];
			Color color = new Color(100, 100, 100, 0);
			for (int k = 0; k < 4; k++)
			{
				Vector2 offset = new Vector2(2.5f, 0).RotatedBy(MathHelper.ToRadians(Main.GameUpdateCount * 3 + k * 90));
				Main.spriteBatch.Draw(texture, position + Main.rand.NextVector2Circular(1.0f, 1.0f) + offset, frame, color * 1.1f * (1f - (item.alpha / 255f)), 0f, origin, scale, SpriteEffects.None, 0f);
			}
			Main.spriteBatch.Draw(texture, position, frame, Color.White * 0.65f, 0f, origin, scale, SpriteEffects.None, 0f);
			return false;
		}
		public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
		{
			Texture2D texture = Main.itemTexture[item.type];
			Color color = new Color(100, 100, 100, 0);
			Vector2 drawOrigin = new Vector2(Main.itemTexture[item.type].Width * 0.5f, item.height * 0.5f);
			for (int k = 0; k < 4; k++)
			{
				Vector2 offset = new Vector2(2.5f, 0).RotatedBy(MathHelper.ToRadians(Main.GameUpdateCount * 3 + k * 90));
				Main.spriteBatch.Draw(texture, item.Center - Main.screenPosition + Main.rand.NextVector2Circular(1.0f, 1.0f) + offset, null, color * 1.1f * (1f - (item.alpha / 255f)), rotation, drawOrigin, scale, SpriteEffects.None, 0f);
			}
			Main.spriteBatch.Draw(texture, item.Center - Main.screenPosition, null, Color.White * 0.65f, rotation, drawOrigin, scale, SpriteEffects.None, 0f);
			return false;
		}
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Phase Bar");
			Tooltip.SetDefault("'It borders on the edge of reality'");
		}
		public override void SetDefaults()
		{
			item.CloneDefaults(ItemID.IronBar);
			item.width = 30;
			item.height = 24;
			item.value = Item.sellPrice(0, 1, 80, 0);
			item.rare = ItemRarityID.LightRed;
			item.maxStack = 99;
			item.placeStyle = 10;
			item.createTile = ModContent.TileType<TheBars>();
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<PhaseOre>(), 30);
			recipe.AddIngredient(ItemID.CrystalShard, 5);
			recipe.AddIngredient(ModContent.ItemType<FragmentOfChaos>(), 1);
			recipe.AddTile(TileID.AdamantiteForge);
			recipe.SetResult(this, 3);
			recipe.AddRecipe();
		}
	}
}