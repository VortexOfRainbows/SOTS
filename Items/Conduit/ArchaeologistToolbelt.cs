using Microsoft.Xna.Framework;
using SOTS.Items.Fragments;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.Conduit
{	
	[AutoloadEquip(EquipType.Waist)]
	public class ArchaeologistToolbelt : ModItem
	{	
		public override void SetStaticDefaults()
		{
			this.SetResearchCost(1);
		}
		public override void SetDefaults()
		{
            Item.width = 36;     
            Item.height = 28;   
            Item.value = Item.sellPrice(0, 5, 0, 0);
			Item.shopCustomPrice = Item.buyPrice(gold: 35);
			Item.rare = ItemRarityID.Blue;
			Item.accessory = true;
			Item.canBePlacedInVanityRegardlessOfConditions = true;
		}
        public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.equippedAnyTileRangeAcc = true;
			SOTSPlayer.ModPlayer(player).ConduitBelt = true;
		}
	}
}