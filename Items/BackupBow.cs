using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace SOTS.Items
{
	public class BackupBow : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Backup Bow");
			Tooltip.SetDefault("Fires a homing arrow behind you when using ranged weapons\nThe arrow does 45% damage");
		}
		public override void SetDefaults()
		{
            item.width = 56;     
            item.height = 50;   
            item.value = 0;
            item.rare = ItemRarityID.Lime;
			item.accessory = true;
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			SOTSPlayer modPlayer = SOTSPlayer.ModPlayer(player);
			modPlayer.backUpBow = true;
		}
	}
}

