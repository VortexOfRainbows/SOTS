using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.Slime
{
	public class VialofAcid : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Vial of Acid");
			Tooltip.SetDefault("'A gooey corrosive substance that excels at dissolving organic compounds'");
		}
		public override void SetDefaults()
		{
			item.width = 24;
			item.height = 24;
			item.maxStack = 999;
			item.rare = ItemRarityID.Blue;
			item.value = Item.sellPrice(0, 0, 2, 0);
		}
	}
	public class CorrosiveGel : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Corrosive Gel");
			Tooltip.SetDefault("'It's like pink gel except it'll melt your fingers if you're not careful'");
		}
		public override void SetDefaults()
		{
			item.width = 20;
			item.height = 22;
			item.maxStack = 999;
			item.useStyle = ItemUseStyleID.SwingThrow;
			item.rare = ItemRarityID.Blue;
			item.value = Item.sellPrice(0, 0, 5, 0);
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<VialofAcid>(), 1);
			recipe.AddIngredient(ItemID.PinkGel, 2);
			recipe.AddTile(TileID.Solidifier);
			recipe.SetResult(this, 2);
			recipe.AddRecipe();
		}
	}
}