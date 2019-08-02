using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.BiomeItems
{
	public class BossBagMargrit : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Treasure Bag");
			Tooltip.SetDefault("Right Click to open");
		}
		public override void SetDefaults()
		{

			item.width = 40;
			item.height = 32;
			item.value = 0;
			item.rare = 6;
			item.expert = true;
			item.maxStack = 99;
			item.consumable = true;
		}
		public override int BossBagNPC => mod.NPCType("CrypticCarver2");
		public override bool CanRightClick()
		{
			return true;
		}
		public override void OpenBossBag(Player player)
		{

			player.QuickSpawnItem(mod.ItemType("CrypticKnife"));
			player.QuickSpawnItem(mod.ItemType("MargritCore"));
			player.QuickSpawnItem(3081,Main.rand.Next(50, 100));
			player.QuickSpawnItem(3086,Main.rand.Next(50, 100));
				
			if(Main.rand.Next(12) == 0)
			player.QuickSpawnItem(mod.ItemType("MargritBlaster"));
		
			if(Main.rand.Next(12) == 0)
			player.QuickSpawnItem(mod.ItemType("MargritBoomer"));
		
			if(Main.rand.Next(12) == 0)
			player.QuickSpawnItem(mod.ItemType("MargritClock"));
		
			if(Main.rand.Next(12) == 0)
			player.QuickSpawnItem(mod.ItemType("MargritToxin"),Main.rand.Next(200, 500));
		
			if(Main.rand.Next(12) == 0)
			player.QuickSpawnItem(mod.ItemType("MargritFork"));
		
			if(Main.rand.Next(12) == 0)
			player.QuickSpawnItem(mod.ItemType("MargritRay"));
		
			if(Main.rand.Next(12) == 0)
			player.QuickSpawnItem(mod.ItemType("MargritRing"));
		
			if(Main.rand.Next(12) == 0)
			player.QuickSpawnItem(mod.ItemType("MargritSentryStaff"));
		
			if(Main.rand.Next(12) == 0)
			player.QuickSpawnItem(mod.ItemType("MargritYoyo"));
		}
	}
}