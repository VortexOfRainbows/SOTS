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
			Item.width = 40;
			Item.height = 44;
			Item.value = 0;
			Item.rare = 3;
			Item.maxStack = 1;
		}
		public override void UpdateInventory(Player player)
		{
			SOTSPlayer modPlayer = (SOTSPlayer)player.GetModPlayer(mod, "SOTSPlayer");	
			modPlayer.weakerCurse = true;
		}
	}
}