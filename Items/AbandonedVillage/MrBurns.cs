using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.AbandonedVillage
{
	public class MrBurns : ModItem
	{
		public override void SetStaticDefaults() => this.SetResearchCost(1);
		public override void SetDefaults()
		{
			Item.maxStack = 1;
            Item.width = 44;
			Item.height = 44;   
            Item.value = Item.sellPrice(0, 0, 50, 0);
            Item.rare = ItemRarityID.Blue;
			Item.accessory = true;
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.VoidPlayer().voidMeterMax2 += 20;
			player.SOTSPlayer().MrBurns = true;
        }
        public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient<CharredWood>(12).AddIngredient(ItemID.Silk, 5).AddTile(TileID.Anvils).Register();
        }
    }
}