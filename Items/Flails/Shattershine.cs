using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SOTS.Projectiles.Pyramid.Aten;
using SOTS.Items.Earth;

namespace SOTS.Items.Flails
{
    public class Shattershine : BaseFlailItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Shattershine");
            Tooltip.SetDefault("Releases a cluster of sparkles upon hitting an enemy, each doing 60% damage\nDoesn't release sparkles while charging");
        }
        public override void SafeSetDefaults()
        {
            Item.Size = new Vector2(34, 38);
            Item.damage = 16;
            Item.value = Item.sellPrice(0, 1, 0, 0);
            Item.rare = ItemRarityID.Blue;
            Item.useTime = 30;
            Item.useAnimation = 30;
            Item.shoot = ModContent.ProjectileType<Projectiles.Earth.Shattershine>();
            Item.shootSpeed = 10f;
            Item.knockBack = 4f;
        }
        public override void AddRecipes()
        {
            Recipe recipe = new Recipe(mod);
            recipe.AddIngredient(ModContent.ItemType<VibrantBar>(), 8);
            recipe.SetResult(this);
            recipe.AddTile(TileID.Anvils);
            recipe.AddRecipe();
        }
    }
}