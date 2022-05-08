using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
 
namespace SOTS.Items.Pyramid
{
    public class SpiritSurfer : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Spirit Surfer");
			Tooltip.SetDefault("Summons an extremely fast spirit board mount\nIncreases void drain by 6 while active");
		}
        public override void SetDefaults()
        {
            Item.width = 36;
            Item.height = 38;
            Item.useTime = 20;
            Item.useAnimation = 20;
            Item.useStyle = ItemUseStyleID.SwingThrow;
			Item.value = Item.sellPrice(0, 5, 0, 0);
            Item.rare = ItemRarityID.LightPurple;
            Item.UseSound = SoundID.Item1;
            Item.noMelee = true;
            Item.mountType = ModContent.MountType<Mounts.SpiritSurfer>();
        }
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<AncientGold.RoyalGoldBrick>(), 50);
			recipe.AddIngredient(ModContent.ItemType<SoulResidue>(), 35);
			recipe.AddIngredient(ItemID.FlyingCarpet, 1);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
    }
}