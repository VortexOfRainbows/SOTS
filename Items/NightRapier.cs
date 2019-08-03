using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items
{
	public class NightRapier : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Night Rapier");
			Tooltip.SetDefault("The power of pure darkness is at your fingertips");
		}
		public override void SetDefaults()
		{

			item.damage = 90;
			item.melee = true;
			item.width = 44;
			item.height = 46;
			item.useTime = 15;
			item.useAnimation = 15;
			item.useStyle = 1;
			item.knockBack = 5f;
			item.value = Item.sellPrice(2, 50, 0, 0);
			item.rare = 9;
			item.UseSound = SoundID.Item1;
			item.autoReuse = true;            
			item.shoot = 306; 
            item.shootSpeed = 14;

		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.LightsBane, 1);
			recipe.AddIngredient(ItemID.ScourgeoftheCorruptor, 1);
			recipe.AddIngredient(ItemID.BrokenHeroSword, 1);
			recipe.AddIngredient(ItemID.BeetleHusk, 8);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}