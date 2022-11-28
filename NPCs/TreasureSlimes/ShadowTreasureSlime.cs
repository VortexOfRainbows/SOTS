using Microsoft.Xna.Framework;
using SOTS.Items.Banners;
using Terraria;
using Terraria.ID;
using System.Collections.Generic;
using static Terraria.ModLoader.ModContent;
using SOTS.Items.Fragments;
using SOTS.Items.Inferno;
using SOTS.Items.Tools;
using Terraria.ModLoader;
using Terraria.GameContent.ItemDropRules;

namespace SOTS.NPCs.TreasureSlimes
{
	public class ShadowTreasureSlime : TreasureSlime
	{
        public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Shadow Treasure Slime");
			NPCID.Sets.TrailCacheLength[NPC.type] = 6;
			NPCID.Sets.TrailingMode[NPC.type] = 2;
		}
		public override void SetDefaults()
		{
			base.SetDefaults();
			NPC.lifeMax = 250;
			NPC.damage = 50;
			NPC.defense = 20;
			NPC.knockBackResist = 0.1f;
			NPC.value = Item.buyPrice(0, 2, 50, 0);
			NPC.Size = new Vector2(32, 38);
			NPC.npcSlots = 1f;
			Banner = NPC.type;
			BannerItem = ItemType<ShadowTreasureSlimeBanner>();
			NPC.lavaImmune = true;
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
				new TreasureSlimeItem(ItemType<BookOfVirtues>(), 1, 1, 0.05f),

				new TreasureSlimeItem(ItemType<Items.AbandonedVillage.VisionAmulet>(), 1, 1, 0.01f)
			};
		}
		public override void ModifyAdditionalLoot(NPCLoot npcLoot)
		{
			npcLoot.Add(ItemDropRule.Common(ItemType<MinersPickaxe>(), 1, 3, 5));
		}
    }
}