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
			Tooltip.SetDefault("Fires a homing arrow behind you when using ranged weapons\nThe arrow does 45% damage and freezes enemies for 1 second\nTest item");
		}
		public override void SetDefaults()
		{
            Item.width = 56;     
            Item.height = 50;   
            Item.value = 0;
            Item.rare = ItemRarityID.Lime;
			Item.accessory = true;
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			SOTSPlayer modPlayer = SOTSPlayer.ModPlayer(player);
			modPlayer.backUpBow = true;
		}
	}
}

