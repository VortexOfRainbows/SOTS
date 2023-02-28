using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.Pyramid
{
	public class CurseWard : ModItem
	{
		public override void SetStaticDefaults()
		{
			this.SetResearchCost(1);
		}
		public override void SetDefaults()
		{
			Item.width = 40;
			Item.height = 44;
			Item.value = 0;
			Item.rare = ItemRarityID.Orange;
			Item.maxStack = 1;
		}
		public override void UpdateInventory(Player player)
		{
			SOTSPlayer modPlayer = SOTSPlayer.ModPlayer(player);	
			modPlayer.weakerCurse = true;
		}
	}
}