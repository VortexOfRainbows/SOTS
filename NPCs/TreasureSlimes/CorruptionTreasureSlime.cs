using Microsoft.Xna.Framework;
using SOTS.Items.Banners;
using Terraria;
using Terraria.ID;
using System.Collections.Generic;
using static Terraria.ModLoader.ModContent;
using SOTS.Items.Fragments;
using SOTS.Items.Inferno;
using SOTS.Items.Tools;
using SOTS.Items.GhostTown;
using SOTS.Items.Void;
using SOTS.Items;

namespace SOTS.NPCs.TreasureSlimes
{
	public class CorruptionTreasureSlime : TreasureSlime
	{
        public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Corruption Treasure Slime");
			NPCID.Sets.TrailCacheLength[NPC.type] = 6;
			NPCID.Sets.TrailingMode[NPC.type] = 2;
		}
		public override void SetDefaults()
		{
			base.SetDefaults();
			NPC.lifeMax = 200;
			NPC.damage = 40;
			NPC.defense = 16;
			NPC.knockBackResist = 0.05f;
			NPC.value = Item.buyPrice(0, 4, 50, 0);
			NPC.Size = new Vector2(32, 40);
			NPC.npcSlots = 1f;
			Banner = NPC.type;
			BannerItem = ItemType<CorruptionTreasureSlimeBanner>();
			LootAmt = 3;
			gelColor = new Color(210, 157, 215, 100);
			items = new List<TreasureSlimeItem>()
			{
				new TreasureSlimeItem(ItemType<OldKey>(), 1, 1, 1f), //guaranteed
				new TreasureSlimeItem(ItemType<AncientSteelBar>(), 9, 15, 1f),
				new TreasureSlimeItem(ItemID.Ebonwood, 60, 300, 1),
				new TreasureSlimeItem(ItemID.DemoniteOre, 30, 60, 1f),
				new TreasureSlimeItem(ItemID.RottenChunk, 30, 60, 0.25f),
				new TreasureSlimeItem(ItemID.RottenEgg, 30, 60, 0.25f),
				
				new TreasureSlimeItem(ItemID.AncientShadowGreaves, 1, 1, 0.125f),
				new TreasureSlimeItem(ItemID.AncientShadowHelmet, 1, 1, 0.125f),
				new TreasureSlimeItem(ItemID.AncientShadowScalemail, 1, 1, 0.125f),
				new TreasureSlimeItem(ItemID.EatersBone, 1, 1, 0.1f),
				new TreasureSlimeItem(ItemID.WrathPotion, 1, 4, 0.5f),
				new TreasureSlimeItem(ItemID.ThornsPotion, 1, 4, 0.5f),
				new TreasureSlimeItem(ItemID.Musket, 1, 1, 1f),
				new TreasureSlimeItem(ItemID.ShadowOrb, 1, 1, 1f),
				new TreasureSlimeItem(ItemID.BandofStarpower, 1, 1, 1f),
				new TreasureSlimeItem(ItemID.Vilethorn, 1, 1, 1f),
				new TreasureSlimeItem(ItemID.BallOHurt, 1, 1, 1f),
				new TreasureSlimeItem(ItemType<ZombieHand>(), 1, 1, 0.2f),
				new TreasureSlimeItem(ItemType<FragmentOfEvil>(), 3, 6, 0.3f),
				new TreasureSlimeItem(ItemType<FoulConcoction>(), 5, 5, 0.3f),

				new TreasureSlimeItem(ItemType<VisionAmulet>(), 1, 1, 0.01f)
			};
		}
        public override void AdditionalLoot()
		{
			Item.NewItem((int)NPC.position.X, (int)NPC.position.Y, NPC.width, NPC.height, ItemType<ExplosiveKnife>(), 10 + Main.rand.Next(11));
		}
    }
}