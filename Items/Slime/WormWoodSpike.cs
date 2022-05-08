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
            Item.damage = 32;
            Item.width = 34;
            Item.height = 34;
            Item.value = Item.sellPrice(0, 2, 0, 0);
            Item.rare = 4;
            Item.noMelee = true;
            Item.useStyle = 5;
            Item.useAnimation = 40;
            Item.useTime = 40;
            Item.knockBack = 4.5f;
            Item.noUseGraphic = true; 
            Item.shoot = ModContent.ProjectileType<Projectiles.WormWoodSpike>();
            Item.shootSpeed = 14.5f;
            Item.UseSound = SoundID.Item1;
            Item.melee = true; 
            Item.channel = true;
        }
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ModContent.ItemType<CorrosiveGel>(), 32);
            recipe.AddIngredient(ModContent.ItemType<Wormwood>(), 16);
            recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
    }
}