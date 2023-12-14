using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using Terraria.ID;
using SOTS.NPCs.Boss.Curse;
using Terraria.GameContent.ItemDropRules;

namespace SOTS.Items.Pyramid
{
	public class CurseBag : ModItem
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
			Item.maxStack = 9999;
			Item.consumable = true;
		}
		public override bool CanRightClick()
		{
			return true;
		}
		public override void ModifyItemLoot(ItemLoot itemLoot)
		{
			itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<HeartInAJar>()));
			itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<CursedMatter>(), 1, 14, 24));
			itemLoot.Add(ItemDropRule.CoinsBasedOnNPCValue(ModContent.NPCType<NPCs.Boss.Curse.PharaohsCurse>()));
		}
	}
}