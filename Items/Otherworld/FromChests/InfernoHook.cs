using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.Otherworld.FromChests
{
	public class InfernoHook : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Inferno Hook");
			Tooltip.SetDefault("A very fast hook that strikes enemies as it travels");
		}
		public override void SetDefaults()
		{
			Item.CloneDefaults(ItemID.AmethystHook);
			Item.damage = 33;
			Item.DamageType = DamageClass.Melee;
			Item.knockBack = 3f;
            Item.width = 40;  
            Item.height = 40;   
            Item.value = Item.sellPrice(0, 3, 80, 0);
            Item.rare = ItemRarityID.LightPurple;
			Item.shoot = mod.ProjectileType("InfernoHook"); 
            Item.shootSpeed = 26f;
		}
		public override void AddRecipes()
		{
			Recipe recipe = new Recipe(mod);
			recipe.AddIngredient(null, "WormWoodHook", 1);
			recipe.AddIngredient(null, "FragmentOfInferno", 2);
			recipe.AddIngredient(null, "OtherworldlyAlloy", 10);
			recipe.AddTile(mod.TileType("HardlightFabricatorTile"));
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}
