using Microsoft.Xna.Framework;
using SOTS.Items.Banners;
using Terraria;
using Terraria.ID;
using System.Collections.Generic;
using static Terraria.ModLoader.ModContent;
using SOTS.Items.Fragments;
using SOTS.Items.Void;
using SOTS.Items.Slime;
using Terraria.GameContent.ItemDropRules;
using Terraria.ModLoader;

namespace SOTS.NPCs.TreasureSlimes
{
	public class BasicTreasureSlime : TreasureSlime
	{
        public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Treasure Slime");
			NPCID.Sets.TrailCacheLength[NPC.type] = 6;
			NPCID.Sets.TrailingMode[NPC.type] = 2;
		}
		public override void SetDefaults()
		{
			base.SetDefaults();
			NPC.lifeMax = 75;
			NPC.damage = 15;
			NPC.defense = 8;
			NPC.knockBackResist = 0.6f;
			NPC.value = Item.buyPrice(0, 0, 40, 0);
			NPC.Size = new Vector2(32, 40);
			NPC.npcSlots = 1f;
			Banner = NPC.type;
			BannerItem = ItemType<TreasureSlimeBanner>();
			LootAmt = 3;
			gelColor = new Color(154, 155, 156, 100);
			items = new List<TreasureSlimeItem>()
			{
				new TreasureSlimeItem(ItemID.HerbBag, 3, 3, 1),
				new TreasureSlimeItem(ItemID.Wood, 60, 300, 1),
				new TreasureSlimeItem(ItemID.CopperOre, 30, 72, 0.25f),
				new TreasureSlimeItem(ItemID.TinOre, 30, 72, 0.25f),
				new TreasureSlimeItem(ItemID.LeadOre, 30, 72, 0.25f),
				new TreasureSlimeItem(ItemID.IronOre, 30, 72, 0.25f),
				new TreasureSlimeItem(ItemID.Leather, 5, 5, 1),
				new TreasureSlimeItem(ItemID.StoneBlock, 30, 60, 1),

				new TreasureSlimeItem(ItemID.LivingWoodWand, 1, 1, 0.1f),
				new TreasureSlimeItem(ItemID.LeafWand, 1, 1, 0.1f),
				new TreasureSlimeItem(ItemID.LivingLoom, 1, 1, 0.1f),
				new TreasureSlimeItem(ItemID.Spear, 1, 1, 0.5f),
				new TreasureSlimeItem(ItemID.WoodenBoomerang, 1, 1, 0.5f),
				new TreasureSlimeItem(ItemID.LifeformAnalyzer, 1, 1, 1f),
				new TreasureSlimeItem(ItemID.Aglet, 1, 1, 0.5f),
				new TreasureSlimeItem(ItemID.Radar, 1, 1, 0.1f),
				new TreasureSlimeItem(ItemID.ClimbingClaws, 1, 1, 0.5f),
				new TreasureSlimeItem(ItemID.CordageGuide, 1, 1, 0.1f),
				new TreasureSlimeItem(ItemID.Umbrella, 1, 1, 0.2f),
				new TreasureSlimeItem(ItemID.Blowpipe, 1, 1, 0.2f),
				new TreasureSlimeItem(ItemType<FragmentOfNature>(), 3, 6, 1f),
				new TreasureSlimeItem(ItemType<AlmondMilk>(), 5, 5, 0.25f),

				new TreasureSlimeItem(ItemType<Items.AbandonedVillage.VisionAmulet>(), 1, 1, 0.01f)
			};
		}
		public override void ModifyAdditionalLoot(NPCLoot npcLoot)
		{
			npcLoot.Add(ItemDropRule.Common(ItemType<GelAxe>(), 1, 20, 30));
		}
    }
}