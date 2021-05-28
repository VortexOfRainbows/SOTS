using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace SOTS.Items.Pyramid
{
	public class CurseBag : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Treasure Bag");
			Tooltip.SetDefault("Right Click to open");
			Main.RegisterItemAnimation(item.type, new DrawAnimationVertical(5, 7));
		}
		public override void SetDefaults()
		{
			item.width = 36;
			item.height = 34;
			item.value = 0;
			item.rare = 6;
			item.expert = true;
			item.maxStack = 99;
			item.consumable = true;
		}
		public override int BossBagNPC => mod.NPCType("PharaohsCurse");
		public override bool CanRightClick()
		{
			return true;
		}
		public override void OpenBossBag(Player player)
		{
			player.QuickSpawnItem(ModContent.ItemType<HeartInAJar>());
			player.QuickSpawnItem(mod.ItemType("CursedMatter"),Main.rand.Next(14, 25));
		}
	}
}