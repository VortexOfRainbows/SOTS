using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.ObjectData;
using SOTS.Items.Pyramid;
using SOTS.Items.Potions;
using SOTS.Items.Flails;
using Terraria.DataStructures;
using Terraria.GameContent.ItemDropRules;
using SOTS.NPCs.Boss;

namespace SOTS.Items.Fishing
{
	public class PyramidCrate : ModItem
	{
		public override void SetStaticDefaults()
		{
			this.SetResearchCost(10);
			ItemID.Sets.IsFishingCrate[Type] = true;
		}
		public override void SetDefaults()
		{
			Item.DefaultToPlaceableTile(ModContent.TileType<PyramidCrateTile>());
			Item.width = 12; //The hitbox dimensions are intentionally smaller so that it looks nicer when fished up on a bobber
			Item.height = 12;
			Item.maxStack = 999;
			Item.rare = ItemRarityID.Orange;
			Item.value = Item.sellPrice(0, 1);
			Item.consumable = true;
		}
		public override bool CanRightClick()
		{
			return true;
		}
        public override void ModifyItemLoot(ItemLoot itemLoot)
		{
			int[] themedDrops = new int[] {
				ModContent.ItemType<LuckyPurpleBalloon>(),
				ModContent.ItemType<EmeraldBracelet>(),
				ModContent.ItemType<ImperialPike>(),
				ModContent.ItemType<PharaohsCane>(),
				ModContent.ItemType<PitatiLongbow>(),
				ModContent.ItemType<SandstoneEdge>(),
				ModContent.ItemType<SandstoneWarhammer>(),
				ModContent.ItemType<ShiftingSands>(),
				ModContent.ItemType<RoyalMagnum>(),
				ModContent.ItemType<SunlightAmulet>(),
				ModContent.ItemType<Aten>(),
				ItemID.FlyingCarpet,
				ItemID.SandstorminaBottle
			};
			itemLoot.Add(ItemDropRule.OneFromOptionsNotScalingWithLuck(1, themedDrops));
			itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<JuryRiggedDrill>(), 2, 10, 20));

			IItemDropRule[] oreTypes = new IItemDropRule[] {
				ItemDropRule.Common(ItemID.SilverBar, 1, 10, 20),
				ItemDropRule.Common(ItemID.TungstenBar, 1, 10, 20),
				ItemDropRule.Common(ItemID.GoldBar, 1, 10, 20),
				ItemDropRule.Common(ItemID.PlatinumBar, 1, 10, 20),
				ItemDropRule.Common(ItemID.DemoniteBar, 1, 10, 20),
				ItemDropRule.Common(ItemID.CrimtaneBar, 1, 10, 20),
			};
			itemLoot.Add(new OneFromRulesRule(6, oreTypes));

			IItemDropRule[] potionTypes = new IItemDropRule[] {
				ItemDropRule.Common(ItemID.ObsidianSkinPotion, 1, 2, 4),
				ItemDropRule.Common(ItemID.SpelunkerPotion, 1, 2, 4),
				ItemDropRule.Common(ItemID.HunterPotion, 1, 2, 4),
				ItemDropRule.Common(ItemID.MiningPotion, 1, 2, 4),
				ItemDropRule.Common(ModContent.ItemType<RoughskinPotion>(), 1, 2, 4),
				ItemDropRule.Common(ModContent.ItemType<SoulAccessPotion>(), 1, 2, 4),
			};
			itemLoot.Add(new OneFromRulesRule(6, potionTypes));

			IItemDropRule[] resourceTypes = new IItemDropRule[] {
				ItemDropRule.Common(ModContent.ItemType<Snakeskin>(), 1, 10, 25),
				ItemDropRule.Common(ModContent.ItemType<SoulResidue>(), 1, 10, 25),
			};
			itemLoot.Add(new OneFromRulesRule(6, resourceTypes));

			IItemDropRule[] resourcePotions = new IItemDropRule[] {
				ItemDropRule.Common(ItemID.HealingPotion, 1, 5, 15),
				ItemDropRule.Common(ItemID.ManaPotion, 1, 5, 15),
			};
			itemLoot.Add(new OneFromRulesRule(2, resourcePotions));


			IItemDropRule[] highendBait = new IItemDropRule[] {
				ItemDropRule.Common(ItemID.JourneymanBait, 1, 2, 5),
				ItemDropRule.Common(ItemID.JourneymanBait, 1, 2, 5),
				ItemDropRule.Common(ItemID.MasterBait, 1, 2, 5),
			};
			itemLoot.Add(new OneFromRulesRule(2, highendBait));

