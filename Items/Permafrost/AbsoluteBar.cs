using SOTS.Items.Fragments;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.Permafrost
{
	public class AbsoluteBar : ModItem
	{
		public override void SetStaticDefaults()
		{
			this.SetResearchCost(25);
		}
		public override void SetDefaults()
		{
			Item.CloneDefaults(ItemID.IronBar);
			Item.width = 30;
			Item.height = 24;
			Item.value = Item.sellPrice(0, 1, 30, 0);
			Item.rare = ItemRarityID.Pink;
			Item.maxStack = 9999;
			Item.placeStyle = 7;
			Item.createTile = ModContent.TileType<TheBars>();
        }
        public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient(ItemID.HallowedBar, 1).AddIngredient<SoulOfPlight>(1).AddTile(TileID.AdamantiteForge).Register();
        }
    }
}