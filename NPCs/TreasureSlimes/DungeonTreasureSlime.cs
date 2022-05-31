using Microsoft.Xna.Framework;
using SOTS.Items.Banners;
using Terraria;
using Terraria.ID;
using System.Collections.Generic;
using static Terraria.ModLoader.ModContent;
using SOTS.Items.Fragments;
using SOTS.Items.Inferno;
using SOTS.Items.Tools;
using SOTS.Items.Void;
using Terraria.GameContent.ItemDropRules;
using Terraria.ModLoader;

namespace SOTS.NPCs.TreasureSlimes
{
	public class DungeonTreasureSlime : TreasureSlime
	{
		public static Color color = new Color(132, 141, 206, 100);
        public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Dungeon Treasure Slime");
			NPCID.Sets.TrailCacheLength[NPC.type] = 6;
			NPCID.Sets.TrailingMode[NPC.type] = 2;
		}
		public override void SetDefaults()
		{
			base.SetDefaults();
			NPC.lifeMax = 200;
			NPC.damage = 40;
			NPC.defense = 36; //has crazy defense, but other stats are less than shadow
			NPC.knockBackResist = 0.04f;
			NPC.value = Item.buyPrice(0, 5, 0, 0);
			NPC.Size = new Vector2(32, 40);
			NPC.npcSlots = 1f;
			Banner = NPC.type;
			BannerItem = ItemType<DungeonTreasureSlimeBanner>();
			NPC.lavaImmune = true;
			LootAmt = 4;
			gelColor = color;
			items = new List<TreasureSlimeItem>()
			{
				new TreasureSlimeItem(ItemID.Bone, 40, 80, 1f),
				
				new TreasureSlimeItem(ItemID.Handgun, 1, 1, 1),
				new TreasureSlimeItem(ItemID.AquaScepter, 1, 1, 1),
				new TreasureSlimeItem(ItemID.MagicMissile, 1, 1, 1),
				new TreasureSlimeItem(ItemID.BlueMoon, 1, 1, 1),
				new TreasureSlimeItem(ItemID.CobaltShield, 1, 1, 1),
				new TreasureSlimeItem(ItemID.Muramasa, 1, 1, 1),
				new TreasureSlimeItem(ItemID.Valor, 1, 1, 1f),
				new TreasureSlimeItem(ItemID.WaterBolt, 1, 1, 1f),

				new TreasureSlimeItem(ItemID.ShadowKey, 1, 1, 0.5f),
				new TreasureSlimeItem(ItemID.BoneWand, 1, 1, 0.5f),
				new TreasureSlimeItem(ItemID.AncientNecroHelmet, 1, 1, 0.5f),
				new TreasureSlimeItem(ItemID.Nazar, 1, 1, 0.5f),
				new TreasureSlimeItem(ItemID.ArmorPolish, 1, 1, 0.5f), //technically makes this obtainable before hardmode, but this shouldn't be a problem
				new TreasureSlimeItem(ItemID.TallyCounter, 1, 1, 0.5f),
				new TreasureSlimeItem(ItemID.ClothierVoodooDoll, 1, 1, 0.5f),
				new TreasureSlimeItem(ItemID.BoneKey, 1, 1, 0.1f),
				new TreasureSlimeItem(ItemID.BewitchingTable, 1, 1, 0.1f),
				new TreasureSlimeItem(ItemID.AlchemyTable, 1, 1, 0.1f),
				new TreasureSlimeItem(ItemID.BoneWelder, 1, 1, 0.1f),

				new TreasureSlimeItem(ItemID.MagicPowerPotion, 1, 4, 0.25f),
				new TreasureSlimeItem(ItemID.EndurancePotion, 1, 4, 0.25f),
				new TreasureSlimeItem(ItemID.GravitationPotion, 1, 4, 0.25f),
				new TreasureSlimeItem(ItemType<FragmentOfTide>(), 3, 6, 0.25f),
				new TreasureSlimeItem(ItemType<AvocadoSoup>(), 5, 5, 0.25f),

				new TreasureSlimeItem(ItemType<Items.GhostTown.VisionAmulet>(), 1, 1, 0.01f)
			};
		}
		public override void ModifyAdditionalLoot(NPCLoot npcLoot)
		{
			npcLoot.Add(ItemDropRule.OneFromOptions(1, new int[] { ItemID.WaterCandle, ItemID.GoldenKey, ItemID.LockBox }));
			npcLoot.Add(ItemDropRule.Common(ItemID.Book, 3, 5, 10));
		}
    }
}