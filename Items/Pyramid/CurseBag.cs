using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using Terraria.ID;
using SOTS.NPCs.Boss.Curse;

namespace SOTS.Items.Pyramid
{
	public class CurseBag : ModItem
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
		public override int BossBagNPC => ModContent.NPCType<PharaohsCurse>();
		public override bool CanRightClick()
		{
			return true;
		}
		public override void OpenBossBag(Player player)
		{
			player.QuickSpawnItem(ModContent.ItemType<HeartInAJar>());
			player.QuickSpawnItem(ModContent.ItemType<CursedMatter>(),Main.rand.Next(14, 25));
		}
	}
}