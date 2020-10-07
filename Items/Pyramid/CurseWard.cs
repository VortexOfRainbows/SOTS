using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.Pyramid
{
	public class CurseWard : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Curse Ward");
			Tooltip.SetDefault("Weakens the pyramid's curse while in the inventory\nNo longer needed, as the curse will weaken automatically after killing the BoC or EoW");
		}
		public override void SetDefaults()
		{
			item.width = 40;
			item.height = 44;
			item.value = 0;
			item.rare = 3;
			item.maxStack = 1;
		}
		public override void UpdateInventory(Player player)
		{
			SOTSPlayer modPlayer = (SOTSPlayer)player.GetModPlayer(mod, "SOTSPlayer");	
			modPlayer.weakerCurse = true;
		}
	}
}