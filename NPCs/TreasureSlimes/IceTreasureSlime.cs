using Microsoft.Xna.Framework;
using SOTS.Items.Banners;
using Terraria;
using Terraria.ID;
using System.Collections.Generic;
using static Terraria.ModLoader.ModContent;
using SOTS.Items.Fragments;
using SOTS.Items.Void;
using SOTS.Items.Slime;
using SOTS.Items.Permafrost;
using SOTS.Items.AbandonedVillage;
using Terraria.ModLoader;
using Terraria.GameContent.ItemDropRules;

namespace SOTS.NPCs.TreasureSlimes
{
	public class IceTreasureSlime : TreasureSlime
	{
        public override void SetStaticDefaults()
		{
			NPCID.Sets.TrailCacheLength[NPC.type] = 6;
			NPCID.Sets.TrailingMode[NPC.type] = 2;
		}
		public override void SetDefaults()
		{
			base.SetDefaults();
			NPC.lifeMax = 125;
			NPC.damage = 25;
			NPC.defense = 10;
			NPC.knockBackResist = 0.5f;
			NPC.value = Item.buyPrice(0, 1, 0, 0);
			NPC.Size = new Vector2(32, 34);
			NPC.npcSlots = 1f;
			Banner = NPC.type;
			BannerItem = ItemType<FrozenTreasureSlimeBanner>();
			LootAmt = 4;
			gelColor = new Color(106, 210, 255, 100);
			items = new List<TreasureSlimeItem>()
			{
				new TreasureSlimeItem(ItemType<AncientSteelBar>(), 9, 15, 1f),
				new TreasureSlimeItem(ItemID.BorealWood, 60, 300, 1f),
				new TreasureSlimeItem(ItemID.LeadOre, 30, 72, 0.25f),
				new TreasureSlimeItem(ItemID.IronOre, 30, 72, 0.25f),
				new TreasureSlimeItem(ItemID.SilverOre, 30, 72, 0.25f),
				new TreasureSlimeItem(ItemID.TungstenOre, 30, 72, 0.25f),
				new TreasureSlimeItem(ItemType<FrigidIce>(), 40, 80, 1f),
				new TreasureSlimeItem(ItemID.IceBlock, 30, 90, 0.3f),
				new TreasureSlimeItem(ItemID.SnowBlock, 30, 90, 0.3f),

				new TreasureSlimeItem(ItemID.SpelunkerPotion, 1, 4, 0.2f),
				new TreasureSlimeItem(ItemID.WaterWalkingPotion, 1, 4, 0.2f),
				new TreasureSlimeItem(ItemID.FeatherfallPotion, 1, 4, 0.2f),
				new TreasureSlimeItem(ItemID.WarmthPotion, 1, 4, 0.2f),
				new TreasureSlimeItem(ItemID.IceBoomerang, 1, 1, 1f),
				new TreasureSlimeItem(ItemID.IceBlade, 1, 1, 1f),
				new TreasureSlimeItem(ItemID.IceSkates, 1, 1, 1f),
				new TreasureSlimeItem(ItemID.SnowballCannon, 1, 1, 1f),
				new TreasureSlimeItem(ItemID.BlizzardinaBottle, 1, 1, 1f),
				new TreasureSlimeItem(ItemID.FlurryBoots, 1, 1, 1f),
				new TreasureSlimeItem(ItemID.IceMirror, 1, 1, 0.5f),
				new TreasureSlimeItem(ItemID.SnowballLauncher, 1, 1, 0.1f),
				new TreasureSlimeItem(ItemID.VikingHelmet, 1, 1, 0.2f),
				new TreasureSlimeItem(ItemID.IceMachine, 1, 1, 0.1f),
				new TreasureSlimeItem(ItemID.Fish, 1, 1, 0.1f),
				new TreasureSlimeItem(ItemID.Compass, 1, 1, 0.1f),
				new TreasureSlimeItem(ItemType<FragmentOfPermafrost>(), 3, 6, 1f),
				new TreasureSlimeItem(ItemType<StrawberryIcecream>(), 5, 5, 0.25f),

				new TreasureSlimeItem(ItemType<Items.AbandonedVillage.VisionAmulet>(), 1, 1, 0.01f)
			};
		}
		public override void ModifyAdditionalLoot(NPCLoot npcLoot)
		{
			npcLoot.Add(ItemDropRule.Common(ItemType<GelAxe>(), 1, 20, 30));
		}
	}
}