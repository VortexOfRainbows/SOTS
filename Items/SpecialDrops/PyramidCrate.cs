using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;



namespace SOTS.Items.SpecialDrops
{
	public class PyramidCrate : ModItem
	{	
		public override void SetDefaults()
		{
			item.width = 34;
			item.height = 34;
			item.value = 0;
			item.rare = 1;
			item.maxStack = 99;
			item.consumable = true;
			item.useAnimation = 15;
			item.useTime = 10;
			item.placeStyle = 0;
			item.useStyle = 1;
			item.autoReuse = true;
			item.createTile = mod.TileType("PyramidCrateTile");
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
				player.QuickSpawnItem(mod.ItemType("LuckyPurpleBalloon"),1); //additional item compared to chest
			}
			if(randomItem == 1)
			{
				player.QuickSpawnItem(mod.ItemType("Aten"),1);
			}
			if(randomItem == 2)
			{
				player.QuickSpawnItem(mod.ItemType("EmeraldBracelet"),1);
			}
			if(randomItem == 3)
			{
				player.QuickSpawnItem(mod.ItemType("ImperialPike"),1);
			}
			if(randomItem == 4)
			{
				player.QuickSpawnItem(mod.ItemType("PharaohsCane"),1);
			}
			if(randomItem == 5)
			{
				player.QuickSpawnItem(mod.ItemType("PitatiLongbow"),1);
			}
			if(randomItem == 6)
			{
				player.QuickSpawnItem(mod.ItemType("SandstoneEdge"),1);
			}
			if(randomItem == 7)
			{
				player.QuickSpawnItem(mod.ItemType("SandstoneWarhammer"),1);
			}
			if(randomItem == 8)
			{
				player.QuickSpawnItem(mod.ItemType("RoyalMagnum"),1);
			}
			if(randomItem == 9)
			{
				player.QuickSpawnItem(mod.ItemType("ShiftingSands"),1);
			}
			if(randomItem == 10)
			{
				player.QuickSpawnItem(mod.ItemType("SunlightAmulet"),1);
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
				player.QuickSpawnItem(mod.ItemType("JuryRiggedDrill"), Main.rand.Next(11) + 10);
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
						player.QuickSpawnItem(mod.ItemType("RoughskinPotion"), Main.rand.Next(3) + 2);
					if(rand2 == 5)
						player.QuickSpawnItem(mod.ItemType("SoulAccessPotion"), Main.rand.Next(3) + 2);
				}
				else if(rand == 2) //pyramid drops
				{
					player.QuickSpawnItem(mod.ItemType("Snakeskin"), Main.rand.Next(16) + 10);
					player.QuickSpawnItem(mod.ItemType("SoulResidue"), Main.rand.Next(16) + 10);
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
						player.QuickSpawnItem(mod.ItemType("RoughskinPotion"), Main.rand.Next(3) + 2);
					if(rand2 == 5)
						player.QuickSpawnItem(mod.ItemType("SoulAccessPotion"), Main.rand.Next(3) + 2);
				}
				else if(rand <= 10) //pyramid drops
				{
					player.QuickSpawnItem(mod.ItemType("Snakeskin"), Main.rand.Next(16) + 10);
					player.QuickSpawnItem(mod.ItemType("SoulResidue"), Main.rand.Next(16) + 10);
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
}