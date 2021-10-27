using Terraria;
using Terraria.ID;
using Terraria.ModLoader;		

namespace SOTS.Items.Fragments
{
	public class CursedIcosahedron : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Cursed Icosahedron");
			Tooltip.SetDefault("Critical strikes may cause a release of cursed thunder, dealing 50% critical damage\nCritical strikes may also cause frostburn or flaming explosions, dealing 50% critical damage\n3% increased crit chance");
		}
		public override void SetDefaults()
		{
			item.width = 24;
			item.height = 28;
			item.value = Item.sellPrice(0, 5, 50, 0);
            item.rare = ItemRarityID.Yellow;
			item.accessory = true;
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			SOTSPlayer modPlayer = (SOTSPlayer)player.GetModPlayer(mod, "SOTSPlayer");	
			modPlayer.CritCurseFire = true;
			player.meleeCrit += 3;
			player.rangedCrit += 3;
			player.magicCrit += 3;
			player.thrownCrit += 3;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<BorealisIcosahedron>(), 1);
			recipe.AddIngredient(ModContent.ItemType<HellfireIcosahedron>(), 1);
			recipe.AddIngredient(ModContent.ItemType<DissolvingUmbra>(), 1); 
			recipe.AddIngredient(ItemID.CursedFlame, 10); 
			recipe.AddTile(TileID.TinkerersWorkbench);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}
