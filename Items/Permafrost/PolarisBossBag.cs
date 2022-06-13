using SOTS.NPCs.Boss.Polaris;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.Permafrost
{
	public class PolarisBossBag : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Treasure Bag");
			Tooltip.SetDefault("{$CommonItemTooltip.RightClickToOpen}");
		}
		public override void SetDefaults()
		{
			Item.width = 48;
			Item.height = 34;
			Item.value = 0;
			Item.rare = ItemRarityID.Cyan;
			Item.expert = true;
			Item.maxStack = 999;
			Item.consumable = true;
		}
		public override int BossBagNPC => ModContent.NPCType<Polaris>();
		public override bool CanRightClick() { return true; }
		public override void OpenBossBag(Player player)
		{
			player.TryGettingDevArmor(player.GetSource_OpenItem(Type));
			player.QuickSpawnItem(player.GetSource_OpenItem(Type), ModContent.ItemType<FrigidHourglass>());
			player.QuickSpawnItem(player.GetSource_OpenItem(Type), ModContent.ItemType <AbsoluteBar>(), Main.rand.Next(26, 43));
			player.QuickSpawnItem(player.GetSource_OpenItem(Type), ItemID.FrostCore, Main.rand.Next(2) + 1);
		}
	}
}