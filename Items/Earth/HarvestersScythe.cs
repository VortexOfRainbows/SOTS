using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.Earth
{
	public class HarvestersScythe : ModItem
	{	
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Harvester's Scythe");
			Tooltip.SetDefault("Increases critical strike damage by 20%\nEnemies have a 10% chance to drop double the loot, or 20% when killed by critical strike");
		}
		public override void SetDefaults()
		{
			item.maxStack = 1;
            item.width = 24;     
            item.height = 32;   
            item.value = Item.sellPrice(0, 0, 50, 0);
            item.rare = ItemRarityID.Blue;
			item.accessory = true;
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			SOTSPlayer.ModPlayer(player).CritBonusMultiplier += 0.2f;
			SOTSPlayer.ModPlayer(player).HarvestersScythe = true;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<VibrantBar>(), 4);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}