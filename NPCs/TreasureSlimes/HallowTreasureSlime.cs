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

namespace SOTS.NPCs.TreasureSlimes
{
	public class HallowTreasureSlime : TreasureSlime
	{
        public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Hallowed Treasure Slime");
			NPCID.Sets.TrailCacheLength[npc.type] = 6;
			NPCID.Sets.TrailingMode[npc.type] = 2;
		}
		public override void SetDefaults()
		{
			base.SetDefaults();
			npc.lifeMax = 900;
			npc.damage = 80;
			npc.defense = 30;
			npc.knockBackResist = 0.1f;
			npc.value = Item.buyPrice(0, 10, 0, 0);
			npc.Size = new Vector2(32, 44);
			npc.npcSlots = 1f;
			banner = npc.type;
			bannerItem = ItemType<HallowTreasureSlimeBanner>();
			npc.lavaImmune = true;
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
			};
		}
        public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
        {
			npc.lifeMax = 1500;
			npc.damage = 140;
        }
        public override void AdditionalLoot()
		{
			Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemID.CrystalShard, 10 + Main.rand.Next(6));
		}
    }
}