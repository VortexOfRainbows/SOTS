using SOTS.NPCs.Boss;
using Terraria;
using Terraria.GameContent.ItemDropRules;
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
			ItemID.Sets.BossBag[Type] = true;
			ItemID.Sets.PreHardmodeLikeBossBag[Type] = false;
			this.SetResearchCost(3);
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
        public override void ModifyItemLoot(ItemLoot itemLoot)
		{
			//itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<SubspaceLocket>()));
			itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<SanguiteBar>(), 1, 16, 29));
		}
	}
}