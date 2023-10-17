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
            this.SetResearchCost(1);
        }
        public override void SafeSetDefaults()
        {
            Item.Size = new Vector2(44, 42);
            Item.damage = 45;
            Item.value = Item.sellPrice(0, 7, 25, 0);
            Item.rare = ItemRarityID.Lime;
            Item.useTime = 30;
            Item.useAnimation = 30;
            Item.shoot = ModContent.ProjectileType<Projectiles.Permafrost.NorthStar.NorthStar>();
            Item.shootSpeed = 15;
            Item.knockBack = 5;
            Item.channel = true;
        }
        public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient(ModContent.ItemType<AbsoluteBar>(), 12).AddIngredient(ModContent.ItemType<Aten>(), 1).AddIngredient(ModContent.ItemType<Shattershine>(), 1).AddTile(TileID.MythrilAnvil).Register();
        }
    }
}