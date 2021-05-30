using SOTS.NPCs.Boss.Polaris;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.IceStuff
{
	public class PolarisBossBag : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Treasure Bag");
			Tooltip.SetDefault("Right click to open");
		}
		public override void SetDefaults()
		{
			item.width = 48;
			item.height = 34;
			item.value = 0;
			item.rare = ItemRarityID.Cyan;
			item.expert = true;
			item.maxStack = 99;
			item.consumable = true;
		}
		public override int BossBagNPC => ModContent.NPCType<Polaris>();
		public override bool CanRightClick() { return true; }
		public override void OpenBossBag(Player player)
		{
			player.QuickSpawnItem(ModContent.ItemType<FrigidHourglass>());
			player.QuickSpawnItem(mod.ItemType("AbsoluteBar"), Main.rand.Next(26, 43));
			player.QuickSpawnItem(ItemID.FrostCore, Main.rand.Next(2) + 1);
		}
	}
}