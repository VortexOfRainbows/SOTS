using SOTS.Items.Fragments;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.Permafrost
{
	public class FrostedKey : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Frost Key");
			Tooltip.SetDefault("'Cold to the touch'");
			this.SetResearchCost(1);
		}
		public override void SetDefaults()
		{
			Item.width = 22;
			Item.height = 36;
			Item.value = Item.sellPrice(0, 7, 0, 0);
			Item.rare = ItemRarityID.LightPurple;
			Item.maxStack = 1;
		}
		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient<DissolvingAurora>(1).AddIngredient(ItemID.FrostCore, 1).AddIngredient(ItemID.SoulofSight, 3).AddIngredient(ItemID.SoulofMight, 3).AddTile(TileID.MythrilAnvil).Register();
		}
	}
}