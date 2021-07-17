using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.SpecialDrops
{
	public class ZombieHand : ModItem
	{	
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Zombie Hand");
			Tooltip.SetDefault("Allows melee swings to harm Town NPCs\n'Finally, I can kill the painter!'");
		}
		public override void SetDefaults()
		{
			item.maxStack = 1;
            item.width = 22;     
            item.height = 34;   
            item.value = Item.sellPrice(0, 0, 20, 0);
            item.rare = ItemRarityID.Blue;
			item.accessory = true;
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			SOTSPlayer modPlayer = SOTSPlayer.ModPlayer(player);
			modPlayer.CanKillNPC = true;
		}
	}
}