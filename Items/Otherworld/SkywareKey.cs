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
			Vector2 drawOrigin = new Vector2(Terraria.GameContent.TextureAssets.Item[Item.type].Value.Width * 0.5f, Item.height * 0.5f);
			Main.spriteBatch.Draw(texture, new Vector2((float)(Item.Center.X - (int)Main.screenPosition.X), (float)(Item.Center.Y - (int)Main.screenPosition.Y) + 2), null, color, rotation, drawOrigin, scale, SpriteEffects.None, 0f);
		}
		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault("Opens one locked Skyware Chest");
			this.SetResearchCost(1);
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
			//Item.useStyle = ItemUseStyleID.Swing;
			//Item.createTile = mod.TileType("LockedSkywareChest");
			//	Item.placeStyle = 1;
		}
		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(ModContent.ItemType<MeteoriteKey>(), 1).AddTile(Mod.Find<ModTile>("TransmutationAltarTile").Type).Register();
			CreateRecipe(1).AddIngredient(ModContent.ItemType<StrangeKey>(), 1).AddTile(Mod.Find<ModTile>("TransmutationAltarTile").Type).Register();
		}
	}
}