using SOTS.Items.Fragments;
using SOTS.Items.Tools;
using SOTS.NPCs.Boss.Excavator;
using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.AbandonedVillage
{
	public class ExcavatorBossBag : ModItem
	{
		public override void SetStaticDefaults()
		{
			ItemID.Sets.BossBag[Type] = true;
			ItemID.Sets.PreHardmodeLikeBossBag[Type] = true;
			this.SetResearchCost(3);
		}
		public override void SetDefaults()
		{
			Item.width = 48;
			Item.height = 32;
			Item.value = 0;
			Item.rare = ItemRarityID.Orange;
			Item.expert = true;
			Item.maxStack = 9999;
			Item.consumable = true;
		}
		public override bool CanRightClick() { return true; }
		public override void ModifyItemLoot(ItemLoot itemLoot)
		{
            //itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<FrigidHourglass>()));
            itemLoot.Add(ItemDropRule.FewFromOptions(2, 1, 
				ModContent.ItemType<EarthBreaker>(), 
				ModContent.ItemType<EarthGrinder>(),
				ModContent.ItemType<GuardianGreatsword>(),
				ModContent.ItemType<FortressCrasher>(),
				ModContent.ItemType<MagmaBeam>()));
			itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<OldKey>(), 1, 1, 4));
			itemLoot.Add(ItemDropRule.CoinsBasedOnNPCValue(ModContent.NPCType<Excavator>()));
            itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<FragmentOfEarth>(), 1, 10, 20));
            itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<EarthenPlating>(), 1, 60, 100));
            itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<ExcavatorMask>(), 7));
            itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<DrillHand>(), 1));
        }
    }
}