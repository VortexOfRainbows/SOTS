using SOTS.NPCs.Boss;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.GelGear
{
	public class PinkyBag : ModItem
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
			item.rare = ItemRarityID.LightPurple;
			item.expert = true;
			item.maxStack = 99;
			item.consumable = true;
		}
		public override int BossBagNPC => ModContent.NPCType<PutridPinkyPhase2>();
		public override bool CanRightClick()
		{
			return true;
		}
		public override void OpenBossBag(Player player)
		{
			player.QuickSpawnItem(mod.ItemType("PutridEye"));
			player.QuickSpawnItem(ModContent.ItemType<VialofAcid>(), Main.rand.Next(20, 30));
			player.QuickSpawnItem(ItemID.PinkGel,Main.rand.Next(40, 60));
			player.QuickSpawnItem(mod.ItemType("Wormwood"), Main.rand.Next(20, 30));

			int rand = Main.rand.Next(12);
			if(rand == 0)
				player.QuickSpawnItem(mod.ItemType("GelWings"));
			if(rand == 1)
				player.QuickSpawnItem(mod.ItemType("WormWoodParasite"));
			if(rand == 2)
				player.QuickSpawnItem(mod.ItemType("WormWoodHelix"));
			if(rand == 3)
				player.QuickSpawnItem(mod.ItemType("WormWoodCrystal"), Main.rand.Next(333, 667));
			if(rand == 4)
				player.QuickSpawnItem(mod.ItemType("WormWoodHook"));
			if(rand == 5)
				player.QuickSpawnItem(mod.ItemType("WormWoodCollapse"));
			if(rand == 6)
				player.QuickSpawnItem(mod.ItemType("WormWoodScepter"));
			if(rand == 7)
				player.QuickSpawnItem(mod.ItemType("WormWoodStaff"));
			if(rand == 8)
				player.QuickSpawnItem(mod.ItemType("WormWoodSpike"));
		}
	}
}