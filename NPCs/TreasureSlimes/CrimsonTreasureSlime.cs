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
	public class CrimsonTreasureSlime : TreasureSlime
	{
        public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Crimson Treasure Slime");
			NPCID.Sets.TrailCacheLength[npc.type] = 6;
			NPCID.Sets.TrailingMode[npc.type] = 2;
		}
		public override void SetDefaults()
		{
			base.SetDefaults();
			NPC.lifeMax = 220;
			NPC.damage = 45; //hits harder than corruption, and has more health, but has 4 less defense
			NPC.defense = 8;
			NPC.knockBackResist = 0.05f;
			npc.value = Item.buyPrice(0, 4, 50, 0);
			npc.Size = new Vector2(32, 36);
			npc.npcSlots = 1f;
			Banner = NPC.type;
			BannerItem = ItemType<CrimsonTreasureSlimeBanner>();
			LootAmt = 3;
			gelColor = new Color(145, 33, 30, 100);
			items = new List<TreasureSlimeItem>()
			{
				new TreasureSlimeItem(ItemType<OldKey>(), 1, 1, 1f), //guaranteed
				new TreasureSlimeItem(ItemType<AncientSteelBar>(), 9, 15, 1f),
				new TreasureSlimeItem(ItemID.Shadewood, 60, 300, 1),
				new TreasureSlimeItem(ItemID.CrimtaneOre, 30, 60, 1f),
				new TreasureSlimeItem(ItemID.Vertebrae, 30, 60, 0.25f),
				new TreasureSlimeItem(ItemID.RottenEgg, 30, 60, 0.25f),
				
				//new TreasureSlimeItem(ItemID.AncientShadowGreaves, 1, 1, 0.2f), //has 3 less items than corruption for now ig
				//new TreasureSlimeItem(ItemID.AncientShadowHelmet, 1, 1, 0.2f),
				//new TreasureSlimeItem(ItemID.AncientShadowScalemail, 1, 1, 0.2f),
				new TreasureSlimeItem(ItemID.BoneRattle, 1, 4, 0.1f),
				new TreasureSlimeItem(ItemID.RagePotion, 1, 4, 0.5f),
				new TreasureSlimeItem(ItemID.HeartreachPotion, 1, 1, 0.5f),
				new TreasureSlimeItem(ItemID.TheUndertaker, 1, 1, 1f),
				new TreasureSlimeItem(ItemID.CrimsonHeart, 1, 1, 1f),
				new TreasureSlimeItem(ItemID.PanicNecklace, 1, 1, 1f),
				new TreasureSlimeItem(ItemID.CrimsonRod, 1, 1, 1f),
				new TreasureSlimeItem(ItemID.TheRottedFork, 1, 1, 1f),
				new TreasureSlimeItem(ItemType<ZombieHand>(), 1, 1, 0.2f),
				new TreasureSlimeItem(ItemType<FragmentOfEvil>(), 3, 6, 0.3f),
				new TreasureSlimeItem(ItemType<FoulConcoction>(), 5, 5, 0.3f),

				new TreasureSlimeItem(ItemType<Items.GhostTown.VisionAmulet>(), 1, 1, 0.01f)
			};
		}
        public override void AdditionalLoot()
		{
			Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemType<ExplosiveKnife>(), 10 + Main.rand.Next(11));
		}
    }
}