using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.Otherworld
{
	class SkywareKey : ModItem
	{
		public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
		{
			Texture2D texture = Mod.Assets.Request<Texture2D>("Items/Otherworld/SkywareKeyGlow").Value;
			Color color = Color.White;
			Vector2 drawOrigin = new Vector2(Main.itemTexture[Item.type].Width * 0.5f, Item.height * 0.5f);
			Main.spriteBatch.Draw(texture, new Vector2((float)(Item.Center.X - (int)Main.screenPosition.X), (float)(Item.Center.Y - (int)Main.screenPosition.Y) + 2), null, color, rotation, drawOrigin, scale, SpriteEffects.None, 0f);
		}
		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault("Opens one locked Skyware Chest");
		}
		public override void SetDefaults()
		{
			//Item.CloneDefaults(ItemID.GoldenKey);
			Item.width = 22;
			Item.height = 36;
			Item.maxStack = 99;
			Item.rare = 2;

			//Item.useAnimation = 15;
			//Item.useTime = 10;
			//Item.useStyle = 1;
			//Item.createTile = mod.TileType("LockedSkywareChest");
			//	Item.placeStyle = 1;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<MeteoriteKey>(), 1);
			recipe.AddTile(mod.TileType("TransmutationAltarTile"));
			recipe.SetResult(this, 1);
			recipe.AddRecipe();

			recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<StrangeKey>(), 1);
			recipe.AddTile(mod.TileType("TransmutationAltarTile"));
			recipe.SetResult(this, 1);
			recipe.AddRecipe();
		}
	}
}