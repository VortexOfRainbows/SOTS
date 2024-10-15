using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.AbandonedVillage
{
	public class BackupBow : ModItem
	{
		public override void SetStaticDefaults()
		{
			this.SetResearchCost(1);
		}
		public override void SetDefaults()
		{
            Item.width = 36;     
            Item.height = 60;   
            Item.value = 0;
            Item.rare = ItemRarityID.Blue;
			Item.accessory = true;
			Item.hasVanityEffects = true;
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			SOTSPlayer modPlayer = SOTSPlayer.ModPlayer(player);
			modPlayer.backUpBow = true;
			modPlayer.backUpBowVisual = !hideVisual;
		}
	}
}

