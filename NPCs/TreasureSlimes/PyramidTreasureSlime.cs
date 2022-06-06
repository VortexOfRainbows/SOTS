using Microsoft.Xna.Framework;
using SOTS.Items.Banners;
using Terraria;
using Terraria.ID;
using System.Collections.Generic;
using static Terraria.ModLoader.ModContent;
using SOTS.Items.Fragments;
using SOTS.Items.Void;
using SOTS.Items.Pyramid;
using SOTS.Items;
using SOTS.Items.Pyramid;
using SOTS.Items.Flails;
using Terraria.ModLoader;
using Terraria.GameContent.ItemDropRules;

namespace SOTS.NPCs.TreasureSlimes
{
	public class PyramidTreasureSlime : TreasureSlime
	{
        public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Pyramid Treasure Slime");
			NPCID.Sets.TrailCacheLength[NPC.type] = 6;
			NPCID.Sets.TrailingMode[NPC.type] = 2;
		}
		public override void SetDefaults()
		{
			base.SetDefaults();
			NPC.lifeMax = 200;
			NPC.damage = 35;
			NPC.defense = 18;
			NPC.knockBackResist = 0.1f;
			NPC.value = Item.buyPrice(0, 4, 0, 0);
			NPC.Size = new Vector2(32, 38);
			NPC.npcSlots = 1f;
			Banner = NPC.type;
			BannerItem = ItemType<PyramidTreasureSlimeBanner>();
			LootAmt = 4;
			gelColor = new Color(186, 168, 84, 100);
			items = new List<TreasureSlimeItem>()
			{
				new TreasureSlimeItem(ItemID.SandBlock, 60, 300, 1f),
				new TreasureSlimeItem(ItemID.GoldOre, 30, 72, 0.25f),
				new TreasureSlimeItem(ItemID.PlatinumOre, 30, 72, 0.25f),
				new TreasureSlimeItem(ItemID.CrimtaneOre, 30, 72, 0.25f),
				new TreasureSlimeItem(ItemID.DemoniteOre, 30, 72, 0.25f),
				new TreasureSlimeItem(ItemType<RoyalGoldBrick>(), 60, 120, 0.5f),
				new TreasureSlimeItem(ItemType<RoyalRubyShard>(), 10, 30, 0.2f),

				new TreasureSlimeItem(ItemID.HunterPotion, 1, 4, 0.2f),
				new TreasureSlimeItem(ItemID.TrapsightPotion, 1, 4, 0.2f),
				new TreasureSlimeItem(ItemID.SandstorminaBottle, 1, 1, 0.75f),
				new TreasureSlimeItem(ItemID.FlyingCarpet, 1, 1, 0.75f),
				new TreasureSlimeItem(ItemID.PharaohsMask, 1, 1, 0.05f),
				new TreasureSlimeItem(ItemID.PharaohsRobe, 1, 1, 0.05f),
				new TreasureSlimeItem(ItemType<AnubisHat>(), 1, 1, 0.1f),
				new TreasureSlimeItem(ItemType<Aten>(), 1, 1, 1f),
				new TreasureSlimeItem(ItemType<EmeraldBracelet>(), 1, 1, 1f),
				new TreasureSlimeItem(ItemType<ImperialPike>(), 1, 1, 1f),
				new TreasureSlimeItem(ItemType<PharaohsCane>(), 1, 1, 0.2f),
				new TreasureSlimeItem(ItemType<PitatiLongbow>(), 1, 1, 1f),
				new TreasureSlimeItem(ItemType<SandstoneEdge>(), 1, 1, 1f),
				new TreasureSlimeItem(ItemType<SandstoneWarhammer>(), 1, 1, 1f),
				new TreasureSlimeItem(ItemType<SandstoneEdge>(), 1, 1, 1f),
				new TreasureSlimeItem(ItemType<ShiftingSands>(), 1, 1, 1f),
				new TreasureSlimeItem(ItemType<SunlightAmulet>(), 1, 1, 1f),
				new TreasureSlimeItem(ItemType<ExplosiveKnife>(), 30, 50, 1f),
				new TreasureSlimeItem(ItemType<FragmentOfEarth>(), 3, 6, 0.2f),
				new TreasureSlimeItem(ItemType<CursedCaviar>(), 5, 5, 0.2f),

				new TreasureSlimeItem(ItemType<Items.GhostTown.VisionAmulet>(), 1, 1, 0.01f)
			};
		}
		public override void ModifyAdditionalLoot(NPCLoot npcLoot)
		{
			npcLoot.Add(ItemDropRule.Common(ItemType<JuryRiggedDrill>(), 1, 4, 8));
			npcLoot.Add(ItemDropRule.Common(ItemType<Snakeskin>(), 2, 5, 10)
				.OnFailedRoll(ItemDropRule.Common(ItemType<SoulResidue>(), 1, 5, 10)));
		}
    }
}