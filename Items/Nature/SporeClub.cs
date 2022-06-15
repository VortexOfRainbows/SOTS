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
			this.SetResearchCost(1);
		}
		public override void SetDefaults()
		{
			Item.damage = 11;
			Item.DamageType = DamageClass.Melee;
			Item.width = 46;
			Item.height = 52;
			Item.useTime = 30;
			Item.useAnimation = 30;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.knockBack = 5.75f;
			Item.value = Item.sellPrice(0, 0, 20, 0);
			Item.rare = ItemRarityID.Blue;
			Item.UseSound = SoundID.Item18;
			Item.autoReuse = false;            
			Item.shoot = ModContent.ProjectileType<ShroomSpore>(); 
            Item.shootSpeed = 5f;
		}
		public override void AddRecipes()
		{
			CreateRecipe(1).AddRecipeGroup(RecipeGroupID.Wood, 20).AddIngredient(ModContent.ItemType<FragmentOfNature>(), 4).AddIngredient(ItemID.Mushroom, 10).AddTile(TileID.WorkBenches).Register();
		}
	}
}