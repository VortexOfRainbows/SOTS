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
			item.damage = 11;
			item.melee = true;
			item.width = 46;
			item.height = 50;
			item.useTime = 30;
			item.useAnimation = 30;
			item.useStyle = ItemUseStyleID.SwingThrow;
			item.knockBack = 5.75f;
			item.value = Item.sellPrice(0, 0, 20, 0);
			item.rare = ItemRarityID.Blue;
			item.UseSound = SoundID.Item18;
			item.autoReuse = false;            
			item.shoot = mod.ProjectileType("ShroomSpore"); 
            item.shootSpeed = 5f;
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