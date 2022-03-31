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
			Tooltip.SetDefault("{$CommonItemTooltip.RightClickToOpen}");
		}
		public override void SetDefaults()
		{
			item.width = 40;
			item.height = 34;
			item.value = 0;
			item.rare = ItemRarityID.Red;
			item.maxStack = 999;
			item.consumable = true;
			item.expert = true;
		}
		public override int BossBagNPC => ModContent.NPCType<SubspaceSerpentHead>();
		public override bool CanRightClick()
		{
			return true;
		}
		public override void OpenBossBag(Player player)
		{
			player.TryGettingDevArmor();
			player.QuickSpawnItem(ModContent.ItemType<SubspaceLocket>());
			player.QuickSpawnItem(ModContent.ItemType<SanguiteBar>(), Main.rand.Next(16, 30));
		}
	}
}