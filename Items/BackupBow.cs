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
			Tooltip.SetDefault("Attaches a bow to your back which fires arrows behind you for 50% damage");
			this.SetResearchCost(1);
		}
		public override void SetDefaults()
		{
            Item.width = 36;     
            Item.height = 60;   
            Item.value = 0;
            Item.rare = ItemRarityID.Blue;
			Item.accessory = true;
			Item.canBePlacedInVanityRegardlessOfConditions = true;
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			SOTSPlayer modPlayer = SOTSPlayer.ModPlayer(player);
			modPlayer.backUpBow = true;
			modPlayer.backUpBowVisual = true;
		}
	}
}

