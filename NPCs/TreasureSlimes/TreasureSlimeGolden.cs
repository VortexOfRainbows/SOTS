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

namespace SOTS.NPCs.TreasureSlimes
{
	public class GoldTreasureSlime : TreasureSlime
	{
        public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Gold Treasure Slime");
			NPCID.Sets.TrailCacheLength[npc.type] = 6;
			NPCID.Sets.TrailingMode[npc.type] = 2;
		}
		public override void SetDefaults()
		{
			base.SetDefaults();
			npc.lifeMax = 100;
			npc.damage = 20;
			npc.defense = 16;
			npc.knockBackResist = 0.4f;
			npc.value = Item.buyPrice(0, 4, 0, 0);
			npc.Size = new Vector2(32, 36);
			npc.npcSlots = 1f;
			banner = npc.type;
			bannerItem = ItemType<TreasureSlimeBanner>();
			items = new List<TreasureSlimeItem>()
			{
				new TreasureSlimeItem(ItemID.GoldBar, 10, 20),
				new TreasureSlimeItem(ItemID.PlatinumBar, 10, 20),
				new TreasureSlimeItem(ItemID.MagicMirror, 1, 1),
				new TreasureSlimeItem(ItemID.EnchantedBoomerang, 1, 1),
				new TreasureSlimeItem(ItemID.Dynamite, 5, 15),
				new TreasureSlimeItem(ItemID.HermesBoots, 1, 1),
				new TreasureSlimeItem(ItemID.CloudinaBottle, 1, 1),
				new TreasureSlimeItem(ItemID.Extractinator, 1, 1)
			};
		}
        public override void AdditionalLoot()
        {
            base.AdditionalLoot();
        }
    }
}