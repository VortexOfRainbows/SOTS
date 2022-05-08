using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
 
namespace SOTS.Items.Pyramid
{
    public class LuckyPurpleBalloon : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Lucky Purple Balloon");
			Tooltip.SetDefault("Grants an additional fishing line\nCounts as a light pet");
		}
        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.Carrot);
            Item.shoot = mod.ProjectileType("LuckyPurpleBalloon");
            Item.buffType = mod.BuffType("PurpleBalloon");
            Item.value = Item.sellPrice(0, 2, 25, 0);
            Item.rare = 5;
			Item.width = 18;
			Item.height = 42;
        }
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "CursedMatter", 4);
			recipe.AddIngredient(ItemID.Sapphire, 1);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
        public override void UseStyle(Player player)
        {
            if (player.whoAmI == Main.myPlayer && player.itemTime == 0)
            {
                player.AddBuff(Item.buffType, 3600, true);
            }
        }
    }
}