			itemLoot.Add(ItemDropRule.Common(ItemID.GoldCoin, 4, 5, 12));
		}
        /*public override void RightClick(Player player)
		{
			else
			{
				int rand = Main.rand.Next(15);
				if(rand == 0) //ores
				{
					int rand2 = Main.rand.Next(6);
					if(rand2 == 0)
						player.QuickSpawnItem(player.GetSource_OpenItem(Item.type), ItemID.SilverBar, Main.rand.Next(11) + 10);
					if(rand2 == 1)
						player.QuickSpawnItem(player.GetSource_OpenItem(Item.type), ItemID.TungstenBar, Main.rand.Next(11) + 10);
					if(rand2 == 2)
						player.QuickSpawnItem(player.GetSource_OpenItem(Item.type), ItemID.GoldBar, Main.rand.Next(11) + 10);
					if(rand2 == 3)
						player.QuickSpawnItem(player.GetSource_OpenItem(Item.type), ItemID.PlatinumBar, Main.rand.Next(11) + 10);
					if(rand2 == 4)
						player.QuickSpawnItem(player.GetSource_OpenItem(Item.type), ItemID.DemoniteBar, Main.rand.Next(11) + 10);
					if(rand2 == 5)
						player.QuickSpawnItem(player.GetSource_OpenItem(Item.type), ItemID.CrimtaneBar, Main.rand.Next(11) + 10);
				}
				else if(rand <= 2) //hardmode ores
				{
					int rand2 = Main.rand.Next(6);
					if(rand2 == 0)
						player.QuickSpawnItem(player.GetSource_OpenItem(Item.type), ItemID.CobaltBar, Main.rand.Next(13) + 8);
					if(rand2 == 1)
						player.QuickSpawnItem(player.GetSource_OpenItem(Item.type), ItemID.PalladiumBar, Main.rand.Next(13) + 8);
					if(rand2 == 2)
						player.QuickSpawnItem(player.GetSource_OpenItem(Item.type), ItemID.MythrilBar, Main.rand.Next(13) + 8);
					if(rand2 == 3)
						player.QuickSpawnItem(player.GetSource_OpenItem(Item.type), ItemID.OrichalcumBar, Main.rand.Next(13) + 8);
					if(rand2 == 4)
						player.QuickSpawnItem(player.GetSource_OpenItem(Item.type), ItemID.AdamantiteBar, Main.rand.Next(13) + 8);
					if(rand2 == 5)
						player.QuickSpawnItem(player.GetSource_OpenItem(Item.type), ItemID.TitaniumBar, Main.rand.Next(13) + 8);
				}
				else if(rand <= 6) //potions
				{
					int rand2 = Main.rand.Next(6);
					if(rand2 == 0)
						player.QuickSpawnItem(player.GetSource_OpenItem(Item.type), ItemID.ObsidianSkinPotion, Main.rand.Next(3) + 2);
					if(rand2 == 1)
						player.QuickSpawnItem(player.GetSource_OpenItem(Item.type), ItemID.SpelunkerPotion, Main.rand.Next(3) + 2);
					if(rand2 == 2)
						player.QuickSpawnItem(player.GetSource_OpenItem(Item.type), ItemID.HunterPotion, Main.rand.Next(3) + 2);
					if(rand2 == 3)
						player.QuickSpawnItem(player.GetSource_OpenItem(Item.type), ItemID.MiningPotion, Main.rand.Next(3) + 2);
					if(rand2 == 4)
						player.QuickSpawnItem(player.GetSource_OpenItem(Item.type), ModContent.ItemType<RoughskinPotion>(), Main.rand.Next(3) + 2);
					if(rand2 == 5)
						player.QuickSpawnItem(player.GetSource_OpenItem(Item.type), ModContent.ItemType<SoulAccessPotion>(), Main.rand.Next(3) + 2);
				}
				else if(rand <= 10) //pyramid drops
				{
					player.QuickSpawnItem(player.GetSource_OpenItem(Item.type), ModContent.ItemType<Snakeskin>(), Main.rand.Next(16) + 10);
					player.QuickSpawnItem(player.GetSource_OpenItem(Item.type), ModContent.ItemType<SoulResidue>(), Main.rand.Next(16) + 10);
				}
				else //final heal/mana potions
				{
					int rand2 = Main.rand.Next(2);
					if(rand2 == 0)
						player.QuickSpawnItem(player.GetSource_OpenItem(Item.type), ItemID.HealingPotion, Main.rand.Next(10) + 5);
					if(rand2 == 1)
						player.QuickSpawnItem(player.GetSource_OpenItem(Item.type), ItemID.ManaPotion, Main.rand.Next(10) + 5);
				}
			}	
		}*/
	}
	public class PyramidCrateTile : ModTile
	{
		public override void SetStaticDefaults()
		{
			Main.tileSolidTop[Type] = true;
			Main.tileFrameImportant[Type] = true;
			Main.tileTable[Type] = true;
			TileObjectData.newTile.CopyFrom(TileObjectData.Style2x2);
			TileObjectData.newTile.CoordinateHeights = new int[] { 16, 18 };
			LocalizedText name = CreateMapEntryName();
			AddMapEntry(new Color(200, 180, 100), name);
			TileObjectData.addTile(Type);
		}
	}
}