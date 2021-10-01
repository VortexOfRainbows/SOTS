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
using SOTS.Items.Inferno;

namespace SOTS.NPCs.TreasureSlimes
{
	public class ShadowTreasureSlime : TreasureSlime
	{
        public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Shadow Treasure Slime");
			NPCID.Sets.TrailCacheLength[npc.type] = 6;
			NPCID.Sets.TrailingMode[npc.type] = 2;
		}
		public override void SetDefaults()
		{
			base.SetDefaults();
			npc.lifeMax = 250;
			npc.damage = 50;
			npc.defense = 20;
			npc.knockBackResist = 0.1f;
			npc.value = Item.buyPrice(0, 5, 0, 0);
			npc.Size = new Vector2(32, 38);
			npc.npcSlots = 1f;
			banner = npc.type;
			bannerItem = ItemType<ShadowTreasureSlimeBanner>();
			npc.lavaImmune = true;
			LootAmt = 3;
			gelColor = new Color(98, 88, 176, 100);
			items = new List<TreasureSlimeItem>()
			{
				new TreasureSlimeItem(ItemID.Obsidian, 50, 100, 1f),
				new TreasureSlimeItem(ItemID.Hellstone, 30, 90, 0.25f),
				new TreasureSlimeItem(ItemID.Meteorite, 30, 90, 0.25f),
				
				new TreasureSlimeItem(ItemID.HellwingBow, 1, 1, 1),
				new TreasureSlimeItem(ItemID.Sunfury, 1, 1, 1),
				new TreasureSlimeItem(ItemID.FlowerofFire, 1, 1, 1),
				new TreasureSlimeItem(ItemID.Flamelash, 1, 1, 1),
				new TreasureSlimeItem(ItemID.DarkLance, 1, 1, 1),
				new TreasureSlimeItem(ItemID.ObsidianRose, 1, 1, 0.5f),
				new TreasureSlimeItem(ItemID.DemonScythe, 1, 1, 0.75f),
				new TreasureSlimeItem(ItemID.Cascade, 1, 1, 0.25f),
				new TreasureSlimeItem(ItemID.MagmaStone, 1, 1, 0.5f),
				new TreasureSlimeItem(ItemID.ObsidianSkinPotion, 1, 4, 0.25f),
				new TreasureSlimeItem(ItemID.InfernoPotion, 1, 4, 0.25f),
				new TreasureSlimeItem(ItemID.LifeforcePotion, 1, 4, 0.25f),
				new TreasureSlimeItem(ItemID.HeartreachPotion, 1, 4, 0.25f),
				new TreasureSlimeItem(ItemType<FragmentOfInferno>(), 3, 6, 0.2f),
				new TreasureSlimeItem(ItemType<BookOfVirtues>(), 1, 1, 0.05f)
			};
		}
        public override void AdditionalLoot()
		{
			Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemType<MinersPickaxe>(), 3 + Main.rand.Next(3));
		}
    }
}