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
			Item.maxStack = 1;
            Item.width = 24;     
            Item.height = 32;   
            Item.value = Item.sellPrice(0, 0, 50, 0);
            Item.rare = ItemRarityID.Blue;
			Item.accessory = true;
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			SOTSPlayer.ModPlayer(player).CritBonusMultiplier += 0.2f;
			SOTSPlayer.ModPlayer(player).HarvestersScythe = true;
		}
		public override void AddRecipes()
		{
			Recipe recipe = new Recipe(mod);
			recipe.AddIngredient(ModContent.ItemType<VibrantBar>(), 4);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}