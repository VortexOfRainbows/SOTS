using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.ObjectData;
using SOTS.Items.Pyramid;
using SOTS.Items.Potions;
using SOTS.Items.Flails;
using Terraria.DataStructures;

namespace SOTS.Items.Fishing
{
	public class PyramidCrate : ModItem
	{	
		public override void SetDefaults()
		{
			Item.CloneDefaults(ItemID.StoneBlock);
			Item.Size = new Vector2(34, 34);
			Item.autoReuse = true;
			Item.createTile = ModContent.TileType<PyramidCrateTile>();
		}
		public override bool CanRightClick()
		{
			return true;
		}
		public override void RightClick(Player player)
		{
			int randomItem = Main.rand.Next(30); //pyramid chest loot
			if(randomItem == 0)
			{
				player.QuickSpawnItem(player.GetSource_OpenItem(Item.type), ModContent.ItemType<LuckyPurpleBalloon>(), 1); //additional item compared to chest
			}
			if(randomItem == 1)
			{
				player.QuickSpawnItem(player.GetSource_OpenItem(Item.type), ModContent.ItemType<Aten>(),1);
			}
			if(randomItem == 2)
			{
				player.QuickSpawnItem(player.GetSource_OpenItem(Item.type), ModContent.ItemType<EmeraldBracelet>(),1);
			}
			if(randomItem == 3)
			{
				player.QuickSpawnItem(player.GetSource_OpenItem(Item.type), ModContent.ItemType<ImperialPike>(),1);
			}
			if(randomItem == 4)
			{
				player.QuickSpawnItem(player.GetSource_OpenItem(Item.type), ModContent.ItemType<PharaohsCane>(),1);
			}
			if(randomItem == 5)
			{
				player.QuickSpawnItem(player.GetSource_OpenItem(Item.type), ModContent.ItemType<PitatiLongbow>(),1);
			}
			if(randomItem == 6)
			{
				player.QuickSpawnItem(player.GetSource_OpenItem(Item.type), ModContent.ItemType<SandstoneEdge>(),1);
			}
			if(randomItem == 7)
			{
				player.QuickSpawnItem(player.GetSource_OpenItem(Item.type), ModContent.ItemType<SandstoneWarhammer>(),1);
			}
			if(randomItem == 8)
			{
				player.QuickSpawnItem(player.GetSource_OpenItem(Item.type), ModContent.ItemType<RoyalMagnum>(),1);
			}
			if(randomItem == 9)
			{
				player.QuickSpawnItem(player.GetSource_OpenItem(Item.type), ModContent.ItemType<ShiftingSands>(),1);
			}
			if(randomItem == 10)
			{
				player.QuickSpawnItem(player.GetSource_OpenItem(Item.type), ModContent.ItemType<SunlightAmulet>(),1);
			}
			if(randomItem == 11)
			{
				player.QuickSpawnItem(player.GetSource_OpenItem(Item.type), ItemID.FlyingCarpet,1);
			}
			if(randomItem == 12)
			{
				player.QuickSpawnItem(player.GetSource_OpenItem(Item.type), ItemID.SandstorminaBottle,1);
			}
			if(randomItem >= 13)
			{
				player.QuickSpawnItem(player.GetSource_OpenItem(Item.type), ModContent.ItemType<JuryRiggedDrill>(), Main.rand.Next(11) + 10);
			}
			if(!Main.hardMode)
			{
				int rand = Main.rand.Next(5);
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
				else if(rand == 1) //potions
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
				else if(rand == 2) //pyramid drops
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
			if(Main.rand.Next(3) < 2) //bait
			{
				player.QuickSpawnItem(player.GetSource_OpenItem(Item.type), ItemID.JourneymanBait, Main.rand.Next(4) + 1);
			}
			else //master bait
			{
				player.QuickSpawnItem(player.GetSource_OpenItem(Item.type), ItemID.MasterBait, Main.rand.Next(4) + 1);
			}
			if(Main.rand.NextBool(4)) //gold coins
			{
				player.QuickSpawnItem(player.GetSource_OpenItem(Item.type), ItemID.GoldCoin, Main.rand.Next(8) + 5);
			}
		}
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Pyramid Crate");
			Tooltip.SetDefault("Right click to open");
		}
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
			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Pyramid Crate");
			AddMapEntry(new Color(200, 180, 100), name);
			TileObjectData.addTile(Type);
		}
		public override void KillMultiTile(int i, int j, int frameX, int frameY)
		{
			Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 32, 16, ModContent.ItemType<PyramidCrate>()); //this defines what to drop when this tile is destroyed
		}
	}
}