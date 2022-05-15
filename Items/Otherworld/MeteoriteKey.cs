using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.Otherworld
{
	class MeteoriteKey : ModItem
	{
		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault("Opens one locked Meteorite Chest");
		}
		public override void SetDefaults()
		{
			//Item.CloneDefaults(ItemID.GoldenKey);
			Item.width = 18;
			Item.height = 36;
			Item.maxStack = 99; 
			Item.rare = 2;

			//Item.useAnimation = 15;
			//Item.useTime = 10;
			//Item.useStyle = ItemUseStyleID.Swing;
			//Item.createTile = mod.TileType("LockedMeteoriteChest");
			//Item.placeStyle = 1;
		}
		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(ModContent.ItemType<SkywareKey>(), 1).AddTile(mod.TileType("TransmutationAltarTile")).Register();
			CreateRecipe(1).AddIngredient(ModContent.ItemType<StrangeKey>(), 1).AddTile(mod.TileType("TransmutationAltarTile")).Register();
		}
	}
}