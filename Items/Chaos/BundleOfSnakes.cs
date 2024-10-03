using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.Chaos
{	
	public class BundleOfSnakes : ModItem
	{	
		public override void SetStaticDefaults()
		{
			this.SetResearchCost(1);
		}
		public override void SetDefaults()
		{
			Item.damage = 22;
			Item.DamageType = DamageClass.Summon;
			Item.knockBack = 1;
            Item.width = 50;     
            Item.height = 56;   
            Item.value = Item.sellPrice(0, 5, 0, 0);
			Item.rare = ItemRarityID.LightRed;
			Item.accessory = true;
		}
        public override void UpdateAccessory(Player player, bool hideVisual)
		{
			SOTSPlayer modPlayer = SOTSPlayer.ModPlayer(player);
            if (!Main.dayTime)
                player.lifeRegen += 1;
            modPlayer.BundleSnakeDamage += SOTSPlayer.ApplyDamageClassModWithGeneric(player, DamageClass.Summon, Item.damage);
        }
        public override bool WeaponPrefix() => false;
        public override bool MagicPrefix() => false;
        public override bool MeleePrefix() => false;
        public override bool RangedPrefix() => false;
    }
}