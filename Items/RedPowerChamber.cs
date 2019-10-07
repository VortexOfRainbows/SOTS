using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items
{
	public class RedPowerChamber : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Red Power Chamber");
			Tooltip.SetDefault("'Uses power from the heart'\nIncreases damage by 3% but decreases max life by 10 while in the inventory");
		}
		public override void SetDefaults()
		{

			item.width = 30;
			item.height = 30;
			item.value = 1250;
			item.rare = 2;
			item.maxStack = 4;
		}
		public override void UpdateInventory(Player player)
		{
			
				for(int i = item.stack; i > 0; i--)
				{
					if(player.statLifeMax2 > 20)
					{
						player.statLifeMax2 -= 10;
						player.meleeDamage += 0.03f;
						player.rangedDamage += 0.03f;
						player.thrownDamage += 0.03f;
						player.magicDamage += 0.03f;
					}
				}
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.LifeCrystal, 1);
			recipe.AddIngredient(ItemID.GoldBar, 4);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
			
			recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.LifeCrystal, 1);
			recipe.AddIngredient(ItemID.PlatinumBar, 4);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}