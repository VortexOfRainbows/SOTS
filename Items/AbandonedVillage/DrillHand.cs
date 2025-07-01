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
			SOTSPlayer sp = player.SOTSPlayer();
			sp.DrillHand = true;
			sp.DrillHandVanity = !hideVisual;
            if (!Main.SmartCursorIsUsed && player.HeldItem.pick > 0)
			{
				sp.Pick3x3 = true;
                sp.bonusPickaxePower -= 15;
				player.pickSpeed += 0.5f;
			}
			else
				player.pickSpeed -= 0.15f;
		}
        public override void UpdateVanity(Player player)
        {
            SOTSPlayer sp = player.SOTSPlayer();
            sp.DrillHandVanity = true;
        }
    }
}