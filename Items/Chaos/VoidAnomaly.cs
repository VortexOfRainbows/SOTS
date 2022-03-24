using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using SOTS.Void;

namespace SOTS.Items.Chaos
{
	public class VoidAnomaly : ModItem
	{
        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White;
        }
        public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Void Anomaly");
			Tooltip.SetDefault("Increases void gain by 10 and void regeneration speed by 10%\nGetting hit will convert void into life for a duration\nIncreases the potency of Void Shock and Void Recovery");
		}
		public override void SetDefaults()
		{
            item.width = 44;     
            item.height = 26;   
            item.value = Item.sellPrice(gold: 10);
            item.rare = ItemRarityID.Yellow;
			item.accessory = true;
			item.expert = true;
		}
        public override void UpdateAccessory(Player player, bool hideVisual)
		{
			SOTSPlayer modPlayer = SOTSPlayer.ModPlayer(player);
			VoidPlayer vPlayer = VoidPlayer.ModPlayer(player);
			vPlayer.bonusVoidGain += 10f;
			vPlayer.voidRegenSpeed += 0.1f;
			//modPlayer.VMincubator = true;
			modPlayer.VoidAnomaly = true;
		}
	}
}

