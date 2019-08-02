using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
 
namespace SOTS.Items.SpectreCog
{
    public class EctoApple : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Ecto Apple");
			Tooltip.SetDefault("Summons a true patronus unicorn, counts as a light pet");
		}
        public override void SetDefaults()
        {
            item.CloneDefaults(ItemID.Carrot);
            item.shoot = mod.ProjectileType("PetName");
            item.buffType = mod.BuffType("Purity");
			item.damage = 100;
        }
 
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "ReanimationMaterial", 11);
			
            recipe.AddIngredient(null, "SpectreManipulator", 1);
            recipe.AddTile(TileID.LunarCraftingStation);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
 
        public override void UseStyle(Player player)
        {
            if (player.whoAmI == Main.myPlayer && player.itemTime == 0)
            {
                player.AddBuff(item.buffType, 3600, true);
            }
        }
    }
}