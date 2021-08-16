using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.Fragments
{
	public class EyeOfChaos : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Eye of Chaos");
			Tooltip.SetDefault("Increases crit chance by 20%");
		}
		public override void SetDefaults()
		{
            item.width = 44;     
            item.height = 42;     
            item.value = Item.sellPrice(0, 7, 25, 0);
            item.rare = 8;
			item.accessory = true;
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.meleeCrit += 20;
			player.rangedCrit += 20;
			player.magicCrit += 20;
			player.thrownCrit += 20;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "SnakeEyes", 1);
			recipe.AddIngredient(null, "ChaosBadge", 1);
			recipe.AddIngredient(ItemID.EyeoftheGolem, 1);
			//recipe.AddIngredient(ItemID.SpookyWood, 1); //To be replaced later (dissolving chaos)
			recipe.AddTile(TileID.TinkerersWorkbench);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}
