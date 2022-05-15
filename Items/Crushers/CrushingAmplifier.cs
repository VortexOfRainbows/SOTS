using SOTS.Void;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.Crushers
{
	public class CrushingAmplifier : ModItem
	{	
		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault("Extends the range of Crushers by 1\nAlso increases melee damage by 5%");
		}
		public override void SetDefaults()
		{
			Item.maxStack = 1;
            Item.width = 32;     
            Item.height = 34;   
            Item.value = Item.buyPrice(0, 6, 0, 0);
            Item.rare = ItemRarityID.Orange;
			Item.accessory = true;
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			VoidPlayer vPlayer = VoidPlayer.ModPlayer(player);
			vPlayer.BonusCrushRangeMax++;
			vPlayer.BonusCrushRangeMin++;
			player.GetDamage(DamageClass.Melee) += 0.05f;
		}
	}
}