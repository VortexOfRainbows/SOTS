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
			Item.width = 32;
			Item.height = 32;
			Item.value = 0;
			Item.rare = ItemRarityID.LightPurple;
			Item.expert = true;
			Item.maxStack = 999;
			Item.consumable = true;
		}
		public override int BossBagNPC => ModContent.NPCType<PutridPinkyPhase2>();
		public override bool CanRightClick()
		{
			return true;
		}
		public override void OpenBossBag(Player player)
		{
			player.QuickSpawnItem(player.GetSource_OpenItem(Type), ModContent.ItemType<PutridEye>());
			player.QuickSpawnItem(player.GetSource_OpenItem(Type), ModContent.ItemType<VialofAcid>(), Main.rand.Next(20, 30));
			player.QuickSpawnItem(player.GetSource_OpenItem(Type), ItemID.PinkGel,Main.rand.Next(40, 60));
			player.QuickSpawnItem(player.GetSource_OpenItem(Type), ModContent.ItemType<Wormwood>(), Main.rand.Next(20, 30));
			int rand = Main.rand.Next(10);
			if(rand == 0)
				player.QuickSpawnItem(player.GetSource_OpenItem(Type), ModContent.ItemType<GelWings>());
			if(rand == 1)
				player.QuickSpawnItem(player.GetSource_OpenItem(Type), ModContent.ItemType<WormWoodParasite>());
			if(rand == 2)
				player.QuickSpawnItem(player.GetSource_OpenItem(Type), ModContent.ItemType<WormWoodHelix>());
			if(rand == 3)
				player.QuickSpawnItem(player.GetSource_OpenItem(Type), ModContent.ItemType<WormWoodHook>());
			if(rand == 4)
				player.QuickSpawnItem(player.GetSource_OpenItem(Type), ModContent.ItemType<WormWoodCollapse>());
			if(rand == 5)
				player.QuickSpawnItem(player.GetSource_OpenItem(Type), ModContent.ItemType<WormWoodScepter>());
			if(rand == 6)
				player.QuickSpawnItem(player.GetSource_OpenItem(Type), ModContent.ItemType<WormWoodStaff>());
			if(rand == 7)
				player.QuickSpawnItem(player.GetSource_OpenItem(Type), ModContent.ItemType<WormWoodSpike>());
		}
	}
}