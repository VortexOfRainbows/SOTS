using Terraria;
using Terraria.ID;
using SOTS.Void;
using Terraria.ModLoader;

namespace SOTS.Items.GelGear
{
	public class WormWoodParasite : ModItem
	{	
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Wormwood Parasite");
			Tooltip.SetDefault("Increases void regen speed by 1 and void crit by 8%\nCritical strikes heal small amounts of void\nIncreases max void by 20\nLowers life regen speed by 1");
		}
		public override void SetDefaults()
		{
            item.width = 34;     
            item.height = 30;   
            item.value = Item.sellPrice(0, 1, 80, 0);
            item.rare = ItemRarityID.LightRed;
			item.accessory = true;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "WormWoodCore", 1);
			recipe.AddIngredient(null, "Wormwood", 16);
			recipe.AddIngredient(ItemID.PinkGel, 32);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			SOTSPlayer modPlayer = (SOTSPlayer)player.GetModPlayer(mod, "SOTSPlayer");
			modPlayer.CritVoidsteal += 0.33f;
			VoidPlayer voidPlayer = VoidPlayer.ModPlayer(player);
			voidPlayer.voidMeterMax2 += 20;
			voidPlayer.voidRegen += 0.1f;
			voidPlayer.voidCrit += 8;
			if (player.lifeRegen > 0)
				player.lifeRegen -= 1;
		}
	}
}
