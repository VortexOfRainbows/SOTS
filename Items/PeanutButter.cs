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
			this.SetResearchCost(1);
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
			Item.hasVanityEffects = true;
		}
        public override void UpdateAccessory(Player player, bool hideVisual)
		{
			SOTSPlayer modPlayer = SOTSPlayer.ModPlayer(player);
			player.lifeRegen += 1;
			modPlayer.petPinky += SOTSPlayer.ApplyDamageClassModWithGeneric(player, Item.DamageType, Item.damage);
        }
        public override bool WeaponPrefix()
        {
            return false;
        }
    }
}