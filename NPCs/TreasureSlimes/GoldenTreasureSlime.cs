using Microsoft.Xna.Framework;
using SOTS.Items.Banners;
using Terraria;
using Terraria.ID;
using System.Collections.Generic;
using static Terraria.ModLoader.ModContent;

using SOTS.Items.ChestItems;
using SOTS.Items.Fragments;
using SOTS.Items.Crushers;
using SOTS.Items.Tools;
using SOTS.Items;
using Terraria.GameContent.ItemDropRules;
using Terraria.ModLoader;

namespace SOTS.NPCs.TreasureSlimes
{
	public class GoldenTreasureSlime : TreasureSlime
	{
        public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Golden Treasure Slime");
			NPCID.Sets.TrailCacheLength[NPC.type] = 6;
			NPCID.Sets.TrailingMode[NPC.type] = 2;
		}
		public override void SetDefaults()
		{
			base.SetDefaults();
			NPC.lifeMax = 150;
			NPC.damage = 20;
			NPC.defense = 12;
			NPC.knockBackResist = 0.4f;
			NPC.value = Item.buyPrice(0, 1, 50, 0);
			NPC.Size = new Vector2(32, 38);
			NPC.npcSlots = 1f;
			Banner = NPC.type;
			BannerItem = ItemType<GoldenTreasureSlimeBanner>();
			LootAmt = 4; 
			gelColor = new Color(255, 255, 133, 100);
			items = new List<TreasureSlimeItem>()
			{
				new TreasureSlimeItem(ItemID.SilverOre, 30, 72, 0.25f),
				new TreasureSlimeItem(ItemID.TungstenOre, 30, 72, 0.25f),
				new TreasureSlimeItem(ItemID.GoldOre, 30, 72, 0.25f),
				new TreasureSlimeItem(ItemID.PlatinumOre, 30, 72, 0.25f),

				new TreasureSlimeItem(ItemID.MiningShirt, 1, 1, 0.5f),
				new TreasureSlimeItem(ItemID.MiningPants, 1, 1, 0.5f),
				new TreasureSlimeItem(ItemID.BonePickaxe, 1, 1, 0.1f),
				new TreasureSlimeItem(ItemID.WhoopieCushion, 1, 1, 0.05f),
				new TreasureSlimeItem(ItemID.MetalDetector, 1, 1, 0.05f),
				new TreasureSlimeItem(ItemID.DepthMeter, 1, 1, 0.05f),
				new TreasureSlimeItem(ItemID.BandofRegeneration, 1, 1),
				new TreasureSlimeItem(ItemID.MagicMirror, 1, 1, 0.5f),
				new TreasureSlimeItem(ItemID.CloudinaBottle, 1, 1),
				new TreasureSlimeItem(ItemID.HermesBoots, 1, 1),
				new TreasureSlimeItem(ItemID.EnchantedBoomerang, 1, 1),
				new TreasureSlimeItem(ItemID.ShoeSpikes, 1, 1),
				new TreasureSlimeItem(ItemID.FlareGun, 1, 1, 0.25f),
				new TreasureSlimeItem(ItemID.Flare, 25, 50, 0.25f),
				new TreasureSlimeItem(ItemID.Extractinator, 1, 1),
				new TreasureSlimeItem(ItemID.LavaCharm, 1, 1, 0.1f),
				new TreasureSlimeItem(ItemID.SpelunkerPotion, 1, 4, 0.75f),
				new TreasureSlimeItem(ItemID.AmberMosquito, 1, 1, 0.25f),
				new TreasureSlimeItem(ItemType<Items.AbandonedVillage.OldKey>(), 1, 1, 0.75f),
				new TreasureSlimeItem(ItemType<ManicMiner>(), 1, 1, 0.1f),
				new TreasureSlimeItem(ItemType<MinersSword>(), 1, 1, 0.5f),
				new TreasureSlimeItem(ItemType<CrushingAmplifier>(), 1, 1, 0.5f),
				new TreasureSlimeItem(ItemType<ShieldofDesecar>(), 1, 1, 0.2f),
				new TreasureSlimeItem(ItemType<ShieldofStekpla>(), 1, 1, 0.2f),
				new TreasureSlimeItem(ItemType<FragmentOfEarth>(), 3, 6, 1f),

				new TreasureSlimeItem(ItemType<Items.AbandonedVillage.VisionAmulet>(), 1, 1, 0.01f)
			};
		}
		public override void ModifyAdditionalLoot(NPCLoot npcLoot)
		{
			npcLoot.Add(ItemDropRule.Common(ItemType<MinersPickaxe>(), 1, 3, 5));
		}
    }
}