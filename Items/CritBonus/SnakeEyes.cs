using SOTS.Items.Fragments;
using SOTS.Items.Pyramid;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.CritBonus
{
	public class SnakeEyes : ModItem
	{ 
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Snake Eyes");
			Tooltip.SetDefault("Increases crit chance by 8%");
		}
		public override void SetDefaults()
		{
            item.width = 32;     
            item.height = 38;     
            item.value = Item.sellPrice(0, 0, 40, 0);
            item.rare = ItemRarityID.Pink;
			item.accessory = true;
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.meleeCrit += 8;
			player.rangedCrit += 8;
			player.magicCrit += 8;
			player.thrownCrit += 8;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<Snakeskin>(), 10);
			recipe.AddIngredient(ModContent.ItemType<FragmentOfEarth>(), 4);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}
