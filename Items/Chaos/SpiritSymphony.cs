using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using SOTS.Void;

namespace SOTS.Items.Chaos
{
	public class SpiritSymphony : ModItem
	{
        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White;
        }
        public override void SetStaticDefaults()
		{
			this.SetResearchCost(1);
		}
		public override void SetDefaults()
		{
            Item.width = 38;     
            Item.height = 36;   
            Item.value = Item.sellPrice(gold: 10);
            Item.rare = ItemRarityID.Yellow;
			Item.accessory = true;
			Item.expert = true;
		}
        public override bool CanAccessoryBeEquippedWith(Item equippedItem, Item incomingItem, Player player)
        {
			if (equippedItem.type == ModContent.ItemType<SpiritInsignia>() || incomingItem.type == ModContent.ItemType<SpiritInsignia>())
				return false;
            return true;
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
		{
			SOTSPlayer modPlayer = SOTSPlayer.ModPlayer(player);
			VoidPlayer vPlayer = VoidPlayer.ModPlayer(player);
			vPlayer.bonusVoidGain += 1f;
			vPlayer.voidRegenSpeed += 0.25f;
			//modPlayer.VMincubator = true;
			modPlayer.SpiritSymphony = true;
		}
	}
}

