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
	public class TreasureSlimeGolden : TreasureSlime
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
	}
}