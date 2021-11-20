using SOTS.NPCs.Boss.CelestialSerpent;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.Celestial
{
	public class CelestialBag : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Treasure Bag");
			Tooltip.SetDefault("{$CommonItemTooltip.RightClickToOpen}");
		}
		public override void SetDefaults()
		{
			item.width = 44;
			item.height = 34;
			item.value = 0;
			item.rare = ItemRarityID.LightPurple;
			item.expert = true;
			item.maxStack = 999;
			item.consumable = true;
		}
		public override int BossBagNPC => ModContent.NPCType<CelestialSerpentHead>();
		public override bool CanRightClick()
		{
			return true;
		}
		public override void OpenBossBag(Player player)
		{
			player.TryGettingDevArmor();
			player.QuickSpawnItem(ModContent.ItemType<AngelicCatalyst>());
			player.QuickSpawnItem(ModContent.ItemType<StarShard>() ,Main.rand.Next(16, 30));
			if(Main.rand.Next(10) == 0)
				player.QuickSpawnItem(ModContent.ItemType<StrangeFruit>(), 1);
		}
	}
}