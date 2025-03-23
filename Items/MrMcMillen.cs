using SOTS.Items.AbandonedVillage;
using SOTS.Items.Invidia;
using SOTS.Void;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items
{
	public class MrMcMillen : ModItem
	{	
		public override void SetStaticDefaults() => this.SetResearchCost(1);
		public override void SetDefaults()
		{
			Item.maxStack = 1;
            Item.width = 94;     
            Item.height = 42;   
            Item.value = Item.sellPrice(0, 3, 0, 0);
			Item.rare = ItemRarityID.Orange;
			Item.accessory = true;
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
            player.SOTSPlayer().MrBurns = true;
            player.VoidPlayer().voidMeterMax2 += 50;
            player.VoidPlayer().voidRegenSpeed += 0.5f;
        }
        public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient<MrBurns>(1).AddIngredient<EmptyNecklace>(1).AddTile(TileID.TinkerersWorkbench).Register();
        }
    }
}