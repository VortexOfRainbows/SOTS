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
			item.CloneDefaults(ItemID.AmethystHook);
			item.damage = 33;
			item.melee = true;
			item.knockBack = 3f;
            item.width = 40;  
            item.height = 40;   
            item.value = Item.sellPrice(0, 3, 80, 0);
            item.rare = ItemRarityID.LightPurple;
			item.shoot = mod.ProjectileType("InfernoHook"); 
            item.shootSpeed = 26f;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "WormWoodHook", 1);
			recipe.AddIngredient(null, "FragmentOfInferno", 2);
			recipe.AddIngredient(null, "OtherworldlyAlloy", 10);
			recipe.AddTile(mod.TileType("HardlightFabricatorTile"));
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}
