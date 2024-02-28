using SOTS.FakePlayer;
using SOTS.Items.Fragments;
using SOTS.Items.Potions;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.Tide
{	
	public class HydrokineticAntennae : ModItem
	{	
		public override void SetStaticDefaults()
		{
			this.SetResearchCost(1);
		}
		public override void SetDefaults()
		{
            Item.width = 42;     
            Item.height = 42;   
            Item.value = Item.sellPrice(0, 3, 0, 0);
			Item.rare = ItemRarityID.LightRed;
			Item.accessory = true;
		}
        public override void UpdateAccessory(Player player, bool hideVisual)
		{
			FakeModPlayer modPlayer = FakeModPlayer.ModPlayer(player);
            modPlayer.hasHydroFakePlayer = true;
			player.SOTSPlayer().StatShareMeleeAndSummon = true;
        }
		public override void AddRecipes()
		{
			//CreateRecipe(1).AddIngredient(ItemID.LifeCrystal, 1).AddIngredient(ModContent.ItemType<DissolvingDeluge>(), 1).AddIngredient(ModContent.ItemType<RipplePotion>(), 8).AddTile(TileID.TinkerersWorkbench).Register();
		}
	}
}