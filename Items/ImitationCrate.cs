using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
namespace SOTS.Items
{
	public class ImitationCrate : ModItem
	{
		public override void SetDefaults()
		{
			item.width = 34;
			item.height = 34;
			item.value = 0;
			item.rare = 2;
			item.maxStack =99;
			item.consumable = true;
		}
		public override bool CanRightClick()
		{
			return true;
		}
		public override void RightClick(Player player)
		{
			if(player.name == "Turtle" || player.name == "TurtleTem" || player.name == "Tris")
			{
				player.QuickSpawnItem(mod.ItemType("QueenSpikey"),1);
				player.QuickSpawnItem(ItemID.FamiliarWig, 1);
				player.QuickSpawnItem(ItemID.PlatinumPickaxe, 1);
			}
			if(player.name == "IceCream" || player.name == "Ice Cream")
			{
				player.QuickSpawnItem(mod.ItemType("IceCreamOre"),10);
				player.QuickSpawnItem(ItemID.FamiliarWig, 1);
			}
			if(player.name == "Crimson" || player.name == "Corruption")
			{
				player.QuickSpawnItem(mod.ItemType("CrimCruptPotion"),10);
				player.QuickSpawnItem(ItemID.FamiliarWig, 1);
				player.QuickSpawnItem(ItemID.BugNet, 1);
				if(Main.rand.Next(2) == 0)
				{
				player.QuickSpawnItem(ItemID.DeathbringerPickaxe, 1);
				player.QuickSpawnItem(ItemID.FisherofSouls, 1);
				}
				else
				{
				player.QuickSpawnItem(ItemID.NightmarePickaxe, 1);
				player.QuickSpawnItem(ItemID.Fleshcatcher, 1);
					
				}
				
			}
			if(player.name == "EnergyPlayz07" || player.name == "EnergyBoy07")
			{
				player.QuickSpawnItem(ItemID.PlatinumHelmet, 1);
				player.QuickSpawnItem(ItemID.PlatinumChainmail, 1);
				player.QuickSpawnItem(ItemID.PlatinumGreaves, 1);
				player.QuickSpawnItem(ItemID.UnluckyYarn, 1);
			}
			if(player.name == "Minez" || player.name == "TheMinez")
			{
				player.QuickSpawnItem(mod.ItemType("KingSmugMug"),1);
				player.QuickSpawnItem(ItemID.FamiliarWig, 1);
				player.QuickSpawnItem(ItemID.GoldPickaxe, 1);
			}
			if(player.name == "Zombeebear" || player.name == "ZombeeBear")
			{
				player.QuickSpawnItem(mod.ItemType("Grenadier"),1);
				player.QuickSpawnItem(ItemID.FamiliarWig, 1);
			}
			if(player.name == "Megumin" || player.name == "megumin")
			{
				player.QuickSpawnItem(mod.ItemType("MeguminHat"),1);
				player.QuickSpawnItem(mod.ItemType("MeguminShirt"),1);
				player.QuickSpawnItem(mod.ItemType("MeguminLeggings"),1);
				player.QuickSpawnItem(mod.ItemType("TheMelter"),1);
				player.QuickSpawnItem(3580, 1);
				player.QuickSpawnItem(ItemID.ManaCrystal, 9);
				player.QuickSpawnItem(ItemID.FamiliarWig, 1);
			}
			if(player.name == "Deo" || player.name == "deo" || player.name == "Deoxys" || player.name == "deoxys" || player.name == "DeoxysA" || player.name == "deoxysA" || player.name == "deoxysa" || player.name == "Deoxysa")
			{
				player.QuickSpawnItem(mod.ItemType("DeoxysABall"),1);
				player.QuickSpawnItem(ItemID.FamiliarWig, 1);
			}
			if(player.name == "FlameFreezer")
			{
				player.QuickSpawnItem(ItemID.PearlwoodSword,1);
				player.QuickSpawnItem(mod.ItemType("BowSonOfBow"),1);
				player.QuickSpawnItem(ItemID.Wood,150);
			}
			
			player.QuickSpawnItem(ItemID.LifeCrystal,1);
			player.QuickSpawnItem(ItemID.ManaCrystal,1);
			
		}
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Starter Crate");
			Tooltip.SetDefault("");
		}
	}
}