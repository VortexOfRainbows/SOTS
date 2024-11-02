using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.AbandonedVillage
{
	public class RockingHorse : ModItem
	{
		public override void SetStaticDefaults() => this.SetResearchCost(1);
		public override void SetDefaults()
		{
			Item.maxStack = 1;
            Item.width = 44;
			Item.height = 30;   
            Item.value = Item.sellPrice(0, 0, 50, 0);
            Item.rare = ItemRarityID.Blue;
			Item.accessory = true;
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.moveSpeed += 0.05f;
			player.VoidPlayer().bonusVoidGain += 2;
        }
        public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient<CharredWood>(24).AddIngredient<AncientSteelBar>(2).AddTile(TileID.Anvils).Register();
        }
    }
}