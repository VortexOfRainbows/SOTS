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

namespace SOTS.NPCs.TreasureSlimes
{
	public class IceTreasureSlime : TreasureSlime
	{
        public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Ice Treasure Slime");
			NPCID.Sets.TrailCacheLength[npc.type] = 6;
			NPCID.Sets.TrailingMode[npc.type] = 2;
		}
		public override void SetDefaults()
		{
			base.SetDefaults();
			npc.lifeMax = 125;
			npc.damage = 25;
			npc.defense = 10;
			npc.knockBackResist = 0.5f;
			npc.value = Item.buyPrice(0, 2, 0, 0);
			npc.Size = new Vector2(32, 30);
			npc.npcSlots = 1f;
			banner = npc.type;
			bannerItem = ItemType<FrozenTreasureSlimeBanner>();
			LootAmt = 4;
			gelColor = new Color(106, 210, 255, 100);
			items = new List<TreasureSlimeItem>()
			{
				new TreasureSlimeItem(ItemID.BorealWood, 60, 300, 1f),
				new TreasureSlimeItem(ItemID.LeadOre, 30, 72, 0.25f),
				new TreasureSlimeItem(ItemID.IronOre, 30, 72, 0.25f),
				new TreasureSlimeItem(ItemID.SilverOre, 30, 72, 0.25f),
				new TreasureSlimeItem(ItemID.TungstenOre, 30, 72, 0.25f),
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
				new TreasureSlimeItem(ItemType<StrawberryIcecream>(), 5, 5, 0.25f)
			};
		}
        public override void AdditionalLoot()
        {
			Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemType<GelAxe>(), 20 + Main.rand.Next(11));
		}
    }
}