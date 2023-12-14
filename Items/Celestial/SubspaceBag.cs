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
			Item.maxStack = 9999;
			Item.consumable = true;
			Item.expert = true;
		}
		public int BossBagNPC => ModContent.NPCType<SubspaceSerpentHead>();
		public override bool CanRightClick()
		{
			return true;
		}
        public override void ModifyItemLoot(ItemLoot itemLoot)
		{
			itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<SerpentSpine>()));
			itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<SanguiteBar>(), 1, 16, 29));
			itemLoot.Add(ItemDropRule.CoinsBasedOnNPCValue(BossBagNPC));
		}
	}
}