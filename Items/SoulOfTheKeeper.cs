using SOTS.Items.Conduit;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items
{
	public class SoulOfTheKeeper : ModItem
	{	
		public override void SetStaticDefaults()
		{
			this.SetResearchCost(1);
		}
		public override void SetDefaults()
		{
			Item.maxStack = 1;
            Item.width = 36;     
            Item.height = 32;   
            Item.value = Item.sellPrice(0, 5, 0, 0);
            Item.rare = ItemRarityID.LightRed;
			Item.accessory = true;
			Item.defense = 1;
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
        {
            SOTSPlayer modPlayer = SOTSPlayer.ModPlayer(player);
            modPlayer.GoldenTrowel = true;
            modPlayer.KeepersBox = true;
            modPlayer.DamageGenerateMoney += 1;
            player.GetCritChance(DamageClass.Generic) += 5;
            player.tileSpeed += 0.05f;
        }
        public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient<SwallowedPenny>(1).AddIngredient<KeepersBox>(1).AddIngredient<GoldenTrowel>(1)
                .AddTile(TileID.TinkerersWorkbench).Register();
        }
    }
}