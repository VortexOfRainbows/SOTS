using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.IceStuff
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
			item.width = 22;
			item.height = 36;
			item.value = Item.sellPrice(0, 7, 0, 0);
			item.rare = ItemRarityID.LightPurple;
			item.maxStack = 1;
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