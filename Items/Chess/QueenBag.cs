using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.Chess
{
	public class QueenBag : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Treasure Bag");
			Tooltip.SetDefault("Right Click to open");
		}
		public override void SetDefaults()
		{

			item.width = 32;
			item.height = 32;
			item.value = 0;
			item.rare = 8;
			item.expert = true;
			item.maxStack = 99;
			item.consumable = true;
			//bossBagNPC = mod.NPCType("Queen");
		}
		public override int BossBagNPC => mod.NPCType("Queen");
		public override bool CanRightClick()
		{
			return true;
		}
		public override void OpenBossBag(Player player)
		{

			player.QuickSpawnItem(mod.ItemType("QueenHolyGuard"));
			player.QuickSpawnItem(mod.ItemType("QueenSkip"),Main.rand.Next(1, 8));
			
			if(Main.rand.Next(5) == 0)
			player.QuickSpawnItem(mod.ItemType("QueenSpikey"));
				
		}
	}
}