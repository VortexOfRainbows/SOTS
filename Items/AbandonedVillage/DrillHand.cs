using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.AbandonedVillage
{
	public class DrillHand : ModItem
	{
		public override void SetStaticDefaults()
		{
			this.SetResearchCost(1);
		}
		public override void SetDefaults()
		{
			Item.maxStack = 1;
            Item.width = 30;
			Item.height = 52;   
            Item.value = Item.sellPrice(0, 4, 0, 0);
            Item.rare = ItemRarityID.Orange;
			Item.accessory = true;
			Item.expert = true;
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
		{
			if (!Main.SmartCursorIsUsed)
			{
				player.SOTSPlayer().Pick3x3 = true;
				player.SOTSPlayer().bonusPickaxePower -= 10;
				player.pickSpeed += 0.5f;
			}
			else
				player.pickSpeed -= 0.15f;
		}
	}
}