using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.AbandonedVillage
{	
	public class LittleWoes : ModItem
	{
        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White * 0.8f;
        }
        public override void SetStaticDefaults()
		{
			this.SetResearchCost(1);
		}
		public override void SetDefaults()
		{
			Item.damage = 15;
			Item.DamageType = DamageClass.Summon;
			Item.knockBack = 1f;
            Item.width = 38;     
            Item.height = 52;   
            Item.value = Item.sellPrice(0, 1, 0, 0);
			Item.rare = ItemRarityID.Green;
			Item.accessory = true;
		}
        public override void UpdateAccessory(Player player, bool hideVisual)
		{
			SOTSPlayer modPlayer = SOTSPlayer.ModPlayer(player);
			if (!Main.dayTime)
				player.VoidPlayer().bonusVoidGain++;
			modPlayer.LittleWoeDamage += SOTSPlayer.ApplyDamageClassModWithGeneric(player, DamageClass.Summon, Item.damage);
        }
        public override bool WeaponPrefix() => false;
        public override bool MagicPrefix() => false;
        public override bool MeleePrefix() => false;
        public override bool RangedPrefix() => false;
    }
}