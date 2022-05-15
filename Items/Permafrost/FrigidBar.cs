using SOTS.Items.Fragments;
using SOTS.Items.GhostTown;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.Permafrost
{
	public class FrigidBar : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Frigid Bar");
		}
		public override void SetDefaults()
		{
			Item.CloneDefaults(ItemID.IronBar);
			Item.width = 24;
			Item.height = 22;
            Item.value = Item.sellPrice(0, 0, 6, 0);
			Item.rare = ItemRarityID.Green;
			Item.maxStack = 99;
			Item.placeStyle = 8;
			Item.createTile = ModContent.TileType<TheBars>();
		}
		public override void AddRecipes()
		{
			CreateRecipe(2).AddIngredient(ModContent.ItemType<FrigidIce>(), 10).AddIngredient(ModContent.ItemType<AncientSteelBar>(), 1).AddIngredient(ModContent.ItemType<FragmentOfPermafrost>(), 1).AddTile(TileID.Anvils).Register();
        }
	}
}