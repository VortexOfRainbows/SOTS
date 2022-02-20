using SOTS.Items.Fragments;
using SOTS.Projectiles.Nature;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.Nature
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
			item.height = 52;
			item.useTime = 30;
			item.useAnimation = 30;
			item.useStyle = ItemUseStyleID.SwingThrow;
			item.knockBack = 5.75f;
			item.value = Item.sellPrice(0, 0, 20, 0);
			item.rare = ItemRarityID.Blue;
			item.UseSound = SoundID.Item18;
			item.autoReuse = false;            
			item.shoot = ModContent.ProjectileType<ShroomSpore>(); 
            item.shootSpeed = 5f;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddRecipeGroup(RecipeGroupID.Wood, 20);
			recipe.AddIngredient(ModContent.ItemType<FragmentOfNature>(), 4);
			recipe.AddIngredient(ItemID.Mushroom, 10);
			recipe.AddTile(TileID.WorkBenches);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}