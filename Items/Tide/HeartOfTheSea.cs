using SOTS.Items.Fragments;
using SOTS.Items.Potions;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.Tide
{	
	public class HeartOfTheSea : ModItem
	{	
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Heart of the Sea");
			Tooltip.SetDefault("Release waves of damage periodically\nRelease more waves at lower health\nWaves ignore up to 8 defense\nIncreases max life by 20\nWaves disabled when hidden\n'Death by heartbeat'");
			this.SetResearchCost(1);
		}
		public override void SetDefaults()
		{
            Item.width = 42;     
            Item.height = 40;   
            Item.value = Item.sellPrice(0, 2, 50, 0);
			Item.rare = ItemRarityID.Orange;
			Item.accessory = true;
		}
        public override void UpdateAccessory(Player player, bool hideVisual)
		{
			SOTSPlayer modPlayer = SOTSPlayer.ModPlayer(player);
			modPlayer.rippleBonusDamage += 2;
			if(!hideVisual)
				modPlayer.rippleEffect = true;
			player.statLifeMax2 += 20;
			modPlayer.rippleTimer++;
		}
		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(ItemID.LifeCrystal, 1).AddIngredient(ModContent.ItemType<DissolvingDeluge>(), 1).AddIngredient(ModContent.ItemType<RipplePotion>(), 8).AddTile(TileID.TinkerersWorkbench).Register();
		}
	}
}