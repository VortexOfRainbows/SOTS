using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items
{
	public class BluePowerChamber : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Blue Power Chamber");
			Tooltip.SetDefault("'Uses power from the soul'\nIncreases damage by 1% but decreases max mana by 20 while in the inventory");
		}
		public override void SetDefaults()
		{

			item.width = 30;
			item.height = 30;
            item.value = Item.sellPrice(0, 0, 10, 0);
			item.rare = 1;
			item.maxStack = 3;
		}
		public override void UpdateInventory(Player player)
		{
				for(int i = item.stack; i > 0; i--)
				{
					if(player.statManaMax2 > 20)
					{
						player.statManaMax2 -= 20;
						player.allDamage += 0.01f;
					}
				}
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.ManaCrystal, 1);
			recipe.AddIngredient(null, "Goblinsteel", 3);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}