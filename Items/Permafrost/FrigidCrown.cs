using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.Permafrost
{
	[AutoloadEquip(EquipType.Head)]
	
	public class FrigidCrown : ModItem
	{
		public override void SetDefaults()
		{
			Item.width = 26;
			Item.height = 20;
            Item.value = Item.sellPrice(0, 1, 0, 0);
			Item.rare = ItemRarityID.Green;
			Item.defense = 3;
		}
		public override void SetStaticDefaults()
		{
			this.SetResearchCost(1);
		}
		public override void UpdateEquip(Player player)
		{
			SOTSPlayer modPlayer = SOTSPlayer.ModPlayer(player);
			modPlayer.shardSpellExtra++;
		}
		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient<FrigidBar>(8).AddTile(TileID.Anvils).Register();
		}
	}
}