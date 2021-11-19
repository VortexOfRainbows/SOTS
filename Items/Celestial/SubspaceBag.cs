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
			item.width = 30;
			item.height = 32;
			item.value = 0;
			item.rare = ItemRarityID.Red;
			item.maxStack = 999;
			item.consumable = true;
			item.expert = true;
		}
		public override int BossBagNPC => mod.NPCType("SubspaceSerpentHead");
		public override bool CanRightClick()
		{
			return true;
		}
		public override void OpenBossBag(Player player)
		{
			player.QuickSpawnItem(ModContent.ItemType<SubspaceLocket>());
			player.QuickSpawnItem(mod.ItemType("SanguiteBar"), Main.rand.Next(16, 30));
		}
	}
}
