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
			player.TryGettingDevArmor();
			player.QuickSpawnItem(ModContent.ItemType<SubspaceLocket>());
			player.QuickSpawnItem(ModContent.ItemType<SanguiteBar>(), Main.rand.Next(16, 30));
		}
	}
}