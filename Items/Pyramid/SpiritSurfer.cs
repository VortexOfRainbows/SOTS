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
			Tooltip.SetDefault("Summons an extremely fast spirit board mount\nDecreases flat void regeneration by 6 while active");
		}
        public override void SetDefaults()
        {
            item.width = 36;
            item.height = 38;
            item.useTime = 20;
            item.useAnimation = 20;
            item.useStyle = ItemUseStyleID.SwingThrow;
			item.value = Item.sellPrice(0, 5, 0, 0);
            item.rare = ItemRarityID.LightPurple;
            item.UseSound = SoundID.Item1;
            item.noMelee = true;
            item.mountType = ModContent.MountType<Mounts.SpiritSurfer>();
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