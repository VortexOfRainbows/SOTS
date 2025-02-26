using SOTS.Items.AbandonedVillage;
using SOTS.Items.Invidia;
using SOTS.Void;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items
{
	public class MrEepy : ModItem
	{	
		public override void SetStaticDefaults() => this.SetResearchCost(1);
		public override void SetDefaults()
		{
			Item.maxStack = 1;
            Item.width = 36;     
            Item.height = 50;   
            Item.value = Item.sellPrice(0, 3, 0, 0);
			Item.rare = ItemRarityID.Orange;
			Item.accessory = true;
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.SOTSPlayer().Dreamcatcher = true;
            player.SOTSPlayer().MrBurns = true;
            player.VoidPlayer().voidMeterMax2 += 50;
            player.GetDamage<VoidGeneric>() += 0.10f;
        }
        public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient<MrBurns>(1).AddIngredient<Dreamcatcher>(1).AddTile(TileID.TinkerersWorkbench).Register();
        }
    }
}