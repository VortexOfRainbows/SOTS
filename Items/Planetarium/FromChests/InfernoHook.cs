using SOTS.Items.Fragments;
using SOTS.Items.Slime;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.Planetarium.FromChests
{
	public class InfernoHook : ModItem
	{
		public override void SetStaticDefaults()
		{
			this.SetResearchCost(1);
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
			Item.shoot = ModContent.ProjectileType<Projectiles.Planetarium.InfernoHook>(); 
            Item.shootSpeed = 26f;
		}
		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient<WormWoodHook>(1).AddIngredient<FragmentOfInferno>(2).AddIngredient<OtherworldlyAlloy>(10).AddTile(ModContent.TileType<Furniture.HardlightFabricatorTile>()).Register();
		}
	}
}
