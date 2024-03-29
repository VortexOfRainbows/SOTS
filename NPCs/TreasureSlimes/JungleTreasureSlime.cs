using Microsoft.Xna.Framework;
using SOTS.Items.Banners;
using Terraria;
using Terraria.ID;
using System.Collections.Generic;
using static Terraria.ModLoader.ModContent;
using SOTS.Items.Fragments;
using SOTS.Items.Inferno;
using SOTS.Items.Tools;
using SOTS.Items.AbandonedVillage;
using SOTS.Items.Void;
using SOTS.Items;
using SOTS.Items.Potions;
using Terraria.ModLoader;
using Terraria.GameContent.ItemDropRules;

namespace SOTS.NPCs.TreasureSlimes
{
	public class JungleTreasureSlime : TreasureSlime
	{
        public override void SetStaticDefaults()
		{
			NPCID.Sets.TrailCacheLength[NPC.type] = 6;
			NPCID.Sets.TrailingMode[NPC.type] = 2;
		}
		public override void SetDefaults()
		{
			base.SetDefaults();
			NPC.lifeMax = 325;
			NPC.damage = 50; //hits as hard and has way more health than shadow, but with less defense and less kb resist
			NPC.defense = 0;
			NPC.knockBackResist = 0.15f;
			NPC.value = Item.buyPrice(0, 2, 40, 0);
			NPC.Size = new Vector2(32, 40);
			NPC.npcSlots = 1f;
			Banner = NPC.type;
			BannerItem = ItemType<JungleTreasureSlimeBanner>();
			LootAmt = 4;
			gelColor = new Color(123, 173, 75, 100);
			items = new List<TreasureSlimeItem>()
			{
				new TreasureSlimeItem(ItemID.HerbBag, 1, 3, 1),
				new TreasureSlimeItem(ItemID.Hive, 90, 150, 0.5f),
				new TreasureSlimeItem(ItemID.RichMahogany, 60, 300, 1),
				new TreasureSlimeItem(ItemID.Stinger, 6, 14, 0.3f),
				new TreasureSlimeItem(ItemID.Vine, 4, 8, 0.3f),

				new TreasureSlimeItem(ItemID.AncientCobaltHelmet, 1, 1, 0.2f),
				new TreasureSlimeItem(ItemID.AncientCobaltBreastplate, 1, 1, 0.2f),
				new TreasureSlimeItem(ItemID.AncientCobaltLeggings, 1, 1, 0.2f),
				new TreasureSlimeItem(ItemID.ArchaeologistsHat, 1, 1, 0.3f),
				new TreasureSlimeItem(ItemID.RobotHat, 1, 1, 0.3f),
				new TreasureSlimeItem(ItemID.JungleRose, 1, 1, 0.3f),
				new TreasureSlimeItem(ItemID.LivingMahoganyLeafWand, 1, 1, 0.1f),
				new TreasureSlimeItem(ItemID.LivingMahoganyWand, 1, 1, 0.1f),
				new TreasureSlimeItem(ItemID.HoneyDispenser, 1, 1, 0.1f),
				new TreasureSlimeItem(ItemID.Seaweed, 1, 1, 0.3f),

				new TreasureSlimeItem(ItemID.FiberglassFishingPole, 1, 1, 0.3f),
				new TreasureSlimeItem(ItemID.FlowerBoots, 1, 1, 0.3f),
				new TreasureSlimeItem(ItemID.NaturesGift, 1, 1, 0.5f),
				new TreasureSlimeItem(ItemID.StaffofRegrowth, 1, 1, 0.5f),
				new TreasureSlimeItem(ItemID.SummoningPotion, 1, 4, 0.5f),
				new TreasureSlimeItem(ItemID.Bezoar, 1, 1, 1f),

				new TreasureSlimeItem(ItemID.AnkletoftheWind, 1, 1, 1f),
				new TreasureSlimeItem(ItemID.FeralClaws, 1, 1, 1f),
				new TreasureSlimeItem(ItemID.Boomstick, 1, 1, 1f),
				new TreasureSlimeItem(ItemType<VibePotion>(), 1, 4, 0.5f),
				new TreasureSlimeItem(ItemType<FragmentOfNature>(), 3, 6, 0.3f),
				new TreasureSlimeItem(ItemType<AlmondMilk>(), 5, 5, 0.3f),

				new TreasureSlimeItem(ItemType<Items.AbandonedVillage.VisionAmulet>(), 1, 1, 0.01f)
			};
		}
		public override void ModifyAdditionalLoot(NPCLoot npcLoot)
		{
			npcLoot.Add(ItemDropRule.Common(ItemID.JungleSpores, 1, 3, 6));
		}
    }
}