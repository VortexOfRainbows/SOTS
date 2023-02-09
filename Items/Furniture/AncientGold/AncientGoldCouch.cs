using Microsoft.Xna.Framework;
using SOTS.Items.Pyramid;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.Furniture.AncientGold
{
    public class AncientGoldCouch : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Ancient Gold Sofa");
            this.SetResearchCost(1);
        }
        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.StoneBlock);
            Item.Size = new Vector2(38, 24);
            Item.rare = ItemRarityID.Blue;
            Item.createTile = ModContent.TileType<AncientGoldCouchTile>();
        }
        public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient(ModContent.ItemType<RoyalGoldBrick>(), 5).AddIngredient(ItemID.Silk, 2).AddTile(TileID.Sawmill).Register();
        }
    }
    public class AncientGoldCouchTile : Sofa<AncientGoldCouch>
    {

    }
}