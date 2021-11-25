using Microsoft.Xna.Framework;
using SOTS.Items.Banners;
using Terraria;
using Terraria.ID;
using System.Collections.Generic;
using static Terraria.ModLoader.ModContent;
using SOTS.Items.Fragments;
using SOTS.Items.Void;
using SOTS.Items.Slime;

namespace SOTS.NPCs.TreasureSlimes
{
	public class BasicTreasureSlime : TreasureSlime
	{
        public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Treasure Slime");
			NPCID.Sets.TrailCacheLength[npc.type] = 6;
			NPCID.Sets.TrailingMode[npc.type] = 2;
		}
		public override void SetDefaults()
		{
			base.SetDefaults();
			npc.lifeMax = 75;
			npc.damage = 15;
			npc.defense = 8;
			npc.knockBackResist = 0.6f;
			npc.value = Item.buyPrice(0, 0, 80, 0);
			npc.Size = new Vector2(32, 40);
			npc.npcSlots = 1f;
			banner = npc.type;
			bannerItem = ItemType<TreasureSlimeBanner>();
			LootAmt = 3;
			gelColor = new Color(154, 155, 156, 100);
			items = new List<TreasureSlimeItem>()
			{
				new TreasureSlimeItem(ItemID.Wood, 60, 300, 1),
				new TreasureSlimeItem(ItemID.CopperOre, 30, 72, 0.25f),
				new TreasureSlimeItem(ItemID.TinOre, 30, 72, 0.25f),
				new TreasureSlimeItem(ItemID.LeadOre, 30, 72, 0.25f),
				new TreasureSlimeItem(ItemID.IronOre, 30, 72, 0.25f),
				new TreasureSlimeItem(ItemID.Leather, 5, 5, 1),
				new TreasureSlimeItem(ItemID.StoneBlock, 30, 60, 1),

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
				new TreasureSlimeItem(ItemType<AlmondMilk>(), 5, 5, 0.25f)
			};
		}
        public override void AdditionalLoot()
        {
			Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemType<GelAxe>(), 20 + Main.rand.Next(11));
		}
    }
}