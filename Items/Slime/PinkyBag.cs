using SOTS.Items.Crushers;
using SOTS.NPCs.Boss;
using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.Slime
{
	public class PinkyBag : ModItem
	{
		public override void SetStaticDefaults()
		{
			ItemID.Sets.BossBag[Type] = true;
			ItemID.Sets.PreHardmodeLikeBossBag[Type] = true;
			this.SetResearchCost(3);
		}
		public override void SetDefaults()
		{
			Item.width = 32;
			Item.height = 32;
			Item.value = 0;
			Item.rare = ItemRarityID.LightPurple;
			Item.expert = true;
			Item.maxStack = 999;
			Item.consumable = true;
		}
		public override bool CanRightClick()
		{
			return true;
		}
		public override void ModifyItemLoot(ItemLoot itemLoot)
		{
			itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<PutridEye>()));
			itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<VialofAcid>(), 1, 20, 30));
			itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<Wormwood>(), 1, 20, 30));
			itemLoot.Add(ItemDropRule.Common(ItemID.PinkGel, 1, 40, 60));
			itemLoot.Add(ItemDropRule.NotScalingWithLuck(ModContent.ItemType<PutridPinkyMask>(), 7));
			IItemDropRule[] oreTypes = new IItemDropRule[] {
				ItemDropRule.Common(ModContent.ItemType<GelWings>()),
				ItemDropRule.Common(ModContent.ItemType<WormWoodParasite>()),
				ItemDropRule.Common(ModContent.ItemType<WormWoodHelix>()),
				ItemDropRule.Common(ModContent.ItemType<WormWoodHook>()),
				ItemDropRule.Common(ModContent.ItemType<WormWoodCollapse>()),
				ItemDropRule.Common(ModContent.ItemType<WormWoodScepter>()),
				ItemDropRule.Common(ModContent.ItemType<WormWoodStaff>())
			};
			itemLoot.Add(new OneFromRulesRule(1, oreTypes));
			itemLoot.Add(ItemDropRule.CoinsBasedOnNPCValue(ModContent.NPCType<PutridPinkyPhase2>()));
		}
	}
}