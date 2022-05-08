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
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "DissolvingAurora", 1);
			recipe.AddIngredient(ItemID.FrostCore, 1);
			recipe.AddIngredient(ItemID.SoulofSight, 3);
			recipe.AddIngredient(ItemID.SoulofMight, 3);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this, 1);
			recipe.AddRecipe();
		}
	}
}