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
            item.Size = new Vector2(34, 38);
            item.damage = 16;
            item.value = Item.sellPrice(0, 1, 10, 0);
            item.rare = ItemRarityID.Blue;
            item.useTime = 30;
            item.useAnimation = 30;
            item.shoot = ModContent.ProjectileType<Projectiles.Earth.Shattershine>();
            item.shootSpeed = 10f;
            item.knockBack = 4f;
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ModContent.ItemType<VibrantBar>(), 8);
            recipe.SetResult(this);
            recipe.AddTile(TileID.Anvils);
            recipe.AddRecipe();
        }
    }
}