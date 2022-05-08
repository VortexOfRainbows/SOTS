using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.ObjectData;
using SOTS.Items.Pyramid;
using SOTS.Items.Potions;
using SOTS.Items.Flails;

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
				player.QuickSpawnItem(ModContent.ItemType<LuckyPurpleBalloon>(), 1); //additional item compared to chest
			}
			if(randomItem == 1)
			{
				player.QuickSpawnItem(ModContent.ItemType<Aten>(),1);
			}
			if(randomItem == 2)
			{
				player.QuickSpawnItem(ModContent.ItemType<EmeraldBracelet>(),1);
			}
			if(randomItem == 3)
			{
				player.QuickSpawnItem(ModContent.ItemType<ImperialPike>(),1);
			}
			if(randomItem == 4)
			{
				player.QuickSpawnItem(ModContent.ItemType<PharaohsCane>(),1);
			}
			if(randomItem == 5)
			{
				player.QuickSpawnItem(ModContent.ItemType<PitatiLongbow>(),1);
			}
			if(randomItem == 6)
			{
				player.QuickSpawnItem(ModContent.ItemType<SandstoneEdge>(),1);
			}
			if(randomItem == 7)
			{
				player.QuickSpawnItem(ModContent.ItemType<SandstoneWarhammer>(),1);
			}
			if(randomItem == 8)
			{
				player.QuickSpawnItem(ModContent.ItemType<RoyalMagnum>(),1);
			}
			if(randomItem == 9)
			{
				player.QuickSpawnItem(ModContent.ItemType<ShiftingSands>(),1);
			}
			if(randomItem == 10)
			{
				player.QuickSpawnItem(ModContent.ItemType<SunlightAmulet>(),1);
			}
			if(randomItem == 11)
			{
				player.QuickSpawnItem(ItemID.FlyingCarpet,1);
			}
			if(randomItem == 12)
			{
				player.QuickSpawnItem(ItemID.SandstorminaBottle,1);
			}
			if(randomItem >= 13)
			{
				player.QuickSpawnItem(ModContent.ItemType<JuryRiggedDrill>(), Main.rand.Next(11) + 10);
			}
			if(!Main.hardMode)
			{
				int rand = Main.rand.Next(5);
				if(rand == 0) //ores
				{
					int rand2 = Main.rand.Next(6);
					if(rand2 == 0)
						player.QuickSpawnItem(ItemID.SilverBar, Main.rand.Next(11) + 10);
					if(rand2 == 1)
						player.QuickSpawnItem(ItemID.TungstenBar, Main.rand.Next(11) + 10);
					if(rand2 == 2)
						player.QuickSpawnItem(ItemID.GoldBar, Main.rand.Next(11) + 10);
					if(rand2 == 3)
						player.QuickSpawnItem(ItemID.PlatinumBar, Main.rand.Next(11) + 10);
					if(rand2 == 4)
						player.QuickSpawnItem(ItemID.DemoniteBar, Main.rand.Next(11) + 10);
					if(rand2 == 5)
						player.QuickSpawnItem(ItemID.CrimtaneBar, Main.rand.Next(11) + 10);
						
				}
				else if(rand == 1) //potions
				{
					int rand2 = Main.rand.Next(6);
					if(rand2 == 0)
						player.QuickSpawnItem(ItemID.ObsidianSkinPotion, Main.rand.Next(3) + 2);
					if(rand2 == 1)
						player.QuickSpawnItem(ItemID.SpelunkerPotion, Main.rand.Next(3) + 2);
					if(rand2 == 2)
						player.QuickSpawnItem(ItemID.HunterPotion, Main.rand.Next(3) + 2);
					if(rand2 == 3)
						player.QuickSpawnItem(ItemID.MiningPotion, Main.rand.Next(3) + 2);
					if(rand2 == 4)
						player.QuickSpawnItem(ModContent.ItemType<RoughskinPotion>(), Main.rand.Next(3) + 2);
					if(rand2 == 5)
						player.QuickSpawnItem(ModContent.ItemType<SoulAccessPotion>(), Main.rand.Next(3) + 2);
				}
				else if(rand == 2) //pyramid drops
				{
					player.QuickSpawnItem(ModContent.ItemType<Snakeskin>(), Main.rand.Next(16) + 10);
					player.QuickSpawnItem(ModContent.ItemType<SoulResidue>(), Main.rand.Next(16) + 10);
				}
				else //final heal/mana potions
				{
					int rand2 = Main.rand.Next(2);
					if(rand2 == 0)
						player.QuickSpawnItem(ItemID.HealingPotion, Main.rand.Next(10) + 5);
					if(rand2 == 1)
						player.QuickSpawnItem(ItemID.ManaPotion, Main.rand.Next(10) + 5);
				}
			}
			else
			{
				int rand = Main.rand.Next(15);
				if(rand == 0) //ores
				{
					int rand2 = Main.rand.Next(6);
					if(rand2 == 0)
						player.QuickSpawnItem(ItemID.SilverBar, Main.rand.Next(11) + 10);
					if(rand2 == 1)
						player.QuickSpawnItem(ItemID.TungstenBar, Main.rand.Next(11) + 10);
					if(rand2 == 2)
						player.QuickSpawnItem(ItemID.GoldBar, Main.rand.Next(11) + 10);
					if(rand2 == 3)
						player.QuickSpawnItem(ItemID.PlatinumBar, Main.rand.Next(11) + 10);
					if(rand2 == 4)
						player.QuickSpawnItem(ItemID.DemoniteBar, Main.rand.Next(11) + 10);
					if(rand2 == 5)
						player.QuickSpawnItem(ItemID.CrimtaneBar, Main.rand.Next(11) + 10);
				}
				else if(rand <= 2) //hardmode ores
				{
					int rand2 = Main.rand.Next(6);
					if(rand2 == 0)
						player.QuickSpawnItem(ItemID.CobaltBar, Main.rand.Next(13) + 8);
					if(rand2 == 1)
						player.QuickSpawnItem(ItemID.PalladiumBar, Main.rand.Next(13) + 8);
					if(rand2 == 2)
						player.QuickSpawnItem(ItemID.MythrilBar, Main.rand.Next(13) + 8);
					if(rand2 == 3)
						player.QuickSpawnItem(ItemID.OrichalcumBar, Main.rand.Next(13) + 8);
					if(rand2 == 4)
						player.QuickSpawnItem(ItemID.AdamantiteBar, Main.rand.Next(13) + 8);
					if(rand2 == 5)
						player.QuickSpawnItem(ItemID.TitaniumBar, Main.rand.Next(13) + 8);
				}
				else if(rand <= 6) //potions
				{
					int rand2 = Main.rand.Next(6);
					if(rand2 == 0)
						player.QuickSpawnItem(ItemID.ObsidianSkinPotion, Main.rand.Next(3) + 2);
					if(rand2 == 1)
						player.QuickSpawnItem(ItemID.SpelunkerPotion, Main.rand.Next(3) + 2);
					if(rand2 == 2)
						player.QuickSpawnItem(ItemID.HunterPotion, Main.rand.Next(3) + 2);
					if(rand2 == 3)
						player.QuickSpawnItem(ItemID.MiningPotion, Main.rand.Next(3) + 2);
					if(rand2 == 4)
						player.QuickSpawnItem(ModContent.ItemType<RoughskinPotion>(), Main.rand.Next(3) + 2);
					if(rand2 == 5)
						player.QuickSpawnItem(ModContent.ItemType<SoulAccessPotion>(), Main.rand.Next(3) + 2);
				}
				else if(rand <= 10) //pyramid drops
				{
					player.QuickSpawnItem(ModContent.ItemType<Snakeskin>(), Main.rand.Next(16) + 10);
					player.QuickSpawnItem(ModContent.ItemType<SoulResidue>(), Main.rand.Next(16) + 10);
				}
				else //final heal/mana potions
				{
					int rand2 = Main.rand.Next(2);
					if(rand2 == 0)
						player.QuickSpawnItem(ItemID.HealingPotion, Main.rand.Next(10) + 5);
					if(rand2 == 1)
						player.QuickSpawnItem(ItemID.ManaPotion, Main.rand.Next(10) + 5);
				}
			}
			if(Main.rand.Next(3) < 2) //bait
			{
				player.QuickSpawnItem(ItemID.JourneymanBait, Main.rand.Next(4) + 1);
			}
			else //master bait
			{
				player.QuickSpawnItem(ItemID.MasterBait, Main.rand.Next(4) + 1);
			}
			if(Main.rand.Next(4) == 0) //gold coins
			{
				player.QuickSpawnItem(ItemID.GoldCoin, Main.rand.Next(8) + 5);
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
		public override void SetDefaults()
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
			Item.NewItem(i * 16, j * 16, 32, 16, ModContent.ItemType<PyramidCrate>()); //this defines what to drop when this tile is destroyed
		}
	}
}