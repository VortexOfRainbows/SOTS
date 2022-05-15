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
            Item.Size = new Vector2(44, 42);
            Item.damage = 42;
            Item.value = Item.sellPrice(0, 7, 25, 0);
            Item.rare = ItemRarityID.Lime;
            Item.useTime = 30;
            Item.useAnimation = 30;
            Item.shoot = ModContent.ProjectileType<Projectiles.Permafrost.NorthStar.NorthStar>();
            Item.shootSpeed = 15;
            Item.knockBack = 5;
        }
        public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient(ModContent.ItemType<AbsoluteBar>(), 12).AddIngredient(ModContent.ItemType<Aten>(), 1).AddIngredient(ModContent.ItemType<Shattershine>(), 1).AddTile(TileID.MythrilAnvil).Register();
        }
    }
}