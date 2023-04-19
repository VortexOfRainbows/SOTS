using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using SOTS.Items.Otherworld.Furniture;

namespace SOTS.Items.Otherworld
{
	class MeteoriteKey : ModItem
	{
		public override void SetStaticDefaults()
		{
			this.SetResearchCost(1);
		}
		public override void SetDefaults()
		{
			//Item.CloneDefaults(ItemID.GoldenKey);
			Item.width = 18;
			Item.height = 36;
			Item.maxStack = 99; 
			Item.rare = ItemRarityID.Green;
			Item.value = Item.sellPrice(0, 1, 0, 0);
			Item.shopCustomPrice = Item.buyPrice(0, 50, 0, 0);
			//Item.useAnimation = 15;
			//Item.useTime = 10;
			//Item.useStyle = ItemUseStyleID.Swing;
			//Item.createTile = mod.TileType("LockedMeteoriteChest");
			//Item.placeStyle = 1;
		}
		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(ModContent.ItemType<SkywareKey>(), 1).AddTile<TransmutationAltarTile>().Register();
			CreateRecipe(1).AddIngredient(ModContent.ItemType<StrangeKey>(), 1).AddTile<TransmutationAltarTile>().Register();
		}
	}
}