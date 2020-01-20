using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;



namespace SOTS.Items.SpecialDrops
{
	public class GeodeCrate : ModItem
	{	
		public override void SetDefaults()
		{
			item.width = 34;
			item.height = 34;
			item.value = 250000;
			item.rare = 2;
			item.maxStack = 99;
			item.consumable = true;
			item.useTime = 10;
			item.useAnimation = 15;
			item.placeStyle = 0;
			item.useStyle = 1;
			item.autoReuse = true;
			item.createTile = mod.TileType("GeodeCrateTile");
		}
		public override bool CanRightClick()
		{
			return true;
		}
		public override void RightClick(Player player)
		{
			int randomItem = Main.rand.Next(3);
			if(randomItem == 0)
			{
				player.QuickSpawnItem(mod.ItemType("LuckyPurpleBalloon"),1);
			}
			else if(randomItem == 1)
			{
				if(Main.rand.Next(2) == 0 && NPC.downedBoss3)
				{
					player.QuickSpawnItem(ItemID.ObsidianShield,1);
				}
				else
				{
					player.QuickSpawnItem(ItemID.ObsidianHorseshoe,1);
				}
			}
			else if(randomItem == 2)
			{
				player.QuickSpawnItem(mod.ItemType("ObsidianEruption"),1);
			}
			
			if(Main.rand.Next(5) <= 1)
				player.QuickSpawnItem(ItemID.MasterBait,Main.rand.Next(1,5));
			
			if(Main.rand.Next(3) <= 1)
				player.QuickSpawnItem(ItemID.JourneymanBait,Main.rand.Next(1,5));
			
			if(Main.rand.Next(3) == 1)
				player.QuickSpawnItem(ItemID.SilverCoin,Main.rand.Next(50,90));
			
			if(Main.rand.Next(3) == 1)
				player.QuickSpawnItem(ItemID.GoldCoin,Main.rand.Next(1,8));
			
			if(Main.rand.Next(37) == 1)
			{
				player.QuickSpawnItem(ItemID.HermesBoots,1);
				if(Main.rand.Next(5) == 1)
				{
				player.QuickSpawnItem(mod.ItemType("WingedKnife"),1);
				}
			}
			else if(Main.rand.Next(36) == 1)
			{
				player.QuickSpawnItem(ItemID.ShoeSpikes,1);
				if(Main.rand.Next(5) == 1)
				{
				player.QuickSpawnItem(mod.ItemType("SpikedClub"),1);
				}
			}
			else if(Main.rand.Next(35) == 1)
			{
				player.QuickSpawnItem(ItemID.LavaCharm,1);
				if(Main.rand.Next(5) == 1)
				{
				player.QuickSpawnItem(mod.ItemType("LavaPelter"),1);
				}
			}
			if(Main.rand.Next(79) == 1)
			{
				player.QuickSpawnItem(mod.ItemType("CaveIn"),1);
			}
			else if(Main.rand.Next(78) == 1)
			{
				player.QuickSpawnItem(mod.ItemType("Grenadier"),1);
			}
			if(Main.rand.Next(100) == 1)
			{
				player.QuickSpawnItem(mod.ItemType("ShieldofDesecar"),1);
			}
			else if(Main.rand.Next(99) == 1)
			{
				player.QuickSpawnItem(mod.ItemType("ShieldofStekpla"),1);
			}
		}
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Geode Crate");
			Tooltip.SetDefault("Right click to open");
		}
	}
}