using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.Conduit
{
	public class AnomalyInterceptor : ModItem
	{	
		public override void SetStaticDefaults()
		{
			this.SetResearchCost(1);
		}
		public override void SetDefaults()
		{
			Item.maxStack = 1;
            Item.width = 44;     
            Item.height = 56;   
            Item.value = Item.buyPrice(5, 0, 0, 0);
            Item.rare = ItemRarityID.Orange;
			Item.accessory = true;
			Item.hasVanityEffects = true;
		}
        public override void UpdateInventory(Player player)
		{
			SOTSPlayer modPlayer = SOTSPlayer.ModPlayer(player);
            modPlayer.AnomalyLocator = modPlayer.BetterAnomalyLocator = true;
        }
        public override void AddRecipes()
        {
            CreateRecipe(1)
                .AddIngredient<AnomalyLocator>(1)
                .AddIngredient<WonderEgg>(8)
                .AddTile(TileID.TinkerersWorkbench).Register();
        }
    }
}