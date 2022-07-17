using Microsoft.Xna.Framework;
using SOTS.Items.Banners;
using Terraria;
using Terraria.ID;
using System.Collections.Generic;
using static Terraria.ModLoader.ModContent;
using SOTS.Items.Fragments;
using SOTS.Items.Inferno;
using SOTS.Items.Tools;
using SOTS.Void;
using Terraria.GameContent.ItemDropRules;
using Terraria.ModLoader;

namespace SOTS.NPCs.TreasureSlimes
{
	public class HallowTreasureSlime : TreasureSlime
	{
        public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Hallowed Treasure Slime");
			NPCID.Sets.TrailCacheLength[NPC.type] = 6;
			NPCID.Sets.TrailingMode[NPC.type] = 2;
		}
		public override void SetDefaults()
		{
			base.SetDefaults();
			NPC.lifeMax = 900;
			NPC.damage = 80;
			NPC.defense = 30;
			NPC.knockBackResist = 0.1f;
			NPC.value = Item.buyPrice(0, 10, 0, 0);
			NPC.Size = new Vector2(32, 44);
			NPC.npcSlots = 1f;
			Banner = NPC.type;
			BannerItem = ItemType<HallowTreasureSlimeBanner>();
			NPC.lavaImmune = true;
			LootAmt = 3;
			Color c = VoidPlayer.ChaosPink;
			c.A = 100;
			gelColor = c;
			items = new List<TreasureSlimeItem>()
			{
				new TreasureSlimeItem(ItemID.PearlstoneBlock, 60, 300, 1f),
				new TreasureSlimeItem(ItemID.Pearlwood, 60, 300, 1f),
				new TreasureSlimeItem(ItemID.CobaltOre, 40, 120, 0.33f),
				new TreasureSlimeItem(ItemID.PalladiumOre, 40, 120, 0.33f),
				new TreasureSlimeItem(ItemID.MythrilOre, 40, 120, 0.33f),
				new TreasureSlimeItem(ItemID.OrichalcumOre, 40, 120, 0.33f),
				new TreasureSlimeItem(ItemID.AdamantiteOre, 40, 120, 0.33f),
				new TreasureSlimeItem(ItemID.TitaniumOre, 40, 120, 0.33f),

				new TreasureSlimeItem(ItemID.RodofDiscord, 1, 1, 0.033f),
				new TreasureSlimeItem(ItemID.BlessedApple, 1, 1, 0.033f),
				new TreasureSlimeItem(ItemID.HallowedKey, 1, 1, 0.033f),

				new TreasureSlimeItem(ItemID.CrystalSerpent, 1, 1, 1),
				new TreasureSlimeItem(ItemID.UnicornonaStick, 1, 1, 1f),
				new TreasureSlimeItem(ItemID.Megaphone, 1, 1, 1),
				new TreasureSlimeItem(ItemID.FastClock, 1, 1, 1),
				new TreasureSlimeItem(ItemID.TrifoldMap, 1, 1, 1),
				new TreasureSlimeItem(ItemID.LightShard, 1, 1, 1f),

				new TreasureSlimeItem(ItemID.ScalyTruffle, 1, 1, 0.5f),
				new TreasureSlimeItem(ItemID.Chik, 1, 1, 1f),

				new TreasureSlimeItem(ItemID.LovePotion, 1, 4, 0.25f),
				new TreasureSlimeItem(ItemID.TeleportationPotion, 1, 4, 0.25f),
				new TreasureSlimeItem(ItemID.LifeforcePotion, 1, 4, 0.25f),
				new TreasureSlimeItem(ItemID.HeartreachPotion, 1, 4, 0.25f),
				new TreasureSlimeItem(ItemID.GreaterHealingPotion, 12, 18, 0.25f),
				new TreasureSlimeItem(ItemID.GreaterManaPotion, 12, 18, 0.25f),
				new TreasureSlimeItem(ItemID.Bacon, 3, 5, 0.25f),

				new TreasureSlimeItem(ItemType<FragmentOfChaos>(), 3, 6, 0.2f),

				new TreasureSlimeItem(ItemType<Items.AbandonedVillage.VisionAmulet>(), 1, 1, 0.01f)
			};
		}
        public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
        {
			NPC.lifeMax = NPC.lifeMax * 5 / 6;
			NPC.damage = (int)(NPC.damage * 7 / 8);
		}
		public override void ModifyAdditionalLoot(NPCLoot npcLoot)
		{
			npcLoot.Add(ItemDropRule.Common(ItemID.CrystalShard, 1, 10, 15));
		}
    }
}