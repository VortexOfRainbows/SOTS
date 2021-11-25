using SOTS.Items.Crushers;
using SOTS.NPCs.Boss;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.Slime
{
	public class PinkyBag : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Treasure Bag");
			Tooltip.SetDefault("{$CommonItemTooltip.RightClickToOpen}");
		}
		public override void SetDefaults()
		{
			item.width = 32;
			item.height = 32;
			item.value = 0;
			item.rare = ItemRarityID.LightPurple;
			item.expert = true;
			item.maxStack = 999;
			item.consumable = true;
		}
		public override int BossBagNPC => ModContent.NPCType<PutridPinkyPhase2>();
		public override bool CanRightClick()
		{
			return true;
		}
		public override void OpenBossBag(Player player)
		{
			player.QuickSpawnItem(ModContent.ItemType<PutridEye>());
			player.QuickSpawnItem(ModContent.ItemType<VialofAcid>(), Main.rand.Next(20, 30));
			player.QuickSpawnItem(ItemID.PinkGel,Main.rand.Next(40, 60));
			player.QuickSpawnItem(ModContent.ItemType<Wormwood>(), Main.rand.Next(20, 30));
			int rand = Main.rand.Next(10);
			if(rand == 0)
				player.QuickSpawnItem(ModContent.ItemType<GelWings>());
			if(rand == 1)
				player.QuickSpawnItem(ModContent.ItemType<WormWoodParasite>());
			if(rand == 2)
				player.QuickSpawnItem(ModContent.ItemType<WormWoodHelix>());
			if(rand == 3)
				player.QuickSpawnItem(ModContent.ItemType<WormWoodHook>());
			if(rand == 4)
				player.QuickSpawnItem(ModContent.ItemType<WormWoodCollapse>());
			if(rand == 5)
				player.QuickSpawnItem(ModContent.ItemType<WormWoodScepter>());
			if(rand == 6)
				player.QuickSpawnItem(ModContent.ItemType<WormWoodStaff>());
			if(rand == 7)
				player.QuickSpawnItem(ModContent.ItemType<WormWoodSpike>());
		}
	}
}