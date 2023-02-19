using SOTS.NPCs.Boss.Polaris;
using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.Permafrost
{
	public class PolarisBossBag : ModItem
	{
		public override void SetStaticDefaults()
		{
			ItemID.Sets.BossBag[Type] = true;
			ItemID.Sets.PreHardmodeLikeBossBag[Type] = false;
			this.SetResearchCost(3);
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
		public override void ModifyItemLoot(ItemLoot itemLoot)
		{
			itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<FrigidHourglass>()));
			itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<AbsoluteBar>(), 1, 26, 42));
			itemLoot.Add(ItemDropRule.Common(ItemID.FrostCore, 1, 1, 2));
		}
	}
}