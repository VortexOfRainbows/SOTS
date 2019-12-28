using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.Fragments
{
	public class SporeClub : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Spore Club");
			Tooltip.SetDefault("Launch a spore that may confuse enemies");
		}
		public override void SetDefaults()
		{

			item.damage = 9;
			item.melee = true;
			item.width = 36;
			item.height = 36;
			item.useTime = 34;
			item.useAnimation = 34;
			item.useStyle = 1;
			item.knockBack = 5.75f;
			item.value = Item.sellPrice(0, 0, 20, 0);
			item.rare = 1;
			item.UseSound = SoundID.Item18;
			item.autoReuse = false;            
			item.shoot = mod.ProjectileType("ShroomSpore"); 
            item.shootSpeed = 4.5f;

		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.Wood, 20);
			recipe.AddIngredient(null, "FragmentOfNature", 4);
			recipe.AddIngredient(ItemID.Mushroom, 10);
			recipe.AddTile(TileID.WorkBenches);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}