using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.AbandonedVillage
{
	public class EarthDrive : ModItem
	{
		public override void SetStaticDefaults()
		{
			this.SetResearchCost(1);
		}
		public override void SetDefaults()
		{
			Item.maxStack = 1;
            Item.width = 32;
			Item.height = 32;   
            Item.value = Item.sellPrice(0, 1, 0, 0);
            Item.rare = ItemRarityID.Blue;
			Item.accessory = true;
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			SOTSPlayer modPlayer = player.SOTSPlayer();
			modPlayer.Earthdrive = true;
		}
	}
}