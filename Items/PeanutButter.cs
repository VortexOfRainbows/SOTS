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
			Item.damage = 17;
			Item.DamageType = DamageClass.Summon;
            Item.width = 22;     
            Item.height = 28;   
            Item.value = Item.sellPrice(0, 3, 0, 0);
			Item.rare = ItemRarityID.LightRed;
			Item.accessory = true;
		}
        public override void UpdateAccessory(Player player, bool hideVisual)
		{
			SOTSPlayer modPlayer = SOTSPlayer.ModPlayer(player);
			player.lifeRegen += 1;
			modPlayer.petPinky += SOTSPlayer.ApplyDamageClassModWithGeneric(player, Item.DamageType, Item.damage);
		}
	}
}