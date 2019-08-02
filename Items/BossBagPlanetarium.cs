using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items
{
	public class BossBagPlanetarium : ModItem
	{
		
		public override void SetDefaults()
		{
			item.maxStack = 999;
			item.consumable = true;
			item.width = 24;
			item.height = 24;

			item.rare = 9;
			item.expert = true;
			
		}
		
		public override int BossBagNPC => mod.NPCType("EtherealEntity3");
		
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Treasure Bag");
			Tooltip.SetDefault("{$CommonItemTooltip.RightClickToOpen}");
}
		public override bool CanRightClick()
		{
			return true;
		}
		public override void OpenBossBag(Player player)
		{

			player.QuickSpawnItem(mod.ItemType("EtherealPickaxe"), 1);
			
			player.QuickSpawnItem(mod.ItemType("PlanetaryCore"));
			player.QuickSpawnItem(mod.ItemType("EmptyPlanetariumOrb"),Main.rand.Next(20, 40));
				
			if(Main.rand.Next(12) == 0)
			player.QuickSpawnItem(mod.ItemType("PlanetaryWarper"));
		
			if(Main.rand.Next(12) == 0)
			player.QuickSpawnItem(mod.ItemType("PlanetaryShip"));
		
			if(Main.rand.Next(12) == 0)
			player.QuickSpawnItem(mod.ItemType("PlanetaryLatch"));
		
			if(Main.rand.Next(12) == 0)
			player.QuickSpawnItem(mod.ItemType("PlanetaryCapsule"),Main.rand.Next(200, 500));
		
			if(Main.rand.Next(12) == 0)
			player.QuickSpawnItem(mod.ItemType("PlanetaryDragon"));
		
			if(Main.rand.Next(12) == 0)
			player.QuickSpawnItem(mod.ItemType("TearCrash"));
		
			if(Main.rand.Next(12) == 0)
			player.QuickSpawnItem(mod.ItemType("PlanetaryEnigma"));
		
			if(Main.rand.Next(12) == 0)
			player.QuickSpawnItem(mod.ItemType("PlanetaryScepter"));
		
			if(Main.rand.Next(12) == 0)
			player.QuickSpawnItem(mod.ItemType("PlanetariumHalo"));
			
				
		}
	}
}