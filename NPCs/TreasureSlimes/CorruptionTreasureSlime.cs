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
			NPCID.Sets.TrailCacheLength[npc.type] = 6;
			NPCID.Sets.TrailingMode[npc.type] = 2;
		}
		public override void SetDefaults()
		{
			base.SetDefaults();
			npc.lifeMax = 200;
			npc.damage = 40;
			npc.defense = 16;
			npc.knockBackResist = 0.05f;
			npc.value = Item.buyPrice(0, 4, 50, 0);
			npc.Size = new Vector2(32, 40);
			npc.npcSlots = 1f;
			banner = npc.type;
			bannerItem = ItemType<CorruptionTreasureSlimeBanner>();
			LootAmt = 3;
			gelColor = new Color(210, 157, 215, 100);
			items = new List<TreasureSlimeItem>()
			{
				new TreasureSlimeItem(ItemType<OldKey>(), 1, 1, 1f), //guaranteed
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
				new TreasureSlimeItem(ItemType<FoulConcoction>(), 5, 5, 0.3f)
			};
		}
        public override void AdditionalLoot()
		{
			Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemType<ExplosiveKnife>(), 10 + Main.rand.Next(11));
		}
    }
}