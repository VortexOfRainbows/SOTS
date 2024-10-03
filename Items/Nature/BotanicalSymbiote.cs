using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.Nature
{	
	public class BotanicalSymbiote : ModItem
	{	
		public override void SetStaticDefaults()
		{
			this.SetResearchCost(1);
		}
		public override void SetDefaults()
		{
			Item.damage = 11;
			Item.DamageType = DamageClass.Summon;
			Item.knockBack = 0.4f;
            Item.width = 26;     
            Item.height = 38;   
            Item.value = Item.sellPrice(0, 1, 0, 0);
			Item.rare = ItemRarityID.Green;
			Item.accessory = true;
		}
        public override void UpdateAccessory(Player player, bool hideVisual)
		{
			SOTSPlayer modPlayer = SOTSPlayer.ModPlayer(player);
			if(Main.dayTime)
				player.lifeRegen += 1;
			modPlayer.symbioteDamage += SOTSPlayer.ApplyDamageClassModWithGeneric(player, DamageClass.Summon, Item.damage);
        }
        public override bool WeaponPrefix() => false;
        public override bool MagicPrefix() => false;
        public override bool MeleePrefix() => false;
        public override bool RangedPrefix() => false;
    }
}