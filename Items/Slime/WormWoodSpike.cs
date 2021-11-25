using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.Slime
{
    public class WormWoodSpike : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Goopwood Spike");
			Tooltip.SetDefault("Enemies get stuck on it");
		}
        public override void SetDefaults()
        {
            item.damage = 32;
            item.width = 34;
            item.height = 34;
            item.value = Item.sellPrice(0, 2, 0, 0);
            item.rare = 4;
            item.noMelee = true;
            item.useStyle = 5;
            item.useAnimation = 40;
            item.useTime = 40;
            item.knockBack = 4.5f;
            item.noUseGraphic = true; 
            item.shoot = mod.ProjectileType("WormWoodSpike");
            item.shootSpeed = 14.5f;
            item.UseSound = SoundID.Item1;
            item.melee = true; 
            item.channel = true;
        }
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ModContent.ItemType<CorrosiveGel>(), 32);
            recipe.AddIngredient(null, "Wormwood", 16);
            recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
    }
}