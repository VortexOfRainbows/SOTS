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
			Tooltip.SetDefault("Uses power from the soul\nIncreases damage by 1% but decreases max mana by 20 while in the inventory");
		}
		public override void SetDefaults()
		{

			item.width = 30;
			item.height = 30;
			item.value = 1250;
			item.rare = 2;
			item.maxStack = 3;
		}
		public override void UpdateInventory(Player player)
		{
				for(int i = item.stack; i > 0; i--)
				{
					if(player.statManaMax2 > 20)
					{
						player.statManaMax2 -= 20;
						player.meleeDamage += 0.01f;
						player.rangedDamage += 0.01f;
						player.thrownDamage += 0.01f;
						player.magicDamage += 0.01f;
					}
				}
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.ManaCrystal, 1);
			recipe.AddIngredient(null, "Goblinsteel", 4);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}