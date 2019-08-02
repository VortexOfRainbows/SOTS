using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.IceStuff
{
	public class ShardKingBossBag : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Treasure Bag");
			Tooltip.SetDefault("Right click to open");
		}
		public override void SetDefaults()
		{

			item.width = 52;
			item.height = 32;
			item.value = 0;
			item.rare = 9;
			item.expert = true;
			item.maxStack = 99;
			item.consumable = true;
			//bossBagNPC = mod.NPCType("ShardKing");
		}
		public override int BossBagNPC => mod.NPCType("ShardKing");
		public override bool CanRightClick()
		{
			return true;
		}
		public override void OpenBossBag(Player player)
		{

			player.QuickSpawnItem(mod.ItemType("IceDeployer"));
			player.QuickSpawnItem(mod.ItemType("AbsoluteBar"),Main.rand.Next(16, 33));
			
				
		}
	}
}