using Terraria;
using Terraria.ID;
using SOTS.Void;
using Terraria.ModLoader;

namespace SOTS.Items.Slime
{
	public class WormWoodParasite : ModItem
	{	
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Goopwood Parasite");
			Tooltip.SetDefault("Increases void gain by 2 and void critical strike chance by 8%\nCritical strikes heal small amounts of void\nDecreases max void by 20");
		}
		public override void SetDefaults()
		{
            Item.width = 34;     
            Item.height = 30;   
            Item.value = Item.sellPrice(0, 2, 0, 0);
			Item.rare = ItemRarityID.Green;
			Item.accessory = true;
		}
		public override void AddRecipes()
		{
			Recipe recipe = new Recipe(mod);
			recipe.AddIngredient(ModContent.ItemType<CorrosiveGel>(), 32);
			recipe.AddIngredient(ModContent.ItemType<Wormwood>(), 16);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			SOTSPlayer modPlayer = SOTSPlayer.ModPlayer(player);
			modPlayer.CritVoidsteal += 0.33f;
			VoidPlayer voidPlayer = VoidPlayer.ModPlayer(player);
			voidPlayer.voidMeterMax2 -= 20;
			voidPlayer.bonusVoidGain += 2;
			voidPlayer.voidCrit += 8;
		}
	}
}
