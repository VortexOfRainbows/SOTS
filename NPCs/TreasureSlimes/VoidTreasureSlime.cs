using Microsoft.Xna.Framework;
using SOTS.Items.Banners;
using Terraria;
using Terraria.ID;
using System.Collections.Generic;
using static Terraria.ModLoader.ModContent;
using SOTS.Items.Fragments;
using Terraria.GameContent.ItemDropRules;
using Terraria.ModLoader;
using SOTS.Helpers;
using SOTS.Items.Invidia;
using SOTS.Items.Invidia.MoonShard;
using SOTS.Items.Potions;

namespace SOTS.NPCs.TreasureSlimes
{
	public class VoidTreasureSlime : TreasureSlime
	{
        public override void SetStaticDefaults()
		{
			NPCID.Sets.TrailCacheLength[NPC.type] = 6;
			NPCID.Sets.TrailingMode[NPC.type] = 2;
		}
		public override void SetDefaults()
		{
			base.SetDefaults();
			NPC.lifeMax = 1000;
			NPC.damage = 150; //This is a LOT of damage
			NPC.defense = 20;
			NPC.knockBackResist = 0.12f;
			NPC.value = Item.buyPrice(0, 8, 0, 0);
			NPC.Size = new Vector2(32, 42);
			NPC.npcSlots = 1.5f;
			Banner = NPC.type;
			BannerItem = ItemType<VoidTreasureSlimeBanner>();
			NPC.lavaImmune = true;
			LootAmt = 7;
			Color c = ColorHelper.Evostone;
			c.A = 100;
			gelColor = c;
			items = new List<TreasureSlimeItem>()
			{
				//8 different shards
				new(ItemType<MoonShard1>(), 1, 1, 1f),
                new(ItemType<MoonShard2>(), 1, 1, 1f),
                new(ItemType<MoonShard3>(), 1, 1, 1f),
                new(ItemType<MoonShard4>(), 1, 1, 1f),
                new(ItemType<MoonShard5>(), 1, 1, 1f),
                new(ItemType<MoonShard6>(), 1, 1, 1f),
                new(ItemType<MoonShard7>(), 1, 1, 1f),
                new(ItemType<MoonShard8>(), 1, 1, 1f),

				//7 different key items
                new(ItemType<UnholyGrail>(), 1, 1, 1f),
                new(ItemType<GobletOfEntrails>(), 1, 1, 1f),
                new(ItemType<EmptyNecklace>(), 1, 1, 1f),
                new(ItemType<Dreamcatcher>(), 1, 1, 1f),
                new(ItemType<MartianWarhorn>(), 1, 1, 1f),
                new(ItemType<HardlightHook>(), 1, 1, 1f),
                new(ItemType<LevMirror>(), 1, 1, 1f),
                //new(ItemType<Sunbulb>(), 1, 1, 1f), //You are not allowed to get more than 1 sunbulb!

				//7 different fragments
                new(ItemType<FragmentOfTide>(), 3, 6, 0.5f),
				new(ItemType<FragmentOfInferno>(), 3, 6, 0.5f),
				new(ItemType<FragmentOfEvil>(), 3, 6, 0.5f),
                new(ItemType<FragmentOfNature>(), 3, 6, 0.5f),
                new(ItemType<FragmentOfEarth>(), 3, 6, 0.5f),
                new(ItemType<FragmentOfPermafrost>(), 3, 6, 0.5f),
                new(ItemType<FragmentOfOtherworld>(), 3, 6, 0.5f),

				//Tonics dropping are extremely powerful, as they may be shimmered into the dissolving elements... So they must be rare
				//7 different tonics
                new(ItemType<BlightfulTonic>(), 1, 1, 0.1f),
                new(ItemType<SeismicTonic>(), 1, 1, 0.1f),
                new(ItemType<GlacialTonic>(), 1, 1, 0.1f),
                new(ItemType<AbyssalTonic>(), 1, 1, 0.1f),
                new(ItemType<StarlightTonic>(), 1, 1, 0.1f),
                new(ItemType<HereticTonic>(), 1, 1, 0.1f),
                new(ItemType<BlazingTonic>(), 1, 1, 0.1f),

				//7 different gems
                new(ItemID.Amethyst, 10, 20, 0.2f),
                new(ItemID.Ruby, 10, 20, 0.2f),
                new(ItemID.Amber, 10, 20, 0.2f),
                new(ItemID.Topaz, 10, 20, 0.2f),
                new(ItemID.Sapphire, 10, 20, 0.2f),
                new(ItemID.Emerald, 10, 20, 0.2f),
                new(ItemID.Diamond, 10, 20, 0.2f),

                //new(ItemType<FragmentOfChaos>(), 3, 6, 0.2f), //Only one it won't drop is chaos
                //new(ItemType<EtherealTonic>(), 3, 6, 0.5f),
            };
        }
        public override void ApplyDifficultyAndPlayerScaling(int numPlayers, float balance, float bossAdjustment)/* tModPorter Note: bossLifeScale -> balance (bossAdjustment is different, see the docs for details) */
        {
			NPC.damage = (int)(NPC.damage * 7 / 8);
		}
		public override void ModifyAdditionalLoot(NPCLoot npcLoot)
		{
			npcLoot.Add(ItemDropRule.Common(ItemType<RunicEvostone>(), 5, 20, 50));
			npcLoot.Add(ItemDropRule.Common(ItemType<RunicEvostoneBrick>(), 5, 20, 50));
			npcLoot.Add(ItemDropRule.Common(ItemType<InvidiaPetal>(), 1, 10, 15));
        }
    }
}