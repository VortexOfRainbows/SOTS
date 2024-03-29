using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
 
namespace SOTS.Items.Pyramid
{
    public class SpiritSurfer : ModItem
    {
		public override void SetStaticDefaults()
		{
			this.SetResearchCost(1);
        }
        public override void SetDefaults()
        {
            Item.width = 36;
            Item.height = 38;
            Item.useTime = 20;
            Item.useAnimation = 20;
            Item.useStyle = ItemUseStyleID.Swing;
			Item.value = Item.sellPrice(0, 5, 0, 0);
            Item.rare = ItemRarityID.Orange;
            Item.UseSound = SoundID.Item1;
            Item.noMelee = true;
            Item.mountType = ModContent.MountType<Mounts.SpiritSurfer>();
        }
		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(ModContent.ItemType<RoyalGoldBrick>(), 50).AddIngredient(ModContent.ItemType<SoulResidue>(), 35).AddIngredient(ItemID.FlyingCarpet, 1).AddTile(TileID.Anvils).Register();
		}
    }
}