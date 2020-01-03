
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
			Tooltip.SetDefault("Summons an extremely fast spirit board mount\nDecreases void regen by 40 while active");
		}
        public override void SetDefaults()
        {
            
            item.width = 28;
            item.height = 26;
            item.useTime = 20;
            item.useAnimation = 20;
            item.useStyle = 1;
			item.value = Item.sellPrice(0, 5, 0, 0);
            item.rare = 6;
            item.UseSound = SoundID.Item1;
            item.noMelee = true;
            item.mountType = mod.MountType("SpiritSurfer");
        }
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "SoulResidue", 35);
			recipe.AddIngredient(ItemID.FlyingCarpet, 1);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
    }
}