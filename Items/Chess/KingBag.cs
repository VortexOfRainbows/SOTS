using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.Chess
{
	public class KingBag : ModItem
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
			//bossBagNPC = mod.NPCType("King");
		}
		public override int BossBagNPC => mod.NPCType("King");
		public override bool CanRightClick()
		{
			return true;
		}
		public override void OpenBossBag(Player player)
		{

			player.QuickSpawnItem(mod.ItemType("KingCross"));
			player.QuickSpawnItem(mod.ItemType("KingTrinity"),Main.rand.Next(1, 8));
			
			if(Main.rand.Next(5) == 0)
			player.QuickSpawnItem(mod.ItemType("KingSmugMug"));
				
		}
	}
}