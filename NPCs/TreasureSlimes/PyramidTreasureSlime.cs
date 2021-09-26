using Microsoft.Xna.Framework;
using SOTS.Items.Banners;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System.Collections.Generic;
using System.Linq;
using System;
using static Terraria.ModLoader.ModContent;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Items.SpecialDrops;
using SOTS.Items.ChestItems;
using SOTS.Items.Fragments;
using SOTS.Items.Void;
using SOTS.Items.Pyramid;
using SOTS.Items;
using SOTS.Items.Pyramid.AncientGold;

namespace SOTS.NPCs.TreasureSlimes
{
	public class PyramidTreasureSlime : TreasureSlime
	{
        public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Pyramid Treasure Slime");
			NPCID.Sets.TrailCacheLength[npc.type] = 6;
			NPCID.Sets.TrailingMode[npc.type] = 2;
		}
		public override void SetDefaults()
		{
			base.SetDefaults();
			npc.lifeMax = 200;
			npc.damage = 35;
			npc.defense = 18;
			npc.knockBackResist = 0.1f;
			npc.value = Item.buyPrice(0, 4, 0, 0);
			npc.Size = new Vector2(32, 38);
			npc.npcSlots = 1f;
			banner = npc.type;
			bannerItem = ItemType<PyramidTreasureSlimeBanner>();
			LootAmt = 4;
			gelColor = new Color(186, 168, 84, 100);
			items = new List<TreasureSlimeItem>()
			{
				new TreasureSlimeItem(ItemID.SandBlock, 60, 300, 1f),
				new TreasureSlimeItem(ItemID.GoldOre, 30, 72, 0.25f),
				new TreasureSlimeItem(ItemID.PlatinumOre, 30, 72, 0.25f),
				new TreasureSlimeItem(ItemID.CrimtaneOre, 30, 72, 0.25f),
				new TreasureSlimeItem(ItemID.DemoniteOre, 30, 72, 0.25f),
				new TreasureSlimeItem(ItemType<RoyalGoldBrick>(), 30, 90, 0.2f),
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
				new TreasureSlimeItem(ItemType<CursedCaviar>(), 5, 5, 0.2f)
			};
		}
        public override void AdditionalLoot()
		{
			Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemType<JuryRiggedDrill>(), 4 + Main.rand.Next(5));
			if (Main.rand.NextBool(2))
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemType<Snakeskin>(), 5 + Main.rand.Next(6));
			else
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemType<SoulResidue>(), 5 + Main.rand.Next(6));
		}
    }
}