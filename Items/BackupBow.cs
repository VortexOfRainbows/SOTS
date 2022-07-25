using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items
{
	public class BackupBow : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Backup Bow");
			Tooltip.SetDefault("Fires a homing arrow behind you when using ranged weapons\nThe arrow does 45% damage and freezes enemies for 1 second\nTest item");
			this.SetResearchCost(1);
		}
		public override void SetDefaults()
		{
            Item.width = 36;     
            Item.height = 60;   
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

