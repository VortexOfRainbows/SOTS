using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.Pyramid
{
	public class CurseWard : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Curse Ward");
			Tooltip.SetDefault("Weakens the pyramid's curse while in the inventory");
		}
		public override void SetDefaults()
		{

			item.width = 40;
			item.height = 44;
			item.value = 0;
			item.rare = 3;
			item.maxStack = 1;
		}
		public override void UpdateInventory(Player player)
		{
			SOTSPlayer modPlayer = (SOTSPlayer)player.GetModPlayer(mod, "SOTSPlayer");	
			modPlayer.weakerCurse = true;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.LifeCrystal, 1);
			recipe.AddIngredient(ItemID.ShadowScale, 10);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
			
			recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.LifeCrystal, 1);
			recipe.AddIngredient(ItemID.TissueSample, 10);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}