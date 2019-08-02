using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;



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
			if(player.name == "Rem")
			{
				player.QuickSpawnItem(mod.ItemType("MourningStar"),1);
				player.QuickSpawnItem(ItemID.FamiliarWig, 1);
			}
			if(player.name == "Crimson" || player.name == "Corruption")
			{
				player.QuickSpawnItem(mod.ItemType("CrimCruptPotion"),10);
				player.QuickSpawnItem(ItemID.FamiliarWig, 1);
				player.QuickSpawnItem(ItemID.DemonWings, 1);
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
				player.QuickSpawnItem(mod.ItemType("PlanetariumOrb"),2);
				player.QuickSpawnItem(ItemID.AngelWings, 1);
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
			if(player.name == "X" || player.name == "Xypher"|| player.name == "Heavens")
			{
				player.QuickSpawnItem(mod.ItemType("Heart"),1);
				player.QuickSpawnItem(ItemID.JungleRose, 1);
				player.QuickSpawnItem(ItemID.JungleShirt, 1);
				player.QuickSpawnItem(ItemID.JunglePants, 1);
				player.QuickSpawnItem(mod.ItemType("XyphersGift"),1);
				player.QuickSpawnItem(ItemID.EmeraldStaff,1);
			}
			if(player.name == "O" || player.name == "Omega")
			{
				player.QuickSpawnItem(mod.ItemType("Nova"),1);
				player.QuickSpawnItem(ItemID.FamiliarWig, 1);
				player.QuickSpawnItem(ItemID.ShadowHelmet, 1);
				player.QuickSpawnItem(ItemID.ShadowScalemail, 1);
				player.QuickSpawnItem(ItemID.ShadowGreaves, 1);
				player.QuickSpawnItem(mod.ItemType("OmegasWish"),1);
			}
			if(player.name == "Black_Hole")
			{
				player.QuickSpawnItem(mod.ItemType("Vacuum"),1);
				player.QuickSpawnItem(ItemID.FamiliarWig, 1);
			}
			if(player.name == "Straffex")
			{
				player.QuickSpawnItem(mod.ItemType("Discharge"),1);
				player.QuickSpawnItem(ItemID.FamiliarWig, 1);
			}
			if(player.name == "S" || player.name == "Saturn")
			{
				player.QuickSpawnItem(mod.ItemType("Saturnus"),1);
				player.QuickSpawnItem(mod.ItemType("SaturnHelmet"),1);
				player.QuickSpawnItem(mod.ItemType("SaturnChestplate"),1);
				player.QuickSpawnItem(mod.ItemType("SaturnLeggings"),1);
				player.QuickSpawnItem(mod.ItemType("SaturnWings"),1);
				player.QuickSpawnItem(ItemID.Shuriken, 250);
			}
			if(player.name == "L" || player.name == "Libra")
			{
				player.QuickSpawnItem(mod.ItemType("Patience"),1);
				player.QuickSpawnItem(ItemID.AngelHalo, 1);
				player.QuickSpawnItem(ItemID.AngelWings, 1);
				player.QuickSpawnItem(mod.ItemType("LibrasBackupBow"),1);
				player.QuickSpawnItem(ItemID.PlatinumBow, 1);
				player.QuickSpawnItem(ItemID.WoodenArrow, 250);
			}
			if(player.name == "E" || player.name == "Endre" || player.name == "Lilith")
			{
				player.QuickSpawnItem(mod.ItemType("Enigma"),1);
				player.QuickSpawnItem(ItemID.CrimsonHelmet, 1);
				player.QuickSpawnItem(ItemID.CrimsonScalemail, 1);
				player.QuickSpawnItem(ItemID.CrimsonGreaves, 1);
				player.QuickSpawnItem(mod.ItemType("LostSoul"),1);
				player.QuickSpawnItem(ItemID.SlimeStaff, 1);
				if(player.name == "Lilith")
				{
				player.QuickSpawnItem(mod.ItemType("MourningStar"),1);
				}
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
			DisplayName.SetDefault("Wooden Crate");
			Tooltip.SetDefault("");
		}
	}
}