using SOTS.NPCs.Boss;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.Celestial
{
	public class SubspaceBag : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Treasure Bag");
			Tooltip.SetDefault("{$CommonItemTooltip.RightClickToOpen}/nCurrently yields no expert exclusive item, as the current one is not yet working in 1.4");
		}
		public override void SetDefaults()
		{
			Item.width = 40;
			Item.height = 34;
			Item.value = 0;
			Item.rare = ItemRarityID.Red;
			Item.maxStack = 999;
			Item.consumable = true;
			Item.expert = true;
		}
		public override int BossBagNPC => ModContent.NPCType<SubspaceSerpentHead>();
		public override bool CanRightClick()
		{
			return true;
		}
		public override void OpenBossBag(Player player)
		{
			player.TryGettingDevArmor(player.GetSource_OpenItem(Type));
			//player.QuickSpawnItem(player.GetSource_OpenItem(Type), ModContent.ItemType<SubspaceLocket>());
			player.QuickSpawnItem(player.GetSource_OpenItem(Type), ModContent.ItemType<SanguiteBar>(), Main.rand.Next(16, 30));
		}
	}
}