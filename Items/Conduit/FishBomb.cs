using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SOTS.Items.Fragments;
using Microsoft.Xna.Framework;

namespace SOTS.Items.Conduit
{
	public class FishBomb : ModItem
	{
		public override void SetStaticDefaults()
		{
			this.SetResearchCost(99);
		}
		public override void SetDefaults()
		{
			Item.CloneDefaults(ItemID.BombFish);
			Item.width = 54;
			Item.height = 40;
			Item.useTime += 10;
			Item.useAnimation += 10;
            Item.rare = ModContent.RarityType<AnomalyRarity>();
            Item.autoReuse = true;
            Item.consumable = true;
            Item.shoot = ModContent.ProjectileType<Projectiles.Tide.FishBomb>(); 
            Item.shootSpeed += 2.5f;
        }
        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
			damage = 30;
        }
        public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(ItemID.ApprenticeBait, 1).AddIngredient(ItemID.BombFish, 1).AddIngredient<FragmentOfTide>(1).AddIngredient<SkipSoul>(6).Register();
			CreateRecipe(2).AddIngredient(ItemID.JourneymanBait, 1).AddIngredient(ItemID.BombFish, 2).AddIngredient<FragmentOfTide>(2).AddIngredient<SkipSoul>(8).Register();
			CreateRecipe(3).AddIngredient(ItemID.MasterBait, 1).AddIngredient(ItemID.BombFish, 3).AddIngredient<FragmentOfTide>(3).AddIngredient<SkipSoul>(10).Register();
        }
    }
}