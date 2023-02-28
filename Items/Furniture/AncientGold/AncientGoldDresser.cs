using Microsoft.Xna.Framework;
using SOTS.Items.Pyramid;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;

namespace SOTS.Items.Furniture.AncientGold
{
    public class AncientGoldDresser : ModItem
    {
        public override void SetStaticDefaults() => this.SetResearchCost(1);
        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.StoneBlock);
            Item.Size = new Vector2(34, 24);
            Item.rare = ItemRarityID.Blue;
            Item.createTile = ModContent.TileType<AncientGoldDresserTile>();
        }
        public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient(ModContent.ItemType<RoyalGoldBrick>(), 16).AddTile(TileID.Sawmill).Register();
        }
    }
    public class AncientGoldDresserTile : Dresser
    {
        public override bool CanExplode(int i, int j)
        {
            return false;
        }
        protected override int DresserDrop => ModContent.ItemType<AncientGoldDresser>();
        protected override string DresserName => Language.GetTextValue("Mods.SOTS.ContainerName.AncientGoldDresser");
    }
}