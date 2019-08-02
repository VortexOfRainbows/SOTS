using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items
{
	public class AntilionLootBag : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Treasure Bag");
			Tooltip.SetDefault("Right Click to open");
		}
		public override void SetDefaults()
		{

			item.width = 36;
			item.height = 32;
			item.value = 0;
			item.rare = 8;
			item.expert = true;
			item.maxStack = 99;
			item.consumable = true;
		}
		
		public override int BossBagNPC => mod.NPCType("Antilion");

		public override bool CanRightClick()
		{
			return true;
		}
		public override void OpenBossBag(Player player)
		{

			player.QuickSpawnItem(mod.ItemType("DesertEye"));
			player.QuickSpawnItem(mod.ItemType("AntimaterialMandible"),Main.rand.Next(5, 8));
			player.QuickSpawnItem(mod.ItemType("BrassBar"),Main.rand.Next(5, 8));
			player.QuickSpawnItem(mod.ItemType("SteelBar"),Main.rand.Next(5, 8));
			player.QuickSpawnItem(ItemID.SoulofNight,Main.rand.Next(1, 11));
			player.QuickSpawnItem(ItemID.SoulofLight,Main.rand.Next(1, 11));
			
			player.QuickSpawnItem(ItemID.CopperBar,Main.rand.Next(0, 5));
			player.QuickSpawnItem(ItemID.TinBar,Main.rand.Next(0, 5));
			player.QuickSpawnItem(ItemID.LeadBar,Main.rand.Next(0, 5));
			player.QuickSpawnItem(ItemID.IronBar,Main.rand.Next(0, 5));
			player.QuickSpawnItem(ItemID.SilverBar,Main.rand.Next(0, 5));
			player.QuickSpawnItem(ItemID.GoldBar,Main.rand.Next(0, 5));
			player.QuickSpawnItem(ItemID.TungstenBar,Main.rand.Next(0, 5));
			player.QuickSpawnItem(ItemID.PlatinumBar,Main.rand.Next(0, 5));
				
		}
	}
}