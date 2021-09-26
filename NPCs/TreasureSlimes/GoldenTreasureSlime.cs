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

namespace SOTS.NPCs.TreasureSlimes
{
	public class GoldenTreasureSlime : TreasureSlime
	{
        public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Golden Treasure Slime");
			NPCID.Sets.TrailCacheLength[npc.type] = 6;
			NPCID.Sets.TrailingMode[npc.type] = 2;
		}
		public override void SetDefaults()
		{
			base.SetDefaults();
			npc.lifeMax = 150;
			npc.damage = 20;
			npc.defense = 12;
			npc.knockBackResist = 0.4f;
			npc.value = Item.buyPrice(0, 3, 0, 0);
			npc.Size = new Vector2(32, 38);
			npc.npcSlots = 1f;
			banner = npc.type;
			bannerItem = ItemType<GoldenTreasureSlimeBanner>();
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
				new TreasureSlimeItem(ItemType<ManicMiner>(), 1, 1, 0.1f),
				new TreasureSlimeItem(ItemType<MinersSword>(), 1, 1, 0.5f),
				new TreasureSlimeItem(ItemType<ShieldofDesecar>(), 1, 1, 0.2f),
				new TreasureSlimeItem(ItemType<ShieldofStekpla>(), 1, 1, 0.2f),
				new TreasureSlimeItem(ItemType<FragmentOfEarth>(), 3, 6, 1f)
			};
		}
        public override void AdditionalLoot()
        {
			Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemType<MinersPickaxe>(), 3 + Main.rand.Next(3));
		}
    }
}