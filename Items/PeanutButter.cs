using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items
{	
	public class PeanutButter : ModItem
	{	
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Peanut Butter");
			Tooltip.SetDefault("Summons a pet Putrid Pinky to assist in combat\nLatches onto enemies, slowing them down and draining life\nIncreases life regeneration by 1");
		}
		public override void SetDefaults()
		{
			item.damage = 17;
			item.summon = true;
            item.width = 22;     
            item.height = 28;   
            item.value = Item.sellPrice(0, 3, 0, 0);
			item.rare = ItemRarityID.LightRed;
			item.accessory = true;
		}
        public override void UpdateAccessory(Player player, bool hideVisual)
		{
			SOTSPlayer modPlayer = SOTSPlayer.ModPlayer(player);
			player.lifeRegen += 1;
			modPlayer.petPinky += (int)(item.damage * (1f + (player.minionDamage - 1f) + (player.allDamage - 1f)));
		}
	}
}