using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.Fragments
{
	public class ChaosBadge : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Chaos Badge");
			Tooltip.SetDefault("Increases crit chance by 10%");
		}
		public override void SetDefaults()
		{
            item.width = 38;     
            item.height = 38;     
            item.value = Item.sellPrice(0, 0, 75, 0);
            item.rare = 6;
			item.accessory = true;
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.meleeCrit += 10;
			player.rangedCrit += 10;
			player.magicCrit += 10;
			player.thrownCrit += 10;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.CobaltBar, 10);
			recipe.AddIngredient(null, "FragmentOfChaos", 4);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
			
			recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.PalladiumBar, 10);
			recipe.AddIngredient(null, "FragmentOfChaos", 4);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}
