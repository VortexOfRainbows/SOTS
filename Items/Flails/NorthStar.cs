using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SOTS.Items.Permafrost;

namespace SOTS.Items.Flails
{
    public class NorthStar : BaseFlailItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("North Star");
            Tooltip.SetDefault("Conjures polar stars that do 70% damage and explode for 210% damage");
        }
        public override void SafeSetDefaults()
        {
            item.Size = new Vector2(44, 42);
            item.damage = 42;
            item.value = Item.sellPrice(0, 7, 25, 0);
            item.rare = ItemRarityID.Lime;
            item.useTime = 30;
            item.useAnimation = 30;
            item.shoot = ModContent.ProjectileType<Projectiles.Permafrost.NorthStar.NorthStar>();
            item.shootSpeed = 15;
            item.knockBack = 5;
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ModContent.ItemType<AbsoluteBar>(), 12);
            recipe.AddIngredient(ModContent.ItemType<Aten>(), 1);
            recipe.AddIngredient(ModContent.ItemType<Shattershine>(), 1);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}