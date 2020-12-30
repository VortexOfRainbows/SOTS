using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.Otherworld
{
	public class TheAdvisorBossBag : ModItem
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
			item.rare = 6;
			item.expert = true;
			item.maxStack = 99;
			item.consumable = true;
			//bossBagNPC = mod.NPCType("PutridPinky2Head");
		}
		public override int BossBagNPC => mod.NPCType("TheAdvisorHead");
		public override bool CanRightClick()
		{
			return true;
		}
		public override void OpenBossBag(Player player)
		{
			player.QuickSpawnItem(mod.ItemType("TwilightGyroscope"));

			if(Main.rand.NextBool(3))
				player.QuickSpawnItem(mod.ItemType("StarlightAlloy"), Main.rand.Next(12, 19));
			else
				player.QuickSpawnItem(mod.ItemType("SkywareKey"));


			if (Main.rand.NextBool(3))
				player.QuickSpawnItem(mod.ItemType("OtherworldlyAlloy"), Main.rand.Next(12, 19));
			else
				player.QuickSpawnItem(mod.ItemType("MeteoriteKey"));


			if (Main.rand.NextBool(3))
				player.QuickSpawnItem(mod.ItemType("HardlightAlloy"), Main.rand.Next(12, 19));
			else
				player.QuickSpawnItem(mod.ItemType("StrangeKey"));
		}
	}
}