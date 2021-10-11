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
			DisplayName.SetDefault("Botanical Symbiote");
			Tooltip.SetDefault("Increases life regeneration by 1 during the day\nGrants a Blooming Hook to all minions");
		}
		public override void SetDefaults()
		{
			item.damage = 11;
			item.summon = true;
            item.width = 26;     
            item.height = 38;   
            item.value = Item.sellPrice(0, 1, 0, 0);
			item.rare = ItemRarityID.Green;
			item.accessory = true;
		}
        public override void UpdateAccessory(Player player, bool hideVisual)
		{
			SOTSPlayer modPlayer = SOTSPlayer.ModPlayer(player);
			if(Main.dayTime)
				player.lifeRegen += 1;
			modPlayer.symbioteDamage += (int)(item.damage * (1f + (player.minionDamage - 1f) + (player.allDamage - 1f))) + 1;
		}
	}
}