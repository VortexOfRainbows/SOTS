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
			Item.shoot = Mod.Find<ModProjectile>("InfernoHook").Type; 
            Item.shootSpeed = 26f;
		}
		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(null, "WormWoodHook", 1).AddIngredient(null, "FragmentOfInferno", 2).AddIngredient(null, "OtherworldlyAlloy", 10).AddTile(mod.TileType("HardlightFabricatorTile")).Register();
		}
	}
}
